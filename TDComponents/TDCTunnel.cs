using System;
using System.Collections.Generic;
using System.Linq;
using SFMLStart.Data;
using TimeDRODPOF.TDLib;
using VeeTileEngine2012;

namespace TimeDRODPOF.TDComponents
{
    [Serializable]
    public class TDCTunnel : Component
    {
        private readonly TDCDirection _directionComponent;
        private readonly TDCRender _renderComponent;
        private readonly TDCSpecialSquare _specialSquareComponent;

        public TDCTunnel(TDCRender mRenderComponent, TDCDirection mDirectionComponent, TDCSpecialSquare mSpecialSquareComponent)
        {
            _renderComponent = mRenderComponent;
            _directionComponent = mDirectionComponent;
            _specialSquareComponent = mSpecialSquareComponent;

            _renderComponent.OnDraw += Draw;
            _specialSquareComponent.OnMoveFromAllowed += SpecialSquareFrom;
            _specialSquareComponent.OnMoveToAllowed += SpecialSquareTo;
        }

        private bool SpecialSquareFrom(Entity mEntity, int mNextX, int mNextY, bool mNextSuccess, out bool mAbort)
        {
            mAbort = false;

            var isTunnelUser = mEntity.HasTag(TDLTags.TunnelUser);
            if (isTunnelUser) return true;

            var cKiller = mEntity.GetComponent<TDCKiller>();
            return cKiller != null && cKiller.TargetTags.Any(x => Field.HasEntityByTag(X, Y, x));
        }

        private bool SpecialSquareTo(Entity mEntity, int mNextX, int mNextY, bool mNextSuccess, out bool mAbort)
        {
            mAbort = false;

            if (!mEntity.HasTag(TDLTags.TunnelUser)) return true;

            var directionVector = _directionComponent.DirectionVector;

            if (mNextX == directionVector.X && mNextY == directionVector.Y)
            {
                TDLSounds.Play("SoundTunnel");

                var rayX = X;
                var rayY = Y;
                while (true)
                {
                    rayX += directionVector.X;
                    rayY += directionVector.Y;
                    if (Field.GetTileOrNullTile(rayX, rayY) == Field.NullTile) break;
                    if (Field.HasEntityByTag(rayX, rayY, TDLTags.Tunnel)) goto tunnelFound;
                }

                rayX = X;
                rayY = Y;
                while (true)
                {
                    rayX -= directionVector.X;
                    rayY -= directionVector.Y;
                    if (Field.GetTileOrNullTile(rayX, rayY) == Field.NullTile) return false;
                    if (Field.HasEntityByTag(rayX, rayY, TDLTags.Tunnel)) goto tunnelFound;
                }

                tunnelFound:
                mAbort = true;
                if (TDLTags.GroundAllowedTags.Any(x => Field.HasEntityByTag(rayX, rayY, x)))
                    if (!TDLTags.GroundObstacleTags.Any(x => Field.HasEntityByTag(rayX, rayY, x)))
                        foreach (var entity in new List<Entity>(Field.GetEntitiesByTag(X, Y, TDLTags.TunnelUser)))
                            entity.Move(rayX, rayY);

                return false;
            }

            return true;
        }

        private void Draw() { _renderComponent.GetSprite(0).TextureRect = Assets.GetTileset("dirtiles").GetTextureRect(_directionComponent.DirectionString); }
    }
}