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

            if (InputHelper.Instance.IsLeftMousePressed())
            {
                Player.Movement move = Player.Movement.Left;

                if (rotation == MathHelper.ToRadians(0.0f))
                {
                    move = Player.Movement.Right;
                }
                else if (rotation == MathHelper.ToRadians(90.0f))
                {
                    move = Player.Movement.Down;
                }
                else if (rotation == MathHelper.ToRadians(180.0f))
                {
                    move = Player.Movement.Left;
                }
                else if (rotation == MathHelper.ToRadians(270.0f))
                {
                    move = Player.Movement.Up;
                }

                MovementTile t = new MovementTile(new Rectangle((int)InputHelper.Instance.MousePos().X, (int)InputHelper.Instance.MousePos().Y, 32, 32), move, true);
                movement.Add(t);
            }
            else if (InputHelper.Instance.IsRightMousePressed())
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
            base.Update(time);
        }

        public override void Draw(SpriteBatch batch)
        {
            batch.Draw(arrow, InputHelper.Instance.MousePos(), arrow.Bounds, Color.FromNonPremultiplied(255, 255, 255, 100), movementRotation, arrow.Bounds.Center.toVector2(), 1f, SpriteEffects.None, 0.0f);

            base.Draw(batch);
        }
    }
}
