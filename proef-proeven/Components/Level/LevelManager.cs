using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Newtonsoft.Json;
using proef_proeven.Components.Util;
using proef_proeven.Components.LoadData;

namespace proef_proeven.Components.Level
{
    class LevelManager
    {
        static LevelManager instance;
        public static LevelManager Instance
        {
            get
            {
                if(instance == null)
                {
                    instance = new LevelManager();
                    instance.FirstRun();
                }
                return instance;
            }
        }

        List<LevelData> levelData;

        string dataDir = @"\Data\";

        /// <summary>
        /// Create directory and files if needed otherwise loads them
        /// </summary>
        private void FirstRun()
        {
            IOHelper.Instance.CreateDirectory(dataDir);

            if (!IOHelper.Instance.DoesFileExist(dataDir + "levels.json"))
                CreateAndSaveLevels();
            else
            {
                LoadData();

                if (levelData == null)
                    CreateAndSaveLevels();
            }
        }

        public int LevelCount
        {
            get { return levelData.Count; }
        }

        /// <summary>
        /// Get an level by its ID
        /// </summary>
        /// <param name="id">The ID of the level</param>
        /// <returns>The level or an error level</returns>
        public LevelData GetLevel(int id)
        {
            if(CheckID(id))
            {
                return levelData[id];
            }
            return new LevelData(-1, "ERROR level " + id + " does not exist");
        }

        /// <summary>
        /// Creates the levels file and save's it to the right dir
        /// </summary>
        private void CreateAndSaveLevels()
        {
            levelData = new List<LevelData>();

            for (int i = 0; i < 8; i++)
            {
                levelData.Add(new LevelData(i));
                if (i == 0)
                    levelData[i].Unlocked = true;
            }

            IOHelper.Instance.WriteFile(dataDir + "levels.json", JsonConvert.SerializeObject(levelData));
        }

        /// <summary>
        /// This will save the level progress
        /// </summary>
        public void SaveData()
        {
            IOHelper.Instance.WriteFile(dataDir + "levels.json", JsonConvert.SerializeObject(levelData));
        }

        /// <summary>
        /// This will load the level progress
        /// </summary>
        public void LoadData()
        {
            levelData = JsonConvert.DeserializeObject<List<LevelData>>(IOHelper.Instance.ReadFile(dataDir + "levels.json"));
        }

        /// <summary>
        /// This will reset the level progress
        /// </summary>
        public void ResetData()
        {
            for(int i = 0; i < levelData.Count; i++)
            {
                // this will not lock the first level
                LockLevel(i);
                ChangeLevelData(i, 0, false);
            }
        }

        /// <summary>
        /// This will lock an level
        /// </summary>
        /// <param name="id">The ID of the level to lock</param>
        private void LockLevel(int id)
        {
            if(id > 0 && CheckID(id))
                levelData[id].Unlocked = false;
        }

        /// <summary>
        /// Unlock an level
        /// </summary>
        /// <param name="id">The ID of the level to unlock</param>
        public void UnlockLevel(int id)
        {
            if (CheckID(id))
            {
                levelData[id].Unlocked = true;
            }
        }

        /// <summary>
        /// Check if an level is unlocked
        /// </summary>
        /// <param name="id">The ID of the level to check</param>
        /// <returns>If the level is unlocked</returns>
        public bool IsUnlocked(int id)
        {
            return levelData[id].Unlocked;
        }

        /// <summary>
        /// Win an level
        /// </summary>
        /// <param name="id">The ID of the winning level</param>
        /// <param name="tries">The amount of tries from that level</param>
        public void WinLevel(int id, int tries)
        {
            ChangeLevelData(id, tries, true);
        }

        /// <summary>
        /// Change the data of an level
        /// </summary>
        /// <param name="id">the ID of the level that needs to be changed</param>
        /// <param name="tries">The amount of tries</param>
        /// <param name="done">If the level is completed</param>
        public void ChangeLevelData(int id, int tries, bool done)
        {
            if (CheckID(id))
            {
                levelData[id].Tries = tries;
                levelData[id].Beaten = done;
            }
        }

        /// <summary>
        /// Check if the level id is in bounds
        /// </summary>
        /// <param name="id">The ID to check</param>
        /// <returns>If the id can be used</returns>
        private bool CheckID(int id)
        {
            return id >= 0 && id < levelData.Count;
        }

        public void SaveLevel(LevelFormat lvl, int levelID)
        {
            IOHelper.Instance.WriteFile(@"\levels\" + "level"+levelID+".json", JsonConvert.SerializeObject(lvl));
        }
    }
}
