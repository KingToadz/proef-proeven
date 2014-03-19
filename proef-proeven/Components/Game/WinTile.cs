using Microsoft.Xna.Framework;
using proef_proeven.Components.Game.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace proef_proeven.Components.Game
{
    class WinTile : BaseTile, ICollidable
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

        public WinTile(Rectangle size)
            :base(size)
        {}

        public void Collide(ICollidable collider)
        {
            // shouldn't be called
        }
    }
}
