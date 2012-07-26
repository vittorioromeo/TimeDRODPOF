using System;
using System.Collections.Generic;
using TimeDRODPOF.TDLib;
using VeeTileEngine2012;

namespace TimeDRODPOF.TDComponents
{
    [Serializable]
    public class TDCHotTile : Component
    {
        private readonly List<Entity> _onMeEntities;
        private readonly List<int> _onMeTurnsToBurn;
        private readonly TDCSpecialSquare _specialSquareComponent;
        private readonly int _turnsToBurn;

        public TDCHotTile(TDCSpecialSquare mSpecialSquareComponent, int mTurnsToBurn)
        {
            _specialSquareComponent = mSpecialSquareComponent;
            _onMeEntities = new List<Entity>();
            _onMeTurnsToBurn = new List<int>();
            _turnsToBurn = mTurnsToBurn;

            _specialSquareComponent.OnMoveFromAllowed += SpecialSquareFrom;
            _specialSquareComponent.OnMoveToAllowed += SpecialSquareTo;
        }

        public override void NextTurn()
        {
            base.NextTurn();

            var indexesToRemove = new List<int>();

            for (var i = 0; i < _onMeEntities.Count; i++)
            {
                _onMeTurnsToBurn[i] = _onMeTurnsToBurn[i] - 1;

                if (_onMeTurnsToBurn[i] != 0) continue;

                TDLMethods.Burn(_onMeEntities[i]);
                indexesToRemove.Add(i);
            }

            foreach (var i in indexesToRemove)
            {
                _onMeEntities.RemoveAt(i);
                _onMeTurnsToBurn.RemoveAt(i);
            }
        }

        private bool SpecialSquareFrom(Entity mEntity, int mNextX, int mNextY, bool mNextSuccess, out bool mAbort)
        {
            mAbort = false;
            if (!mNextSuccess) return true;

            var indexesToRemove = new List<int>();

            for (var i = 0; i < _onMeEntities.Count; i++)
                if (_onMeEntities[i] == mEntity)
                    indexesToRemove.Add(i);

            foreach (var i in indexesToRemove)
            {
                _onMeEntities.RemoveAt(i);
                _onMeTurnsToBurn.RemoveAt(i);
            }
            return true;
        }

        private bool SpecialSquareTo(Entity mEntity, int mNextX, int mNextY, bool mNextSuccess, out bool mAbort)
        {
            mAbort = false;
            if (!mNextSuccess) return true;

            if (!mEntity.HasTag(TDLTags.NotAffectedByHeat))
                if (!_onMeEntities.Contains(mEntity))
                {
                    _onMeEntities.Add(mEntity);
                    _onMeTurnsToBurn.Add(_turnsToBurn);
                }

            return true;
        }
    }
}