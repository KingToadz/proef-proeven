using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using proef_proeven.Components;
using proef_proeven.Components.Game;
using proef_proeven.Components.Game.Interfaces;
using proef_proeven.Components.Level;
using proef_proeven.Components.LoadData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace proef_proeven.Screens
{
    class GameScreen : BaseScreen
    {
        int levelID;

        Grid backgroundGrid;
        Player player;

        List<object> GameObjects;

        ClickAbleObject objective1;
        ClickAbleObject objective2;
        ClickAbleObject objective3;

        ClickAbleObject backButton;


        /// <summary>
        /// Clickable objects will set an objective to true if it's clicked
        /// </summary>
        List<Objective> objectives;

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
        }

        public override void LoadContent(ContentManager content)
        {
            LevelLoader loader = new LevelLoader(levelID);

            if (loader.LevelLoaded)
            {
                List<ClickAbleInfo> click = loader.level.clickObjectsInfo;

                foreach(ClickAbleInfo info in click)
                {
                    ClickAbleObject clickObj = new ClickAbleObject();
                    clickObj.StartPosition  = info.position;
                    clickObj.Position       = info.position;
                    clickObj.moveToPosition = info.moveToPosition;
                    clickObj.Image          = content.Load<Texture2D>(info.texturePath);
                    clickObj.ObjectiveID    = info.objectiveID;
                    GameObjects.Add(clickObj);
                }

                player = new Player();
                player.StartPosition = loader.level.playerInfo.position;
                player.LoadContent(content);
                player.ChangeMovement(loader.level.playerInfo.startMovement);

                
            }
            else
            {
                ///////////////////////////////////////////////////////////////// OLD still in use
                objectives.Add(new Objective("objective 1"));
                objectives.Add(new Objective("Objective 2"));
                objectives.Add(new Objective("Objective 3"));

                Texture2D tileSheet = content.Load<Texture2D>("tiles");
                

                int tileWidth = 64;
                int tileHeight = 64;
                int cols = (Game1.Instance.ScreenRect.Width / tileWidth) + 1;
                int rows = (Game1.Instance.ScreenRect.Height / tileHeight) + 1;

                backgroundGrid = new Grid(cols, rows);

                for (int row = 0; row < rows; row++)
                {
                    for (int col = 0; col < cols; col++)
                    {
                        backgroundGrid.AddTile(new Tile(tileSheet, new Rectangle(col * tileWidth, row * tileHeight, tileWidth, tileHeight), new Rectangle(0 * 16, 9 * 16, 16, 16)), row, col);
                    }
                }
                GameObjects.Add(backgroundGrid);

                player.StartPosition = new Vector2(100, 100);
                player.LoadContent(content);
                GameObjects.Add(player);

                objective1 = new ClickAbleObject();
                objective1.Image = content.Load<Texture2D>(@"buttons\button");
                objective1.onClick += OnClickHandler;
                objective1.ObjectiveID = 0;
                objective1.Position = objective1.StartPosition = new Vector2(50, 200);
                objective1.moveToPosition = objective1.Position + new Vector2(300, 0);

                objective2 = new ClickAbleObject();
                objective2.Image = content.Load<Texture2D>(@"buttons\button");
                objective2.onClick += OnClickHandler;
                objective2.ObjectiveID = 1;
                objective2.Position = objective2.StartPosition = new Vector2(50, 400);
                objective2.moveToPosition = objective2.Position + new Vector2(300, 0);

                objective3 = new ClickAbleObject();
                objective3.Image = content.Load<Texture2D>(@"buttons\button");
                objective3.onClick += OnClickHandler;
                objective3.ObjectiveID = 2;
                objective3.Position = objective3.StartPosition = new Vector2(50, 600);
                objective3.moveToPosition = objective3.Position + new Vector2(300, 0);

                WinTile win = new WinTile(new Rectangle(0, Game1.Instance.ScreenRect.Height - 20, 500, 20));
                GameObjects.Add(win);

                backButton = new ClickAbleObject();
                backButton.Image = content.Load<Texture2D>(@"buttons\help");
                backButton.onClick += OnClickHandler;
                backButton.ObjectiveID = -1;
                backButton.Position = new Vector2(750, 600);

                GameObjects.Add(objective1);
                GameObjects.Add(objective2);
                GameObjects.Add(objective3);
            }

            base.LoadContent(content);
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
            if(sender is ClickAbleObject)
            {
                ClickAbleObject s = sender as ClickAbleObject;

                if(s == backButton)
                {
                    LevelManager.Instance.WinLevel(levelID, player.Tries);
                    LevelManager.Instance.UnlockLevel(levelID + 1);
                    ScreenManager.Instance.PopScreen();
                    return;
                }
 
                if(!objectives[s.ObjectiveID].Done)
                {
                    if (s.moveToPosition == Vector2.Zero)
                        s.Position = new Vector2(s.Position.X - 400, s.Position.Y);
                    else
                        s.Position = s.moveToPosition;
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
                    if (o is ICollidable)
                    {
                        ICollidable collideable = o as ICollidable;

                        // Only check collision if the object is moving
                        if (collideable.Delta != Vector2.Zero)
                        {
                            foreach (object o2 in GameObjects)
                            {
                                // Check if it is not itself and the other one is an ICollidable
                                if (o2 is ICollidable && o != o2)
                                {
                                    // Only the moving object should collide for now. like the player or an car
                                    if (collideable.Boundingbox.Intersects((o2 as ICollidable).Boundingbox))
                                        collideable.Collide(o2 as ICollidable);
                                }
                            }
                        }
                    }
                }
            }

            base.Update(dt);
        }

        public override void Draw(SpriteBatch batch)
        {

            foreach (object o in GameObjects)
            {
                if (o is IDrawAble)
                {
                    (o as IDrawAble).Draw(batch);
                }
            }

#if DEBUG
            float yMargin = 0;
            // Just use one letter to find the height and add some extra margin
            float deltaMargin = Game1.Instance.fontRenderer.StringSize("H").Height + 5;
            foreach (Objective objective in objectives)
            {
                Game1.Instance.fontRenderer.DrawText(batch, new Vector2(10, 10 + yMargin), objective.Name + ": " + objective.Done, Color.Black);
                yMargin += deltaMargin;
            }
#endif

            if(player.Won)
            {
                Game1.Instance.fontRenderer.DrawText(batch, new Vector2(10, 10 + yMargin), "You win! It took you " + player.Tries + " Tries", Color.Black);
                backButton.Draw(batch);
            }

            base.Draw(batch);
        }
    }
}
