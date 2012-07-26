#region
using System;
using System.Collections.Generic;
using System.Linq;
using SFMLStart.Utilities;
using VeeTileEngine2012;

#endregion

namespace TimeDRODPOF.TDComponents
{
    [Serializable]
    public class TDCMovement : Component
    {
        #region MovementType enum
        public enum MovementType
        {
            Direct,
            BeelineNormal,
            BeelineSmart,
            FlexibleNormal
        }
        #endregion

        private readonly List<TDVector2> _obstaclePositions;

        private bool _ignoreReverseThisTurn;
        private bool _skipMovementThisTurn;
        private bool _success;

        public TDCMovement(MovementType mMovement, string[] mAllowedTags, string[] mObstacleTags, string[] mExceptionTags)
        {
            IgnoreEntities = new HashSet<Entity>();
            _obstaclePositions = new List<TDVector2>();

            Movement = mMovement;
            AllowedTags = mAllowedTags;
            ObstacleTags = mObstacleTags;
            ExceptionTags = mExceptionTags;
        }

        public MovementType Movement { get; set; }
        public int NextX { get; private set; }
        public int NextY { get; private set; }
        public int TargetX { get; private set; }
        public int TargetY { get; private set; }
        public bool IsReverse { get; set; }
        public bool PrefersHorizontal { get; set; }
        public string[] AllowedTags { get; set; }
        public string[] ObstacleTags { get; set; }
        public string[] ExceptionTags { get; set; }
        public HashSet<Entity> IgnoreEntities { get; private set; }
        public Action OnMovementFail { get; set; }
        public Action OnMovementSuccess { get; set; }
        public Action<int, int> OnMovedOutsideBounds { get; set; }

        private void TryMovement()
        {
            switch (Movement)
            {
                case MovementType.Direct:
                    DirectMovement();
                    break;
                case MovementType.BeelineNormal:
                    BeelineNormalMovement();
                    break;
                case MovementType.BeelineSmart:
                    BeelineSmartMovement();
                    break;
                case MovementType.FlexibleNormal:
                    FlexibleNormalMovement();
                    break;
            }
        }

        private bool CheckSpecialSquares(bool mNextSuccess)
        {
            var abort = false;
            var componentsToTest = new List<Tuple<TDCSpecialSquare, bool>>();
            _obstaclePositions.Clear();

            foreach (TDCSpecialSquare component in Field.GetComponents(X, Y, typeof (TDCSpecialSquare)))
                componentsToTest.Add(new Tuple<TDCSpecialSquare, bool>(component, true));

            if (Field.IsTileValid(X + NextX, Y + NextY))
                foreach (TDCSpecialSquare component in Field.GetComponents(X + NextX, Y + NextY, typeof (TDCSpecialSquare)))
                    componentsToTest.Add(new Tuple<TDCSpecialSquare, bool>(component, false));

            componentsToTest.Sort((a, b) => a.Item1.Priority.CompareTo(b.Item1.Priority));

            foreach (var component in componentsToTest)
            {
                if (component.Item2)
                {
                    if (!component.Item1.InvokeOnMoveFromAllowed(Entity, NextX, NextY, mNextSuccess, out abort))
                    {
                        _obstaclePositions.Add(new TDVector2(X + NextX, Y + NextY));
                        mNextSuccess = false;
                    }
                }
                else
                {
                    if (!component.Item1.InvokeOnMoveToAllowed(Entity, NextX, NextY, mNextSuccess, out abort))
                    {
                        _obstaclePositions.Add(new TDVector2(X + NextX, Y + NextY));
                        mNextSuccess = false;
                    }
                }
            }

            /*
            foreach (var component in new List<TDCSpecialSquare>(Field.GetComponents(X, Y, typeof (TDCSpecialSquare)).Cast<TDCSpecialSquare>()).OrderBy(x => x.Priority))
                if (!component.InvokeOnMoveFromAllowed(Entity, NextX, NextY, mNextSuccess, out abort))
                {
                    _obstaclePositions.Add(new Vector2i(X + NextX, Y + NextY));
                    mNextSuccess = false;
                }

            if (Field.IsTileValid(X + NextX, Y + NextY))
                foreach (var component in new List<TDCSpecialSquare>(Field.GetComponents(X + NextX, Y + NextY, typeof (TDCSpecialSquare)).Cast<TDCSpecialSquare>()).OrderBy(x => x.Priority))
                    if (!component.InvokeOnMoveToAllowed(Entity, NextX, NextY, mNextSuccess, out abort))
                    {
                        _obstaclePositions.Add(new Vector2i(X + NextX, Y + NextY));
                        mNextSuccess = false;
                    }*/

            return abort;
        }

        private void DirectMovement()
        {
            var nextSuccess = IsNextAllowed(Field, X + NextX, Y + NextY, AllowedTags);
            if (IsNextObstacle(Field, X + NextX, Y + NextY, ObstacleTags, IgnoreEntities, ExceptionTags)) nextSuccess = false;
            if (CheckSpecialSquares(nextSuccess)) nextSuccess = false;
            if (IsNextObstaclePosition(_obstaclePositions, X + NextX, Y + NextY)) nextSuccess = false;
            _success = nextSuccess;
        }
        private void BeelineNormalMovement()
        {
            var xInts = new int[3];
            var yInts = new int[3];

            xInts[0] = NextX;
            yInts[0] = NextY;

            if (!PrefersHorizontal)
            {
                xInts[1] = 0;
                yInts[1] = NextY;
                xInts[2] = NextX;
                yInts[2] = 0;
            }
            else
            {
                xInts[1] = NextX;
                yInts[1] = 0;
                xInts[2] = 0;
                yInts[2] = NextY;
            }

            BeelineCommonMovement(xInts, yInts);
        }
        private void BeelineCommonMovement(IList<int> xInts, IList<int> yInts)
        {
            for (var i = 0; i < 3; i++)
            {
                var nextSuccess = true;
                NextX = xInts[i];
                NextY = yInts[i];
                if (!IsNextAllowed(Field, X + NextX, Y + NextY, AllowedTags)) nextSuccess = false;
                if (IsNextObstacle(Field, X + NextX, Y + NextY, ObstacleTags, IgnoreEntities, ExceptionTags)) nextSuccess = false;
                if (CheckSpecialSquares(nextSuccess))
                {
                    _success = false;
                    break;
                }
                if (IsNextObstaclePosition(_obstaclePositions, X + NextX, Y + NextY)) nextSuccess = false;
                _success = nextSuccess;
                if (_success) break;
            }
        }
        private void BeelineSmartMovement()
        {
            var xInts = new int[3];
            var yInts = new int[3];

            xInts[0] = NextX;
            yInts[0] = NextY;

            if (Math.Abs(X - TargetX) >= Math.Abs(Y - TargetY))
            {
                xInts[1] = 0;
                yInts[1] = NextY;
                xInts[2] = NextX;
                yInts[2] = 0;
            }
            else
            {
                xInts[1] = NextX;
                yInts[1] = 0;
                xInts[2] = 0;
                yInts[2] = NextY;
            }

            BeelineCommonMovement(xInts, yInts);
        }
        private void FlexibleNormalMovement()
        {
            var xInts = new int[3];
            var yInts = new int[3];

            xInts[0] = NextX;
            yInts[0] = NextY;

            if (NextX != 0 &&
                NextY != 0)
            {
                if (Math.Abs(X - TargetX) >= Math.Abs(Y - TargetY))
                {
                    xInts[1] = 0;
                    yInts[1] = NextY;
                    xInts[2] = NextX;
                    yInts[2] = 0;
                }
                else
                {
                    xInts[1] = NextX;
                    yInts[1] = 0;
                    xInts[2] = 0;
                    yInts[2] = NextY;
                }
            }
            else if (NextX == 0)
            {
                if (NextY == -1)
                {
                    xInts[1] = -1;
                    yInts[1] = -1;
                    xInts[2] = 1;
                    yInts[2] = -1;
                }
                else if (NextY == 1)
                {
                    xInts[1] = -1;
                    yInts[1] = 1;
                    xInts[2] = 1;
                    yInts[2] = 1;
                }
            }
            else if (NextY == 0)
            {
                if (NextX == -1)
                {
                    xInts[1] = -1;
                    yInts[1] = -1;
                    xInts[2] = -1;
                    yInts[2] = 1;
                }
                else if (NextX == 1)
                {
                    xInts[1] = 1;
                    yInts[1] = -1;
                    xInts[2] = 1;
                    yInts[2] = 1;
                }
            }

            BeelineCommonMovement(xInts, yInts);
        }

        public override void Refresh()
        {
            base.Refresh();

            _obstaclePositions.Clear();
            _success = true;
            TargetX = X;
            TargetY = Y;
            NextX = NextY = 0;
        }
        public override void NextTurn()
        {
            base.NextTurn();

            if (_skipMovementThisTurn)
            {
                _skipMovementThisTurn = false;
                return;
            }

            if (TargetX == X && TargetY == Y) return;

            var nextXY = GetNextXY(Entity, TargetX, TargetY);
            var firstNextX = NextX = nextXY.X;
            var firstNextY = NextY = nextXY.Y;

            if (_ignoreReverseThisTurn)
                _ignoreReverseThisTurn = false;
            else if (IsReverse)
            {
                NextX *= -1;
                NextY *= -1;
            }

            if (NextX == 0 && NextY == 0) return;

            TryMovement();

            if (_success)
            {
                Entity.Move(X + NextX, Y + NextY);
                OnMovementSuccess.SafeInvoke();
            }
            else
            {
                OnMovementFail.SafeInvoke();
                if (!Field.IsTileValid(X + firstNextX, Y + firstNextY))
                    if (OnMovedOutsideBounds != null)
                        OnMovedOutsideBounds(X + firstNextX, Y + firstNextY);
            }
        }

        public void SkipMovementThisTurn() { _skipMovementThisTurn = true; }
        public void IgnoreReverseThisTurn() { _ignoreReverseThisTurn = true; }
        public void SetTarget(int mX, int mY)
        {
            TargetX = mX;
            TargetY = mY;
        }

        public static TDVector2 GetNextXY(Entity mEntity, int mTargetX, int mTargetY)
        {
            int nextX = 0, nextY = 0;

            if (mEntity.X < mTargetX) nextX = 1;
            else if (mEntity.X > mTargetX) nextX = -1;

            if (mEntity.Y < mTargetY) nextY = 1;
            else if (mEntity.Y > mTargetY) nextY = -1;

            return new TDVector2(nextX, nextY);
        }
        private static bool TileHasEntityIgnore(Field mField, int mX, int mY, string mTag, IEnumerable<Entity> mIgnoreEntities, IEnumerable<string> mExceptionTags)
        {
            var tempTile = mField.GetTile(mX, mY);
            if (tempTile == null) return false;

            if (mIgnoreEntities == null) mIgnoreEntities = new Entity[] {};
            if (mExceptionTags == null) mExceptionTags = new string[] {};

            return mField.GetEntitiesByTag(mX, mY, mTag).Where(entity => !mExceptionTags.Any(entity.HasTag)).Any(entity => !mIgnoreEntities.Contains(entity));
        }
        public static bool IsNextAllowed(Field mField, int mX, int mY, IEnumerable<string> mAllowedTags) { return mField.IsTileValid(mX, mY) && mAllowedTags.Any(x => mField.HasEntityByTag(mX, mY, x)); }
        public static bool IsNextObstacle(Field mField, int mX, int mY, IEnumerable<string> mObstacleTags,
                                          IEnumerable<Entity> mIgnoreEntities, IEnumerable<string> mExceptionTags) { return mObstacleTags.Any(x => TileHasEntityIgnore(mField, mX, mY, x, mIgnoreEntities, mExceptionTags)); }
        private static bool IsNextObstaclePosition(IEnumerable<TDVector2> mObstaclePositions, int mX, int mY)
        {
            return mObstaclePositions != null &&
                   mObstaclePositions.Any(vector => vector.X == mX && vector.Y == mY);
        }
    }
}