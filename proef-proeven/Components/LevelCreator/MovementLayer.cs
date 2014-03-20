using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using proef_proeven.Components.Game;
using proef_proeven.Components.LoadData;
using proef_proeven.Components.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace proef_proeven.Components.LevelCreator
{
    class MovementLayer : BaseLayer
    {
        List<MovementTile> movement;

        float rotation;
        Texture2D arrow;

        Rectangle tileSize;
        bool dragging;

        public MovementLayer()
        {
            movement = new List<MovementTile>();
        }

        public override List<object> getObjects()
        {
            return movement.ToList<object>();
        }

        public override void LoadContent(ContentManager content)
        {
            arrow = content.Load<Texture2D>(@"level-editor\required\arrow");
            rotation = 0.0f;

            base.LoadContent(content);
        }

        public override void LoadLevel(LoadData.LevelFormat level)
        {
            foreach (MovementTileInfo info in level.moveTiles)
            {
                if (!info.WinningTile)
                    movement.Add(new MovementTile(new Rectangle(info.X, info.Y, info.Width, info.Height), info.movement, true));                    
            }

            base.LoadLevel(level);
        }

        public override void Update(GameTime time)
        {
            if (InputHelper.Instance.IsKeyPressed(Keys.A))
            {
                rotation = MathHelper.ToRadians(180.0f);
            }
            else if (InputHelper.Instance.IsKeyPressed(Keys.D))
            {
                rotation = MathHelper.ToRadians(0.0f);
            }
            else if (InputHelper.Instance.IsKeyPressed(Keys.S))
            {
                rotation = MathHelper.ToRadians(90.0f);
            }
            else if (InputHelper.Instance.IsKeyPressed(Keys.W))
            {
                rotation = MathHelper.ToRadians(270.0f);
            }

            /*
              if (InputHelper.Instance.IsLeftMousePressed())
            {
                if (!dragging)
                {
                    dragging = true;
                    winTileSize.X = (int)InputHelper.Instance.MousePos().X;
                    winTileSize.Y = (int)InputHelper.Instance.MousePos().Y;
                    winTileSize.Width = (int)Math.Abs(InputHelper.Instance.MousePos().X - winTileSize.X);
                    winTileSize.Height = (int)Math.Abs(InputHelper.Instance.MousePos().Y - winTileSize.Y);
                }
            }
            else if (InputHelper.Instance.LeftMouseDown())
            {
                winTileSize.Width = (int)Math.Abs(InputHelper.Instance.MousePos().X - winTileSize.X);
                winTileSize.Height = (int)Math.Abs(InputHelper.Instance.MousePos().Y - winTileSize.Y);
            }
            else if (InputHelper.Instance.IsLeftMouseReleased() && dragging)
            {
                WinTile win = new WinTile(winTileSize);
                tiles.Add(win);
                dragging = false;
            }
            else if (InputHelper.Instance.IsRightMousePressed() && !dragging)
            {
                for (int i = 0; i < tiles.Count; i++)
                {
                    WinTile o = tiles[i];

                    if (o.Boundingbox.Contains(InputHelper.Instance.MousePos().toPoint()))
                    {
                        tiles.Remove(o);
                    }
                }
            }

            // check for left mouse up
            if(!InputHelper.Instance.LeftMouseDown())
            {
                dragging = false;
            }


            if (dragging)
            {
                blockLayerChange = true;
                winTileSize = GetInvertedRectangle(winTileSize);
            }
            else
            {
                blockLayerChange = false;
            }
            */
            if (InputHelper.Instance.IsLeftMousePressed())
            {
                Player.Movement move = GetMovement();

                if (!dragging)
                {
                    dragging = true;
                    tileSize.X = (int)InputHelper.Instance.MousePos().X;
                    tileSize.Y = (int)InputHelper.Instance.MousePos().Y;
                    tileSize.Width = (int)Math.Abs(InputHelper.Instance.MousePos().X - tileSize.X);
                    tileSize.Height = (int)Math.Abs(InputHelper.Instance.MousePos().Y - tileSize.Y);
                }
            }
            else if (InputHelper.Instance.LeftMouseDown())
            {
                tileSize.Width = (int)Math.Abs(InputHelper.Instance.MousePos().X - tileSize.X);
                tileSize.Height = (int)Math.Abs(InputHelper.Instance.MousePos().Y - tileSize.Y);
            }
            else if (InputHelper.Instance.IsLeftMouseReleased() && dragging)
            {
                MovementTile win = new MovementTile(tileSize, GetMovement(), true);
                movement.Add(win);
                dragging = false;
            }
            else if (InputHelper.Instance.IsRightMousePressed() && !dragging)
            {
                for (int i = 0; i < movement.Count; i++)
                {
                    MovementTile o = movement[i];

                    if (o.Boundingbox.Contains(InputHelper.Instance.MousePos().toPoint()))
                    {
                        movement.Remove(o);
                    }
                }
            }

            // check for left mouse up
            if(!InputHelper.Instance.LeftMouseDown())
            {
                dragging = false;
            }


            if (dragging)
            {
                blockLayerChange = true;
                tileSize = GetInvertedRectangle(tileSize);
            }
            else
            {
                blockLayerChange = false;
            }


            base.Update(time);
        }

        Rectangle GetInvertedRectangle(Rectangle checkRect)
        {
            Rectangle r = new Rectangle();

            r.X = checkRect.X;
            r.Y = checkRect.Y;

            if (checkRect.Width < 0)
            {
                r.X += checkRect.Width;
                r.Width = -checkRect.Width;
            }
            else
            {
                r.Width = checkRect.Width;
            }

            if (checkRect.Height < 0)
            {
                r.Y += checkRect.Height;
                r.Height = -checkRect.Height;
            }
            else
            {
                r.Height = checkRect.Height;
            }

            return r;
        }

        private Player.Movement GetMovement()
        {
            if (rotation == MathHelper.ToRadians(0.0f))
            {
                return Player.Movement.Right;
            }
            else if (rotation == MathHelper.ToRadians(90.0f))
            {
                return Player.Movement.Down;
            }
            else if (rotation == MathHelper.ToRadians(180.0f))
            {
                return Player.Movement.Left;
            }
            else if (rotation == MathHelper.ToRadians(270.0f))
            {
                return Player.Movement.Up;
            }
            else 
                return Player.Movement.Left;
        }

        public override void Draw(SpriteBatch batch)
        {
            foreach (MovementTile t in movement)
                t.Draw(batch);

            if (ActiveLayer)
            {
                if (dragging)
                {
                    batch.Draw(arrow, tileSize, arrow.Bounds, Color.FromNonPremultiplied(255, 255, 255, 100), rotation, arrow.Bounds.Center.toVector2(), SpriteEffects.None, 0.0f);
                    RectangleRender.Draw(batch, tileSize);
                }
                else
                    batch.Draw(arrow, InputHelper.Instance.MousePos(), arrow.Bounds, Color.FromNonPremultiplied(255, 255, 255, 100), rotation, arrow.Bounds.Center.toVector2(), 1f, SpriteEffects.None, 0.0f);
            }


            base.Draw(batch);
        }
    }
}
