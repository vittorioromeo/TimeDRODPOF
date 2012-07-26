#region
using System;
using System.Collections.Generic;
using System.Linq;
using VeeTileEngine2012;

#endregion

namespace TimeDRODPOF.TDComponents
{
    [Serializable]
    public class TDCKiller : Component
    {
        public TDCKiller(string[] mTargetTags, Action<Entity> mKillAction)
        {
            TargetTags = mTargetTags;
            KillAction = mKillAction;
        }

        public string[] TargetTags { get; set; }
        public Action<Entity> KillAction { get; set; }

        public override void NextTurn()
        {
            base.NextTurn();

            foreach (var entity in TargetTags.SelectMany(x => new List<Entity>(Field.GetEntitiesByTag(X, Y, x))))
                KillAction.Invoke(entity);
        }
    }
}