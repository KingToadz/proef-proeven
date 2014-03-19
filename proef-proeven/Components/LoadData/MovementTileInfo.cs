using Microsoft.Xna.Framework;
using proef_proeven.Components.Game;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace proef_proeven.Components.LoadData
{
    class MovementTileInfo
    {
        /// <summary>
        /// Size of the direction tile
        /// </summary>
        public Vector2 Position;

        /// <summary>
        /// Size in an vector2 for cleaner json. x = width, y = height
        /// </summary>
        public Vector2 Size;

        /// <summary>
        /// The movement of the tile
        /// </summary>
        public Player.Movement movement;
    }
}
