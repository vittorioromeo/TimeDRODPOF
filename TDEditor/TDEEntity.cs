#region
using System.Collections.Generic;
using System.Linq;
using SFML.Graphics;

#endregion

namespace TimeDRODPOF.TDEditor
{
    public class TDEEntity
    {
        public TDEEntity(TDEOutline mOutline)
        {
            Outline = mOutline;
            Sprite = new Sprite(mOutline.Texture) {TextureRect = mOutline.TextureRect};

            Parameters = new List<TDEEntityParameter>();

            for (var index = 0; index < mOutline.ParameterInfos.Count; index++)
            {
                var parameterInfo = mOutline.ParameterInfos[index];
                Parameters.Add(new TDEEntityParameter(parameterInfo.Name, parameterInfo.ParameterType) {Value = mOutline.ParameterDefaultValues[index]});
            }
        }

        public TDEOutline Outline { get; set; }
        public TDETile Tile { get; set; }
        public Sprite Sprite { get; set; }
        public int Layer { get { return Outline.Layer; } }
        public int EditorGroup { get { return Outline.EditorGroup; } }
        public string Name { get { return Outline.Name; } }
        public List<TDEEntityParameter> Parameters { get; set; }

        public TDEEntity Clone() { return new TDEEntity(Outline) {Parameters = CloneParameters()}; }
        public List<TDEEntityParameter> CloneParameters() { return Parameters.Select(parameter => parameter.Clone()).ToList(); }
    }
}