#region
using SFMLStart.Data;
using TimeDRODPOF.TDComponents;
using VeeTileEngine2012;
using Utils = SFMLStart.Utilities.Utils;

#endregion

namespace TimeDRODPOF.TDLib
{
    public static class TDLMethods
    {
        public static void TryRecalculation(ref bool mRecalculationNeeded, TDCRecalculateSprites mRecalculateComponent)
        {
            if (!mRecalculationNeeded) return;
            mRecalculateComponent.RecalculateNearbySprites();
            mRecalculationNeeded = false;
        }

        public static void AttachBrokenOverlay(TDCRender mRenderComponent)
        {
            mRenderComponent.AddSprite(Assets.GetTileset("brokenoverlaytiles").GetSprite(Utils.Random.Next(0, 3),
                                                                                       Utils.Random.Next(0, 3),
                                                                                       Assets.GetTexture(@"environment\brokenoverlay")));
        }

        public static void AttachCrackedOverlay(TDCRender mRenderComponent, int mX)
        {
            mRenderComponent.AddSprite(Assets.GetTileset("brokenoverlaytiles").GetSprite(mX, 0,
                                                                                       Assets.GetTexture(@"environment\crackedoverlay")));
        }

        public static void Kill(Entity mEntity)
        {
            mEntity.Destroy();
            TDLSounds.Play("SoundKill1");
        }

        public static void Burn(Entity mEntity)
        {
            mEntity.Destroy();
            TDLSounds.Play("SoundKill1");
        }

        public static bool WeaponMove(Entity mEntity, int mX, int mY)
        {
            if (mEntity.Move(mX, mY)) return true;

            mEntity.IsOutOfField = true;
            mEntity.GetComponent<TDCWeapon>().IsOutOfBounds = true;
            return false;
        }
        public static void CheckForBrains(Field mField, bool mPlaySound = false)
        {
            foreach (var entity in mField.GetEntitiesByTag(TDLTags.AffectedByBrain))
                entity.GetComponent<TDCMovementTargetAI>().IsPathfinder = mField.HasEntityByTag(TDLTags.Brain);

            if (!mField.HasEntityByTag(TDLTags.Brain) && mPlaySound) TDLSounds.Play("SoundNoBrains");
        }
    }
}