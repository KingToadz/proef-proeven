using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using proef_proeven.Components;
using proef_proeven.Components.Game;
using proef_proeven.Components.Level;
using proef_proeven.Components.LevelCreator;
using proef_proeven.Components.LevelCreator.Layers;
using proef_proeven.Components.LoadData;
using System;
using System.Collections.Generic;
using System.Linq;

namespace proef_proeven.Screens
{
    internal class LevelCreator : BaseScreen
    {
        private List<string> layerInfo = new List<string>(){
            "Background. Use A or D to switch.",
            "Object layer. Use A or D to switch object. W and S for other state.",
            "Player layer. Use W A S D to change the start direction.",
            "Movement Tiles layer Use W A S D to change the direction",
            "Win tile layer create win tiles",
            "Create new boundingboxes for items",
            "new layer"
        };

        private int curLayer = 0;

        private int levelID;

        private List<BaseLayer> layers;

        public LevelCreator(int levelID)
        {
            this.levelID = levelID;

            layers = new List<BaseLayer>();
            //layers.Add(new BackgroundLayer());
            layers.Add(new GridLayer());
            layers.Add(new ClickableObjectLayer());
            layers.Add(new PlayerLayer());
            layers.Add(new MovementLayer());
            layers.Add(new WinTileLayer());
            layers.Add(new DecorationLayer());
            layers.Add(new BoundingboxEditor());
            layers[0].ChangeActive(true);
        }

        public override void LoadContent(ContentManager content)
        {
            foreach (BaseLayer bl in layers)
            {
                bl.LoadContent(content);
            }

            LevelLoader loader = new LevelLoader(levelID);
            loader.Load();

            if (loader.LevelLoaded)
            {
                foreach (BaseLayer bl in layers)
                {
                    bl.LoadLevel(loader.level);
                }
            }

            base.LoadContent(content);
        }

        private LevelFormat GetLevelFormat()
        {
            LevelFormat lvl = new LevelFormat();

            List<object> allLayerObjects = AllLayerObjects();

            foreach (object o in allLayerObjects)
            {
                if (o is Player)
                {
                    lvl.playerInfo = (o as Player).Info;
                }
                else if (o is ClickableObject)
                {
                    lvl.clickObjectsInfo.Add((o as ClickableObject).Info);
                }
                else if (o is MovementTile)
                {
                    lvl.moveTiles.Add((o as MovementTile).Info);
                }
                else if (o is WinTile)
                {
                    lvl.moveTiles.Add((o as WinTile).Info);
                }
                else if (o is Decoration)
                {
                    lvl.decoration.Add((o as Decoration).Info);
                }
                else if (o is GridTileInfo)
                {
                    lvl.Grid.Add((o as GridTileInfo));
                }
            }

            return lvl;
        }

        public void TestLevel(bool showTestingText)
        {
            ScreenManager.Instance.SetScreenNoTransition(new GameScreen(GetLevelFormat(), showTestingText));
        }

        public List<object> AllLayerObjects()
        {
            List<object> all = new List<object>();

            foreach (BaseLayer bl in layers)
            {
                all.AddRange(bl.getObjects());
            }

            return all;
        }

        public void SaveLevel()
        {
            LevelManager.Instance.SaveLevel(GetLevelFormat(), levelID);
        }

        public override void Update(GameTime dt)
        {
            layers[curLayer].Update(dt);

            if (InputHelper.Instance.IsKeyPressed(Keys.Enter))
            {
                SaveLevel();
                ScreenshotLevel();
            }

            if (InputHelper.Instance.IsKeyPressed(Keys.Space) && !layers[curLayer].BlockLayerChange)
                TestLevel(true);

            if (InputHelper.Instance.IsKeyPressed(Keys.Up) && curLayer > 0 && !layers[curLayer].BlockLayerChange)
            {
                layers[curLayer].ChangeActive(false);
                curLayer--;
                layers[curLayer].ChangeActive(true);
            }
            else if (InputHelper.Instance.IsKeyPressed(Keys.Down) && curLayer < layerInfo.Count - 1 && !layers[curLayer].BlockLayerChange)
            {
                layers[curLayer].ChangeActive(false);
                curLayer++;
                layers[curLayer].ChangeActive(true);
            }

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
            foreach (BaseLayer bl in layers)
            {
                bl.Draw(batch);
            }

            Game1.Instance.fontRenderer.DrawText(batch, new Vector2(5, 5), layerInfo[curLayer], Color.Black);
            Game1.Instance.fontRenderer.DrawText(batch, new Vector2(5, 10 + Game1.Instance.fontRenderer.CharSize.Height), layers[curLayer].LayerInfo, Color.Black);

            base.Draw(batch);
        }

        public void ScreenshotLevel()
        {
            TestLevel(false);

            System.Threading.Thread.Sleep(100);

            // Need to wait until the level is loaded and the transition is done
            // this will freeze the game for an small time
            while (ScreenManager.Instance.IsLoading)
            {
                System.Threading.Thread.Sleep(10);
            }

            System.Threading.Thread.Sleep(10);

            Game1.Instance.ScreenShot(levelID);
            ScreenManager.Instance.PopScreen();
            Console.WriteLine("Screenshot succes saved " + levelID + ".png");
        }
    }
}