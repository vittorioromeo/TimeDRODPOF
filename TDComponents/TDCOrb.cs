using System;
using SFMLStart.Data;
using TimeDRODPOF.TDLib;
using VeeTileEngine2012;

namespace TimeDRODPOF.TDComponents
{
    [Serializable]
    public class TDCOrb : Component
    {
        private readonly TDCIDCaller _idCallerComponent;
        private readonly bool _isBroken;
        private readonly TDCRender _renderComponent;
        private int _health;
        private int _struckTime;

        public TDCOrb(bool mIsBroken, int mHealth, TDCRender mRenderComponent, TDCIDCaller mIDCallerComponent)
        {
            _isBroken = mIsBroken;
            _health = mHealth;
            _renderComponent = mRenderComponent;
            _idCallerComponent = mIDCallerComponent;

            mRenderComponent.OnDraw += Draw;
        }

        private void Draw()
        {
            if (_struckTime > 0)
            {
                _renderComponent.GetSprite(0).TextureRect = Assets.GetTileset("orbtiles").GetTextureRect("struck");
                _struckTime--;
            }
            else
                _renderComponent.GetSprite(0).TextureRect = Assets.GetTileset("orbtiles").GetTextureRect("on");
        }

        public void Struck(TDCWeapon mWeapon)
        {
            if (_isBroken)
            {
                TDLSounds.Play("SoundBrokenWall");
                _health--;

                if (_health < 2) _renderComponent.GetSprite(1).TextureRect = Assets.GetTileset("brokenoverlaytiles").GetTextureRect(1, 0);
            }

            if (_health == 0)
            {
                Entity.Destroy();
                return;
            }

            TDLSounds.Play("SoundOrbHit");
            _struckTime = 10;
            _idCallerComponent.SendCalls();
        }
    }
}