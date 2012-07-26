#region
using System;
using System.Collections.Generic;
using System.Linq;
using VeeTileEngine2012;

#endregion

namespace TimeDRODPOF.TDComponents
{
    [Serializable]
    public class TDCFloodFiller : Component
    {
        private readonly HashSet<Entity> _alreadyTested;
        private readonly HashSet<Entity> _result;
        private readonly string _tag;

        public TDCFloodFiller(string mTag)
        {
            _tag = mTag;
            _result = new HashSet<Entity>();
            _alreadyTested = new HashSet<Entity>();
        }

        public IEnumerable<Entity> GetAttachedEntities()
        {
            _result.Clear();
            _alreadyTested.Clear();

            RecursiveFloodFill(Entity, _tag);
            return _result;
        }

        private void RecursiveFloodFill(Entity mEntity, string mTag)
        {
            if (_alreadyTested.Contains(mEntity)) return;
            _alreadyTested.Add(mEntity);

            var w = mEntity.Field.GetEntitiesByTag(mEntity.X - 1, mEntity.Y, mTag).FirstOrDefault();
            var e = mEntity.Field.GetEntitiesByTag(mEntity.X + 1, mEntity.Y, mTag).FirstOrDefault();
            var n = mEntity.Field.GetEntitiesByTag(mEntity.X, mEntity.Y - 1, mTag).FirstOrDefault();
            var s = mEntity.Field.GetEntitiesByTag(mEntity.X, mEntity.Y + 1, mTag).FirstOrDefault();

            if (w != null) RecursiveFloodFill(w, mTag);
            if (e != null) RecursiveFloodFill(e, mTag);
            if (n != null) RecursiveFloodFill(n, mTag);
            if (s != null) RecursiveFloodFill(s, mTag);

            _result.Add(mEntity);
        }
    }
}