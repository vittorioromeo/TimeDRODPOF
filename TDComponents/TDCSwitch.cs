#region
using System;
using System.Collections.Generic;
using SFML.Graphics;
using SFMLStart.Data;
using SFMLStart.Utilities;
using VeeTileEngine2012;

#endregion

namespace TimeDRODPOF.TDComponents
{
    [Serializable]
    public class TDCSwitch : Component
    {
        private readonly TDCRender _renderComponent;
        private readonly int _spriteIndex;

        public TDCSwitch(TDCRender mRenderComponent, bool mIsOff = false, int mSpriteIndex = 0)
        {
            IsOff = mIsOff;
            _renderComponent = mRenderComponent;
            _spriteIndex = mSpriteIndex;

            OnlyOnTags = new List<string>();
            OnlyOffTags = new List<string>();
        }

        public bool IsOff { get; private set; }
        public string OffTextureName { get; set; }
        public string OnTextureName { get; set; }
        public TDVector2 OffStart { get; set; }
        public TDVector2 OffEnd { get; set; }
        public TDVector2 OnStart { get; set; }
        public TDVector2 OnEnd { get; set; }

        public List<string> OnlyOnTags { get; set; }
        public List<string> OnlyOffTags { get; set; }
        public Action OnTurnOff { get; set; }
        public Action OnTurnOn { get; set; }

        public override void Added()
        {
            base.Added();
            if (IsOff) TurnOff();
            else TurnOn();
        }

        public void TurnOn()
        {
            IsOff = false;

            if (_renderComponent != null)
            {
                if (OnTextureName != null) _renderComponent.GetSprite(_spriteIndex).Texture = Assets.GetTexture(OnTextureName);
                if (GetOnTextureRect() != null) _renderComponent.GetSprite(_spriteIndex).TextureRect = (IntRect) GetOnTextureRect();
            }

            foreach (var tagToAdd in OnlyOnTags) Entity.AddTags(tagToAdd);
            foreach (var tagToRemove in OnlyOffTags) Entity.RemoveTags(tagToRemove);

            OnTurnOn.SafeInvoke();
        }

        public void TurnOff()
        {
            IsOff = true;

            if (_renderComponent != null)
            {
                if (OffTextureName != null) _renderComponent.GetSprite(_spriteIndex).Texture = Assets.GetTexture(OffTextureName);
                if (GetOffTextureRect() != null) _renderComponent.GetSprite(_spriteIndex).TextureRect = (IntRect) GetOffTextureRect();
            }

            foreach (var tagToAdd in OnlyOffTags) Entity.AddTags(tagToAdd);
            foreach (var tagToRemove in OnlyOnTags) Entity.RemoveTags(tagToRemove);

            OnTurnOff.SafeInvoke();
        }

        public void Toggle()
        {
            if (IsOff) TurnOn();
            else TurnOff();
        }

        private IntRect? GetOffTextureRect() { return new IntRect(OffStart.X, OffStart.Y, OffEnd.X, OffEnd.Y); }
        private IntRect? GetOnTextureRect() { return new IntRect(OnStart.X, OnStart.Y, OnEnd.X, OnEnd.Y); }

        public void SetOffTextureRect(IntRect mIntRect)
        {
            OffStart = new TDVector2(mIntRect.Left, mIntRect.Top);
            OffEnd = new TDVector2(mIntRect.Width, mIntRect.Height);
        }
        public void SetOnTextureRect(IntRect mIntRect)
        {
            OnStart = new TDVector2(mIntRect.Left, mIntRect.Top);
            OnEnd = new TDVector2(mIntRect.Width, mIntRect.Height);
        }
    }
}