#region
using System;
using System.Collections.Generic;
using VeeTileEngine2012;

#endregion

namespace TimeDRODPOF.TDComponents
{
    [Serializable]
    public class TDCIDCaller : Component
    {
        private readonly List<Tuple<int, int>> _callTuples;

        public TDCIDCaller(List<int> mTargetIDs, List<int> mTargetEffects)
        {
            _callTuples = new List<Tuple<int, int>>();

            if (mTargetIDs == null || mTargetEffects == null || mTargetIDs.Count != mTargetEffects.Count)
                _callTuples.Add(new Tuple<int, int>(-1, 0));
            else
            {
                for (var i = 0; i < mTargetIDs.Count; i++)
                {
                    var targetID = mTargetIDs[i];
                    var targetEffect = mTargetEffects[i];

                    _callTuples.Add(new Tuple<int, int>(targetID, targetEffect));
                }
            }
        }

        public void SendCalls()
        {
            var components = Field.GetComponents(typeof (TDCID));

            foreach (TDCID hasID in components)
                foreach (var tuple in _callTuples)
                    if (tuple.Item1 != -1 && hasID.IDs.Contains(tuple.Item1))
                        hasID.RecieveCall(this, tuple.Item2);
        }
    }
}