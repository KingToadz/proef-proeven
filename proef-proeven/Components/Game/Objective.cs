using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace proef_proeven.Components.Game
{
    class Objective
    {
        public bool Done;
        public string Name;

        public Objective(string name)
        {
            this.Name = name;
            this.Done = false;
        }
    }
}
