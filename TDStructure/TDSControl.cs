namespace TimeDRODPOF.TDStructure
{
    public class TDSControl
    {
        public TDSHold CurrentHold { get; set; }
        public TDSLevel CurrentLevel { get; set; }
        public TDSRoom CurrentRoom { get; set; }

        public static TDSHold CreateHold(TDSControl mControl, string mHoldName)
        {
            var hold = new TDSHold(mHoldName);
            mControl.CurrentHold = hold;
            return hold;
        }
        public static TDSLevel CreateLevel(TDSHold mHold, string mLevelName, int mRoomWidth, int mRoomHeight)
        {
            var level = new TDSLevel(mLevelName, mRoomWidth, mRoomHeight);
            mHold.AddLevel(level);
            return level;
        }
        public static TDSRoom CreateRoom(TDSLevel mLevel, int mRoomX, int mRoomY, bool mIsRequired, bool mIsSecret)
        {
            var room = new TDSRoom(mLevel, mRoomX, mRoomY, mIsRequired, mIsSecret);
            mLevel.AddRoom(room);
            return room;
        }
    }
}