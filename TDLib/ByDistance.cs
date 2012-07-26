#region
using System;
using System.Collections.Generic;
using VeeTileEngine2012;

#endregion

namespace TimeDRODPOF.TDLib
{
    public class ByDistance : IComparer<Entity>
    {
        private readonly int _checkX, _checkY;
        private readonly Func<int, int, int, int, double> _distanceFunction;

        public ByDistance(Func<int, int, int, int, double> mDistanceFunction, int mCheckX, int mCheckY)
        {
            _distanceFunction = mDistanceFunction;
            _checkX = mCheckX;
            _checkY = mCheckY;
        }

        #region IComparer<Entity> Members
        public int Compare(Entity a, Entity b) { return _distanceFunction(a.X, a.Y, _checkX, _checkY).CompareTo(_distanceFunction(b.X, b.Y, _checkX, _checkY)); }
        #endregion
    }
}