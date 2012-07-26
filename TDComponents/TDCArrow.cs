using System;
using VeeTileEngine2012;

namespace TimeDRODPOF.TDComponents
{
    [Serializable]
    public class TDCArrow : Component
    {
        private readonly TDCDirection _directionComponent;
        private readonly TDCSwitch _switchComponent;

        public TDCArrow(TDCSpecialSquare mSpecialSquareComponent, TDCDirection mDirectionComponent, TDCSwitch mSwitchComponent)
        {
            _directionComponent = mDirectionComponent;
            _switchComponent = mSwitchComponent;

            mSpecialSquareComponent.OnMoveFromAllowed += SpecialSquareMove;
            mSpecialSquareComponent.OnMoveToAllowed += SpecialSquareMove;
        }

        private bool SpecialSquareMove(Entity mEntity, int mNextX, int mNextY, bool mNextSuccess, out bool mAbort)
        {
            mAbort = false;
            if (_switchComponent.IsOff) return true;

            var xy = _directionComponent.DirectionVector;

            if (xy.X == 0 || xy.Y == 0) return !((mNextX != xy.X && mNextX == -xy.X) || (mNextY != xy.Y && mNextY == -xy.Y));
            return !((mNextX == 0 || mNextX == -xy.X) && (mNextY == 0 || mNextY == -xy.Y));
        }
    }
}