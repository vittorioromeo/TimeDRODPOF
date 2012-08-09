#region
using System;
using TimeDRODPOF.TDLib;
using VeeTileEngine2012;

#endregion

namespace TimeDRODPOF.TDComponents
{
    [Serializable]
    public class TDCHitByWeapon : Component
    {
        #region HitActions enum
        public enum HitActions
        {
            Break,
            KillBrain,
            Kill,
            BreakIfOff
        }
        #endregion

        public TDCHitByWeapon(Action<TDCWeapon> mHitAction) { HitAction = mHitAction; }
        public TDCHitByWeapon(HitActions mHitAction)
        {
            switch (mHitAction)
            {
                case HitActions.Break:
                    HitAction += Break;
                    break;
                case HitActions.KillBrain:
                    HitAction += KillBrain;
                    break;
                case HitActions.Kill:
                    HitAction += Kill;
                    break;
                case HitActions.BreakIfOff:
                    HitAction += BreakIfOff;
                    break;
            }
        }

        public Action<TDCWeapon> HitAction { get; set; }

        private void Break(TDCWeapon mWeapon)
        {
            Entity.Destroy();
            TDLSounds.Play("SoundBrokenWall");
        }
        private void KillBrain(TDCWeapon mWeapon)
        {
            Entity.Destroy();
            TDLSounds.Play("SoundKill1");
            TDLMethods.CheckForBrains(Field, true);
        }
        private void Kill(TDCWeapon mWeapon)
        {
            Entity.Destroy();
            TDLSounds.Play("SoundKill1");
        }
        private void BreakIfOff(TDCWeapon mWeapon)
        {
            var cSwitch = Entity.GetComponent<TDCSwitch>();
            if (cSwitch.IsOff) return;
            Entity.Destroy();
            TDLSounds.Play("SoundBrokenWall");
        }
    }
}