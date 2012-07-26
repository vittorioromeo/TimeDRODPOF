#region
using System;
using System.Linq;
using VeeTileEngine2012;

#endregion

namespace TimeDRODPOF.TDComponents
{
    [Serializable]
    public class TDCRecalculateSprites : Component
    {
        public TDCRecalculateSprites(string mTag, Action<Entity, string, Func<Entity, bool>> mRecalculationMethod, Func<Entity, bool> mCondition = null)
        {
            Tag = mTag;
            RecalculationMethod = mRecalculationMethod;
            Radius = 1;
            Condition = mCondition ?? (x => true);
        }

        public string Tag { get; set; }
        public Action<Entity, string, Func<Entity, bool>> RecalculationMethod { get; set; }
        public int Radius { get; set; }
        public Func<Entity, bool> Condition { get; set; }

        public override void Added()
        {
            base.Added();
            RecalculateNearbySprites();
        }

        public override void Removed()
        {
            base.Removed();
            RecalculateNearbySprites();
        }

        public void RecalculateNearbySprites()
        {
            for (var iY = -Radius; iY < Radius + 1; iY++)
                for (var iX = -Radius; iX < Radius + 1; iX++)
                    foreach (var entity in Field.GetEntitiesByTag(X + iX, Y + iY, Tag).Where(Condition))
                        RecalculationMethod.Invoke(entity, Tag, Condition);
        }
    }
}