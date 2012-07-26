#region
using System.Collections.Generic;
using System.Reflection;
using SFML.Graphics;
using SFMLStart.Data;

#endregion

namespace TimeDRODPOF.TDEditor
{
    public class TDEOutline
    {
        public TDEOutline(int mUID, int mEditorGroup, string mName, int mLayer, string mTextureName, string mTilesetName = null, string mLabelName = null)
        {
            UID = mUID;
            Name = mName;
            Layer = mLayer;
            Texture = Assets.GetTexture(mTextureName);
            if (mTilesetName == null || mLabelName == null) TextureRect = new IntRect(0, 0, TDUtils.TextureSize, TDUtils.TextureSize);
            else TextureRect = Assets.Tilesets[mTilesetName].GetTextureRect(mLabelName);
            EditorGroup = mEditorGroup;
            ParameterInfos = new List<ParameterInfo>();
            ParameterDefaultValues = new List<object>();
        }

        public int UID { get; set; }
        public string Name { get; set; }
        public int Layer { get; set; }
        public Texture Texture { get; set; }
        public IntRect TextureRect { get; set; }
        public int EditorGroup { get; set; }
        public List<ParameterInfo> ParameterInfos { get; set; }
        public List<object> ParameterDefaultValues { get; set; }
        public MethodInfo MethodInfo { get; set; }

        public void AddParameter(ParameterInfo mParameter, object mDefaultValue)
        {
            ParameterInfos.Add(mParameter);

            if (mParameter.ParameterType == typeof (List<int>) && mDefaultValue == null)
                mDefaultValue = new List<int> {-1};

            ParameterDefaultValues.Add(mDefaultValue);
        }
    }
}