using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using proef_proeven.Components.Animations;
using proef_proeven.Components.Game.Interfaces;
using proef_proeven.Components.LoadData;
using proef_proeven.Components.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace proef_proeven.Components.Game
{
    class Player : IUpdateAble, IDrawAble, ICollidAble, IResetAble
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
        /// Start movement of the player object. Used for the Reset
        /// </summary>
        public Movement StartMovement = Movement.Idle;

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
        /// Use the custom boundingbox
        /// </summary>
        private bool useCustomboundingbox;

        /// <summary>
        /// A custom boundingbox for this object
        /// </summary>
        private Rectangle customBoundingbox;

        /// <summary>
        /// Boundingbox for the ICollidable interface
        /// </summary>
        public Rectangle Boundingbox
        {
            get
            {
                if(!useCustomboundingbox)
                    return new Rectangle((int)position.X, (int)position.Y, frameWidth, frameHeight);
                else
                    return new Rectangle((int)position.X + customBoundingbox.X, (int)position.Y + customBoundingbox.Y, customBoundingbox.Width, customBoundingbox.Height);
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

        /// <summary>
        /// Start information of the player.
        /// Used for level saving
        /// </summary>
        public PlayerInfo Info
        {
            get
            {
                PlayerInfo info = new PlayerInfo();
                info.position = StartPosition;
                info.startMovement = StartMovement;
                info.useCustomBoundingbox = useCustomboundingbox;
                if(useCustomboundingbox)
                {
                    info.x = customBoundingbox.X;
                    info.y = customBoundingbox.Y;
                    info.width = customBoundingbox.Width;
                    info.height = customBoundingbox.Height;
                }
                return info;
            }
        }

        public void LoadContent(ContentManager content)
        {
            currentMovement = Movement.Idle;
            position = StartPosition;

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

        public void SetCustomBoundingbox(Rectangle bounds)
        {
            useCustomboundingbox = true;
            customBoundingbox = bounds;
        }

        public void RemoveCustomBoundingBox()
        {
            useCustomboundingbox = false;
            customBoundingbox = new Rectangle();
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

            if(Game1.Instance.CreatorMode)
            {
                RectangleRender.Draw(batch, Boundingbox);
            }
        }

        public void ChangePosition(Vector2 pos)
        {
            position = pos;
        }

        public void Collide(ICollidAble collider)
        {
            if(collider is WinTile)
            {
                Won = true;
                ChangeMovement(Movement.Idle);
            }
            else if(collider is MovementTile)
            {
                Point distance = new Point(collider.Boundingbox.Center.X - this.Boundingbox.Center.X, collider.Boundingbox.Center.Y - this.Boundingbox.Center.Y);
                if (distance.X < collider.Boundingbox.Width / 2 && distance.Y < collider.Boundingbox.Height / 2)
                {
                    ChangeMovement(collider.CurMovement);
                }
            }
            else
            {
                ChangeMovement(collider.CurMovement);

                if(currentMovement == Movement.Dead)
                {
                    Tries++;
                }
            }
        }

        public void Reset()
        {
            Won = false;
            position = StartPosition;
            ChangeMovement(StartMovement);
        }
    }
}
