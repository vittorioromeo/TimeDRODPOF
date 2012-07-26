#region
using System;
using System.Collections.Generic;
using System.Linq;
using VeeTileEngine2012;

#endregion

namespace TimeDRODPOF.TDComponents
{
    [Serializable]
    public class TDCID : Component
    {
        public Action<int> OnCallRecieved;
        public TDCID(params int[] mIDs) { IDs = new List<int>(mIDs); }
        public List<int> IDs { get; set; }

        public void RecieveCall(TDCIDCaller mCaller, int mEffect) { if (OnCallRecieved != null) OnCallRecieved.Invoke(mEffect); }
        public bool SameIDsCondition(Entity mEntity) { return IDs.All(mEntity.GetComponent<TDCID>().IDs.Contains); }
    }
}