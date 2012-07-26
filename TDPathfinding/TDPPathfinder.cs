using System;
using System.Collections.Generic;
using System.Linq;
using TimeDRODPOF.TDLib;
using VeeTileEngine2012;

namespace TimeDRODPOF.TDPathfinding
{
    public class TDPPathfinder
    {
        private const int Offset = 10;
        private readonly Field _field;
        private readonly Dictionary<string, TDPPathmap> _pathmaps;
        private TDPNode[,] _nodes;
        private bool[,] _obstacles;

        public TDPPathfinder(Field mField)
        {
            _field = mField;
            _pathmaps = new Dictionary<string, TDPPathmap>();

            InitializeNodes();
            InitializeObstacles();
            RefreshObstacles(0, 0, _field.Width, _field.Height);
        }

        private void InitializeNodes()
        {
            _nodes = new TDPNode[_field.Width + Offset*2,_field.Height + Offset*2];

            for (var iY = 0; iY < _field.Height; iY++)
                for (var iX = 0; iX < _field.Width; iX++)
                    _nodes[iX + Offset, iY + Offset] = new TDPNode(iX, iY);
        }

        private void InitializeObstacles()
        {
            _obstacles = new bool[_field.Width + Offset*2,_field.Height + Offset*2];

            for (var iY = 0; iY < _obstacles.GetLength(1); iY++)
                for (var iX = 0; iX < _obstacles.GetLength(0); iX++)
                    _obstacles[iX, iY] = true;
        }
        private void RefreshObstacles(int mStartX, int mStartY, int mEndX, int mEndY)
        {
            for (var iY = mStartY; iY < mEndY; iY++)
                for (var iX = mStartX; iX < mEndX; iX++)
                    _obstacles[iX + Offset, iY + Offset] = _field.HasEntityByTag(iX, iY, TDLTags.Wall);
        }

        public void CreatePathmap(string mPathmapName)
        {
            _pathmaps.Add(mPathmapName, new TDPPathmap(_field));
            //_field.TurnActions[TDLTurnActions.End] += GetPathmap(mPathmapName).CalculatePathmap;
        }
        public TDPPathmap GetPathmap(string mPathmapName) { return _pathmaps[mPathmapName]; }

        public TDPNode JumpPointSearch(int mStartX, int mStartY, int mTargetX, int mTargetY)
        {
            var startNode = GetNode(mStartX, mStartY);
            var targetNode = GetNode(mTargetX, mTargetY);

            var openContainer = new HashSet<TDPNode> {startNode};
            var nodesToRefresh = new List<TDPNode> {startNode, targetNode};

            while (openContainer.Count > 0)
            {
                var currentNode = openContainer.OrderBy(x => x.F).First();

                openContainer.Remove(currentNode);
                currentNode.IsClosed = true;

                if (currentNode == targetNode)
                {
                    var finalNode = targetNode;
                    while (finalNode.Parent != startNode) finalNode = finalNode.Parent;

                    foreach (var node in nodesToRefresh) node.Refresh();
                    return finalNode;
                }

                foreach (var neighbor in JPSFindNeighbors(currentNode))
                {
                    var jumpPoint = JPSJump(neighbor, currentNode, targetNode);
                    if (jumpPoint == null) continue;

                    var x = currentNode.X;
                    var y = currentNode.Y;
                    var jx = jumpPoint.X;
                    var jy = jumpPoint.Y;
                    var jumpNode = GetNode(jx, jy);

                    if (jumpNode.IsClosed) continue;

                    var d = Math.Sqrt(TDLDistances.EuclideanDistanceSquared(jx, jy, x, y));
                    var ng = currentNode.G + d; // next `g` value

                    if (openContainer.Contains(jumpNode) && !(ng < jumpNode.G)) continue;

                    jumpNode.G = ng;
                    if (jumpNode.H == 0) jumpNode.H = Math.Sqrt(TDLDistances.EuclideanDistanceSquared(jx, jy, targetNode.X, targetNode.Y));
                    jumpNode.Parent = currentNode;

                    openContainer.Add(jumpNode);
                    nodesToRefresh.Add(jumpNode);
                }
            }

            foreach (var node in nodesToRefresh) node.Refresh();
            return null;
        }
        private TDPNode JPSJump(TDPNode mStartNode, TDPNode mEndNode, TDPNode mTargetNode)
        {
            if (mStartNode == null)
                return null;

            var dx = mStartNode.X - mEndNode.X;
            var dy = mStartNode.Y - mEndNode.Y;

            if (IsObstacle(mStartNode.X, mStartNode.Y)) return null;
            if (mStartNode == mTargetNode) return mStartNode;

            if (dx != 0 && dy != 0)
            {
                if ((!IsObstacle(mStartNode.X - dx, mStartNode.Y + dy) && IsObstacle(mStartNode.X - dx, mStartNode.Y)) ||
                    (!IsObstacle(mStartNode.X + dx, mStartNode.Y - dy) && IsObstacle(mStartNode.X, mStartNode.Y - dy)))
                    return mStartNode;
            }
            else
            {
                if (dx != 0)
                {
                    if ((!IsObstacle(mStartNode.X + dx, mStartNode.Y + 1) && IsObstacle(mStartNode.X, mStartNode.Y + 1)) ||
                        (!IsObstacle(mStartNode.X + dx, mStartNode.Y - 1) && IsObstacle(mStartNode.X, mStartNode.Y - 1)))
                        return mStartNode;
                }
                else
                {
                    if ((!IsObstacle(mStartNode.X + 1, mStartNode.Y + dy) && IsObstacle(mStartNode.X + 1, mStartNode.Y)) ||
                        (!IsObstacle(mStartNode.X - 1, mStartNode.Y + dy) && IsObstacle(mStartNode.X - 1, mStartNode.Y)))
                        return mStartNode;
                }
            }

            if (dx != 0 && dy != 0)
            {
                var jx = JPSJump(GetNode(mStartNode.X + dx, mStartNode.Y), mStartNode, mTargetNode);
                var jy = JPSJump(GetNode(mStartNode.X, mStartNode.Y + dy), mStartNode, mTargetNode);
                if (jx != null || jy != null) return mStartNode;
            }

            if (!IsObstacle(mStartNode.X + dx, mStartNode.Y) || !IsObstacle(mStartNode.X, mStartNode.Y + dy))
                return JPSJump(GetNode(mStartNode.X + dx, mStartNode.Y + dy), mStartNode, mTargetNode);

            return null;
        }
        private IEnumerable<TDPNode> JPSFindNeighbors(TDPNode mNode)
        {
            var result = new List<TDPNode>();

            if (mNode.Parent == null) return GetAdjacentNodes(mNode.X, mNode.Y);

            var dx = (mNode.X - mNode.Parent.X)/Math.Max(Math.Abs(mNode.X - mNode.Parent.X), 1);
            var dy = (mNode.Y - mNode.Parent.Y)/Math.Max(Math.Abs(mNode.Y - mNode.Parent.Y), 1);

            if (dx != 0 && dy != 0)
            {
                if (!IsObstacle(mNode.X, mNode.Y + dy))
                    result.Add(GetNode(mNode.X, mNode.Y + dy));
                if (!IsObstacle(mNode.X + dx, mNode.Y))
                    result.Add(GetNode(mNode.X + dx, mNode.Y));
                if (!IsObstacle(mNode.X, mNode.Y + dy) || !IsObstacle(mNode.X + dx, mNode.Y))
                    result.Add(GetNode(mNode.X + dx, mNode.Y + dy));
                if (IsObstacle(mNode.X - dx, mNode.Y) && !IsObstacle(mNode.X, mNode.Y + dy))
                    result.Add(GetNode(mNode.X - dx, mNode.Y + dy));
                if (IsObstacle(mNode.X, mNode.Y - dy) && !IsObstacle(mNode.X + dx, mNode.Y))
                    result.Add(GetNode(mNode.X + dx, mNode.Y - dy));
            }
            else
            {
                if (dx == 0)
                {
                    if (!IsObstacle(mNode.X, mNode.Y + dy))
                    {
                        if (!IsObstacle(mNode.X, mNode.Y + dy))
                            result.Add(GetNode(mNode.X, mNode.Y + dy));
                        if (IsObstacle(mNode.X + 1, mNode.Y))
                            result.Add(GetNode(mNode.X + 1, mNode.Y + dy));
                        if (IsObstacle(mNode.X - 1, mNode.Y))
                            result.Add(GetNode(mNode.X - 1, mNode.Y + dy));
                    }
                }
                else
                {
                    if (!IsObstacle(mNode.X + dx, mNode.Y))
                    {
                        if (!IsObstacle(mNode.X + dx, mNode.Y))
                            result.Add(GetNode(mNode.X + dx, mNode.Y));
                        if (IsObstacle(mNode.X, mNode.Y + 1))
                            result.Add(GetNode(mNode.X + dx, mNode.Y + 1));
                        if (IsObstacle(mNode.X, mNode.Y - 1))
                            result.Add(GetNode(mNode.X + dx, mNode.Y - 1));
                    }
                }
            }

            return result;
        }

        private TDPNode GetNode(int mX, int mY) { return _nodes[mX + Offset, mY + Offset]; }
        private IEnumerable<TDPNode> GetAdjacentNodes(int mX, int mY)
        {
            yield return GetNode(mX + -1, mY + -1);
            yield return GetNode(mX + 0, mY + -1);
            yield return GetNode(mX + 1, mY + -1);
            yield return GetNode(mX + 1, mY + 0);
            yield return GetNode(mX + 1, mY + 1);
            yield return GetNode(mX + 0, mY + 1);
            yield return GetNode(mX + -1, mY + 1);
            yield return GetNode(mX + -1, mY + 0);
        }

        private bool IsObstacle(int mX, int mY) { return _obstacles[mX + Offset, mY + Offset]; }
    }
}

// PARTHFIDNGIN GALORE BUG!!!