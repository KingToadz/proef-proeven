using Microsoft.Xna.Framework;
using proef_proeven.Components.Game;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace proef_proeven.Components.LoadData
{
    class ClickAbleInfo
    {
        public Vector2 moveToPosition;
        public Vector2 position;
        public int objectiveID;
        public string texturePath;

        /*
        public void n()
        {
            ClickAbleObject info = new ClickAbleObject();
            info.Position = position;
            info.moveToPosition = moveToPosition;
            info.ObjectiveID = objectiveID;
            info.Image = Content.load<Texture2D>(texturePath);
        }
         */
    }
}
