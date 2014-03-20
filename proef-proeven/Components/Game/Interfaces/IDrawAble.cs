using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace proef_proeven.Components.Game.Interfaces
{
    /// <summary>
    /// If inhereting from this class it will be drawn
    /// </summary>
    interface IDrawAble
    {
        /// <summary>
        /// The z index to sort objects
        /// </summary>
        /*
        int zIndex
        {
            get;
        }*/

        /// <summary>
        /// Draw's the objects here
        /// </summary>
        /// <param name="batch">An active batch</param>
        void Draw(SpriteBatch batch);
    }
}
