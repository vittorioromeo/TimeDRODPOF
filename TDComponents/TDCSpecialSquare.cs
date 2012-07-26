#region
using System;
using VeeTileEngine2012;

#endregion

namespace TimeDRODPOF.TDComponents
{
    [Serializable]
    public class TDCSpecialSquare : Component
    {
        #region Delegates
        public delegate bool SpecialSquareEvent(Entity mEntity, int mNextX, int mNextY, bool mSuccess, out bool mAbort);
        #endregion

        public SpecialSquareEvent OnMoveFromAllowed, OnMoveToAllowed;
        public TDCSpecialSquare(int mPriority) { Priority = mPriority; }

        public int Priority { get; private set; }

        public bool InvokeOnMoveFromAllowed(Entity mEntity, int mNextX, int mNextY, bool mSuccess, out bool mAbort)
        {
            mAbort = false;
            var handler = OnMoveFromAllowed;
            return handler == null || handler(mEntity, mNextX, mNextY, mSuccess, out mAbort);
        }
        public bool InvokeOnMoveToAllowed(Entity mEntity, int mNextX, int mNextY, bool mSuccess, out bool mAbort)
        {
            mAbort = false;
            var handler = OnMoveToAllowed;
            return handler == null || handler(mEntity, mNextX, mNextY, mSuccess, out mAbort);
        }
    }
}