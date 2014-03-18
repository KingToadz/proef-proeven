using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using proef_proeven.Components.Game;
using proef_proeven.Components.Game.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace proef_proeven.Components
{
    class MovementTile : IDrawAble, ICollidable
    {
        Player.Movement movedrirection;

#if DEBUG
        Texture2D arrow;
        float rotation;
#endif

        public MovementTile(BoundingBox size, Player.Movement movement)
        {
            movedrirection = movement;
#if DEBUG
            arrow = Game1.Instance.Content.Load<Texture2D>("arrow");

            switch(movement)
            {
                case Player.Movement.Left:
                    rotation = 0.0f;
                    break;
                case Player.Movement.Down:
                    rotation = 90.0f;
                    break;
                case Player.Movement.Right:
                    rotation = 180.0f;
                    break;
                case Player.Movement.Up:
                    rotation = 270.0f;
                    break;
                default:
                    rotation = 0;
                    movement = Player.Movement.Idle;
                    break;
            }
#endif
        }

        private Rectangle boundingbox;
        public Rectangle Boundingbox
        {
            get { return boundingbox; }
        }

        public Vector2 Delta
        {
            get { return Vector2.Zero; }
        }

        public Player.Movement CurMovement
        {
            get { return movedrirection; }
        }

        public void Collide(ICollidable collider)
        {
            // shouldn't be called. This won't move
        }

        public void Draw(SpriteBatch batch)
        {
#if DEBUG
            batch.Draw(arrow, boundingbox, new Rectangle(0, 0, arrow.Width, arrow.Height), Color.White, rotation, new Vector2(arrow.Width / 2, arrow.Height / 2), SpriteEffects.None, 0);
#endif
        }
    }
}
