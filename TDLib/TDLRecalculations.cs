using System;
using SFMLStart.Data;
using TimeDRODPOF.TDComponents;
using VeeTileEngine2012;

namespace TimeDRODPOF.TDLib
{
    public static class TDLRecalculations
    {
        public static void RecalculateWallSprite(Entity mEntity, string mTag, Func<Entity, bool> mCondition)
        {
            var field = mEntity.Field;
            var x = mEntity.X;
            var y = mEntity.Y;
            var sprite = mEntity.GetComponent<TDCRender>().GetSprite(0);
            var tilesetName = "walltiles";
            var tag = mTag;

            #region Fill / Single / Single Cross
            if (field.HasEntityByTag(x - 1, y, tag, mCondition) &&
                field.HasEntityByTag(x + 1, y, tag, mCondition) &&
                field.HasEntityByTag(x - 1, y - 1, tag, mCondition) &&
                field.HasEntityByTag(x + 1, y - 1, tag, mCondition) &&
                field.HasEntityByTag(x - 1, y + 1, tag, mCondition) &&
                field.HasEntityByTag(x + 1, y + 1, tag, mCondition) &&
                field.HasEntityByTag(x, y - 1, tag, mCondition) &&
                field.HasEntityByTag(x, y + 1, tag, mCondition))
            {
                sprite.TextureRect = Assets.GetTileset(tilesetName).GetTextureRect("fill");
                return;
            }

            if (!field.HasEntityByTag(x, y - 1, tag, mCondition) &&
                !field.HasEntityByTag(x, y + 1, tag, mCondition) &&
                !field.HasEntityByTag(x - 1, y, tag, mCondition) &&
                !field.HasEntityByTag(x + 1, y, tag, mCondition))
            {
                sprite.TextureRect = Assets.GetTileset(tilesetName).GetTextureRect("single");
                return;
            }

            if (field.HasEntityByTag(x, y - 1, tag, mCondition) &&
                field.HasEntityByTag(x, y + 1, tag, mCondition) &&
                field.HasEntityByTag(x - 1, y, tag, mCondition) &&
                field.HasEntityByTag(x + 1, y, tag, mCondition) &&
                !field.HasEntityByTag(x - 1, y - 1, tag, mCondition) &&
                !field.HasEntityByTag(x - 1, y + 1, tag, mCondition) &&
                !field.HasEntityByTag(x + 1, y - 1, tag, mCondition) &&
                !field.HasEntityByTag(x + 1, y + 1, tag, mCondition))
            {
                sprite.TextureRect = Assets.GetTileset(tilesetName).GetTextureRect("single_cross");
                return;
            }
            #endregion

            #region End To Fill
            if (field.HasEntityByTag(x - 1, y, tag, mCondition) &&
                field.HasEntityByTag(x + 1, y, tag, mCondition) &&
                field.HasEntityByTag(x - 1, y - 1, tag, mCondition) &&
                field.HasEntityByTag(x + 1, y - 1, tag, mCondition) &&
                !field.HasEntityByTag(x - 1, y + 1, tag, mCondition) &&
                !field.HasEntityByTag(x + 1, y + 1, tag, mCondition) &&
                field.HasEntityByTag(x, y - 1, tag, mCondition) &&
                field.HasEntityByTag(x, y + 1, tag, mCondition))
            {
                sprite.TextureRect = Assets.GetTileset(tilesetName).GetTextureRect("end_tofill_n");
                return;
            }

            if (field.HasEntityByTag(x - 1, y, tag, mCondition) &&
                field.HasEntityByTag(x + 1, y, tag, mCondition) &&
                !field.HasEntityByTag(x - 1, y - 1, tag, mCondition) &&
                !field.HasEntityByTag(x + 1, y - 1, tag, mCondition) &&
                field.HasEntityByTag(x - 1, y + 1, tag, mCondition) &&
                field.HasEntityByTag(x + 1, y + 1, tag, mCondition) &&
                field.HasEntityByTag(x, y - 1, tag, mCondition) &&
                field.HasEntityByTag(x, y + 1, tag, mCondition))
            {
                sprite.TextureRect = Assets.GetTileset(tilesetName).GetTextureRect("end_tofill_s");
                return;
            }

            if (field.HasEntityByTag(x - 1, y, tag, mCondition) &&
                field.HasEntityByTag(x + 1, y, tag, mCondition) &&
                !field.HasEntityByTag(x - 1, y - 1, tag, mCondition) &&
                field.HasEntityByTag(x + 1, y - 1, tag, mCondition) &&
                !field.HasEntityByTag(x - 1, y + 1, tag, mCondition) &&
                field.HasEntityByTag(x + 1, y + 1, tag, mCondition) &&
                field.HasEntityByTag(x, y - 1, tag, mCondition) &&
                field.HasEntityByTag(x, y + 1, tag, mCondition))
            {
                sprite.TextureRect = Assets.GetTileset(tilesetName).GetTextureRect("end_tofill_e");
                return;
            }

            if (field.HasEntityByTag(x - 1, y, tag, mCondition) &&
                field.HasEntityByTag(x + 1, y, tag, mCondition) &&
                field.HasEntityByTag(x - 1, y - 1, tag, mCondition) &&
                !field.HasEntityByTag(x + 1, y - 1, tag, mCondition) &&
                field.HasEntityByTag(x - 1, y + 1, tag, mCondition) &&
                !field.HasEntityByTag(x + 1, y + 1, tag, mCondition) &&
                field.HasEntityByTag(x, y - 1, tag, mCondition) &&
                field.HasEntityByTag(x, y + 1, tag, mCondition))
            {
                sprite.TextureRect = Assets.GetTileset(tilesetName).GetTextureRect("end_tofill_w");
                return;
            }
            #endregion

            #region Internal Fill Corner
            if (field.HasEntityByTag(x - 1, y, tag, mCondition) &&
                field.HasEntityByTag(x + 1, y, tag, mCondition) &&
                field.HasEntityByTag(x - 1, y - 1, tag, mCondition) &&
                field.HasEntityByTag(x + 1, y - 1, tag, mCondition) &&
                !field.HasEntityByTag(x - 1, y + 1, tag, mCondition) &&
                field.HasEntityByTag(x + 1, y + 1, tag, mCondition) &&
                field.HasEntityByTag(x, y - 1, tag, mCondition) &&
                field.HasEntityByTag(x, y + 1, tag, mCondition))
            {
                sprite.TextureRect = Assets.GetTileset(tilesetName).GetTextureRect("internal_fill_corner_w_s");
                return;
            }

            if (field.HasEntityByTag(x - 1, y, tag, mCondition) &&
                field.HasEntityByTag(x + 1, y, tag, mCondition) &&
                field.HasEntityByTag(x - 1, y - 1, tag, mCondition) &&
                field.HasEntityByTag(x + 1, y - 1, tag, mCondition) &&
                field.HasEntityByTag(x - 1, y + 1, tag, mCondition) &&
                !field.HasEntityByTag(x + 1, y + 1, tag, mCondition) &&
                field.HasEntityByTag(x, y - 1, tag, mCondition) &&
                field.HasEntityByTag(x, y + 1, tag, mCondition))
            {
                sprite.TextureRect = Assets.GetTileset(tilesetName).GetTextureRect("internal_fill_corner_e_s");
                return;
            }

            if (field.HasEntityByTag(x - 1, y, tag, mCondition) &&
                field.HasEntityByTag(x + 1, y, tag, mCondition) &&
                !field.HasEntityByTag(x - 1, y - 1, tag, mCondition) &&
                field.HasEntityByTag(x + 1, y - 1, tag, mCondition) &&
                field.HasEntityByTag(x - 1, y + 1, tag, mCondition) &&
                field.HasEntityByTag(x + 1, y + 1, tag, mCondition) &&
                field.HasEntityByTag(x, y - 1, tag, mCondition) &&
                field.HasEntityByTag(x, y + 1, tag, mCondition))
            {
                sprite.TextureRect = Assets.GetTileset(tilesetName).GetTextureRect("internal_fill_corner_w_n");
                return;
            }

            if (field.HasEntityByTag(x - 1, y, tag, mCondition) &&
                field.HasEntityByTag(x + 1, y, tag, mCondition) &&
                field.HasEntityByTag(x - 1, y - 1, tag, mCondition) &&
                !field.HasEntityByTag(x + 1, y - 1, tag, mCondition) &&
                field.HasEntityByTag(x - 1, y + 1, tag, mCondition) &&
                field.HasEntityByTag(x + 1, y + 1, tag, mCondition) &&
                field.HasEntityByTag(x, y - 1, tag, mCondition) &&
                field.HasEntityByTag(x, y + 1, tag, mCondition))
            {
                sprite.TextureRect = Assets.GetTileset(tilesetName).GetTextureRect("internal_fill_corner_e_n");
                return;
            }
            #endregion

            #region Internal Double
            if (field.HasEntityByTag(x - 1, y, tag, mCondition) &&
                field.HasEntityByTag(x + 1, y, tag, mCondition) &&
                !field.HasEntityByTag(x - 1, y - 1, tag, mCondition) &&
                field.HasEntityByTag(x + 1, y - 1, tag, mCondition) &&
                field.HasEntityByTag(x - 1, y + 1, tag, mCondition) &&
                !field.HasEntityByTag(x + 1, y + 1, tag, mCondition) &&
                field.HasEntityByTag(x, y - 1, tag, mCondition) &&
                field.HasEntityByTag(x, y + 1, tag, mCondition))
            {
                sprite.TextureRect = Assets.GetTileset(tilesetName).GetTextureRect("internal_double_sw_ne");
                return;
            }

            if (field.HasEntityByTag(x - 1, y, tag, mCondition) &&
                field.HasEntityByTag(x + 1, y, tag, mCondition) &&
                field.HasEntityByTag(x - 1, y - 1, tag, mCondition) &&
                !field.HasEntityByTag(x + 1, y - 1, tag, mCondition) &&
                !field.HasEntityByTag(x - 1, y + 1, tag, mCondition) &&
                field.HasEntityByTag(x + 1, y + 1, tag, mCondition) &&
                field.HasEntityByTag(x, y - 1, tag, mCondition) &&
                field.HasEntityByTag(x, y + 1, tag, mCondition))
            {
                sprite.TextureRect = Assets.GetTileset(tilesetName).GetTextureRect("internal_double_se_nw");
                return;
            }
            #endregion

            #region X
            if (field.HasEntityByTag(x - 1, y, tag, mCondition) &&
                field.HasEntityByTag(x + 1, y, tag, mCondition) &&
                !field.HasEntityByTag(x - 1, y - 1, tag, mCondition) &&
                !field.HasEntityByTag(x + 1, y - 1, tag, mCondition) &&
                !field.HasEntityByTag(x - 1, y + 1, tag, mCondition) &&
                field.HasEntityByTag(x + 1, y + 1, tag, mCondition) &&
                field.HasEntityByTag(x, y - 1, tag, mCondition) &&
                field.HasEntityByTag(x, y + 1, tag, mCondition))
            {
                sprite.TextureRect = Assets.GetTileset(tilesetName).GetTextureRect("x_nw_fill_se");
                return;
            }

            if (field.HasEntityByTag(x - 1, y, tag, mCondition) &&
                field.HasEntityByTag(x + 1, y, tag, mCondition) &&
                field.HasEntityByTag(x - 1, y - 1, tag, mCondition) &&
                !field.HasEntityByTag(x + 1, y - 1, tag, mCondition) &&
                !field.HasEntityByTag(x - 1, y + 1, tag, mCondition) &&
                !field.HasEntityByTag(x + 1, y + 1, tag, mCondition) &&
                field.HasEntityByTag(x, y - 1, tag, mCondition) &&
                field.HasEntityByTag(x, y + 1, tag, mCondition))
            {
                sprite.TextureRect = Assets.GetTileset(tilesetName).GetTextureRect("x_se_fill_nw");
                return;
            }

            if (field.HasEntityByTag(x - 1, y, tag, mCondition) &&
                field.HasEntityByTag(x + 1, y, tag, mCondition) &&
                !field.HasEntityByTag(x - 1, y - 1, tag, mCondition) &&
                !field.HasEntityByTag(x + 1, y - 1, tag, mCondition) &&
                field.HasEntityByTag(x - 1, y + 1, tag, mCondition) &&
                !field.HasEntityByTag(x + 1, y + 1, tag, mCondition) &&
                field.HasEntityByTag(x, y - 1, tag, mCondition) &&
                field.HasEntityByTag(x, y + 1, tag, mCondition))
            {
                sprite.TextureRect = Assets.GetTileset(tilesetName).GetTextureRect("x_ne_fill_sw");
                return;
            }

            if (field.HasEntityByTag(x - 1, y, tag, mCondition) &&
                field.HasEntityByTag(x + 1, y, tag, mCondition) &&
                !field.HasEntityByTag(x - 1, y - 1, tag, mCondition) &&
                field.HasEntityByTag(x + 1, y - 1, tag, mCondition) &&
                !field.HasEntityByTag(x - 1, y + 1, tag, mCondition) &&
                !field.HasEntityByTag(x + 1, y + 1, tag, mCondition) &&
                field.HasEntityByTag(x, y - 1, tag, mCondition) &&
                field.HasEntityByTag(x, y + 1, tag, mCondition))
            {
                sprite.TextureRect = Assets.GetTileset(tilesetName).GetTextureRect("x_sw_fill_ne");
                return;
            }
            #endregion

            #region Single Vertical / Horizontal
            if (field.HasEntityByTag(x - 1, y, tag, mCondition) &&
                field.HasEntityByTag(x + 1, y, tag, mCondition) &&
                !field.HasEntityByTag(x, y - 1, tag, mCondition) &&
                !field.HasEntityByTag(x, y + 1, tag, mCondition)) sprite.TextureRect = Assets.GetTileset(tilesetName).GetTextureRect("single_horiz");

            if (!field.HasEntityByTag(x - 1, y, tag, mCondition) &&
                !field.HasEntityByTag(x + 1, y, tag, mCondition) &&
                field.HasEntityByTag(x, y - 1, tag, mCondition) &&
                field.HasEntityByTag(x, y + 1, tag, mCondition)) sprite.TextureRect = Assets.GetTileset(tilesetName).GetTextureRect("single_vert");
            #endregion

            #region End
            if (field.HasEntityByTag(x - 1, y, tag, mCondition) &&
                !field.HasEntityByTag(x + 1, y, tag, mCondition) &&
                !field.HasEntityByTag(x, y - 1, tag, mCondition) &&
                !field.HasEntityByTag(x, y + 1, tag, mCondition)) sprite.TextureRect = Assets.GetTileset(tilesetName).GetTextureRect("end_w");

            if (!field.HasEntityByTag(x - 1, y, tag, mCondition) &&
                field.HasEntityByTag(x + 1, y, tag, mCondition) &&
                !field.HasEntityByTag(x, y - 1, tag, mCondition) &&
                !field.HasEntityByTag(x, y + 1, tag, mCondition)) sprite.TextureRect = Assets.GetTileset(tilesetName).GetTextureRect("end_e");

            if (!field.HasEntityByTag(x - 1, y, tag, mCondition) &&
                !field.HasEntityByTag(x + 1, y, tag, mCondition) &&
                field.HasEntityByTag(x, y - 1, tag, mCondition) &&
                !field.HasEntityByTag(x, y + 1, tag, mCondition)) sprite.TextureRect = Assets.GetTileset(tilesetName).GetTextureRect("end_n");

            if (!field.HasEntityByTag(x - 1, y, tag, mCondition) &&
                !field.HasEntityByTag(x + 1, y, tag, mCondition) &&
                !field.HasEntityByTag(x, y - 1, tag, mCondition) &&
                field.HasEntityByTag(x, y + 1, tag, mCondition)) sprite.TextureRect = Assets.GetTileset(tilesetName).GetTextureRect("end_s");
            #endregion

            #region Single Corner Diagonal
            if (field.HasEntityByTag(x - 1, y, tag, mCondition) &&
                !field.HasEntityByTag(x + 1, y, tag, mCondition) &&
                field.HasEntityByTag(x, y - 1, tag, mCondition) &&
                !field.HasEntityByTag(x, y + 1, tag, mCondition)) sprite.TextureRect = Assets.GetTileset(tilesetName).GetTextureRect("single_corner_se");

            if (!field.HasEntityByTag(x - 1, y, tag, mCondition) &&
                field.HasEntityByTag(x + 1, y, tag, mCondition) &&
                field.HasEntityByTag(x, y - 1, tag, mCondition) &&
                !field.HasEntityByTag(x, y + 1, tag, mCondition)) sprite.TextureRect = Assets.GetTileset(tilesetName).GetTextureRect("single_corner_sw");

            if (field.HasEntityByTag(x - 1, y, tag, mCondition) &&
                !field.HasEntityByTag(x + 1, y, tag, mCondition) &&
                !field.HasEntityByTag(x, y - 1, tag, mCondition) &&
                field.HasEntityByTag(x, y + 1, tag, mCondition)) sprite.TextureRect = Assets.GetTileset(tilesetName).GetTextureRect("single_corner_ne");

            if (!field.HasEntityByTag(x - 1, y, tag, mCondition) &&
                field.HasEntityByTag(x + 1, y, tag, mCondition) &&
                !field.HasEntityByTag(x, y - 1, tag, mCondition) &&
                field.HasEntityByTag(x, y + 1, tag, mCondition)) sprite.TextureRect = Assets.GetTileset(tilesetName).GetTextureRect("single_corner_nw");
            #endregion

            #region Fill Corner Diagonal
            if (field.HasEntityByTag(x - 1, y, tag, mCondition) &&
                !field.HasEntityByTag(x + 1, y, tag, mCondition) &&
                field.HasEntityByTag(x - 1, y - 1, tag, mCondition) &&
                field.HasEntityByTag(x, y - 1, tag, mCondition) &&
                !field.HasEntityByTag(x, y + 1, tag, mCondition)) sprite.TextureRect = Assets.GetTileset(tilesetName).GetTextureRect("fill_corner_se");

            if (!field.HasEntityByTag(x - 1, y, tag, mCondition) &&
                field.HasEntityByTag(x + 1, y, tag, mCondition) &&
                field.HasEntityByTag(x + 1, y - 1, tag, mCondition) &&
                field.HasEntityByTag(x, y - 1, tag, mCondition) &&
                !field.HasEntityByTag(x, y + 1, tag, mCondition)) sprite.TextureRect = Assets.GetTileset(tilesetName).GetTextureRect("fill_corner_sw");

            if (field.HasEntityByTag(x - 1, y, tag, mCondition) &&
                !field.HasEntityByTag(x + 1, y, tag, mCondition) &&
                field.HasEntityByTag(x - 1, y + 1, tag, mCondition) &&
                !field.HasEntityByTag(x, y - 1, tag, mCondition) &&
                field.HasEntityByTag(x, y + 1, tag, mCondition)) sprite.TextureRect = Assets.GetTileset(tilesetName).GetTextureRect("fill_corner_ne");

            if (!field.HasEntityByTag(x - 1, y, tag, mCondition) &&
                field.HasEntityByTag(x + 1, y, tag, mCondition) &&
                field.HasEntityByTag(x + 1, y + 1, tag, mCondition) &&
                !field.HasEntityByTag(x, y - 1, tag, mCondition) &&
                field.HasEntityByTag(x, y + 1, tag, mCondition)) sprite.TextureRect = Assets.GetTileset(tilesetName).GetTextureRect("fill_corner_nw");
            #endregion

            #region Fill Corner Orthogonal
            if (field.HasEntityByTag(x - 1, y, tag, mCondition) &&
                field.HasEntityByTag(x + 1, y, tag, mCondition) &&
                field.HasEntityByTag(x - 1, y + 1, tag, mCondition) &&
                field.HasEntityByTag(x + 1, y + 1, tag, mCondition) &&
                !field.HasEntityByTag(x, y - 1, tag, mCondition) &&
                field.HasEntityByTag(x, y + 1, tag, mCondition)) sprite.TextureRect = Assets.GetTileset(tilesetName).GetTextureRect("fill_corner_n");

            if (field.HasEntityByTag(x - 1, y, tag, mCondition) &&
                field.HasEntityByTag(x + 1, y, tag, mCondition) &&
                field.HasEntityByTag(x - 1, y - 1, tag, mCondition) &&
                field.HasEntityByTag(x + 1, y - 1, tag, mCondition) &&
                field.HasEntityByTag(x, y - 1, tag, mCondition) &&
                !field.HasEntityByTag(x, y + 1, tag, mCondition)) sprite.TextureRect = Assets.GetTileset(tilesetName).GetTextureRect("fill_corner_s");

            if (!field.HasEntityByTag(x - 1, y, tag, mCondition) &&
                field.HasEntityByTag(x + 1, y, tag, mCondition) &&
                field.HasEntityByTag(x + 1, y - 1, tag, mCondition) &&
                field.HasEntityByTag(x + 1, y + 1, tag, mCondition) &&
                field.HasEntityByTag(x, y - 1, tag, mCondition) &&
                field.HasEntityByTag(x, y + 1, tag, mCondition)) sprite.TextureRect = Assets.GetTileset(tilesetName).GetTextureRect("fill_corner_w");

            if (field.HasEntityByTag(x - 1, y, tag, mCondition) &&
                !field.HasEntityByTag(x + 1, y, tag, mCondition) &&
                field.HasEntityByTag(x - 1, y - 1, tag, mCondition) &&
                field.HasEntityByTag(x - 1, y + 1, tag, mCondition) &&
                field.HasEntityByTag(x, y - 1, tag, mCondition) &&
                field.HasEntityByTag(x, y + 1, tag, mCondition)) sprite.TextureRect = Assets.GetTileset(tilesetName).GetTextureRect("fill_corner_e");
            #endregion

            #region T
            if (field.HasEntityByTag(x - 1, y, tag, mCondition) &&
                field.HasEntityByTag(x + 1, y, tag, mCondition) &&
                !field.HasEntityByTag(x - 1, y - 1, tag, mCondition) &&
                !field.HasEntityByTag(x + 1, y - 1, tag, mCondition) &&
                field.HasEntityByTag(x, y - 1, tag, mCondition) &&
                !field.HasEntityByTag(x, y + 1, tag, mCondition)) sprite.TextureRect = Assets.GetTileset(tilesetName).GetTextureRect("t_n");

            if (field.HasEntityByTag(x - 1, y, tag, mCondition) &&
                field.HasEntityByTag(x + 1, y, tag, mCondition) &&
                !field.HasEntityByTag(x - 1, y + 1, tag, mCondition) &&
                !field.HasEntityByTag(x + 1, y + 1, tag, mCondition) &&
                !field.HasEntityByTag(x, y - 1, tag, mCondition) &&
                field.HasEntityByTag(x, y + 1, tag, mCondition)) sprite.TextureRect = Assets.GetTileset(tilesetName).GetTextureRect("t_s");

            if (!field.HasEntityByTag(x - 1, y, tag, mCondition) &&
                field.HasEntityByTag(x + 1, y, tag, mCondition) &&
                !field.HasEntityByTag(x + 1, y - 1, tag, mCondition) &&
                !field.HasEntityByTag(x + 1, y + 1, tag, mCondition) &&
                field.HasEntityByTag(x, y - 1, tag, mCondition) &&
                field.HasEntityByTag(x, y + 1, tag, mCondition)) sprite.TextureRect = Assets.GetTileset(tilesetName).GetTextureRect("t_e");

            if (field.HasEntityByTag(x - 1, y, tag, mCondition) &&
                !field.HasEntityByTag(x + 1, y, tag, mCondition) &&
                !field.HasEntityByTag(x - 1, y - 1, tag, mCondition) &&
                !field.HasEntityByTag(x - 1, y + 1, tag, mCondition) &&
                field.HasEntityByTag(x, y - 1, tag, mCondition) &&
                field.HasEntityByTag(x, y + 1, tag, mCondition)) sprite.TextureRect = Assets.GetTileset(tilesetName).GetTextureRect("t_w");
            #endregion

            #region Y
            if (field.HasEntityByTag(x - 1, y, tag, mCondition) &&
                field.HasEntityByTag(x + 1, y, tag, mCondition) &&
                !field.HasEntityByTag(x - 1, y + 1, tag, mCondition) &&
                field.HasEntityByTag(x + 1, y + 1, tag, mCondition) &&
                !field.HasEntityByTag(x, y - 1, tag, mCondition) &&
                field.HasEntityByTag(x, y + 1, tag, mCondition)) sprite.TextureRect = Assets.GetTileset(tilesetName).GetTextureRect("y_w_fill_e_s");

            if (field.HasEntityByTag(x - 1, y, tag, mCondition) &&
                field.HasEntityByTag(x + 1, y, tag, mCondition) &&
                !field.HasEntityByTag(x - 1, y - 1, tag, mCondition) &&
                field.HasEntityByTag(x + 1, y - 1, tag, mCondition) &&
                !field.HasEntityByTag(x, y + 1, tag, mCondition) &&
                field.HasEntityByTag(x, y - 1, tag, mCondition)) sprite.TextureRect = Assets.GetTileset(tilesetName).GetTextureRect("y_w_fill_e_n");

            if (field.HasEntityByTag(x - 1, y, tag, mCondition) &&
                field.HasEntityByTag(x + 1, y, tag, mCondition) &&
                field.HasEntityByTag(x - 1, y + 1, tag, mCondition) &&
                !field.HasEntityByTag(x + 1, y + 1, tag, mCondition) &&
                !field.HasEntityByTag(x, y - 1, tag, mCondition) &&
                field.HasEntityByTag(x, y + 1, tag, mCondition)) sprite.TextureRect = Assets.GetTileset(tilesetName).GetTextureRect("y_e_fill_w_s");

            if (!field.HasEntityByTag(x - 1, y, tag, mCondition) &&
                field.HasEntityByTag(x + 1, y, tag, mCondition) &&
                field.HasEntityByTag(x + 1, y - 1, tag, mCondition) &&
                !field.HasEntityByTag(x + 1, y + 1, tag, mCondition) &&
                field.HasEntityByTag(x, y - 1, tag, mCondition) &&
                field.HasEntityByTag(x, y + 1, tag, mCondition)) sprite.TextureRect = Assets.GetTileset(tilesetName).GetTextureRect("y_s_fill_e_n");

            if (field.HasEntityByTag(x - 1, y, tag, mCondition) &&
                field.HasEntityByTag(x + 1, y, tag, mCondition) &&
                field.HasEntityByTag(x - 1, y - 1, tag, mCondition) &&
                !field.HasEntityByTag(x + 1, y - 1, tag, mCondition) &&
                field.HasEntityByTag(x, y - 1, tag, mCondition) &&
                !field.HasEntityByTag(x, y + 1, tag, mCondition)) sprite.TextureRect = Assets.GetTileset(tilesetName).GetTextureRect("y_w_fill_w_n");

            if (field.HasEntityByTag(x - 1, y, tag, mCondition) &&
                !field.HasEntityByTag(x + 1, y, tag, mCondition) &&
                field.HasEntityByTag(x - 1, y - 1, tag, mCondition) &&
                !field.HasEntityByTag(x - 1, y + 1, tag, mCondition) &&
                field.HasEntityByTag(x, y - 1, tag, mCondition) &&
                field.HasEntityByTag(x, y + 1, tag, mCondition)) sprite.TextureRect = Assets.GetTileset(tilesetName).GetTextureRect("y_s_fill_w_n");

            if (!field.HasEntityByTag(x - 1, y, tag, mCondition) &&
                field.HasEntityByTag(x + 1, y, tag, mCondition) &&
                !field.HasEntityByTag(x + 1, y - 1, tag, mCondition) &&
                field.HasEntityByTag(x + 1, y + 1, tag, mCondition) &&
                field.HasEntityByTag(x, y - 1, tag, mCondition) &&
                field.HasEntityByTag(x, y + 1, tag, mCondition)) sprite.TextureRect = Assets.GetTileset(tilesetName).GetTextureRect("y_n_fill_e_s");

            if (field.HasEntityByTag(x - 1, y, tag, mCondition) &&
                !field.HasEntityByTag(x + 1, y, tag, mCondition) &&
                !field.HasEntityByTag(x - 1, y - 1, tag, mCondition) &&
                field.HasEntityByTag(x - 1, y + 1, tag, mCondition) &&
                field.HasEntityByTag(x, y - 1, tag, mCondition) &&
                field.HasEntityByTag(x, y + 1, tag, mCondition)) sprite.TextureRect = Assets.GetTileset(tilesetName).GetTextureRect("y_n_fill_w_s");
            #endregion
        }

        public static void RecalculateDoorSprite(Entity mEntity, string mTag, Func<Entity, bool> mCondition)
        {
            var field = mEntity.Field;
            var x = mEntity.X;
            var y = mEntity.Y;
            var sprite = mEntity.GetComponent<TDCRender>().GetSprite(0);
            var tilesetName = "doortiles";
            var tag = mTag;

            #region Fill / Single / Single Cross
            if (field.HasEntityByTag(x - 1, y, tag, mCondition) &&
                field.HasEntityByTag(x + 1, y, tag, mCondition) &&
                field.HasEntityByTag(x - 1, y - 1, tag, mCondition) &&
                field.HasEntityByTag(x + 1, y - 1, tag, mCondition) &&
                field.HasEntityByTag(x - 1, y + 1, tag, mCondition) &&
                field.HasEntityByTag(x + 1, y + 1, tag, mCondition) &&
                field.HasEntityByTag(x, y - 1, tag, mCondition) &&
                field.HasEntityByTag(x, y + 1, tag, mCondition))
            {
                sprite.TextureRect = Assets.GetTileset(tilesetName).GetTextureRect("fill");
                return;
            }

            if (!field.HasEntityByTag(x, y - 1, tag, mCondition) &&
                !field.HasEntityByTag(x, y + 1, tag, mCondition) &&
                !field.HasEntityByTag(x - 1, y, tag, mCondition) &&
                !field.HasEntityByTag(x + 1, y, tag, mCondition))
            {
                sprite.TextureRect = Assets.GetTileset(tilesetName).GetTextureRect("single");
                return;
            }

            if (field.HasEntityByTag(x, y - 1, tag, mCondition) &&
                field.HasEntityByTag(x, y + 1, tag, mCondition) &&
                field.HasEntityByTag(x - 1, y, tag, mCondition) &&
                field.HasEntityByTag(x + 1, y, tag, mCondition) &&
                !field.HasEntityByTag(x - 1, y - 1, tag, mCondition) &&
                !field.HasEntityByTag(x - 1, y + 1, tag, mCondition) &&
                !field.HasEntityByTag(x + 1, y - 1, tag, mCondition) &&
                !field.HasEntityByTag(x + 1, y + 1, tag, mCondition))
            {
                sprite.TextureRect = Assets.GetTileset(tilesetName).GetTextureRect("fill");
                return;
            }
            #endregion

            #region End To Fill
            if (field.HasEntityByTag(x - 1, y, tag, mCondition) &&
                field.HasEntityByTag(x + 1, y, tag, mCondition) &&
                field.HasEntityByTag(x - 1, y - 1, tag, mCondition) &&
                field.HasEntityByTag(x + 1, y - 1, tag, mCondition) &&
                !field.HasEntityByTag(x - 1, y + 1, tag, mCondition) &&
                !field.HasEntityByTag(x + 1, y + 1, tag, mCondition) &&
                field.HasEntityByTag(x, y - 1, tag, mCondition) &&
                field.HasEntityByTag(x, y + 1, tag, mCondition))
            {
                sprite.TextureRect = Assets.GetTileset(tilesetName).GetTextureRect("fill");
                return;
            }

            if (field.HasEntityByTag(x - 1, y, tag, mCondition) &&
                field.HasEntityByTag(x + 1, y, tag, mCondition) &&
                !field.HasEntityByTag(x - 1, y - 1, tag, mCondition) &&
                !field.HasEntityByTag(x + 1, y - 1, tag, mCondition) &&
                field.HasEntityByTag(x - 1, y + 1, tag, mCondition) &&
                field.HasEntityByTag(x + 1, y + 1, tag, mCondition) &&
                field.HasEntityByTag(x, y - 1, tag, mCondition) &&
                field.HasEntityByTag(x, y + 1, tag, mCondition))
            {
                sprite.TextureRect = Assets.GetTileset(tilesetName).GetTextureRect("fill");
                return;
            }

            if (field.HasEntityByTag(x - 1, y, tag, mCondition) &&
                field.HasEntityByTag(x + 1, y, tag, mCondition) &&
                !field.HasEntityByTag(x - 1, y - 1, tag, mCondition) &&
                field.HasEntityByTag(x + 1, y - 1, tag, mCondition) &&
                !field.HasEntityByTag(x - 1, y + 1, tag, mCondition) &&
                field.HasEntityByTag(x + 1, y + 1, tag, mCondition) &&
                field.HasEntityByTag(x, y - 1, tag, mCondition) &&
                field.HasEntityByTag(x, y + 1, tag, mCondition))
            {
                sprite.TextureRect = Assets.GetTileset(tilesetName).GetTextureRect("fill");
                return;
            }

            if (field.HasEntityByTag(x - 1, y, tag, mCondition) &&
                field.HasEntityByTag(x + 1, y, tag, mCondition) &&
                field.HasEntityByTag(x - 1, y - 1, tag, mCondition) &&
                !field.HasEntityByTag(x + 1, y - 1, tag, mCondition) &&
                field.HasEntityByTag(x - 1, y + 1, tag, mCondition) &&
                !field.HasEntityByTag(x + 1, y + 1, tag, mCondition) &&
                field.HasEntityByTag(x, y - 1, tag, mCondition) &&
                field.HasEntityByTag(x, y + 1, tag, mCondition))
            {
                sprite.TextureRect = Assets.GetTileset(tilesetName).GetTextureRect("fill");
                return;
            }
            #endregion

            #region Internal Fill Corner
            if (field.HasEntityByTag(x - 1, y, tag, mCondition) &&
                field.HasEntityByTag(x + 1, y, tag, mCondition) &&
                field.HasEntityByTag(x - 1, y - 1, tag, mCondition) &&
                field.HasEntityByTag(x + 1, y - 1, tag, mCondition) &&
                !field.HasEntityByTag(x - 1, y + 1, tag, mCondition) &&
                field.HasEntityByTag(x + 1, y + 1, tag, mCondition) &&
                field.HasEntityByTag(x, y - 1, tag, mCondition) &&
                field.HasEntityByTag(x, y + 1, tag, mCondition))
            {
                sprite.TextureRect = Assets.GetTileset(tilesetName).GetTextureRect("fill");
                return;
            }

            if (field.HasEntityByTag(x - 1, y, tag, mCondition) &&
                field.HasEntityByTag(x + 1, y, tag, mCondition) &&
                field.HasEntityByTag(x - 1, y - 1, tag, mCondition) &&
                field.HasEntityByTag(x + 1, y - 1, tag, mCondition) &&
                field.HasEntityByTag(x - 1, y + 1, tag, mCondition) &&
                !field.HasEntityByTag(x + 1, y + 1, tag, mCondition) &&
                field.HasEntityByTag(x, y - 1, tag, mCondition) &&
                field.HasEntityByTag(x, y + 1, tag, mCondition))
            {
                sprite.TextureRect = Assets.GetTileset(tilesetName).GetTextureRect("fill");
                return;
            }

            if (field.HasEntityByTag(x - 1, y, tag, mCondition) &&
                field.HasEntityByTag(x + 1, y, tag, mCondition) &&
                !field.HasEntityByTag(x - 1, y - 1, tag, mCondition) &&
                field.HasEntityByTag(x + 1, y - 1, tag, mCondition) &&
                field.HasEntityByTag(x - 1, y + 1, tag, mCondition) &&
                field.HasEntityByTag(x + 1, y + 1, tag, mCondition) &&
                field.HasEntityByTag(x, y - 1, tag, mCondition) &&
                field.HasEntityByTag(x, y + 1, tag, mCondition))
            {
                sprite.TextureRect = Assets.GetTileset(tilesetName).GetTextureRect("fill");
                return;
            }

            if (field.HasEntityByTag(x - 1, y, tag, mCondition) &&
                field.HasEntityByTag(x + 1, y, tag, mCondition) &&
                field.HasEntityByTag(x - 1, y - 1, tag, mCondition) &&
                !field.HasEntityByTag(x + 1, y - 1, tag, mCondition) &&
                field.HasEntityByTag(x - 1, y + 1, tag, mCondition) &&
                field.HasEntityByTag(x + 1, y + 1, tag, mCondition) &&
                field.HasEntityByTag(x, y - 1, tag, mCondition) &&
                field.HasEntityByTag(x, y + 1, tag, mCondition))
            {
                sprite.TextureRect = Assets.GetTileset(tilesetName).GetTextureRect("fill");
                return;
            }
            #endregion

            #region Internal Double
            if (field.HasEntityByTag(x - 1, y, tag, mCondition) &&
                field.HasEntityByTag(x + 1, y, tag, mCondition) &&
                !field.HasEntityByTag(x - 1, y - 1, tag, mCondition) &&
                field.HasEntityByTag(x + 1, y - 1, tag, mCondition) &&
                field.HasEntityByTag(x - 1, y + 1, tag, mCondition) &&
                !field.HasEntityByTag(x + 1, y + 1, tag, mCondition) &&
                field.HasEntityByTag(x, y - 1, tag, mCondition) &&
                field.HasEntityByTag(x, y + 1, tag, mCondition))
            {
                sprite.TextureRect = Assets.GetTileset(tilesetName).GetTextureRect("fill");
                return;
            }

            if (field.HasEntityByTag(x - 1, y, tag, mCondition) &&
                field.HasEntityByTag(x + 1, y, tag, mCondition) &&
                field.HasEntityByTag(x - 1, y - 1, tag, mCondition) &&
                !field.HasEntityByTag(x + 1, y - 1, tag, mCondition) &&
                !field.HasEntityByTag(x - 1, y + 1, tag, mCondition) &&
                field.HasEntityByTag(x + 1, y + 1, tag, mCondition) &&
                field.HasEntityByTag(x, y - 1, tag, mCondition) &&
                field.HasEntityByTag(x, y + 1, tag, mCondition))
            {
                sprite.TextureRect = Assets.GetTileset(tilesetName).GetTextureRect("fill");
                return;
            }
            #endregion

            #region X
            if (field.HasEntityByTag(x - 1, y, tag, mCondition) &&
                field.HasEntityByTag(x + 1, y, tag, mCondition) &&
                !field.HasEntityByTag(x - 1, y - 1, tag, mCondition) &&
                !field.HasEntityByTag(x + 1, y - 1, tag, mCondition) &&
                !field.HasEntityByTag(x - 1, y + 1, tag, mCondition) &&
                field.HasEntityByTag(x + 1, y + 1, tag, mCondition) &&
                field.HasEntityByTag(x, y - 1, tag, mCondition) &&
                field.HasEntityByTag(x, y + 1, tag, mCondition))
            {
                sprite.TextureRect = Assets.GetTileset(tilesetName).GetTextureRect("fill");
                return;
            }

            if (field.HasEntityByTag(x - 1, y, tag, mCondition) &&
                field.HasEntityByTag(x + 1, y, tag, mCondition) &&
                field.HasEntityByTag(x - 1, y - 1, tag, mCondition) &&
                !field.HasEntityByTag(x + 1, y - 1, tag, mCondition) &&
                !field.HasEntityByTag(x - 1, y + 1, tag, mCondition) &&
                !field.HasEntityByTag(x + 1, y + 1, tag, mCondition) &&
                field.HasEntityByTag(x, y - 1, tag, mCondition) &&
                field.HasEntityByTag(x, y + 1, tag, mCondition))
            {
                sprite.TextureRect = Assets.GetTileset(tilesetName).GetTextureRect("fill");
                return;
            }

            if (field.HasEntityByTag(x - 1, y, tag, mCondition) &&
                field.HasEntityByTag(x + 1, y, tag, mCondition) &&
                !field.HasEntityByTag(x - 1, y - 1, tag, mCondition) &&
                !field.HasEntityByTag(x + 1, y - 1, tag, mCondition) &&
                field.HasEntityByTag(x - 1, y + 1, tag, mCondition) &&
                !field.HasEntityByTag(x + 1, y + 1, tag, mCondition) &&
                field.HasEntityByTag(x, y - 1, tag, mCondition) &&
                field.HasEntityByTag(x, y + 1, tag, mCondition))
            {
                sprite.TextureRect = Assets.GetTileset(tilesetName).GetTextureRect("fill");
                return;
            }

            if (field.HasEntityByTag(x - 1, y, tag, mCondition) &&
                field.HasEntityByTag(x + 1, y, tag, mCondition) &&
                !field.HasEntityByTag(x - 1, y - 1, tag, mCondition) &&
                field.HasEntityByTag(x + 1, y - 1, tag, mCondition) &&
                !field.HasEntityByTag(x - 1, y + 1, tag, mCondition) &&
                !field.HasEntityByTag(x + 1, y + 1, tag, mCondition) &&
                field.HasEntityByTag(x, y - 1, tag, mCondition) &&
                field.HasEntityByTag(x, y + 1, tag, mCondition))
            {
                sprite.TextureRect = Assets.GetTileset(tilesetName).GetTextureRect("fill");
                return;
            }
            #endregion

            #region Single Vertical / Horizontal
            if (field.HasEntityByTag(x - 1, y, tag, mCondition) &&
                field.HasEntityByTag(x + 1, y, tag, mCondition) &&
                !field.HasEntityByTag(x, y - 1, tag, mCondition) &&
                !field.HasEntityByTag(x, y + 1, tag, mCondition)) sprite.TextureRect = Assets.GetTileset(tilesetName).GetTextureRect("single_horiz");

            if (!field.HasEntityByTag(x - 1, y, tag, mCondition) &&
                !field.HasEntityByTag(x + 1, y, tag, mCondition) &&
                field.HasEntityByTag(x, y - 1, tag, mCondition) &&
                field.HasEntityByTag(x, y + 1, tag, mCondition)) sprite.TextureRect = Assets.GetTileset(tilesetName).GetTextureRect("single_vert");
            #endregion

            #region End
            if (field.HasEntityByTag(x - 1, y, tag, mCondition) &&
                !field.HasEntityByTag(x + 1, y, tag, mCondition) &&
                !field.HasEntityByTag(x, y - 1, tag, mCondition) &&
                !field.HasEntityByTag(x, y + 1, tag, mCondition)) sprite.TextureRect = Assets.GetTileset(tilesetName).GetTextureRect("end_w");

            if (!field.HasEntityByTag(x - 1, y, tag, mCondition) &&
                field.HasEntityByTag(x + 1, y, tag, mCondition) &&
                !field.HasEntityByTag(x, y - 1, tag, mCondition) &&
                !field.HasEntityByTag(x, y + 1, tag, mCondition)) sprite.TextureRect = Assets.GetTileset(tilesetName).GetTextureRect("end_e");

            if (!field.HasEntityByTag(x - 1, y, tag, mCondition) &&
                !field.HasEntityByTag(x + 1, y, tag, mCondition) &&
                field.HasEntityByTag(x, y - 1, tag, mCondition) &&
                !field.HasEntityByTag(x, y + 1, tag, mCondition)) sprite.TextureRect = Assets.GetTileset(tilesetName).GetTextureRect("end_n");

            if (!field.HasEntityByTag(x - 1, y, tag, mCondition) &&
                !field.HasEntityByTag(x + 1, y, tag, mCondition) &&
                !field.HasEntityByTag(x, y - 1, tag, mCondition) &&
                field.HasEntityByTag(x, y + 1, tag, mCondition)) sprite.TextureRect = Assets.GetTileset(tilesetName).GetTextureRect("end_s");
            #endregion

            #region Single Corner Diagonal
            if (field.HasEntityByTag(x - 1, y, tag, mCondition) &&
                !field.HasEntityByTag(x + 1, y, tag, mCondition) &&
                field.HasEntityByTag(x, y - 1, tag, mCondition) &&
                !field.HasEntityByTag(x, y + 1, tag, mCondition)) sprite.TextureRect = Assets.GetTileset(tilesetName).GetTextureRect("fill_corner_se");

            if (!field.HasEntityByTag(x - 1, y, tag, mCondition) &&
                field.HasEntityByTag(x + 1, y, tag, mCondition) &&
                field.HasEntityByTag(x, y - 1, tag, mCondition) &&
                !field.HasEntityByTag(x, y + 1, tag, mCondition)) sprite.TextureRect = Assets.GetTileset(tilesetName).GetTextureRect("fill_corner_sw");

            if (field.HasEntityByTag(x - 1, y, tag, mCondition) &&
                !field.HasEntityByTag(x + 1, y, tag, mCondition) &&
                !field.HasEntityByTag(x, y - 1, tag, mCondition) &&
                field.HasEntityByTag(x, y + 1, tag, mCondition)) sprite.TextureRect = Assets.GetTileset(tilesetName).GetTextureRect("fill_corner_ne");

            if (!field.HasEntityByTag(x - 1, y, tag, mCondition) &&
                field.HasEntityByTag(x + 1, y, tag, mCondition) &&
                !field.HasEntityByTag(x, y - 1, tag, mCondition) &&
                field.HasEntityByTag(x, y + 1, tag, mCondition)) sprite.TextureRect = Assets.GetTileset(tilesetName).GetTextureRect("fill_corner_nw");
            #endregion

            #region Fill Corner Diagonal
            if (field.HasEntityByTag(x - 1, y, tag, mCondition) &&
                !field.HasEntityByTag(x + 1, y, tag, mCondition) &&
                field.HasEntityByTag(x - 1, y - 1, tag, mCondition) &&
                field.HasEntityByTag(x, y - 1, tag, mCondition) &&
                !field.HasEntityByTag(x, y + 1, tag, mCondition)) sprite.TextureRect = Assets.GetTileset(tilesetName).GetTextureRect("fill_corner_se");

            if (!field.HasEntityByTag(x - 1, y, tag, mCondition) &&
                field.HasEntityByTag(x + 1, y, tag, mCondition) &&
                field.HasEntityByTag(x + 1, y - 1, tag, mCondition) &&
                field.HasEntityByTag(x, y - 1, tag, mCondition) &&
                !field.HasEntityByTag(x, y + 1, tag, mCondition)) sprite.TextureRect = Assets.GetTileset(tilesetName).GetTextureRect("fill_corner_sw");

            if (field.HasEntityByTag(x - 1, y, tag, mCondition) &&
                !field.HasEntityByTag(x + 1, y, tag, mCondition) &&
                field.HasEntityByTag(x - 1, y + 1, tag, mCondition) &&
                !field.HasEntityByTag(x, y - 1, tag, mCondition) &&
                field.HasEntityByTag(x, y + 1, tag, mCondition)) sprite.TextureRect = Assets.GetTileset(tilesetName).GetTextureRect("fill_corner_ne");

            if (!field.HasEntityByTag(x - 1, y, tag, mCondition) &&
                field.HasEntityByTag(x + 1, y, tag, mCondition) &&
                field.HasEntityByTag(x + 1, y + 1, tag, mCondition) &&
                !field.HasEntityByTag(x, y - 1, tag, mCondition) &&
                field.HasEntityByTag(x, y + 1, tag, mCondition)) sprite.TextureRect = Assets.GetTileset(tilesetName).GetTextureRect("fill_corner_nw");
            #endregion

            #region Fill Corner Orthogonal
            if (field.HasEntityByTag(x - 1, y, tag, mCondition) &&
                field.HasEntityByTag(x + 1, y, tag, mCondition) &&
                field.HasEntityByTag(x - 1, y + 1, tag, mCondition) &&
                field.HasEntityByTag(x + 1, y + 1, tag, mCondition) &&
                !field.HasEntityByTag(x, y - 1, tag, mCondition) &&
                field.HasEntityByTag(x, y + 1, tag, mCondition)) sprite.TextureRect = Assets.GetTileset(tilesetName).GetTextureRect("fill_corner_n");

            if (field.HasEntityByTag(x - 1, y, tag, mCondition) &&
                field.HasEntityByTag(x + 1, y, tag, mCondition) &&
                field.HasEntityByTag(x - 1, y - 1, tag, mCondition) &&
                field.HasEntityByTag(x + 1, y - 1, tag, mCondition) &&
                field.HasEntityByTag(x, y - 1, tag, mCondition) &&
                !field.HasEntityByTag(x, y + 1, tag, mCondition)) sprite.TextureRect = Assets.GetTileset(tilesetName).GetTextureRect("fill_corner_s");

            if (!field.HasEntityByTag(x - 1, y, tag, mCondition) &&
                field.HasEntityByTag(x + 1, y, tag, mCondition) &&
                field.HasEntityByTag(x + 1, y - 1, tag, mCondition) &&
                field.HasEntityByTag(x + 1, y + 1, tag, mCondition) &&
                field.HasEntityByTag(x, y - 1, tag, mCondition) &&
                field.HasEntityByTag(x, y + 1, tag, mCondition)) sprite.TextureRect = Assets.GetTileset(tilesetName).GetTextureRect("fill_corner_w");

            if (field.HasEntityByTag(x - 1, y, tag, mCondition) &&
                !field.HasEntityByTag(x + 1, y, tag, mCondition) &&
                field.HasEntityByTag(x - 1, y - 1, tag, mCondition) &&
                field.HasEntityByTag(x - 1, y + 1, tag, mCondition) &&
                field.HasEntityByTag(x, y - 1, tag, mCondition) &&
                field.HasEntityByTag(x, y + 1, tag, mCondition)) sprite.TextureRect = Assets.GetTileset(tilesetName).GetTextureRect("fill_corner_e");
            #endregion

            #region T
            if (field.HasEntityByTag(x - 1, y, tag, mCondition) &&
                field.HasEntityByTag(x + 1, y, tag, mCondition) &&
                !field.HasEntityByTag(x - 1, y - 1, tag, mCondition) &&
                !field.HasEntityByTag(x + 1, y - 1, tag, mCondition) &&
                field.HasEntityByTag(x, y - 1, tag, mCondition) &&
                !field.HasEntityByTag(x, y + 1, tag, mCondition)) sprite.TextureRect = Assets.GetTileset(tilesetName).GetTextureRect("fill_corner_s");

            if (field.HasEntityByTag(x - 1, y, tag, mCondition) &&
                field.HasEntityByTag(x + 1, y, tag, mCondition) &&
                !field.HasEntityByTag(x - 1, y + 1, tag, mCondition) &&
                !field.HasEntityByTag(x + 1, y + 1, tag, mCondition) &&
                !field.HasEntityByTag(x, y - 1, tag, mCondition) &&
                field.HasEntityByTag(x, y + 1, tag, mCondition)) sprite.TextureRect = Assets.GetTileset(tilesetName).GetTextureRect("fill_corner_n");

            if (!field.HasEntityByTag(x - 1, y, tag, mCondition) &&
                field.HasEntityByTag(x + 1, y, tag, mCondition) &&
                !field.HasEntityByTag(x + 1, y - 1, tag, mCondition) &&
                !field.HasEntityByTag(x + 1, y + 1, tag, mCondition) &&
                field.HasEntityByTag(x, y - 1, tag, mCondition) &&
                field.HasEntityByTag(x, y + 1, tag, mCondition)) sprite.TextureRect = Assets.GetTileset(tilesetName).GetTextureRect("fill_corner_w");

            if (field.HasEntityByTag(x - 1, y, tag, mCondition) &&
                !field.HasEntityByTag(x + 1, y, tag, mCondition) &&
                !field.HasEntityByTag(x - 1, y - 1, tag, mCondition) &&
                !field.HasEntityByTag(x - 1, y + 1, tag, mCondition) &&
                field.HasEntityByTag(x, y - 1, tag, mCondition) &&
                field.HasEntityByTag(x, y + 1, tag, mCondition)) sprite.TextureRect = Assets.GetTileset(tilesetName).GetTextureRect("fill_corner_e");
            #endregion

            #region Y
            if (field.HasEntityByTag(x - 1, y, tag, mCondition) &&
                field.HasEntityByTag(x + 1, y, tag, mCondition) &&
                !field.HasEntityByTag(x - 1, y + 1, tag, mCondition) &&
                field.HasEntityByTag(x + 1, y + 1, tag, mCondition) &&
                !field.HasEntityByTag(x, y - 1, tag, mCondition) &&
                field.HasEntityByTag(x, y + 1, tag, mCondition)) sprite.TextureRect = Assets.GetTileset(tilesetName).GetTextureRect("fill_corner_n");

            if (field.HasEntityByTag(x - 1, y, tag, mCondition) &&
                field.HasEntityByTag(x + 1, y, tag, mCondition) &&
                !field.HasEntityByTag(x - 1, y - 1, tag, mCondition) &&
                field.HasEntityByTag(x + 1, y - 1, tag, mCondition) &&
                !field.HasEntityByTag(x, y + 1, tag, mCondition) &&
                field.HasEntityByTag(x, y - 1, tag, mCondition)) sprite.TextureRect = Assets.GetTileset(tilesetName).GetTextureRect("fill_corner_s");

            if (field.HasEntityByTag(x - 1, y, tag, mCondition) &&
                field.HasEntityByTag(x + 1, y, tag, mCondition) &&
                field.HasEntityByTag(x - 1, y + 1, tag, mCondition) &&
                !field.HasEntityByTag(x + 1, y + 1, tag, mCondition) &&
                !field.HasEntityByTag(x, y - 1, tag, mCondition) &&
                field.HasEntityByTag(x, y + 1, tag, mCondition)) sprite.TextureRect = Assets.GetTileset(tilesetName).GetTextureRect("fill_corner_n");

            if (!field.HasEntityByTag(x - 1, y, tag, mCondition) &&
                field.HasEntityByTag(x + 1, y, tag, mCondition) &&
                field.HasEntityByTag(x + 1, y - 1, tag, mCondition) &&
                !field.HasEntityByTag(x + 1, y + 1, tag, mCondition) &&
                field.HasEntityByTag(x, y - 1, tag, mCondition) &&
                field.HasEntityByTag(x, y + 1, tag, mCondition)) sprite.TextureRect = Assets.GetTileset(tilesetName).GetTextureRect("fill_corner_w");

            if (field.HasEntityByTag(x - 1, y, tag, mCondition) &&
                field.HasEntityByTag(x + 1, y, tag, mCondition) &&
                field.HasEntityByTag(x - 1, y - 1, tag, mCondition) &&
                !field.HasEntityByTag(x + 1, y - 1, tag, mCondition) &&
                field.HasEntityByTag(x, y - 1, tag, mCondition) &&
                !field.HasEntityByTag(x, y + 1, tag, mCondition)) sprite.TextureRect = Assets.GetTileset(tilesetName).GetTextureRect("fill_corner_s");

            if (field.HasEntityByTag(x - 1, y, tag, mCondition) &&
                !field.HasEntityByTag(x + 1, y, tag, mCondition) &&
                field.HasEntityByTag(x - 1, y - 1, tag, mCondition) &&
                !field.HasEntityByTag(x - 1, y + 1, tag, mCondition) &&
                field.HasEntityByTag(x, y - 1, tag, mCondition) &&
                field.HasEntityByTag(x, y + 1, tag, mCondition)) sprite.TextureRect = Assets.GetTileset(tilesetName).GetTextureRect("fill_corner_e");

            if (!field.HasEntityByTag(x - 1, y, tag, mCondition) &&
                field.HasEntityByTag(x + 1, y, tag, mCondition) &&
                !field.HasEntityByTag(x + 1, y - 1, tag, mCondition) &&
                field.HasEntityByTag(x + 1, y + 1, tag, mCondition) &&
                field.HasEntityByTag(x, y - 1, tag, mCondition) &&
                field.HasEntityByTag(x, y + 1, tag, mCondition)) sprite.TextureRect = Assets.GetTileset(tilesetName).GetTextureRect("fill_corner_w");

            if (field.HasEntityByTag(x - 1, y, tag, mCondition) &&
                !field.HasEntityByTag(x + 1, y, tag, mCondition) &&
                !field.HasEntityByTag(x - 1, y - 1, tag, mCondition) &&
                field.HasEntityByTag(x - 1, y + 1, tag, mCondition) &&
                field.HasEntityByTag(x, y - 1, tag, mCondition) &&
                field.HasEntityByTag(x, y + 1, tag, mCondition)) sprite.TextureRect = Assets.GetTileset(tilesetName).GetTextureRect("fill_corner_e");
            #endregion
        }
    }
}