#region
using System;
using System.Collections.Generic;
using SFML.Graphics;
using SFML.Window;
using SFMLStart.Data;
using SFMLStart.Utilities;
using VeeTileEngine2012;
using Utils = SFMLStart.Utilities.Utils;

#endregion

namespace TimeDRODPOF.TDComponents
{
    [Serializable]
    public class TDCRenderSpriteOutline
    {
        public TDCRenderSpriteOutline(string mTextureName) { TextureName = mTextureName; }
        public TDCRenderSpriteOutline(string mTextureName, IntRect mIntRect)
        {
            TextureName = mTextureName;

            HasIntRect = true;
            StartX = mIntRect.Left;
            StartY = mIntRect.Top;
            EndX = mIntRect.Width;
            EndY = mIntRect.Height;
        }
        public TDCRenderSpriteOutline(string mTilesetName, string mTextureName, string mLabelName)
        {
            TilesetName = mTilesetName;
            TextureName = mTextureName;
            LabelName = mLabelName;
        }

        public string TilesetName { get; set; }
        public string TextureName { get; set; }
        public string LabelName { get; set; }
        public bool HasIntRect { get; private set; }
        public int StartX { get; private set; }
        public int StartY { get; private set; }
        public int EndX { get; private set; }
        public int EndY { get; private set; }
    }

    [Serializable]
    public class TDCRender : Component
    {
        private const float DefaultLerpSpeed = 0.38f;
        private static readonly Vector2f DefaultOrigin = new Vector2f(TDUtils.TileSize/2f, TDUtils.TileSize/2f);

        [NonSerialized] private readonly List<Sprite> _sprites;
        private bool _needsToPosition;

        public TDCRender(params TDCRenderSpriteOutline[] mRenderSpriteOutlines)
        {
            _sprites = new List<Sprite>();
            LerpSpeed = DefaultLerpSpeed;

            Initialize(mRenderSpriteOutlines);
        }

        public bool IsLerped { get; set; }
        public float LerpSpeed { get; set; }
        public Action OnDraw { get; set; }

        public void Initialize(IEnumerable<TDCRenderSpriteOutline> mRenderSpriteOutlines)
        {
            foreach (var outline in mRenderSpriteOutlines)
            {
                Sprite sprite;
                if (outline.TilesetName == null) sprite = new Sprite(Assets.GetTexture(outline.TextureName));
                else sprite = new Sprite(Assets.Tilesets[outline.TilesetName].GetSprite(outline.LabelName, Assets.GetTexture(outline.TextureName)));
                AddSprite(sprite);
            }
        }

        public void AddBlankSprite()
        {
            _sprites.Add(new Sprite());
            _needsToPosition = true;
        }
        public void AddSprite(Sprite mSprite, bool mSmooth = true)
        {
            mSprite.Texture.Smooth = mSmooth;
            mSprite.Origin = DefaultOrigin;
            _sprites.Add(mSprite);

            _needsToPosition = true;
        }
        public void SetSprite(int mIndex, Sprite mSprite) { _sprites[mIndex] = mSprite; }
        public Sprite GetSprite(int mIndex) { return _sprites[mIndex]; }
        public IEnumerable<Sprite> GetSprites() { return _sprites; }

        public void Draw(RenderTarget mRenderTarget)
        {
            if (Entity.IsOutOfField) return;

            OnDraw.SafeInvoke();

            foreach (var sprite in _sprites)
            {
                if (_needsToPosition) sprite.Position = GetDrawPosition(new Vector2f(X, Y));

                var targetPosition = GetDrawPosition(new Vector2f(Entity.X, Entity.Y));
                var position = targetPosition;
                if (IsLerped) position = Utils.Math.Vectors.Lerp(sprite.Position, targetPosition, LerpSpeed);
                sprite.Position = position;
                mRenderTarget.Draw(sprite);
            }

            if (_needsToPosition) _needsToPosition = false;
        }

        private static Vector2f GetDrawPosition(Vector2f mPosition) { return new Vector2f(mPosition.X*TDUtils.TileSize + TDUtils.TileSize/2f, mPosition.Y*TDUtils.TileSize + TDUtils.TileSize/2f); }
    }
}