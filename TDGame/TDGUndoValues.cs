#region
using System.Collections.Generic;

#endregion

namespace TimeDRODPOF.TDGame
{
    public class TDGUndoValues
    {
        public TDGUndoValues() { Inputs = new List<int>(); }
        public int PlayerStartDirection { get; set; }
        public int PlayerStartX { get; set; }
        public int PlayerStartY { get; set; }
        public bool WasRoomClear { get; set; }
        public int ClearTurns { get; set; }
        public List<int> Inputs { get; set; }
    }
}