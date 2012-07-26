using System;
using TimeDRODPOF.TDLib;
using VeeTileEngine2012;

namespace TimeDRODPOF.TDComponents
{
    [Serializable]
    public class TDCOremites : Component
    {
        private readonly TDCSpecialSquare _specialSquareComponent;

        public TDCOremites(TDCSpecialSquare mSpecialSquareComponent)
        {
            _specialSquareComponent = mSpecialSquareComponent;

            _specialSquareComponent.OnMoveFromAllowed += SpecialSquareFrom;
            _specialSquareComponent.OnMoveToAllowed += SpecialSquareTo;
        }

        public override void Added()
        {
            base.Added();
            Field.OnLoad += SheateEntities;
        }

        public override void NextTurn()
        {
            base.NextTurn();
            SheateEntities();
        }

        private bool TrySheateEntity(Entity mEntity)
        {
            var cWielder = mEntity.GetComponent<TDCWielder>();
            if (cWielder.WeaponEntity == null || !cWielder.WeaponEntity.HasTag(TDLTags.AffectedByOremites)) return false;
            cWielder.IsSheated = true;
            return true;
        }

        private void SheateEntities()
        {
            foreach (var entity in Field.GetEntitiesByComponent(X, Y, typeof (TDCWielder)))
                TrySheateEntity(entity);
        }

        private bool SpecialSquareFrom(Entity mEntity, int mNextX, int mNextY, bool mNextSuccess, out bool mAbort)
        {
            mAbort = false;

            var cWielder = mEntity.GetComponent<TDCWielder>();
            if (!mNextSuccess || cWielder == null) return true;

            if (cWielder.WeaponEntity != null && !cWielder.WeaponEntity.HasTag(TDLTags.AffectedByOremites)) return true;

            cWielder.IsSheated = false;

            var directionComponent = mEntity.GetComponent<TDCDirection>();
            if (directionComponent != null) directionComponent.Direction = TDCDirection.GetDirection(new TDVector2(mNextX, mNextY));

            return true;
        }

        private bool SpecialSquareTo(Entity mEntity, int mNextX, int mNextY, bool mNextSuccess, out bool mAbort)
        {
            mAbort = false;

            if (mEntity.GetComponent<TDCWielder>() == null) return true;
            if (!mNextSuccess) return true;
            if (!TrySheateEntity(mEntity)) return true;

            mEntity.GetComponent<TDCDirection>().Direction = TDCDirection.GetDirection(new TDVector2(mNextX, mNextY));

            return true;
        }
    }
}