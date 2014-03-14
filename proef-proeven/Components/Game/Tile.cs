using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace proef_proeven.Components.Game
{
    class Tile
    {
        public Vector2 position;
        Texture2D img;
        Rectangle cliprec;

        public Tile(Texture2D sheet, Vector2 position, Rectangle cliprect)
        {
            img = sheet;
            this.cliprec = cliprect;
            this.position = position;
        }

        public void Draw(SpriteBatch batch)
        {
            batch.Draw(img, new Rectangle((int)position.X, (int)position.Y, cliprec.Width, cliprec.Height), cliprec, Color.White);
        }
    }
}
