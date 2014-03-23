using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
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

        public bool NoTexturesLoaded
        {
            get
            {
                return textures.Count == 0;
            }
        }

        Dictionary<string, Rectangle> customboxes;
        public List<Tuple<string, Texture2D>> textures { get; private set; }

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

        public Dictionary<string, Rectangle> GetAllBoxes()
        {
            return customboxes;
        }

        public void LoadTextures(ContentManager content)
        {
            List<string> files = IOHelper.Instance.FilesInDirectory(@"\Content\level-editor\moveable", "*.png");
            textures = new List<Tuple<string, Texture2D>>();

            for (int i = 0; i < files.Count; i++)
            {
                files[i] = files[i].Remove(files[i].LastIndexOf('.'), files[i].Length - files[i].LastIndexOf('.'));

                if (files[i].Contains(@"\Content\"))
                    files[i] = files[i].Remove(0, @"\Content\".Length);

                Tuple<string, Texture2D> tup = new Tuple<string, Texture2D>(files[i], content.Load<Texture2D>(files[i]));
                textures.Add(tup);

                if(GetBoundingbox(files[i]).IsEmpty)
                {
                    ChangeBox(files[i], tup.Item2.Bounds);
                }
            }
        }

        public bool LoadBoxes()
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
