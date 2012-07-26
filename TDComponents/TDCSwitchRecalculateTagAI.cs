#region
using System;
using VeeTileEngine2012;

#endregion

namespace TimeDRODPOF.TDComponents
{
    [Serializable]
    public class TDCSwitchRecalculateTagAI : Component
    {
        private readonly TDCRecalculateSprites _recalculateComponent;
        private readonly TDCSwitch _switchComponent;

        public TDCSwitchRecalculateTagAI(TDCSwitch mSwitchComponent, TDCRecalculateSprites mRecalculateComponent, string mOffTag, string mOnTag)
        {
            _switchComponent = mSwitchComponent;
            _recalculateComponent = mRecalculateComponent;
            OffTag = mOffTag;
            OnTag = mOnTag;
        }

        public string OffTag { get; set; }
        public string OnTag { get; set; }

        public override void Added()
        {
            base.Added();

            _recalculateComponent.Tag = _switchComponent.IsOff ? OffTag : OnTag;

            _switchComponent.OnTurnOn += () => { _recalculateComponent.Tag = OnTag; };
            _switchComponent.OnTurnOff += () => { _recalculateComponent.Tag = OffTag; };
        }
    }
}