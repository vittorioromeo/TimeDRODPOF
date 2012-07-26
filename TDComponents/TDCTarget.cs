#region
using System;
using System.Collections.Generic;
using System.Linq;
using TimeDRODPOF.TDLib;
using VeeTileEngine2012;

#endregion

namespace TimeDRODPOF.TDComponents
{
    [Serializable]
    public class TDCTarget : Component
    {
        private int _targetNextX, _targetNextY;

        public TDCTarget(params string[] mTargetTags) { TargetTags = mTargetTags; }

        public Entity Target { get; private set; }
        public int TargetDistance { get; private set; }
        public TDVector2 TargetNextXY { get { return new TDVector2(_targetNextX, _targetNextY); } }
        public string[] TargetTags { get; set; }

        private void GetNearestTarget()
        {
            Target = null;

            var targetEntities = new SortedSet<Entity>(new ByDistance((x1, y1, x2, y2) =>
                                                                      TDLDistances.ChebyshevDistance(x1, y1, x2, y2), X, Y));

            foreach (var tag in TargetTags)
            {
                var entities = Field.GetEntitiesByTag(tag).Where(x => x != Entity);
                foreach (var ent in entities) targetEntities.Add(ent);
            }

            if (targetEntities.Count == 0) return;

            var target = targetEntities.First();
            Target = target;
            TargetDistance = TDLDistances.ChebyshevDistance(target.X, target.Y, X, Y);
        }

        public override void NextTurn()
        {
            base.NextTurn();
            GetNearestTarget();

            _targetNextX = _targetNextY = 0;
            if (Target == null) return;

            if (Target.X > X) _targetNextX = 1;
            else if (Target.X < X) _targetNextX = -1;
            if (Target.Y > Y) _targetNextY = 1;
            else if (Target.Y < Y) _targetNextY = -1;
        }
    }
}