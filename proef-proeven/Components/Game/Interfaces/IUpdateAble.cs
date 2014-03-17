using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace proef_proeven.Components.Game.Interfaces
{
    /// <summary>
    /// If inhereting from this class it will be updated
    /// </summary>
    interface IUpdateAble
    {
        /// <summary>
        /// Update the object
        /// </summary>
        /// <param name="dt">The GameTime of Game1 update</param>
        void Update(GameTime dt);
    }
}
