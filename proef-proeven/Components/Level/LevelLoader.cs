using proef_proeven.Components.Game;
using proef_proeven.Components.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace proef_proeven.Components.Level
{
    class LevelLoader
    {
        Player player;
        string backgroundPath;
        List<ClickAbleObject> objectsToMove;

        const string levelDir = @"\Levels\";

        public LevelLoader(int levelID)
        {
            if(IOHelper.Instance.CreateDirectory(levelDir))
            {
                if(IOHelper.Instance.DoesFileExist(levelDir + "level" + levelID + ".json"))
                {

                }
            }
        }

    }
}
