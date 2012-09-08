using System;
using SFMLStart.Data;
using TimeDRODPOF.TDLib;
using VeeTileEngine2012;

namespace TimeDRODPOF.TDComponents
{
    [Serializable]
    public class TDCEgg : Component
    {
        private readonly TDCRender _renderComponent;

        public TDCEgg(TDCRender mRenderComponent, int mTurnsToHatch, bool mFriendly = false)
        {
            _renderComponent = mRenderComponent;

            TurnsToHatch = mTurnsToHatch;
            Friendly = mFriendly;

            _renderComponent.OnDraw += Draw;
        }
        public int TurnsToHatch { get; private set; }
        public bool Friendly { get; set; }

        private void Draw() { _renderComponent.GetSprite(0).TextureRect = Assets.GetTileset("roachtiles").GetTextureRect(4 - TurnsToHatch, 0); }

        public override void NextTurn()
        {
            base.NextTurn();

            TurnsToHatch--;
            if (TurnsToHatch != 0) return;
            Entity.Destroy();
            TDLFactory.Tile = Field.GetTile(X, Y);
            Field.AddEntity(Friendly ? TDLFactory.StalwartRoach() : TDLFactory.Roach());
        }
    }
}