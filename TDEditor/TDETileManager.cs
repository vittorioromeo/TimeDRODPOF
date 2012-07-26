#region
using System.Collections.Generic;

#endregion

namespace TimeDRODPOF.TDEditor
{
    public class TDETileManager
    {
        #region Delegates
        public delegate void EntityCreated(TDEEntity mEntity);
        #endregion

        public EntityCreated OnEntityCreated;

        public TDETileManager(int mWidth, int mHeight) { Clear(mWidth, mHeight); }

        public TDETile[,] Tiles { get; set; }
        public List<TDEEntity> Entities { get; set; }
        public int Width { get { return Tiles.GetLength(0); } }
        public int Height { get { return Tiles.GetLength(1); } }

        private void InvokeOnEntityCreated(TDEEntity mEntity)
        {
            var handler = OnEntityCreated;
            if (handler != null) handler(mEntity);
        }

        public void Clear(int mWidth, int mHeight)
        {
            Entities = new List<TDEEntity>();
            Tiles = new TDETile[mWidth,mHeight];

            for (var iY = 0; iY < mHeight; iY++)
                for (var iX = 0; iX < mWidth; iX++)
                {
                    Tiles[iX, iY] = new TDETile(iX, iY);
                    Tiles[iX, iY].Clear();
                }
        }

        public void CreateEntity(int mX, int mY, TDEEntity mEntity)
        {
            if (!IsValid(mX, mY)) return;

            Entities.Add(mEntity);
            Tiles[mX, mY].AddEntity(mEntity);
            mEntity.Tile = Tiles[mX, mY];

            InvokeOnEntityCreated(mEntity);
        }

        public void DeleteEntity(TDEEntity mEntity)
        {
            Entities.Remove(mEntity);
            mEntity.Tile.RemoveEntity(mEntity);
        }

        public bool IsValid(int mX, int mY) { return mX > -1 && mX < Width && mY > -1 && mY < Height; }
    }
}