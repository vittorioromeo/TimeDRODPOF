using System;
using TimeDRODPOF.TDGame;
using VeeTileEngine2012;

namespace TimeDRODPOF.TDComponents
{
    [Serializable]
    public class TDCRoomSwitch : Component
    {
        [NonSerialized] private readonly TDGGame _game;
        private readonly TDCMovement _movementComponent;

        public TDCRoomSwitch(TDGGame mGame, TDCMovement mMovementComponent)
        {
            _game = mGame;
            _movementComponent = mMovementComponent;

            _movementComponent.OnMovedOutsideBounds += CheckMovement;
        }

        private void CheckMovement(int mX, int mY) { _game.RoomSwitchMovement(Entity, mX, mY); }
    }
}