using Microsoft.Xna.Framework;
using Newtonsoft.Json;
using proef_proeven.Components.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace proef_proeven.Components.LevelCreator
{
    class ClickableObjectManager
    {
        static ClickableObjectManager instance;
        public static ClickableObjectManager Instance
        {
            get
            {
                if (instance == null)
                    instance = new ClickableObjectManager();
                return instance;
            }
        }

        public bool NoBoxesLoaded
        {
            get
            {
                return customboxes.Count == 0;
            }
        }

        Dictionary<string, Rectangle> customboxes;

        ClickableObjectManager()
        {
            customboxes = new Dictionary<string, Rectangle>();
        }

        public void ChangeBox(string imagePath, Rectangle box)
        {
            if (customboxes.ContainsKey(imagePath))
                customboxes[imagePath] = box;
            else
                customboxes.Add(imagePath, box);
        }

        public Rectangle GetBoundingbox(string imagePath)
        {
            if (customboxes.ContainsKey(imagePath))
                return customboxes[imagePath];
            return new Rectangle();
        }

        public bool Load()
        {
            IOHelper.Instance.CreateDirectory(Constants.DATA_DIR);

            if (IOHelper.Instance.DoesFileExist(Constants.DATA_DIR + "customboxes.json"))
            {
                customboxes = JsonConvert.DeserializeObject<Dictionary<string, Rectangle>>(IOHelper.Instance.ReadFile(Constants.DATA_DIR + "customboxes.json"));
                return true;
            }
            return false;
        }

        public void Save()
        {
            IOHelper.Instance.WriteFile(Constants.DATA_DIR + "customboxes.json", JsonConvert.SerializeObject(customboxes));
        }
    }
}
