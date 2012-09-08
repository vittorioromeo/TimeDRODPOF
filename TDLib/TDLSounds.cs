#region
using SFMLStart.Data;

#endregion

namespace TimeDRODPOF.TDLib
{
    public static class TDLSounds
    {
        static TDLSounds() { SoundsEnabled = true; }
        public static bool SoundsEnabled { get; set; }
        public static void Play(string mSoundName)
        {
            if (!SoundsEnabled) return;
            Assets.GetSound(mSoundName).Play();
        }
    }
}