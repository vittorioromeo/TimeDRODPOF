using SFML.Graphics;
using SFML.Window;
using SFML.System;
using SFMLStart;
using SFMLStart.Data;

namespace TimeDRODPOF
{
    public class TDCommon
    {
        private readonly Sprite _floorSprite;
        private readonly GameWindow _gameWindow;

        public TDCommon(GameWindow mGameWindow)
        {
            _gameWindow = mGameWindow;
            _floorSprite = new Sprite(Assets.GetTexture(@"environment\floor\gray")) {Texture = {Repeated = true}};
            GUI = new TDGUI(mGameWindow.RenderWindow);
            SelectionSprite = new Sprite(Assets.GetTexture(@"selection"));
            GameTexture = new RenderTexture(1024, 768) {Smooth = true};
            GameSprite = new Sprite(GameTexture.Texture);
        }

        public TDGUI GUI { get; set; }
        public Sprite SelectionSprite { get; set; }
        public Sprite GameSprite { get; set; }
        public RenderTexture GameTexture { get; set; }
        public TDVector2 TilePosition { get; set; }
        public Vector2f CenterPosition { get { return new Vector2f(GameSprite.Position.X + (int) (GameTexture.Size.X/2f) - 81, GameSprite.Position.Y + (int) (GameTexture.Size.Y/2f)); } }

        public void UpdatePositions()
        {
            TilePosition = new TDVector2((int) (_gameWindow.Camera.MousePosition.X/TDUtils.TileSize), (int) (_gameWindow.Camera.MousePosition.Y/TDUtils.TileSize));
            SelectionSprite.Position = new Vector2f(TilePosition.X*TDUtils.TileSize, TilePosition.Y*TDUtils.TileSize);
        }
        public void RefreshTexture(int mSpriteWidth, int mSpriteHeight)
        {
            _floorSprite.TextureRect = new IntRect(0, 0, mSpriteWidth, mSpriteHeight);
            GameTexture = new RenderTexture((uint) mSpriteWidth, (uint) mSpriteHeight) {Smooth = true};
            GameSprite = new Sprite(GameTexture.Texture);
        }
        public void ClearTexture()
        {
            GameTexture.Clear(Color.White);
            GameTexture.Draw(_floorSprite);
        }
        public void CenterCamera() { _gameWindow.Camera.CenterOn(CenterPosition); }
    }
}