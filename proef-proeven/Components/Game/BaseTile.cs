using Microsoft.Xna.Framework;
using proef_proeven.Components.Game.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace proef_proeven.Components.Game
{
    class BaseTile
    {
        private Vector2 position;
        public Vector2 Position
        {
            get
            {
                return position;
            }

            set
            {
                position = value;
                bounds.X = (int)position.X;
                bounds.Y = (int)position.Y;
            }
        }

        private Rectangle bounds;
        public Rectangle Bounds
        {
            get
            {
                return bounds;
            }

            set
            {
                position.X = value.X;
                position.Y = value.Y;
                bounds = value;
            }
        }

        public BaseTile(Rectangle bounds)
        {
            this.position = new Vector2(bounds.X, bounds.Y);
            this.bounds = bounds;
        }
    }
}
