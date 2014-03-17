using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using proef_proeven.Components.Animations;
using proef_proeven.Components.Game.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace proef_proeven.Components.Game
{
    class Player : IUpdateAble, IDrawAble, ICollidable
    {
        public enum Movement {Left, Right, Up, Down, Idle, Dead }

        Movement currentMovement;
        Vector2 position;
        Dictionary<Movement, Animation> animations;

        /// <summary>
        /// Is the player dead
        /// </summary>
        public bool isDead
        {
            get
            {
                return currentMovement == Movement.Dead ? true : false;
            }
        }

        public void LoadContent(ContentManager content)
        {
            currentMovement = Movement.Idle;
            position = new Vector2(100, 100);


            animations = new Dictionary<Movement, Animation>();
            animations.Add(Movement.Down, new Animation(content.Load<Texture2D>(@"player\player-down"), 32, 32, 3, 1, 3, 6));
            animations.Add(Movement.Left, new Animation(content.Load<Texture2D>(@"player\player-left"), 32, 32, 3, 1, 3, 6));
            animations.Add(Movement.Right, new Animation(content.Load<Texture2D>(@"player\player-right"), 32, 32, 3, 1, 3, 6));
            animations.Add(Movement.Up, new Animation(content.Load<Texture2D>(@"player\player-up"), 32, 32, 3, 1, 3, 6));
            animations.Add(Movement.Dead, new Animation(content.Load<Texture2D>(@"player\player-down"), 32, 32, 3, 1, 3, 6));
            animations.Add(Movement.Idle, new Animation(content.Load<Texture2D>(@"player\player-down"), 32, 32, 3, 1, 3, 6));
        }

        public void ChangeMovement(Movement newMove)
        {
            if(newMove != currentMovement)
            {
                currentMovement = newMove;
                animations[currentMovement].Reset();
            }
        }

        public void Update(GameTime dt)
        {
            animations[currentMovement].Update(dt);
        }

        public void Draw(SpriteBatch batch)
        {
            animations[currentMovement].Draw(batch, position);
        }


        public void Collide(ICollidable collider)
        {
            
        }

        private Rectangle boundingbox;
        public Rectangle Boundingbox
        {
            get
            {
                return boundingbox;
            }
            private set
            {
                boundingbox = value;
            }
        }

        private Vector2 delta;
        public Vector2 Delta
        {
            get
            {
                return delta;
            }
            private set
            {
                delta = value;
            }
        }
    }
}
