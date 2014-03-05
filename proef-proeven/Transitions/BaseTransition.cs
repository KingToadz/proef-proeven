using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace proef_proeven.Transitions
{
    class BaseTransition
    {
        int time;

        public bool Started { get; protected set; }
        public bool Done { get; protected set; }

        public virtual void Update(GameTime time) { }
    }
}
