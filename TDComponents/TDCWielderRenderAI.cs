using System;
using VeeTileEngine2012;

namespace TimeDRODPOF.TDComponents
{
    [Serializable]
    public class TDCWielderRenderAI : Component
    {
        private readonly TDCRenderDirectionAI _renderDirectionAIComponent;
        private readonly TDCWielder _wielderComponent;

        public TDCWielderRenderAI(TDCWielder mWielderComponent, TDCRender mRenderComponent, TDCRenderDirectionAI mRenderDirectionAIComponent)
        {
            _wielderComponent = mWielderComponent;
            _renderDirectionAIComponent = mRenderDirectionAIComponent;

            mRenderComponent.OnDraw += RenderSheated;
        }
        private void RenderSheated() { _renderDirectionAIComponent.Prefix = _wielderComponent.IsSheated ? "sheated_" : ""; }
    }
}