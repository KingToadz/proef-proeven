using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Newtonsoft.Json;
using proef_proeven.Components;
using proef_proeven.Components.Animations;
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
    class GameScreen : BaseScreen
    {
        int levelID;        // The ID of the current level

        Grid backgroundGrid;// The grid that contains the background in tiles
        Player player;      // The player object

        List<object> GameObjects;       // All the objects in this screen that needs to interact in some way
        List<IDrawAble> drawAbleItems;  // All the items that needs to be drawn

        ClickableObject backButton;     // button to go back one screen

        /// <summary>
        /// Clickable objects will set an objective to true if it's clicked
        /// </summary>
        List<Objective> objectives;

        /// <summary>
        /// The loader to load the levels
        /// </summary>
        LevelLoader loader;

        /// <summary>
        /// Bool to check if this is an test called from the levelcreator
        /// </summary>
        bool testing;

        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="level">The level number that should be loaded</param>
        public GameScreen(int level)
        {
            levelID = level;
            GameObjects = new List<object>();
            player = new Player();
            objectives = new List<Objective>();
            drawAbleItems = new List<IDrawAble>();

            loader = new LevelLoader(levelID);
            testing = false;
        }

        public GameScreen(LevelFormat level, bool test = true)
        {
            loader = new LevelLoader(level);
            GameObjects = new List<object>();
            player = new Player();
            objectives = new List<Objective>();
            drawAbleItems = new List<IDrawAble>();
            levelID = -1;
            testing = test;
        }

        public override void LoadContent(ContentManager content)
        {
            loader.Load();              
            // Is the level loading succesfull
            if (loader.LevelLoaded)
            {
                backgroundGrid = new Grid();
                backgroundGrid.LoadFromLevelInfo(loader.level);

                drawAbleItems.Add(backgroundGrid);
                GameObjects.Add(backgroundGrid);

                List<ClickAbleInfo> click = loader.level.clickObjectsInfo;

                foreach (ClickAbleInfo info in click)
                {
                    ClickableObject clickObj = new ClickableObject();
                    // Check if the object has an custom bounds
                    if (info.useCustomBounds)
                    {
                        clickObj.SetCustomBounds(new Rectangle(info.X, info.Y, info.Width, info.Height));
                    }

                    clickObj.StartPosition = info.position;
                    clickObj.Position = info.position;
                    clickObj.moveToPosition = info.moveToPosition;

                    // Check if the object has an animation
                    if (IOHelper.Instance.DoesFileExist(Constants.CONTENT_DIR + info.texturePath + ".ani"))
                    {
                        AnimationInfo aInfo = JsonConvert.DeserializeObject<AnimationInfo>(IOHelper.Instance.ReadFile(Constants.CONTENT_DIR + info.texturePath + ".ani"));
                        clickObj.Animation = new Animation(content.Load<Texture2D>(info.texturePath), aInfo.width, aInfo.height, aInfo.cols, aInfo.rows, aInfo.totalFrames, aInfo.fps);
                    }
                    else
                    {
                        clickObj.Image = content.Load<Texture2D>(info.texturePath);
                    }
                    clickObj.ObjectiveID = info.objectiveID;

                    if (info.useCustomBounds)
                        clickObj.SetCustomBounds(new Rectangle(info.X, info.Y, info.Width, info.Height));

                    clickObj.onClick += OnClickHandler;

                    objectives.Add(new Objective("Objective " + info.objectiveID));

                    drawAbleItems.Add(clickObj);
                    GameObjects.Add(clickObj);
                }

                {// create scope for info
                    PlayerInfo info = loader.level.playerInfo;
                    player = new Player();
                    player.StartPosition = loader.level.playerInfo.position;
                    player.StartMovement = loader.level.playerInfo.startMovement;
                    player.LoadContent(content);
                    player.ChangeMovement(loader.level.playerInfo.startMovement);

                    if (loader.level.playerInfo.useCustomBoundingbox)
                    {
                        player.SetCustomBoundingbox(new Rectangle(info.x, info.y, info.width, info.height));
                    }
                    drawAbleItems.Add(player);
                }

                GameObjects.Add(player);

                foreach (MovementTileInfo info in loader.level.moveTiles)
                {
                    if (info.WinningTile)
                        GameObjects.Add(new WinTile(new Rectangle(info.X, info.Y, info.Width, info.Height)));
                    else
                        GameObjects.Add(new MovementTile(new Rectangle(info.X, info.Y, info.Width, info.Height), info.movement, testing));
                }

                foreach (DecorationInfo info in loader.level.decoration)
                {
                    Decoration decoration = new Decoration();
                    decoration.Position = info.position;
                    decoration.Image = content.Load<Texture2D>(info.ImagePath);
                    drawAbleItems.Add(decoration);
                    GameObjects.Add(decoration);
                }

                drawAbleItems = drawAbleItems.OrderBy(o => o.DrawIndex()).ToList();
            }
            else
            {
                // TODO: show error
                player.Won = true;
            }

            backButton = new ClickableObject();
            if(testing)
            {
                backButton.TexturePath = @"buttons\reset";
                backButton.Image = content.Load<Texture2D>(backButton.TexturePath);
            }
            else
            { 
                backButton.TexturePath = @"buttons\back";
                backButton.Image = content.Load<Texture2D>(backButton.TexturePath);
            }
            backButton.onClick += OnClickHandler;
            backButton.ObjectiveID = -1;
            backButton.Position = new Vector2(Game1.Instance.ScreenRect.Width - backButton.Image.Width - 20, Game1.Instance.ScreenRect.Height - backButton.Image.Height - 20);

            base.LoadContent(content);
        }

        public void SaveLevel()
        {
            LevelFormat lvl = new LevelFormat();
            lvl.playerInfo = player.Info;
            lvl.backgroundPath = "";
            lvl.clickObjectsInfo = new List<ClickAbleInfo>();
            
            foreach(object o in GameObjects)
            {
                if (o is ClickableObject)
                {
                    lvl.clickObjectsInfo.Add((o as ClickableObject).Info);
                }
                else if (o is MovementTile)
                {
                    lvl.moveTiles.Add((o as MovementTile).Info);
                }
                else if(o is WinTile)
                {
                    lvl.moveTiles.Add((o as WinTile).Info);
                }
            }

            LevelManager.Instance.SaveLevel(lvl, levelID);
        }

        public void Reset()
        {
            foreach(object o in GameObjects)
            {
                if(o is IResetAble)
                {
                    (o as IResetAble).Reset();
                }
            }
        }

        public void OnClickHandler(object sender)
        {
            if(sender is ClickableObject)
            {
                ClickableObject s = sender as ClickableObject;

                if(s == backButton)
                {
                    if (!testing)
                    {
                        LevelManager.Instance.WinLevel(levelID, player.Tries);
                        LevelManager.Instance.UnlockLevel(levelID + 1);
                        ScreenManager.Instance.PopScreen();
                    }
                    else
                    {
                        Reset();
                    }
                    return;
                }
 
                if(!objectives[s.ObjectiveID].Done)
                {
                    s.NextPos();
                }
            }
        }

        public void SetObjective(int id, bool newState)
        {
            objectives[id].Done = newState;
        }

        public override void Update(GameTime dt)
        {
            if (player.Won)
            {
                backButton.Update(dt);
            }
            else
            {
                foreach (object o in GameObjects)
                {
                    if (o is IUpdateAble)
                    {
                        (o as IUpdateAble).Update(dt);
                    }
                }

                foreach (object o in GameObjects)
                {
                    if (o is ICollidAble)
                    {
                        ICollidAble collideable = o as ICollidAble;

                        // Only check collision if the object is moving
                        if (collideable.Delta != Vector2.Zero)
                        {
                            foreach (object o2 in GameObjects)
                            {
                                // Check if it is not itself and the other one is an ICollidable
                                if (o2 is ICollidAble && o != o2)
                                {
                                    // Only the moving object should collide for now. like the player or an car
                                    if (collideable.Boundingbox.Intersects((o2 as ICollidAble).Boundingbox))
                                        collideable.Collide(o2 as ICollidAble);
                                }
                            }
                        }
                    }
                }

                if (player.CurMovement == Player.Movement.Dead)
                    Reset();

                drawAbleItems = drawAbleItems.OrderBy(o => o.DrawIndex()).ToList();
                //drawAbleItems.OrderBy(o => o.DrawIndex()).ToList();
            }

            base.Update(dt);
        }

        public override void Draw(SpriteBatch batch)
        {
            for (int i = 0; i < drawAbleItems.Count; i++)
            {
                drawAbleItems[i].Draw(batch);
            }

            float yMargin = 0;

            if(player.Won)
            {
                Game1.Instance.fontRenderer.DrawText(batch, new Vector2(10, 10 + yMargin), "You win! It took you " + player.Tries + " Tries", Color.Black);
                backButton.Draw(batch);
            }

            if(testing)
            {
                Game1.Instance.fontRenderer.DrawText(batch, new Vector2(5, Game1.Instance.ScreenRect.Height - 40), "Testing... Press backspace to go back to editor", Color.Black);
            }

            base.Draw(batch);
        }
    }
}
