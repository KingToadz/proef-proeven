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
        public string backgroundPath { get; private set; }

        /// <summary>
        /// The player info for the human
        /// </summary>
        public PlayerInfo playerInfo { get; private set; }

        /// <summary>
        /// All the objects that can be clicked
        /// </summary>
        public List<ClickAbleInfo> clickObjectsInfo { get; private set; }
    }
}
