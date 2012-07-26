#region
using System;
using VeeTileEngine2012;

#endregion

namespace TimeDRODPOF.TDComponents
{
    [Serializable]
    public class TDCIDSwitchAI : Component
    {
        private readonly TDCID _idComponent;
        private readonly TDCSwitch _switchComponent;

        public TDCIDSwitchAI(TDCSwitch mSwitchComponent, TDCID mIDComponent)
        {
            _switchComponent = mSwitchComponent;
            _idComponent = mIDComponent;
        }

        public override void Added()
        {
            base.Added();

            _idComponent.OnCallRecieved += Called;
        }

        private void Called(int mEffect)
        {
            switch (mEffect)
            {
                case 0:
                    _switchComponent.Toggle();
                    break;
                case 1:
                    _switchComponent.TurnOff();
                    break;
                case 2:
                    _switchComponent.TurnOn();
                    break;
            }
        }
    }
}