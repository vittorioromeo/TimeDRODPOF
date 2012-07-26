using System.Collections.Generic;
using SFMLStart.Data;
using TimeDRODPOF.TDComponents;

namespace TimeDRODPOF.TDLib
{
    public static class TDLComponentFactory
    {
        public static TDCID ID(List<int> mIDs) { return mIDs != null ? new TDCID(mIDs.ToArray()) : new TDCID(-1); }

        public static TDCRender Render(string mTextureName, string mTilesetName, string mLabelName, bool mIsLerped = false)
        {
            return new TDCRender(new TDCRenderSpriteOutline(mTilesetName, mTextureName, mLabelName))
                   {
                       IsLerped = mIsLerped
                   };
        }

        public static TDCSwitch Switch(TDCRender mRenderComponent, bool mIsOff, string mTilesetName = "onofftiles", string mOffLabel = "off", string mOnLabel = "on")
        {
            var result = new TDCSwitch(mRenderComponent, mIsOff);

            result.SetOffTextureRect(Assets.Tilesets[mTilesetName].GetTextureRect(mOffLabel));
            result.SetOnTextureRect(Assets.Tilesets[mTilesetName].GetTextureRect(mOnLabel));

            return result;
        }
        public static TDCSwitch SwitchTexture(TDCRender mRenderComponent, bool mIsOff, string mOffTexture, string mOnTexture)
        {
            return new TDCSwitch(mRenderComponent, mIsOff)
                   {
                       OffTextureName = mOffTexture,
                       OnTextureName = mOnTexture
                   };
        }
        public static TDCRender Render(string mTextureName, bool mIsLerped = false)
        {
            return new TDCRender(new TDCRenderSpriteOutline(mTextureName))
                   {
                       IsLerped = mIsLerped
                   };
        }
    }
}