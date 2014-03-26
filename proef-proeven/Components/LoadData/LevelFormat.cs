using Microsoft.Xna.Framework;
using proef_proeven.Components.Game;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace proef_proeven.Components.LoadData
{
    class LevelFormat
    {
        /// <summary>
        /// The background path from the content dir. 
        /// </summary>
        public string backgroundPath { get; set; }

        /// <summary>
        /// The player info for the human
        /// </summary>
        public PlayerInfo playerInfo { get; set; }

        /// <summary>
        /// All the objects that can be clicked
        /// </summary>
        public List<ClickAbleInfo> clickObjectsInfo { get; set; }

        /// <summary>
        /// All the tiles that can control the players movement. WinTiles included
        /// </summary>
        public List<MovementTileInfo> moveTiles { get; set; }

        /// <summary>
        /// All the tiles that will be used as decoration
        /// </summary>
        public List<DecorationInfo> decoration { get; set; }

        /// <summary>
        /// Constructor news the 2 lists
        /// </summary>
        public LevelFormat()
        {
            clickObjectsInfo = new List<ClickAbleInfo>();
            moveTiles = new List<MovementTileInfo>();
            decoration = new List<DecorationInfo>();
        }

    }
}
