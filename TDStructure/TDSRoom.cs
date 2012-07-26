#region
using TimeDRODPOF.TDEditor;

#endregion

namespace TimeDRODPOF.TDStructure
{
    public class TDSRoom
    {
        public TDSRoom(TDSLevel mLevel, int mRoomX, int mRoomY, bool mIsRequired, bool mIsSecret)
        {
            TileManager = new TDETileManager(mLevel.RoomWidth, mLevel.RoomHeight);
            Level = mLevel;
            X = mRoomX;
            Y = mRoomY;
            IsRequired = mIsRequired;
            IsSecret = mIsSecret;
        }

        public TDETileManager TileManager { get; set; }
        public TDSLevel Level { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
        public bool IsClear { get; set; }
        public bool IsRequired { get; set; }
        public bool IsSecret { get; set; }
    }
}