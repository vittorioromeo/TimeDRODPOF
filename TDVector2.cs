using System;

namespace TimeDRODPOF
{
    [Serializable]
    public struct TDVector2
    {
        private readonly int _x;
        private readonly int _y;

        public TDVector2(int mX, int mY)
        {
            _x = mX;
            _y = mY;
        }

        public int X { get { return _x; } }
        public int Y { get { return _y; } }

        public static TDVector2 Zero { get { return new TDVector2(0, 0); } }

        public static TDVector2 operator -(TDVector2 mFirst) { return new TDVector2(-mFirst.X, -mFirst.Y); }
        public static TDVector2 operator +(TDVector2 mFirst, TDVector2 mSecond) { return new TDVector2(mFirst.X + mSecond.X, mFirst.Y + mSecond.Y); }
        public static TDVector2 operator -(TDVector2 mFirst, TDVector2 mSecond) { return new TDVector2(mFirst.X - mSecond.X, mFirst.Y - mSecond.Y); }
    }
}