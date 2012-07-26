using System;
using TimeDRODPOF.TDLib;
using VeeTileEngine2012;

namespace TimeDRODPOF.TDComponents
{
    [Serializable]
    public class TDCTrapdoor : Component
    {
        private readonly bool _isBroken;
        private readonly bool _isOnWater;
        private readonly TDCSwitch _switchComponent;

        public TDCTrapdoor(bool mIsOnWater, bool mIsBroken, TDCSpecialSquare mSpecialSquareComponent, TDCSwitch mSwitchComponent)
        {
            _isOnWater = mIsOnWater;
            _isBroken = mIsBroken;
            _switchComponent = mSwitchComponent;

            mSpecialSquareComponent.OnMoveFromAllowed += SpecialSquareMove;
        }

        private bool SpecialSquareMove(Entity mEntity, int mNextX, int mNextY, bool mNextSuccess, out bool mAbort)
        {
            mAbort = false;

            if (mEntity.HasTag(TDLTags.WeightHigh) || (_isBroken && mEntity.HasTag(TDLTags.WeightLow)))
                if (!_switchComponent.IsOff && mNextSuccess)
                {
                    Entity.Destroy();
                    TDLFactory.Tile = Entity.Field.GetTile(X, Y);
                    Entity.Field.AddEntity(!_isOnWater ? TDLFactory.Pit() : TDLFactory.Water());
                    TDLSounds.Play("SoundTrapdoor");
                }

            return true;
        }
    }
}