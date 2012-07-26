#region
using System;
using System.Collections.Generic;
using System.Linq;

#endregion

namespace TimeDRODPOF.TDStructure
{
    public class TDSLevel
    {
        public TDSLevel(string mName, int mRoomWidth, int mRoomHeight)
        {
            Name = mName;
            RoomWidth = mRoomWidth;
            RoomHeight = mRoomHeight;
            Rooms = new Dictionary<Tuple<int, int>, TDSRoom>();
        }

        public string Name { get; set; }
        public int RoomWidth { get; set; }
        public int RoomHeight { get; set; }
        public Dictionary<Tuple<int, int>, TDSRoom> Rooms { get; set; }

        public void AddRoom(TDSRoom mRoom)
        {
            foreach (var tuple in Rooms.Keys.Where(x => x.Item1 == mRoom.X && x.Item2 == mRoom.Y))
            {
                Rooms[tuple] = mRoom;
                return;
            }
            Rooms.Add(new Tuple<int, int>(mRoom.X, mRoom.Y), mRoom);
        }

        public TDSRoom GetRoom(int mX, int mY) { return Rooms.FirstOrDefault(x => x.Key.Item1 == mX && x.Key.Item2 == mY).Value; }

        public bool IsClear() { return Rooms.All(room => !room.Value.IsRequired || room.Value.IsClear); }
    }
}