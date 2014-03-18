using proef_proeven.Components.Game;
using proef_proeven.Components.LoadData;
using proef_proeven.Components.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using System.IO;

namespace proef_proeven.Components.Level
{
    class LevelLoader
    {
        public LevelFormat level { get; private set; }
        public bool LevelLoaded = false;

        const string levelDir = @"\Levels\";

        public LevelLoader(int levelID)
        {

            try
            {
                if (IOHelper.Instance.CreateDirectory(levelDir))
                {
                    if (IOHelper.Instance.DoesFileExist(levelDir + "level" + levelID + ".json"))
                    {
                        level = JsonConvert.DeserializeObject<LevelFormat>(IOHelper.Instance.ReadFile(levelDir + "level" + levelID + ".json"));
                        LevelLoaded = true;
                    }
                    else
                    {
                        throw new FileNotFoundException(levelDir + "level" + levelID + ".json doesn't exsist");
                    }
                }
                else
                {
                    throw new Exception("Couldn't create dir");
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
