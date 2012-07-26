using System;
using VeeTileEngine2012;

namespace TimeDRODPOF.TDComponents
{
    [Serializable]
    public class TDCOrthogonalSquare : Component
    {
        private readonly TDCSwitch _switchComponent;

        public TDCOrthogonalSquare(TDCSpecialSquare mSpecialSquareComponent, TDCSwitch mSwitchComponent)
        {
            _switchComponent = mSwitchComponent;

            mSpecialSquareComponent.OnMoveFromAllowed += SpecialSquareMove;
            mSpecialSquareComponent.OnMoveToAllowed += SpecialSquareMove;
        }

        private bool SpecialSquareMove(Entity mEntity, int mNextX, int mNextY, bool mNextSuccess, out bool mAbort)
        {
            mAbort = false;
            return _switchComponent.IsOff || (mNextX == 0 || mNextY == 0);
        }
    }
}