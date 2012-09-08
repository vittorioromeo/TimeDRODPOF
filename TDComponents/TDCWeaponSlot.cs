using System;
using SFML.Window;
using SFMLStart.Data;
using TimeDRODPOF.TDLib;
using VeeTileEngine2012;

namespace TimeDRODPOF.TDComponents
{
    [Serializable]
    public class TDCWeaponSlot : Component
    {
        private readonly TDCRender _renderComponent;

        public TDCWeaponSlot(TDCSpecialSquare mSpecialSquareComponent, TDCRender mRenderComponent)
        {
            _renderComponent = mRenderComponent;
            mSpecialSquareComponent.OnMoveToAllowed += SpecialSquareMove;
        }
        public Entity Weapon { get; set; }

        private bool SpecialSquareMove(Entity mEntity, int mNextX, int mNextY, bool mNextSuccess, out bool mAbort)
        {
            mAbort = false;
            if (!mNextSuccess) return true;

            var cWielder = mEntity.GetComponent<TDCWielder>();
            if (cWielder == null) return true;

            TDLSounds.Play("SoundMimic");

            if (Weapon != null)
            {
                var tempWeapon = cWielder.WeaponEntity;
                cWielder.SetWeapon(Weapon);
                cWielder.IsSheated = false;
                Weapon = tempWeapon;
            }
            else
            {
                cWielder.WeaponComponent.OnUnEquip.SafeInvoke(cWielder);
                Weapon = cWielder.WeaponEntity;
                cWielder.SetWeapon(null);
            }

            if (Weapon != null) Weapon.IsOutOfField = true;
            RecalculateWeaponSprite();
            return true;
        }
        public void RecalculateWeaponSprite()
        {
            if (Weapon == null)
            {
                _renderComponent.GetSprite(1).Scale = new Vector2f(0, 0);
                return;
            }
            _renderComponent.SetSprite(1, Assets.GetTileset("dirtiles").GetSprite("n", Weapon.GetComponent<TDCRender>().GetSprite(0).Texture));
            _renderComponent.GetSprite(1).Origin += new Vector2f(0, 2);
            _renderComponent.GetSprite(1).Rotation = 45;
            _renderComponent.GetSprite(1).Scale = new Vector2f(0.6f, 0.6f);
        }
    }
}