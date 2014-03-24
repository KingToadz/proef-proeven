using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using proef_proeven.Components;
using proef_proeven.Components.Game;
using proef_proeven.Components.Game.Interfaces;
using proef_proeven.Components.Level;
using proef_proeven.Components.LevelCreator;
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
            "Player layer. Use W A S D to change the start direction.",
            "Movement Tiles layer Use W A S D to change the direction",
            "Win tile layer create win tiles",
            "Create new boundingboxes for items"
        };

        int curLayer = 0;

        int levelID;

        List<BaseLayer> layers;

        public LevelCreator(int levelID)
        {
            this.levelID = levelID;

            layers = new List<BaseLayer>();
            layers.Add(new BackgroundLayer());
            layers.Add(new ClickableObjectLayer());
            layers.Add(new PlayerLayer());
            layers.Add(new MovementLayer());
            layers.Add(new WinTileLayer());
            layers.Add(new BoundingboxEditor());
        }

        public override void LoadContent(ContentManager content)
        {
            foreach(BaseLayer bl in layers)
            {
                bl.LoadContent(content);
            }

            LevelLoader loader = new LevelLoader(levelID);
            loader.Load();

            if(loader.LevelLoaded)
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
                else if (o is Tuple<string, Texture2D>)
                {
                    lvl.backgroundPath = (o as Tuple<string, Texture2D>).Item1;
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
            }

            return lvl;
        }

        public void TestLevel()
        {
            ScreenManager.Instance.SetScreen(new GameScreen(GetLevelFormat()));
        }

        
        public List<object> AllLayerObjects()
        {
            List<object> all = new List<object>();

            foreach(BaseLayer bl in layers)
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
            }

            if (InputHelper.Instance.IsKeyPressed(Keys.Space) && !layers[curLayer].BlockLayerChange)
                TestLevel();

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
            foreach(BaseLayer bl in layers)
            {
                bl.Draw(batch);
            }
           
            Game1.Instance.fontRenderer.DrawText(batch, new Vector2(5, 5), layerInfo[curLayer], Color.Black);
            Game1.Instance.fontRenderer.DrawText(batch, new Vector2(5, 10 + Game1.Instance.fontRenderer.CharSize.Height), layers[curLayer].LayerInfo, Color.Black);

            base.Draw(batch);
        }
    }
}
