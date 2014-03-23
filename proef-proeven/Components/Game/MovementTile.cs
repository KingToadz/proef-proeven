using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using proef_proeven.Components.Game.Interfaces;
using proef_proeven.Components.LoadData;
using proef_proeven.Components.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace proef_proeven.Components.Game
{
    class MovementTile : BaseTile, IDrawAble, ICollidAble
    {
        Player.Movement movedrirection;

        float rotation;

        /// <summary>
        /// if this object is used in the level creator
        /// </summary>
        bool inLevelCreator = false;

        /// <summary>
        /// Rectangle for the letter to draw in the level creator
        /// </summary>
        Rectangle wSize;// = Game1.Instance.fontRenderer.StringSize("W");

        /// <summary>
        /// Start information of the player.
        /// Used for level saving
        /// </summary>
        public MovementTileInfo Info
        {
            get
            {
                MovementTileInfo info = new MovementTileInfo();
                info.X = Bounds.X;
                info.Y = Bounds.Y;
                info.Width = Bounds.Width;
                info.Height = Bounds.Height;
                info.movement = movedrirection;
                info.WinningTile = false;
                return info;
            }
        }

        public MovementTile(Rectangle size, Player.Movement movement, bool levelCreator = false)
            :base(size)
        {
            inLevelCreator = levelCreator;
            movedrirection = movement;

            wSize = Game1.Instance.fontRenderer.CharSize;

            if (levelCreator)
            {
                switch (movement)
                {
                    case Player.Movement.Right:
                        rotation = MathHelper.ToRadians(0.0f);
                        break;
                    case Player.Movement.Down:
                        rotation = MathHelper.ToRadians(90.0f);
                        break;
                    case Player.Movement.Left:
                        rotation = MathHelper.ToRadians(180.0f);
                        break;
                    case Player.Movement.Up:
                        rotation = MathHelper.ToRadians(270.0f);
                        break;
                    default:
                        rotation = 0;
                        movement = Player.Movement.Idle;
                        break;
                }
            }
        }

        public Rectangle Boundingbox
        {
            get { return Bounds; }
        }

        public Vector2 Delta
        {
            get { return Vector2.Zero; }
        }

        public Player.Movement CurMovement
        {
            get { return movedrirection; }
        }

        public void Collide(ICollidAble collider)
        {
            // shouldn't be called. This won't move
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

        public void Draw(SpriteBatch batch)
        {
            if(inLevelCreator)
            {
                RectangleRender.Draw(batch, Bounds);
                Game1.Instance.fontRenderer.DrawText(batch, new Vector2((Bounds.X + Bounds.Width / 2) - wSize.Width / 2, (Bounds.Y + Bounds.Height / 2) - wSize.Height), GetMovementChar());
            }
        }
    }
}
