using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using proef_proeven.Components.LoadData;
using proef_proeven.Components.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace proef_proeven.Components.LevelCreator
{
    class BackgroundLayer : BaseLayer
    {
        List<Tuple<string, Texture2D>> backgrounds;
        int currentBG;

        public BackgroundLayer()
        {
            currentBG = 0;
        }

        public override void LoadContent(ContentManager content)
        {
            List<string> backgroundPath = IOHelper.Instance.FilesInDirectory(@"\Content\level-editor\backgrounds", "*.png");

            backgrounds = new List<Tuple<string, Texture2D>>();

            for (int i = 0; i < backgroundPath.Count; i++)
            {
                backgroundPath[i] = backgroundPath[i].Remove(backgroundPath[i].LastIndexOf('.'), backgroundPath[i].Length - backgroundPath[i].LastIndexOf('.'));

                if (backgroundPath[i].Contains(@"\Content\"))
                    backgroundPath[i] = backgroundPath[i].Remove(0, @"\Content\".Length);

                backgrounds.Add(new Tuple<string, Texture2D>(backgroundPath[i], content.Load<Texture2D>(backgroundPath[i])));
            }

            base.LoadContent(content);
        }

        public override void LoadLevel(LevelFormat level)
        {
            List<ClickAbleInfo> click = level.clickObjectsInfo;

            try
            {
                currentBG = 0;
                for (int i = 0; i < backgrounds.Count; i++)
                {
                    if (backgrounds[i].Item1 == level.backgroundPath)
                    {
                        currentBG = i;
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            base.LoadLevel(level);
        }

        public override List<object> getObjects()
        {
            return new List<object>(){ backgrounds[currentBG] };
        }

        public override void Update(GameTime time)
        {
            if (InputHelper.Instance.IsKeyPressed(Keys.A) && currentBG > 0)
                currentBG--;
            else if (InputHelper.Instance.IsKeyPressed(Keys.D) && currentBG < backgrounds.Count - 1)
                currentBG++;

            base.Update(time);
        }

        public override void Draw(SpriteBatch batch)
        {
            batch.Draw(backgrounds[currentBG].Item2, Vector2.Zero, Color.White);
            base.Draw(batch);
        }
    }
}
