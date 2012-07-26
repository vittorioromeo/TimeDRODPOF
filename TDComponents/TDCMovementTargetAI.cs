#region
using System;
using TimeDRODPOF.TDGame;
using TimeDRODPOF.TDLib;
using TimeDRODPOF.TDPathfinding;
using VeeTileEngine2012;

#endregion

namespace TimeDRODPOF.TDComponents
{
    [Serializable]
    public class TDCMovementTargetAI : Component
    {
        [NonSerialized] private readonly TDGInstance _instance;
        private readonly TDCDirection _directionComponent;
        private readonly TDCMovement _movementComponent;
        private readonly TDCTarget _targetComponent;

        public TDCMovementTargetAI(TDGInstance mInstance, TDCMovement mMovementComponent, TDCTarget mTargetComponent, TDCDirection mDirectionComponent,
                                   string mPathmapName, bool mSetsDirection = true, bool mIsPathfinder = false, bool mIsObstinate = true, bool mUsesPathmap = true,
                                   bool mIncludesStartNode = false)
        {
            _instance = mInstance;
            _movementComponent = mMovementComponent;
            _targetComponent = mTargetComponent;
            _directionComponent = mDirectionComponent;
            SetsDirection = mSetsDirection;
            IsPathfinder = mIsPathfinder;
            IsObstinate = mIsObstinate;
            UsesPathmap = mUsesPathmap;
            PathmapName = mPathmapName;
            IncludesStartNode = mIncludesStartNode;
        }

        public bool SetsDirection { get; set; }
        public bool IsPathfinder { get; set; }
        public bool IsObstinate { get; set; }
        public bool UsesPathmap { get; set; }
        public string PathmapName { get; set; }
        public bool IncludesStartNode { get; set; }
        public TDVector2 LastNextXY { get; private set; }
        public TDVector2 LastNodeXY { get; private set; }

        public override void NextTurn()
        {
            base.NextTurn();

            TDPNode nextNode = null;
            var nextXY = TDVector2.Zero;

            if (_targetComponent.Target != null)
                nextXY = TDCMovement.GetNextXY(Entity, _targetComponent.Target.X, _targetComponent.Target.Y);

            if (IsPathfinder)
            {
                if (UsesPathmap)
                {
                    var pathmap = _instance.GetPathmap(PathmapName);
                    if (!pathmap.WasUpdatedThisTurn) pathmap.CalculatePathmap();

                    if (_movementComponent.IsReverse)
                    {
                        nextNode = pathmap.GetWorstAdjacentNode(X, Y, TDLTags.Solid, mIncludeStartNode: IncludesStartNode, mIgnoreEntities: _movementComponent.IgnoreEntities);
                        if (nextNode != null) _movementComponent.IgnoreReverseThisTurn(); // this class takes care of it!
                    }
                    else nextNode = pathmap.GetBestAdjacentNode(X, Y, TDLTags.Solid, mIncludeStartNode: IncludesStartNode, mIgnoreEntities: _movementComponent.IgnoreEntities);
                }
                else
                {
                    if (_targetComponent.Target == null) return;
                    nextNode = _instance.JumpPointSearch(X, Y, _targetComponent.Target.X, _targetComponent.Target.Y);
                }

                if (nextNode != null)
                {
                    nextXY = TDCMovement.GetNextXY(Entity, nextNode.X, nextNode.Y);
                    LastNodeXY = new TDVector2(nextNode.X, nextNode.Y);
                }
                else if (!IsObstinate) return;
            }

            if (SetsDirection)
            {
                if (_movementComponent.IsReverse && (!IsPathfinder || nextNode == null)) _directionComponent.Direction = TDCDirection.GetDirection(-nextXY);
                else _directionComponent.Direction = TDCDirection.GetDirection(nextXY);
            }

            LastNextXY = nextXY;

            _movementComponent.SetTarget(X + nextXY.X, Y + nextXY.Y);

            if (Entity.GetComponent<TDCKiller>() != null)
                foreach (var tag in _targetComponent.TargetTags)
                    foreach (var entity in Field.GetEntitiesByTag(tag))
                        _movementComponent.IgnoreEntities.Add(entity);
        }
    }
}