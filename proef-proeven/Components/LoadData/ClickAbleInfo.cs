﻿using Microsoft.Xna.Framework;
using proef_proeven.Components.Game;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace proef_proeven.Components.LoadData
{
    class ClickAbleInfo
    {
        public List<Vector2> moveToPosition;
        public Vector2 position;
        public int objectiveID;
        public string texturePath;
        public bool useCustomBounds;
        public int X, Y, Width, Height;
    }
}
