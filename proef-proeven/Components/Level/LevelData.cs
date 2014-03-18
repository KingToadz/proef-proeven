using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace proef_proeven.Components.Level
{
    class LevelData
    {
        public int ID = -1;
        public int Tries = 0;
        public bool Beaten = false;
        public bool Unlocked = false;
        public string Name = "Level";

        public LevelData() { }

        public LevelData(int id)
        {
            ID = id;
            Name = "Level " + id;
        }

        public LevelData(int id, string name)
        {
            ID = id;
            Name = name;
        }
    }
}
