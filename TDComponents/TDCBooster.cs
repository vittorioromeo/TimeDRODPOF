using System;
using System.Collections.Generic;
using System.Linq;
using TimeDRODPOF.TDLib;
using VeeTileEngine2012;

namespace TimeDRODPOF.TDComponents
{
    [Serializable]
    public class TDCBooster : Component
    {
        private readonly TDCDirection _directionComponent;
        private readonly TDCSpecialSquare _specialSquareComponent;
        private readonly TDCSwitch _switchComponent;

        public TDCBooster(TDCDirection mDirectionComponent, TDCSwitch mSwitchComponent, TDCSpecialSquare mSpecialSquareComponent)
        {
            _directionComponent = mDirectionComponent;
            _switchComponent = mSwitchComponent;
            _specialSquareComponent = mSpecialSquareComponent;

            _specialSquareComponent.OnMoveFromAllowed += SpecialSquareFrom;
        }

        private bool SpecialSquareFrom(Entity mEntity, int mNextX, int mNextY, bool mNextSuccess, out bool mAbort)
        {
            mAbort = false;

            if (!mEntity.HasTag(TDLTags.BoosterUser) || _switchComponent.IsOff) return true;

            var directionVector = _directionComponent.DirectionVector;

            if (mNextX == directionVector.X && mNextY == directionVector.Y)
            {
                int rayX = X, rayY = Y;
                while (true)
                {
                    rayX += directionVector.X;
                    rayY += directionVector.Y;
                    //if (!result.Instance.IsTileValid(rayX, rayY)) break;
                    if (Field.GetTileOrNullTile(rayX, rayY) == Field.NullTile) return true;
                    if (!Field.HasEntityByTag(rayX, rayY, TDLTags.Booster)) continue;

                    var valid = true;

                    foreach (var booster in Field.GetEntitiesByTag(rayX, rayY, TDLTags.Booster))
                        if (booster.GetComponent<TDCSwitch>().IsOff) valid = false;

                    if (valid)
                        goto boosterFound;
                }

                boosterFound:
                if (TDLTags.GroundAllowedTags.Any(x => Field.HasEntityByTag(rayX, rayY, x)))
                    if (!TDLTags.GroundObstacleTags.Any(x => Field.HasEntityByTag(rayX, rayY, x)))
                    {
                        TDLSounds.Play("SoundTunnel");
                        mAbort = true;

                        foreach (var entity in new List<Entity>(Field.GetEntities(X, Y).Where(x => x.HasTag(TDLTags.BoosterUser))))
                            entity.Move(rayX, rayY);

                        return false;
                    }
            }

            return true;
        }
    }
}