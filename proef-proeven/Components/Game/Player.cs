using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
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
        Dictionary<Movement, Animation> animations;

        private const int frameWidth  = 32;
        private const int frameHeight = 32;

        private const float xSpeed = 4;
        private const float ySpeed = 4;

        private Dictionary<Movement, Vector2> movementList;

        /// <summary>
        /// The current position
        /// </summary>
        Vector2 position = Vector2.Zero;

        /// <summary>
        /// The start position. This is public because of the JSON converter
        /// </summary>
        public Vector2 StartPosition = Vector2.Zero;

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

        /// <summary>
        /// Boundingbox for the ICollidable interface
        /// </summary>
        public Rectangle Boundingbox
        {
            get
            {
                return new Rectangle((int)position.X, (int)position.Y, frameWidth, frameHeight);
            }
        }

        /// <summary>
        /// The current movement of the player
        /// </summary>
        public Movement CurMovement
        {
            get { return currentMovement; }
        }

        /// <summary>
        /// Delta vector for the ICollidable interface
        /// </summary>
        private Vector2 delta;
        public Vector2 Delta
        {
            get
            {
                return delta;
            }
        }

        /// <summary>
        /// Check if the player has won
        /// </summary>
        public bool Won;

        /// <summary>
        /// the amount of tries this level
        /// </summary>
        public int Tries { get; set; }

        public void LoadContent(ContentManager content)
        {
            currentMovement = Movement.Idle;
            position = new Vector2(100, 100);

            movementList = new Dictionary<Movement, Vector2>();
            movementList.Add(Movement.Dead, Vector2.Zero);
            movementList.Add(Movement.Idle, Vector2.Zero);
            movementList.Add(Movement.Left, new Vector2(-xSpeed, 0));
            movementList.Add(Movement.Right,new Vector2(xSpeed, 0));
            movementList.Add(Movement.Up,   new Vector2(0, -ySpeed));
            movementList.Add(Movement.Down, new Vector2(0, ySpeed));

            animations = new Dictionary<Movement, Animation>();
            animations.Add(Movement.Down,   new Animation(content.Load<Texture2D>(@"player\player-down"),   frameWidth, frameHeight, 3, 1, 3, 6));
            animations.Add(Movement.Left,   new Animation(content.Load<Texture2D>(@"player\player-left"),   frameWidth, frameHeight, 3, 1, 3, 6));
            animations.Add(Movement.Right,  new Animation(content.Load<Texture2D>(@"player\player-right"),  frameWidth, frameHeight, 3, 1, 3, 6));
            animations.Add(Movement.Up,     new Animation(content.Load<Texture2D>(@"player\player-up"),     frameWidth, frameHeight, 3, 1, 3, 6));
            animations.Add(Movement.Dead,   new Animation(content.Load<Texture2D>(@"player\player-down"),   frameWidth, frameHeight, 3, 1, 3, 6));
            animations.Add(Movement.Idle,   new Animation(content.Load<Texture2D>(@"player\player-down"),   frameWidth, frameHeight, 1, 1, 1, 6));

            Won = false;
            Tries = 1;
        }

        public void ChangeMovement(Movement newMove)
        {
            if(newMove != currentMovement)
            {
                currentMovement = newMove;
                animations[currentMovement].Reset();

                delta = movementList[currentMovement];
            }
        }

        public void Update(GameTime dt)
        {
#if DEBUG
            if(InputHelper.Instance.IsKeyDown(Keys.Left))
            {
                ChangeMovement(Movement.Left);
            }
            else if (InputHelper.Instance.IsKeyDown(Keys.Right))
            {
                ChangeMovement(Movement.Right);
            }
            else if (InputHelper.Instance.IsKeyDown(Keys.Up))
            {
                ChangeMovement(Movement.Up);
            }
            else if (InputHelper.Instance.IsKeyDown(Keys.Down))
            {
                ChangeMovement(Movement.Down);
            }
            else
            {
                ChangeMovement(Movement.Down);
            }
#endif

            if(Won)
            { 
                ChangeMovement(Movement.Idle); 
            }

            position += delta;

            animations[currentMovement].Update(dt);
        }

        public void Draw(SpriteBatch batch)
        {
            animations[currentMovement].Draw(batch, position);
        }

        public void Collide(ICollidable collider)
        {
            if(collider is WinTile)
            {
                Won = true;
                ChangeMovement(Movement.Idle);
            }
            else
            {
                ChangeMovement(collider.CurMovement);

                if(currentMovement == Movement.Dead)
                {
                    position = StartPosition;
                    Tries++;
                }
            }
        }
    }
}
