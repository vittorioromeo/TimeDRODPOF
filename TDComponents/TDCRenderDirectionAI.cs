#region
using System;
using SFMLStart.Data;
using SFMLStart.Utilities;
using VeeTileEngine2012;

#endregion

namespace TimeDRODPOF.TDComponents
{
    [Serializable]
    public class TDCRenderDirectionAI : Component
    {
        private readonly TDCDirection _directionComponent;
        private readonly TDCRender _renderComponent;

        public TDCRenderDirectionAI(TDCRender mRenderComponent, TDCDirection mDirectionComponent, string mTilesetName, string mPrefix = "", string mSuffix = "")
        {
            _renderComponent = mRenderComponent;
            _directionComponent = mDirectionComponent;
            TilesetName = mTilesetName;
            Prefix = mPrefix;
            Suffix = mSuffix;

            _renderComponent.OnDraw += () => _renderComponent.GetSprites().ForEach(x => x.TextureRect = Assets.Tilesets[TilesetName].GetTextureRect(Prefix + _directionComponent.DirectionString + Suffix));
        }

        public string TilesetName { get; set; }
        public string Prefix { get; set; }
        public string Suffix { get; set; }
    }
}