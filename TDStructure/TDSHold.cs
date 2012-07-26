#region
using System.Collections.Generic;

#endregion

namespace TimeDRODPOF.TDStructure
{
    public class TDSHold
    {
        public TDSHold(string mName)
        {
            Name = mName;
            Levels = new List<TDSLevel>();
        }

        public string Name { get; set; }
        public List<TDSLevel> Levels { get; set; }

        public void AddLevel(TDSLevel mLevel) { Levels.Add(mLevel); }
    }
}