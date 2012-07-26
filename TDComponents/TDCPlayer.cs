using System;
using TimeDRODPOF.TDGame;
using TimeDRODPOF.TDLib;
using VeeTileEngine2012;

namespace TimeDRODPOF.TDComponents
{
    [Serializable]
    public class TDCPlayer : Component
    {
        [NonSerialized] private readonly TDGGame _game;
        private readonly TDCDirection _directionComponent;
        private readonly TDCMovement _movementComponent;
        private readonly TDCWielder _wielderComponent;

        public TDCPlayer(TDGGame mGame, TDCMovement mMovementComponent, TDCDirection mDirectionComponent, TDCWielder mWielderComponent)
        {
            _game = mGame;
            _movementComponent = mMovementComponent;
            _directionComponent = mDirectionComponent;
            _wielderComponent = mWielderComponent;
        }

        public override void NextTurn()
        {
            base.NextTurn();

            var input = _game.LastInput;

            if (input >= 0 && input <= 7)
            {
                var movementVector = TDCDirection.GetDirectionVector(input);
                _movementComponent.SetTarget(X + movementVector.X, Y + movementVector.Y);
                if (_wielderComponent.IsSheated) _directionComponent.Direction = input;
                TDLSounds.Play("SoundStep1");
            }
            else if (input != -1)
            {
                if (input == 8) _wielderComponent.SetDirection(_directionComponent.Direction - 1);
                if (input == 9) _wielderComponent.SetDirection(_directionComponent.Direction + 1);
                TDLSounds.Play("SoundSwing");
            }
        }
    }
}