using System;
using TimeDRODPOF.TDComponents;

namespace TimeDRODPOF
{
    public static class TDUtils
    {
        public const int TileSize = 31;
        public const int TextureSize = 31;
        public const int DefaultRoomWidth = 26;
        public const int DefaultRoomHeight = 23;

        public static void SafeInvoke(this Action<TDCWielder> mAction, TDCWielder mWielderComponent) { if (mAction != null) mAction(mWielderComponent); }
    }
}