using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace proef_proeven.Components.Game.Interfaces
{
    interface ICollidable
    {
        Rectangle Boundingbox
        {
            get;
            set;
        }

        Vector2 Delta
        {
            get;
            set;
        }

        /// <summary>
        /// Called when an ICollidable collides with an other ICollidable
        /// </summary>
        /// <param name="collider">The other object that collides</param>
        void Collide(ICollidable collider);
    }
}
