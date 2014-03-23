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

            if (InputHelper.Instance.IsLeftMousePressed())
            {
                //MovementTile win = new MovementTile(new Rectangle((int)InputHelper.Instance.MousePos().X, (int)InputHelper.Instance.MousePos().X, 32, 32), GetMovement(), true);
                //movement.Add(win);

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
                if (!dragging)
                {
                    tileSize.X = (int)InputHelper.Instance.MousePos().X;
                    tileSize.Y = (int)InputHelper.Instance.MousePos().Y;
                }
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

        private string GetMovementChar()
        {
            if (rotation == MathHelper.ToRadians(0.0f))
            {
                return ">";
            }
            else if (rotation == MathHelper.ToRadians(90.0f))
            {
                return "v";
            }
            else if (rotation == MathHelper.ToRadians(180.0f))
            {
                return "<";
            }
            else if (rotation == MathHelper.ToRadians(270.0f))
            {
                return "^";
            }
            else
                return "*";
        }

        public override void Draw(SpriteBatch batch)
        {
            foreach (MovementTile t in movement)
                t.Draw(batch);

            if (ActiveLayer)
            {
                Rectangle wSize = Game1.Instance.fontRenderer.StringSize(GetMovementChar());

                if (dragging)
                {
                    Game1.Instance.fontRenderer.DrawText(batch, new Vector2((tileSize.X + tileSize.Width / 2) - wSize.Width / 2, (tileSize.Y + tileSize.Height / 2) - wSize.Height), GetMovementChar());
                    RectangleRender.Draw(batch, tileSize);
                }
                else
                    Game1.Instance.fontRenderer.DrawText(batch, new Vector2(tileSize.X - wSize.Width / 2, tileSize.Y  - wSize.Height), GetMovementChar());

            }


            base.Draw(batch);
        }
    }
}
