using System;
using System.Linq;
using TimeDRODPOF.TDLib;
using VeeTileEngine2012;

namespace TimeDRODPOF.TDComponents
{
    [Serializable]
    public class TDCRoachQueen : Component
    {
        private readonly int _turnsToSpawnMax;
        private int _turnsToSpawn;

        public TDCRoachQueen(bool mStalwart, int mTurnsToSpawn)
        {
            IsStalwart = mStalwart;
            _turnsToSpawnMax = _turnsToSpawn = mTurnsToSpawn;
        }

        public bool IsStalwart { get; set; }

        public override void NextTurn()
        {
            base.NextTurn();

            _turnsToSpawn--;

            if (_turnsToSpawn != 0) return;
            _turnsToSpawn = _turnsToSpawnMax;

            for (var iY = -1; iY < 2; iY++)
                for (var iX = -1; iX < 2; iX++)
                    if (TDLTags.GroundAllowedTags.Any(x => Field.HasEntityByTag(X + iX, Y + iY, x)))
                        if (!TDLTags.GroundObstacleTags.Any(x => Field.HasEntityByTag(X + iX, Y + iY, x)))
                            if (!Field.HasEntityByTag(X + iX, Y + iY, TDLTags.PreventSpawn))
                            {
                                TDLFactory.Tile = Field.GetTile(X + iX, Y + iY);
                                Field.AddEntity(IsStalwart ? TDLFactory.StalwartRoachEgg() : TDLFactory.RoachEgg());
                            }
        }
    }
}