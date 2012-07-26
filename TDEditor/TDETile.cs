#region
using System.Collections.Generic;

#endregion

namespace TimeDRODPOF.TDEditor
{
    public class TDETile
    {
        public TDETile(int mX, int mY)
        {
            X = mX;
            Y = mY;

            OrderedEntities = new List<TDEEntity>();
        }

        public int X { get; private set; }
        public int Y { get; private set; }
        public List<TDEEntity> OrderedEntities { get; set; }

        public void AddEntity(TDEEntity mEntity)
        {
            OrderedEntities.Add(mEntity);
            OrderedEntities.Sort((a, b) => a.Layer.CompareTo(b.Layer));
        }

        public void RemoveEntity(TDEEntity mEntity) { OrderedEntities.Remove(mEntity); }
        public void Clear() { OrderedEntities.Clear(); }
    }
}