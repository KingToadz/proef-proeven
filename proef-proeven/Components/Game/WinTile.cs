using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using proef_proeven.Components.Game.Interfaces;
using proef_proeven.Components.LoadData;
using proef_proeven.Components.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace proef_proeven.Components.Game
{
    class WinTile : BaseTile, ICollidAble, IDrawAble
    {
        public Rectangle Boundingbox
        {
            get { return Bounds; }
        }

        public Vector2 Delta
        {
            get { return Vector2.Zero; }
        }

        public Player.Movement CurMovement
        {
            get { return Player.Movement.Idle; }
        }

        /// <summary>
        /// Start information of the WinTile.
        /// Used for level saving
        /// </summary>
        public MovementTileInfo Info
        {
            get
            {
                MovementTileInfo info = new MovementTileInfo();
                info.X = Bounds.X;
                info.Y = Bounds.Y;
                info.Width = Bounds.Width;
                info.Height = Bounds.Height;
                info.WinningTile = true;
                info.movement = Player.Movement.Idle;
                return info;
            }
        }

        public WinTile(Rectangle size)
            :base(size)
        {}

        public void Collide(ICollidAble collider)
        {
            // shouldn't be called
        }

        public void Draw(SpriteBatch batch)
        {
            if(Game1.Instance.CreatorMode)
            {
                RectangleRender.Draw(batch, Bounds);
            }
        }
    }
}
