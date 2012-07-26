#region
using System;
using VeeTileEngine2012;

#endregion

namespace TimeDRODPOF.TDComponents
{
    [Serializable]
    public class TDCDirection : Component
    {
        private int _direction;

        public TDCDirection(int mDirection) { Direction = mDirection; }

        public int Direction { get { return _direction; } set { _direction = WrapDirection(value); } }
        public string DirectionString { get { return GetDirectionString(_direction); } }
        public TDVector2 DirectionVector { get { return GetDirectionVector(_direction); } }

        public static int WrapDirection(int mDirection)
        {
            if (mDirection > 7) return 0;
            if (mDirection < 0) return 7;
            return mDirection;
        }
        public static string GetDirectionString(int mDirection)
        {
            switch (mDirection)
            {
                case 0:
                    return "n";
                case 1:
                    return "ne";
                case 2:
                    return "e";
                case 3:
                    return "se";
                case 4:
                    return "s";
                case 5:
                    return "sw";
                case 6:
                    return "w";
                case 7:
                    return "nw";
            }

            return "n";
        }
        public static TDVector2 GetDirectionVector(int mDirection)
        {
            switch (mDirection)
            {
                case 0:
                    return new TDVector2(0, -1);
                case 1:
                    return new TDVector2(+1, -1);
                case 2:
                    return new TDVector2(+1, 0);
                case 3:
                    return new TDVector2(+1, +1);
                case 4:
                    return new TDVector2(0, +1);
                case 5:
                    return new TDVector2(-1, +1);
                case 6:
                    return new TDVector2(-1, 0);
                case 7:
                    return new TDVector2(-1, -1);
            }

            return TDVector2.Zero;
        }
        public static int GetDirection(TDVector2 mNextXY)
        {
            if (mNextXY.X == 0 &&
                mNextXY.Y == 0) return -1;

            switch (mNextXY.X)
            {
                case 0:
                    if (mNextXY.Y ==
                        -1) return 0;
                    if (mNextXY.Y == 1) return 4;
                    break;
                case -1:
                    if (mNextXY.Y ==
                        -1) return 7;
                    if (mNextXY.Y == 1) return 5;
                    if (mNextXY.Y == 0) return 6;
                    break;
                case 1:
                    if (mNextXY.Y ==
                        -1) return 1;
                    if (mNextXY.Y == 1) return 3;
                    if (mNextXY.Y == 0) return 2;
                    break;
            }

            return 0;
        }
        public static int GetTurningDirection(int mStartDirection, int mEndDirection)
        {
            if (mEndDirection < 0 || mEndDirection > 7) return WrapDirection(mStartDirection);
            if (mStartDirection == mEndDirection) return WrapDirection(mStartDirection);

            var tempRight = 0;
            var tempLeft = 0;

            var tempStart = mStartDirection;
            while (tempStart != mEndDirection)
            {
                tempStart = WrapDirection(tempStart + 1);
                tempRight++;
            }

            tempStart = mStartDirection;
            while (tempStart != mEndDirection)
            {
                tempStart = WrapDirection(tempStart - 1);
                tempLeft++;
            }

            if (tempRight < tempLeft) return WrapDirection(mStartDirection + 1);
            if (tempRight >= tempLeft) return WrapDirection(mStartDirection - 1);
            return WrapDirection(mStartDirection);
        }
        public static int GetDirectionTowards(int mX, int mY, int mTargetX, int mTargetY)
        {
            var x = -(mX - mTargetX);
            if (x != 0) x /= Math.Abs(mX - mTargetX);
            var y = -(mY - mTargetY);
            if (y != 0) y /= Math.Abs(mY - mTargetY);
            return GetDirection(new TDVector2(x, y));
        }
    }
}