using System.Collections.Generic;
using System.Linq;
using TimeDRODPOF.TDLib;
using VeeTileEngine2012;

namespace TimeDRODPOF.TDPathfinding
{
    public class TDPPathmap
    {
        private readonly HashSet<Entity> _entities;
        private readonly Field _field;
        private TDPNode[,] _nodes;

        public TDPPathmap(Field mField)
        {
            _field = mField;
            _entities = new HashSet<Entity>();

            _field.TurnActions[TDLTurnActions.Start] += () => WasUpdatedThisTurn = false;

            InitializeNodes();
        }

        public bool WasUpdatedThisTurn { get; private set; }

        private void InitializeNodes()
        {
            _nodes = new TDPNode[_field.Width,_field.Height];

            for (var iY = 0; iY < _field.Height; iY++)
                for (var iX = 0; iX < _field.Width; iX++)
                    _nodes[iX, iY] = new TDPNode(iX, iY);
        }
        private void RefreshNodes()
        {
            for (var iY = 0; iY < _field.Height; iY++)
                for (var iX = 0; iX < _field.Width; iX++)
                    GetNode(iX, iY).Refresh();
        }

        public void CalculatePathmap()
        {
            RefreshNodes();

            var openNodes = new Queue<TDPNode>();
            foreach (var entity in _entities)
            {
                var node = GetNode(entity.X, entity.Y);
                openNodes.Enqueue(node);
                node.IsClosed = true;
            }

            while (openNodes.Count > 0)
            {
                var currentNode = openNodes.Dequeue();

                var x = currentNode.X;
                var y = currentNode.Y;
                var width = _field.Width - 1;
                var height = _field.Height - 1;
                var neighbors = new TDPNode[8];

                if (x < width)
                {
                    neighbors[0] = GetNode(x + 1, y + 0);
                    if (y < height) neighbors[4] = GetNode(x + 1, y + 1);
                    if (y > 0) neighbors[6] = GetNode(x + 1, y + -1);
                }
                if (x > 0)
                {
                    neighbors[1] = GetNode(x + -1, y + 0);
                    if (y < height) neighbors[5] = GetNode(x - 1, y + 1);
                    if (y > 0) neighbors[7] = GetNode(x - 1, y + -1);
                }
                if (y < height) neighbors[2] = GetNode(x + 0, y + 1);
                if (y > 0) neighbors[3] = GetNode(x + 0, y + -1);

                for (var i = 0; i < neighbors.Length; i++)
                {
                    var neighbor = neighbors[i];
                    if (neighbor == null) continue;
                    if (neighbor.IsClosed) continue;
                    if (IsObstacle(neighbor.X, neighbor.Y))
                    {
                        neighbor.IsObstacle = true;
                        continue;
                    }


                    neighbor.Parent = currentNode;
                    neighbor.G = i > 4 ? neighbor.Parent.G + 14 : neighbor.Parent.G + 10;
                    neighbor.IsVisited = true;
                    neighbor.IsClosed = true;
                    openNodes.Enqueue(neighbor);
                }
            }

            WasUpdatedThisTurn = true;
        }

        public void AddEntity(Entity mEntity) { _entities.Add(mEntity); }
        public void RemoveEntity(Entity mEntity) { _entities.Remove(mEntity); }

        private TDPNode GetNode(int mX, int mY) { return _nodes[mX, mY]; }

        private IEnumerable<TDPNode> GetAdjacentValidNodes(int mX, int mY, string mFlockTag, bool mStepOnTarget, bool mIncludeStartNode, ICollection<Entity> mIgnoreEntities)
        {
            if (mIncludeStartNode) yield return GetNode(mX, mY);

            for (var iY = -1; iY < 2; iY++)
                for (var iX = -1; iX < 2; iX++)
                {
                    var x = mX + iX;
                    var y = mY + iY;

                    if (x == mX && y == mY) continue;
                    if (!_field.IsTileValid(x, y)) continue;

                    var foundTarget = mStepOnTarget && _entities.Any(entity => entity.X == x && entity.Y == y);

                    if ((!_nodes[x, y].IsVisited || IsObstacle(x, y)) && !foundTarget) continue;
                    if (!foundTarget && _field.HasEntityByTag(x, y, mFlockTag, entity => !mIgnoreEntities.Contains(entity))) continue;

                    yield return GetNode(x, y);
                }
        }

        public TDPNode GetBestAdjacentNode(int mX, int mY, string mFlockTag, bool mStepOnTarget = true,
                                           bool mIncludeStartNode = true, HashSet<Entity> mIgnoreEntities = default(HashSet<Entity>)) { return GetAdjacentValidNodes(mX, mY, mFlockTag, mStepOnTarget, mIncludeStartNode, mIgnoreEntities).OrderBy(x => x.F).FirstOrDefault(); }
        public TDPNode GetWorstAdjacentNode(int mX, int mY, string mFlockTag, bool mStepOnTarget = false,
                                            bool mIncludeStartNode = true, HashSet<Entity> mIgnoreEntities = default(HashSet<Entity>)) { return GetAdjacentValidNodes(mX, mY, mFlockTag, mStepOnTarget, mIncludeStartNode, mIgnoreEntities).OrderByDescending(x => x.F).FirstOrDefault(); }

        private bool IsObstacle(int mX, int mY) { return _field.HasEntityByTag(mX, mY, TDLTags.GroundPathmapObstacle); }
    }
}