using System;
using TimeDRODPOF.TDGame;
using VeeTileEngine2012;

namespace TimeDRODPOF.TDComponents
{
    [Serializable]
    public class TDCPathmapper : Component
    {
        [NonSerialized] private readonly TDGInstance _instance;
        private string _pathmapName;

        public TDCPathmapper(TDGInstance mInstance, string mPathmapName)
        {
            _instance = mInstance;
            _pathmapName = mPathmapName;
        }

        public void ChangePathmap(string mPathmapName)
        {
            _instance.GetPathmap(_pathmapName).RemoveEntity(Entity);
            _instance.GetPathmap(mPathmapName).AddEntity(Entity);
            _pathmapName = mPathmapName;
        }

        public override void Added()
        {
            base.Added();
            _instance.GetPathmap(_pathmapName).AddEntity(Entity);
        }
        public override void Removed()
        {
            base.Removed();
            _instance.GetPathmap(_pathmapName).RemoveEntity(Entity);
        }
    }
}