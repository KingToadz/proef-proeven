using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace proef_proeven.Components.Game.Interfaces
{
    interface IResetAble
    {
        /// <summary>
        /// Called to reset the object to the state of the beginning of the level
        /// </summary>
        void Reset();
    }
}
