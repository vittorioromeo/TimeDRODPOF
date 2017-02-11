#region
using SFML.Graphics;
using SFML.Window;
using SFMLStart.Data;
using SFML.System;
#endregion

namespace TimeDRODPOF
{
    public class TDGUI
    {
        private readonly Sprite _guiBackgroundSprite;
        private readonly Sprite _guiFrameSprite;
        private readonly Text _guiHoldText;
        private readonly Text _guiRoomText;
        private readonly Text _guiTurnText;
        private readonly RenderTarget _renderTarget;
        private readonly RectangleShape _scrollBackground;
        private readonly Text _scrollText;
		private readonly Font _myFont = new Font("/usr/share/fonts/TTF/LiberationMono-Regular.ttf");

        public TDGUI(RenderTarget mRenderTarget)
        {
            _renderTarget = mRenderTarget;

            _guiFrameSprite = new Sprite(Assets.GetTexture(@"gui\gameframe3"));
            _guiBackgroundSprite = new Sprite(Assets.GetTexture(@"gui\background"));
            _scrollText = new Text("", _myFont, 14) {Style = Text.Styles.Italic, Position = new Vector2f(45, 45)};
            _scrollBackground = new RectangleShape(new Vector2f(400, 400)) {Position = new Vector2f(38, 34), FillColor = new Color(0, 0, 0, 100)};
            _guiTurnText = new Text("", _myFont, 12) {Style = Text.Styles.Bold, Position = new Vector2f(120, 749), Color = Color.White};
            _guiRoomText = new Text("", _myFont, 12) {Style = Text.Styles.Bold, Position = new Vector2f(300, 749), Color = Color.White};
            _guiHoldText = new Text("", _myFont, 12) {Style = Text.Styles.Bold, Position = new Vector2f(500, 749), Color = Color.White};
        }

        public int Turn { private get; set; }
        public int RoomX { private get; set; }
        public int RoomY { private get; set; }
        public string HoldName { private get; set; }
        public string LevelName { private get; set; }

        public void DrawBackground() { _renderTarget.Draw(_guiBackgroundSprite); }

        public void DrawGUI()
        {
            _guiTurnText.DisplayedString = string.Format("{0}", Turn);
            _guiRoomText.DisplayedString = string.Format("x:{0} y:{1}", RoomX, RoomY);
            _guiHoldText.DisplayedString = string.Format("{0} - {1}", HoldName, LevelName);

            _renderTarget.Draw(_guiFrameSprite);
            _renderTarget.Draw(_guiTurnText);
            _renderTarget.Draw(_guiRoomText);
            _renderTarget.Draw(_guiHoldText);

            if (string.IsNullOrEmpty(_scrollText.DisplayedString)) return;

            _renderTarget.Draw(_scrollBackground);
            _renderTarget.Draw(_scrollText);
        }

        public void SetScrollText(string mText) { _scrollText.DisplayedString = mText; }
    }
}