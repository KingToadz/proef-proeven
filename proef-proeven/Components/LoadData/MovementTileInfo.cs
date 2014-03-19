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
        /// X position of the tile
        /// </summary>
        public int X;

        /// <summary>
        /// Y position of the tile
        /// </summary>
        public int Y;

        /// <summary>
        /// The width of the tile
        /// </summary>
        public int Width;

        /// <summary>
        /// The height of the tile
        /// </summary>
        public int Height;

        /// <summary>
        /// Is this the winning tile
        /// </summary>
        public bool WinningTile;

        /// <summary>
        /// The movement of the tile
        /// </summary>
        public Player.Movement movement;
    }
}
