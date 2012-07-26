using System;
using VeeTileEngine2012;

namespace TimeDRODPOF.TDComponents
{
    [Serializable]
    public class TDCWielderTargetAI : Component
    {
        private readonly TDCDirection _directionComponent;
        private readonly TDCMovement _movementComponent;
        private readonly TDCTarget _targetComponent;
        private readonly TDCWielder _wielderComponent;

        public TDCWielderTargetAI(TDCTarget mTargetComponent, TDCMovement mMovementComponent, TDCDirection mDirectionComponent,
                                  TDCWielder mWielderComponent)
        {
            _targetComponent = mTargetComponent;
            _movementComponent = mMovementComponent;
            _directionComponent = mDirectionComponent;
            _wielderComponent = mWielderComponent;
        }

        public override void NextTurn()
        {
            base.NextTurn();

            if (_targetComponent.Target == null) return;
            var preferredDirection = TDCDirection.GetDirection(_targetComponent.TargetNextXY);

            if (_wielderComponent.IsSheated || _directionComponent.Direction == preferredDirection) return;

            _movementComponent.SkipMovementThisTurn();
            _wielderComponent.SetDirection(TDCDirection.GetTurningDirection(_directionComponent.Direction, preferredDirection));
        }
    }
}