using System;

namespace TimeDRODPOF.TDLib
{
    public static class TDLDistances
    {
        public static double EuclideanDistanceSquared(int mX1, int mY1, int mX2, int mY2) { return Math.Pow(mX1 - mX2, 2) + Math.Pow(mY1 - mY2, 2); }
        public static double ManhattanDistance(int mX1, int mY1, int mX2, int mY2) { return Math.Abs(mX2 - mX1) + Math.Abs(mY2 - mY1); }
        public static int ChebyshevDistance(int mX1, int mY1, int mX2, int mY2) { return Math.Max(Math.Abs(mX2 - mX1), Math.Abs(mY2 - mY1)); }
    }
}