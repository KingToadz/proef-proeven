using Microsoft.Xna.Framework;
using proef_proeven.Components.Game;
using proef_proeven.Components.Game.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace proef_proeven.Components
{
    class WinTile : ICollidable
    {
        private Rectangle boundingbox;
        public Rectangle Boundingbox
        {
            get { return boundingbox; }
        }

        public Vector2 Delta
        {
            get { return Vector2.Zero; }
        }

        public Player.Movement CurMovement
        {
            get { return Player.Movement.Idle; }
        }

        public void Collide(ICollidable collider)
        {
            // shouldn't be called
        }
    }
}
