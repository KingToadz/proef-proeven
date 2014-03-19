using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using proef_proeven.Components;
using proef_proeven.Components.Game;
using proef_proeven.Components.Game.Interfaces;
using proef_proeven.Components.Level;
using proef_proeven.Components.LoadData;
using proef_proeven.Components.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace proef_proeven.Screens
{
    class LevelCreator : BaseScreen
    {

        List<string> layerInfo = new List<string>(){
            "Background. Use A or D to switch.",
            "Object layer. Use A or D to switch object. W and S for other state.",
            "Player layer. Use W A S D to change the start direction."
        };

        string extraInfo = "Left mouse to place. Right mouse to delete. Enter to save. Up or Down to switch between layers.";

        int curLayer = 0;
        int curBackground = 0;

        List<Tuple<string, Texture2D>> textures;
        List<Tuple<string, Texture2D>> backgrounds;
        List<object> GameObjects;

        PlayerInfo playerInfo;

        public override void LoadContent(ContentManager content)
        {
            // Go trough specific folder for objects
            List<string> files = IOHelper.Instance.FilesInDirectory(@"\Content\level-editor", "*.png");
            GameObjects = new List<object>();

            textures = new List<Tuple<string, Texture2D>>();

            for (int i = 0; i < files.Count; i++)
            {
                files[i] = files[i].Remove(files[i].LastIndexOf('.'), files[i].Length - files[i].LastIndexOf('.'));

                if (files[i].Contains(@"\Content\"))
                    files[i] = files[i].Remove(0, @"\Content\".Length);

                textures.Add(new Tuple<string, Texture2D>(files[i], content.Load<Texture2D>(files[i])));

                Console.WriteLine(files[i]);
            }


            List<string> backgroundPath = IOHelper.Instance.FilesInDirectory(@"\Content\level-editor\backgrounds", "*.png");

            backgrounds = new List<Tuple<string, Texture2D>>();

            for (int i = 0; i < backgroundPath.Count; i++)
            {
                backgroundPath[i] = backgroundPath[i].Remove(backgroundPath[i].LastIndexOf('.'), backgroundPath[i].Length - backgroundPath[i].LastIndexOf('.'));

                if (backgroundPath[i].Contains(@"\Content\"))
                    backgroundPath[i] = backgroundPath[i].Remove(0, @"\Content\".Length);

                backgrounds.Add(new Tuple<string, Texture2D>(backgroundPath[i], content.Load<Texture2D>(backgroundPath[i])));

                Console.WriteLine(files[i]);
            }

            base.LoadContent(content);
        }

        public void SaveLevel()
        {
            LevelFormat lvl = new LevelFormat();
            lvl.playerInfo = playerInfo;
            lvl.backgroundPath = backgrounds[curBackground].Item1;
            lvl.clickObjectsInfo = new List<ClickAbleInfo>();

            foreach (object o in GameObjects)
            {
                if (o is ClickAbleObject)
                {
                    lvl.clickObjectsInfo.Add((o as ClickAbleObject).Info);
                }
                else if (o is MovementTile)
                {
                    lvl.moveTiles.Add((o as MovementTile).Info);
                }
                else if (o is WinTile)
                {
                    lvl.moveTiles.Add((o as WinTile).Info);
                }
            }

            LevelManager.Instance.SaveLevel(lvl, 0);
        }

        public override void Update(GameTime dt)
        {
            if (InputHelper.Instance.IsKeyPressed(Keys.Up) && curLayer > 0)
                curLayer--;
            else if (InputHelper.Instance.IsKeyPressed(Keys.Down) && curLayer < layerInfo.Count - 1)
                curLayer++;

            if(curLayer == 0)
            {
                if (InputHelper.Instance.IsKeyPressed(Keys.A) && curBackground > 0)
                    curBackground--;
                else if (InputHelper.Instance.IsKeyPressed(Keys.D) && curBackground < backgrounds.Count - 1)
                    curBackground++;
            }


            if(InputHelper.Instance.IsKeyPressed(Keys.Enter))
            {
                if (Game1.Instance.Window.Title.Contains('*'))
                {
                    SaveLevel();
                    Game1.Instance.Window.Title = Game1.Instance.Window.Title.Remove(Game1.Instance.Window.Title.IndexOf('*'));
                }
            }


            // left mouse place object

            // right mouse remove object

            base.Update(dt);
        }

        private void LevelChanged()
        {
            if (!Game1.Instance.Window.Title.Contains('*'))
            {
                Game1.Instance.Window.Title += '*';
            }
        }

        public override void Draw(SpriteBatch batch)
        {
            batch.Draw(backgrounds[curBackground].Item2, Vector2.Zero, Color.White);

            foreach (object o in GameObjects)
            {
                if (o is IDrawAble)
                {
                    (o as IDrawAble).Draw(batch);
                }
            }


            Game1.Instance.fontRenderer.DrawText(batch, new Vector2(5, 5), layerInfo[curLayer], Color.Black);
            Game1.Instance.fontRenderer.DrawText(batch, new Vector2(5, 10 + Game1.Instance.fontRenderer.StringSize("H").Height), extraInfo, Color.Black);

            base.Draw(batch);
        }
    }
}
