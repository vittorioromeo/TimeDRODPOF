using System;
using System.Linq;
using TimeDRODPOF.TDGame;
using TimeDRODPOF.TDLib;
using VeeTileEngine2012;

namespace TimeDRODPOF.TDComponents
{
    [Serializable]
    public class TDCDoor : Component
    {
        #region DoorType enum
        public enum DoorType
        {
            Green,
            Red,
            Blue,
            Yellow
        };
        #endregion

        private readonly DoorType _doorType;

        [NonSerialized] private readonly TDGGame _game;
        private readonly bool _isOff;
        private readonly TDCRecalculateSprites _recalculateSpritesComponent;
        private readonly TDCSwitch _switchComponent;
        private bool _isRecalculationNeeded;
        private bool _wasTriggered;

        public TDCDoor(DoorType mDoorType, bool mIsOff, TDGGame mGame, TDCSwitch mSwitchComponent, TDCRecalculateSprites mRecalculateSprites)
        {
            _doorType = mDoorType;

            _game = mGame;
            _isOff = mIsOff;

            _switchComponent = mSwitchComponent;
            _recalculateSpritesComponent = mRecalculateSprites;

            _switchComponent.OnTurnOff += SetRecalculationNeeded;
            _switchComponent.OnTurnOn += SetRecalculationNeeded;
        }

        private void SetRecalculationNeeded() { _isRecalculationNeeded = true; }
        private void TryRecalculation() { TDLMethods.TryRecalculation(ref _isRecalculationNeeded, _recalculateSpritesComponent); }

        private void GreenDoorCheck()
        {
            if (_game.IsRoomClear())
                if (_isOff) _switchComponent.TurnOn();
                else _switchComponent.TurnOff();
        }

        public override void Added()
        {
            base.Added();

            Field.OnLoadChecks += _recalculateSpritesComponent.RecalculateNearbySprites;

            if (_doorType == DoorType.Green)
            {
                Field.TurnActions[TDLTurnActions.End] += GreenDoorCheck;
                if (_game.IsRoomClear()) _switchComponent.Toggle();
            }
            else if (_doorType == DoorType.Blue)
            {
                if (_game.IsLevelClear)
                {
                    if (_isOff) _switchComponent.TurnOn();
                    else _switchComponent.TurnOff();

                    Field.TurnActions[TDLTurnActions.Start] += _recalculateSpritesComponent.RecalculateNearbySprites;
                }
            }

            Field.TurnActions[TDLTurnActions.End] += TryRecalculation;
        }

        public override void NextTurn()
        {
            base.NextTurn();

            if (_doorType == DoorType.Red)
            {
                if (_wasTriggered || Field.GetEntitiesByTag(TDLTags.Trapdoor).Any()) return;
                _switchComponent.Toggle();

                TDLSounds.Play("SoundRedDoor");
                _wasTriggered = true;
            }
        }
    }
}