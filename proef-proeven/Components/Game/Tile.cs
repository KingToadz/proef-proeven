using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using proef_proeven.Components.Game.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace proef_proeven.Components.Game
{
    class Tile : BaseTile, IDrawAble
    {
        Texture2D img;
        Rectangle cliprec;

        /// <summary>
        /// Tile constructor
        /// </summary>
        /// <param name="sheet">The texture for the tile</param>
        /// <param name="bounds">bounds where it needs to be shown and size</param>
        /// <param name="cliprect">The cliprect for the tile</param>
        public Tile(Texture2D sheet, Rectangle bounds, Rectangle cliprect)
            : base(bounds)
        {
            img = sheet;
            this.cliprec = cliprect;
        }

        /// <summary>
        /// Draw this tile
        /// </summary>
        /// <param name="batch">The active spritebatch</param>
        public void Draw(SpriteBatch batch)
        {
            batch.Draw(img, Bounds, cliprec, Color.White);
        }
    }
}
