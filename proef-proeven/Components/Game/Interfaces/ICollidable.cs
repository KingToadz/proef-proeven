using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace proef_proeven.Components.Game.Interfaces
{
    interface ICollidAble
    {
        Rectangle Boundingbox
        {
            get;
        }

        Vector2 Delta
        {
            get;
        }

        Player.Movement CurMovement
        {
            get;
        }

        /// <summary>
        /// Called when an ICollidable collides with an other ICollidable
        /// </summary>
        /// <param name="collider">The other object that collides</param>
        void Collide(ICollidAble collider);
    }
}
