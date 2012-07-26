#region
using System;
using System.Collections.Generic;
using System.Linq;
using TimeDRODPOF.TDLib;
using VeeTileEngine2012;

#endregion

namespace TimeDRODPOF.TDComponents
{
    [Serializable]
    public class TDCWielder : Component
    {
        private readonly TDCDirection _directionComponent;
        private readonly TDCMovement _movementComponent;
        private bool _isSheated;

        public TDCWielder(TDCMovement mMovementComponent, TDCDirection mDirectionComponent)
        {
            _movementComponent = mMovementComponent;
            _directionComponent = mDirectionComponent;

            _movementComponent.OnMovementSuccess += SyncWeaponWithWielder;
            _movementComponent.OnMovementFail += SyncWeaponWithWielder;
        }

        public Entity WeaponEntity { get; private set; }
        public TDCWeapon WeaponComponent { get; private set; }
        public bool IsSheated
        {
            get { return IsUnarmed || _isSheated; }
            set
            {
                if (IsUnarmed)
                {
                    _isSheated = true;
                    return;
                }

                _isSheated = value;

                if (WeaponComponent == null || WeaponEntity == null) return;

                if (value)
                {
                    WeaponEntity.IsOutOfField = true;
                    if (WeaponComponent.OnUnEquip != null) WeaponComponent.OnUnEquip(this);
                }
                else
                {
                    WeaponEntity.IsOutOfField = false;
                    if (WeaponComponent.OnEquip != null) WeaponComponent.OnEquip(this);
                }
            }
        }
        public bool IsUnarmed { get { return WeaponEntity == null; } }

        public void SetWeapon(Entity mWeapon)
        {
            if (!IsUnarmed)
            {
                WeaponComponent.OnUnEquip.SafeInvoke(this);
                WeaponEntity.IsOutOfField = true; // fucks up weapon slots
            }

            if (mWeapon == null)
            {
                WeaponEntity = null;
                WeaponComponent = null;
                IsSheated = true;

                return;
            }

            WeaponEntity = mWeapon;
            WeaponComponent = mWeapon.GetComponent<TDCWeapon>();

            WeaponComponent.OnEquip += mWielder => _movementComponent.IgnoreEntities.Add(WeaponEntity);
            WeaponComponent.OnUnEquip += mWielder => _movementComponent.IgnoreEntities.Remove(WeaponEntity);

            if (IsSheated) WeaponComponent.OnUnEquip.SafeInvoke(this);
            else WeaponComponent.OnEquip.SafeInvoke(this);

            Field.OnLoadChecks += SyncWeaponWithWielder; // to insta-sheate on oremites
        }

        public void SyncWeaponWithWielder()
        {
            if (IsUnarmed || IsSheated) return;

            var wielderDirection = _directionComponent.Direction;

            if (WeaponComponent.IsOutOfBounds)
            {
                WeaponComponent.IsOutOfBounds = false;
                WeaponEntity.IsOutOfField = false;
            }

            var weaponDirectionComponent = WeaponEntity.GetComponent<TDCDirection>();
            var oldDirection = weaponDirectionComponent.Direction;
            weaponDirectionComponent.Direction = wielderDirection;

            WeaponEntity.GetComponent<TDCRender>().IsLerped = oldDirection == _directionComponent.Direction;

            var directionVector = _directionComponent.DirectionVector;

            if (TDLMethods.WeaponMove(WeaponEntity, X + directionVector.X, Y + directionVector.Y)) CheckHits();
        }

        private void CheckHits()
        {
            var componentsToCheck = Field.GetComponents(WeaponEntity.X, WeaponEntity.Y, typeof (TDCHitByWeapon)).Cast<TDCHitByWeapon>();
            foreach (var hitByWeaponComponent in new List<TDCHitByWeapon>(componentsToCheck))
                hitByWeaponComponent.HitAction.Invoke(WeaponComponent);
        }

        public override void Removed()
        {
            base.Removed();
            if (WeaponEntity != null) WeaponEntity.Destroy();
            WeaponEntity = null;
            WeaponComponent = null;
        }
        public void SetDirection(int mDirection)
        {
            _directionComponent.Direction = mDirection;
            SyncWeaponWithWielder();
        }
    }
}