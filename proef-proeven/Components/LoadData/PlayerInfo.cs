using Microsoft.Xna.Framework;
using proef_proeven.Components.Game;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace proef_proeven.Components.LoadData
{
    class PlayerInfo
    {
        public Player.Movement startMovement;
        public Vector2 position;

        public bool useCustomBoundingbox;
        public int x;
        public int y;
        public int width;
        public int height;
    }
}
