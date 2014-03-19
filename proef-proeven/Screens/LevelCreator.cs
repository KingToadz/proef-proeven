﻿using Microsoft.Xna.Framework;
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
            "Player layer. Use W A S D to change the start direction.",
            "Movement Tiles layer Use W A S D to change the direction",
            "Win tile layer create win tiles"
        };

        string extraInfo = "Left mouse to place. Right mouse to delete. Enter to save. Up or Down to switch between layers.";

        int curObject = 0;
        int curLayer = 0;
        int curBackground = 0;

        List<Tuple<string, Texture2D>> textures;
        List<Tuple<string, Texture2D>> backgrounds;
        List<object> GameObjects;

        Texture2D pixel;

        Rectangle size;
        Player player;

        bool placing;
        Vector2 startPos;
        Vector2 toGo;

        int clickAbleObjectCount = 0;

        bool dragging;
        Rectangle winTileSize;
        // TODO: use this one. Still needs implementation
        Rectangle movementTileSize;

        float movementRotation = 0;
        Texture2D arrow;

        int levelID;

        public LevelCreator(int levelID)
        {
            this.levelID = levelID;
        }

        public override void LoadContent(ContentManager content)
        {
            player = new Player();
            player.LoadContent(content);

            arrow = content.Load<Texture2D>(@"level-editor\required\arrow");

            size = Game1.Instance.fontRenderer.StringSize("W");

            // Go trough specific folder for objects
            List<string> files = IOHelper.Instance.FilesInDirectory(@"\Content\level-editor\moveable", "*.png");
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

            pixel = new Texture2D(Game1.Instance.GraphicsDevice, 1, 1);
            pixel.SetData<Color>(new Color[1]{ Color.Black });

            base.LoadContent(content);
        }

        public void TestLevel()
        {
            LevelFormat lvl = new LevelFormat();
            lvl.playerInfo = player.Info;
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

            ScreenManager.Instance.SetScreen(new GameScreen(lvl));
        }

        public void SaveLevel()
        {
            LevelFormat lvl = new LevelFormat();
            lvl.playerInfo = player.Info;
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

            LevelManager.Instance.SaveLevel(lvl, levelID);
        }

        public override void Update(GameTime dt)
        {
            bool blockChangeLayer = false;

            if(curLayer == 0)
            {
                if (InputHelper.Instance.IsKeyPressed(Keys.A) && curBackground > 0)
                    curBackground--;
                else if (InputHelper.Instance.IsKeyPressed(Keys.D) && curBackground < backgrounds.Count - 1)
                    curBackground++;
            }
            else if(curLayer == 1)
            {
                if (InputHelper.Instance.IsKeyPressed(Keys.A) && curObject > 0 && !placing)
                { curObject--; }
                else if (InputHelper.Instance.IsKeyPressed(Keys.D) && curObject < textures.Count - 1 && !placing)
                { curObject++; }

                if (InputHelper.Instance.IsLeftMouseReleased())
                {
                    if(placing)
                    {
                        toGo = InputHelper.Instance.MousePos();
                        ClickAbleObject obj = new ClickAbleObject();
                        obj.Image = textures[curObject].Item2;
                        obj.StartPosition = obj.Position = startPos;
                        obj.TexturePath = textures[curObject].Item1;
                        obj.moveToPosition = toGo;
                        obj.ObjectiveID = clickAbleObjectCount;
                        GameObjects.Add(obj);
                        LevelChanged();
                        clickAbleObjectCount++;
                        placing = false;
                    }
                    else
                    {
                        placing = true;
                        startPos = InputHelper.Instance.MousePos();
                    }
                }
                else if (InputHelper.Instance.IsRightMousePressed() && !placing)
                {
                    for (int i = 0; i < GameObjects.Count; i++)
                    {
                        object o = GameObjects[i];

                        if (o is ClickAbleObject)
                        {
                            if ((o as ClickAbleObject).Boundingbox.Contains(InputHelper.Instance.MousePos().toPoint()))
                            {
                                GameObjects.Remove(o);
                                clickAbleObjectCount--;
                                LevelChanged();
                            }
                        }
                    }
                }


                if (placing)
                    blockChangeLayer = true;
            }
            else if(curLayer == 2)
            {
                if (InputHelper.Instance.IsKeyPressed(Keys.A))
                {
                    player.ChangeMovement(Player.Movement.Left);
                    player.StartMovement = Player.Movement.Left;
                    LevelChanged();
                }
                else if (InputHelper.Instance.IsKeyPressed(Keys.D))
                {
                    player.ChangeMovement(Player.Movement.Right);
                    player.StartMovement = Player.Movement.Right;
                    LevelChanged();
                }
                else if (InputHelper.Instance.IsKeyPressed(Keys.S))
                {
                    player.ChangeMovement(Player.Movement.Down);
                    player.StartMovement = Player.Movement.Down;
                    LevelChanged();
                }
                else if (InputHelper.Instance.IsKeyPressed(Keys.W))
                {
                    player.ChangeMovement(Player.Movement.Up);
                    player.StartMovement = Player.Movement.Up;
                    LevelChanged();
                }

                if(InputHelper.Instance.LeftMouseDown())
                {
                    player.StartPosition = InputHelper.Instance.MousePos();
                    player.ChangePosition(player.StartPosition);
                    LevelChanged();
                }
            }
            else if(curLayer == 3)
            {
                if (InputHelper.Instance.IsKeyPressed(Keys.A))
                {
                    movementRotation = MathHelper.ToRadians(180.0f);
                }
                else if (InputHelper.Instance.IsKeyPressed(Keys.D))
                {
                    movementRotation = MathHelper.ToRadians(0.0f);
                }
                else if (InputHelper.Instance.IsKeyPressed(Keys.S))
                {
                    movementRotation = MathHelper.ToRadians(90.0f);
                }
                else if (InputHelper.Instance.IsKeyPressed(Keys.W))
                {
                    movementRotation = MathHelper.ToRadians(270.0f);
                }

                if (InputHelper.Instance.IsLeftMousePressed())
                {
                    Player.Movement move = Player.Movement.Left;

                    if(movementRotation == MathHelper.ToRadians(0.0f))
                    {
                        move = Player.Movement.Right;
                    }
                    else if(movementRotation == MathHelper.ToRadians(90.0f))
                    {
                        move = Player.Movement.Down;
                    }
                    else if(movementRotation == MathHelper.ToRadians(180.0f))
                    {
                        move = Player.Movement.Left;
                    }
                    else if(movementRotation == MathHelper.ToRadians(270.0f))
                    {
                        move = Player.Movement.Up;
                    }

                    MovementTile t = new MovementTile(new Rectangle((int)InputHelper.Instance.MousePos().X, (int)InputHelper.Instance.MousePos().Y, 32, 32), move, true);
                    GameObjects.Add(t);
                    LevelChanged();
                }
                else if(InputHelper.Instance.IsRightMousePressed())
                {
                    for(int i = 0; i < GameObjects.Count; i++)
                    {
                        object o = GameObjects[i];

                        if(o is MovementTile)
                        {
                            if((o as MovementTile).Boundingbox.Contains(InputHelper.Instance.MousePos().toPoint()))
                            {
                                GameObjects.Remove(o);
                                LevelChanged();
                            }
                        }
                    }
                }
            }
            else
            {
                if(InputHelper.Instance.IsLeftMousePressed())
                {
                    if (!dragging){
                        dragging = true;
                        winTileSize.X = (int)InputHelper.Instance.MousePos().X;
                        winTileSize.Y = (int)InputHelper.Instance.MousePos().Y;
                        winTileSize.Width = (int)InputHelper.Instance.MousePos().X - winTileSize.X;
                        winTileSize.Height = (int)InputHelper.Instance.MousePos().Y - winTileSize.Y;
                    }
                }
                else if(InputHelper.Instance.LeftMouseDown())
                {
                    winTileSize.Width = (int)InputHelper.Instance.MousePos().X - winTileSize.X;
                    winTileSize.Height = (int)InputHelper.Instance.MousePos().Y - winTileSize.Y;
                }
                else if(InputHelper.Instance.IsLeftMouseReleased() && dragging)
                {
                    WinTile win = new WinTile(winTileSize);
                    GameObjects.Add(win);
                    dragging = false;
                    LevelChanged();
                }
                else if(InputHelper.Instance.IsRightMousePressed() && !dragging)
                {
                    for (int i = 0; i < GameObjects.Count; i++)
                    {
                        object o = GameObjects[i];

                        if (o is WinTile)
                        {
                            if ((o as WinTile).Boundingbox.Contains(InputHelper.Instance.MousePos().toPoint()))
                            {
                                GameObjects.Remove(o);
                                LevelChanged();
                            }
                        }
                    }
                }


                if (dragging)
                    blockChangeLayer = true;
            }



            if(InputHelper.Instance.IsKeyPressed(Keys.Enter))
            {
                if (Game1.Instance.Window.Title.Contains('*'))
                {
                    SaveLevel();
                    Game1.Instance.Window.Title = Game1.Instance.Window.Title.Remove(Game1.Instance.Window.Title.IndexOf('*'));
                }
            }


            if (InputHelper.Instance.IsKeyPressed(Keys.Up) && curLayer > 0 && !blockChangeLayer)
                curLayer--;
            else if (InputHelper.Instance.IsKeyPressed(Keys.Down) && curLayer < layerInfo.Count - 1 && !blockChangeLayer)
                curLayer++;

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
                if(o is ClickAbleObject)
                {
                    ClickAbleObject click = o as ClickAbleObject;

                    click.Draw(batch);
                    Game1.Instance.fontRenderer.DrawText(batch, click.StartPosition + new Vector2(5, 5), click.ObjectiveID.ToString());
                    batch.Draw(click.Image, click.moveToPosition, Color.FromNonPremultiplied(255, 255, 255, 100));
                    Game1.Instance.fontRenderer.DrawText(batch, click.moveToPosition + new Vector2(5, 5), click.ObjectiveID.ToString());
                }
                else if (o is IDrawAble)
                {
                    (o as IDrawAble).Draw(batch);
                }
                else if(o is WinTile)
                {
                    WinTile w = o as WinTile;
                    batch.Draw(pixel, new Rectangle(w.Bounds.X, w.Bounds.Y, w.Bounds.Width, 1), Color.White);
                    batch.Draw(pixel, new Rectangle(w.Bounds.X, w.Bounds.Y, 1, w.Bounds.Height), Color.White);
                    batch.Draw(pixel, new Rectangle(w.Bounds.X + w.Bounds.Width, w.Bounds.Y, 1, w.Bounds.Height), Color.White);
                    batch.Draw(pixel, new Rectangle(w.Bounds.X, w.Bounds.Y + w.Bounds.Height, w.Bounds.Width, 1), Color.White);
                    Game1.Instance.fontRenderer.DrawText(batch, new Vector2((w.Bounds.X + w.Bounds.Width / 2) - size.Width / 2, (w.Bounds.Y + w.Bounds.Height / 2) - size.Height), "W");
                }
            }

            player.Draw(batch);

            if(curLayer == 1)
            {
                batch.Draw(textures[curObject].Item2, InputHelper.Instance.MousePos(), placing ? Color.FromNonPremultiplied(255, 255, 255, 100) : Color.White);

                if(placing)
                    batch.Draw(textures[curObject].Item2, startPos, Color.White);
            }
            else if(curLayer == 3)
            {
                batch.Draw(arrow, InputHelper.Instance.MousePos(), arrow.Bounds, Color.FromNonPremultiplied(255, 255, 255, 100), movementRotation, arrow.Bounds.Center.toVector2(), 1f, SpriteEffects.None, 0.0f);
            }
            else if(curLayer == 4)
            {
                if (dragging)
                {
                    batch.Draw(pixel, new Rectangle(winTileSize.X, winTileSize.Y, winTileSize.Width, 1), Color.White);
                    batch.Draw(pixel, new Rectangle(winTileSize.X, winTileSize.Y, 1, winTileSize.Height), Color.White);
                    batch.Draw(pixel, new Rectangle(winTileSize.X + winTileSize.Width, winTileSize.Y, 1, winTileSize.Height), Color.White);
                    batch.Draw(pixel, new Rectangle(winTileSize.X, winTileSize.Y + winTileSize.Height, winTileSize.Width, 1), Color.White);
                    Game1.Instance.fontRenderer.DrawText(batch, new Vector2((winTileSize.X + winTileSize.Width / 2) - size.Width / 2, (winTileSize.Y + winTileSize.Height / 2) - size.Height), "W");
                }
            }

            Game1.Instance.fontRenderer.DrawText(batch, new Vector2(5, 5), layerInfo[curLayer], Color.Black);
            Game1.Instance.fontRenderer.DrawText(batch, new Vector2(5, 10 + Game1.Instance.fontRenderer.StringSize("H").Height), extraInfo, Color.Black);

            base.Draw(batch);
        }
    }
}