using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using proef_proeven.Components.Animations;
using proef_proeven.Components.Game.Interfaces;
using proef_proeven.Components.LoadData;
using proef_proeven.Components.Util;

namespace proef_proeven.Components.Game
{
    class Plank : IUpdateAble, IDrawAble, ICollidAble, IResetAble
    {
        private Rectangle startHitbox;
        private Rectangle hitbox;
        /// <summary>
        /// The Hitbox of the object. 
        /// </summary>
        public Rectangle Hitbox
        {
            get
            {
                return hitbox;
            }

            protected set
            {
                hitbox = value;
            }
        }

        private Texture2D image;
        /// <summary>
        /// The image of the clickable object
        /// </summary>
        public Texture2D Image
        {
            get
            {
                return image;
            }

            set
            {
                image = value;
                if (!useCustomBounds)
                {
                    hitbox.Width = value.Width;
                    hitbox.Height = value.Height;
                }
            }
        }

        /// <summary>
        /// The animation if it exists
        /// </summary>
        public Animation Animation;

        /// <summary>
        /// The start position of the object
        /// </summary>
        public Vector2 StartPosition = Vector2.Zero;

        /// <summary>
        /// The path form where the texture was loaded
        /// </summary>
        public string TexturePath = "";

        private Vector2 position;
        /// <summary>
        /// The position of the clickable object
        /// </summary>
        public Vector2 Position
        {
            get { return position; }

            set
            {
                position = value;

                if (useCustomBounds)
                {
                    hitbox.X = (int)value.X + startHitbox.X;
                    hitbox.Y = (int)value.Y + startHitbox.Y;
                }
                else
                {
                    hitbox.X = (int)value.X;
                    hitbox.Y = (int)value.Y;
                }
            }
        }

        /// <summary>
        /// The position the object needs to move to when clicked
        /// </summary>
        public List<Vector2> moveToPosition;

        /// <summary>
        /// The current position in the list moveToPosition
        /// </summary>
        private int currentPosinList = -1;

        public Rectangle Boundingbox
        {
            get { return hitbox; }
        }

        private Vector2 delta;
        public Vector2 Delta
        {
            get { return delta; }
        }

        public Player.Movement CurMovement
        {
            get { return Player.Movement.Current; }
        }

        /// <summary>
        /// The objective id of this clickable object
        /// </summary>
        public int ObjectiveID = -1;

        public delegate void OnClick(object sender);
        public event OnClick onClick;

        private bool useCustomBounds;

        private ClickableObject hidingObject = null;

        /// <summary>
        /// Start information of the player.
        /// Used for level saving
        /// </summary>
        public ClickAbleInfo Info
        {
            get
            {
                ClickAbleInfo info = new ClickAbleInfo();
                info.position = StartPosition;
                info.moveToPosition = moveToPosition;
                info.objectiveID = ObjectiveID;
                info.texturePath = TexturePath;
                info.useCustomBounds = useCustomBounds;
                info.X = startHitbox.X;
                info.Y = startHitbox.Y;
                info.Width = startHitbox.Width;
                info.Height = startHitbox.Height;
                return info;
            }
        }

        public Plank()
        {
            hitbox = new Rectangle();
            moveToPosition = new List<Vector2>();
            onClick += Plank_onClick;
        }

        public Plank(Rectangle bounds)
        {
            SetCustomBounds(bounds);
            moveToPosition = new List<Vector2>();
            onClick += Plank_onClick;
        }

        private void Plank_onClick(object sender)
        {
            delta = Vector2.One * 5;

            if (hidingObject != null)
            {
                hidingObject.ChangeMovement(Player.Movement.Dead);
                hidingObject = null;
            }
        }

        public void SetCustomBounds(Rectangle bounds)
        {
            this.startHitbox = bounds;
            this.hitbox = bounds;
            this.hitbox.X = bounds.X + (int)position.X;
            this.hitbox.Y = bounds.Y + (int)position.Y;
            useCustomBounds = true;
        }

        public void NextPos()
        {
            if (currentPosinList + 1 < moveToPosition.Count)
            {
                currentPosinList++;
                Position = moveToPosition[currentPosinList];
            }
        }

        /// <summary>
        /// Reset the object to the state of the beginning of the level
        /// </summary>
        public void Reset()
        {
            // use the public one to set the boundingbox
            Position = StartPosition;
            currentPosinList = -1;
        }

        /// <summary>
        /// Update the button
        /// This will fire an delegate that can be set with setOnClickDelegate if the object is pressed
        /// </summary>
        /// <param name="dt"></param>
        public virtual void Update(GameTime dt)
        {
            Vector2 mousePos = InputHelper.Instance.MousePos();

            delta.X--;
            if (delta.X < 0)
                delta = Vector2.Zero;

            if (Animation != null)
                Animation.Update(dt);

            if (InputHelper.Instance.IsLeftMouseReleased())
            {
                if (image != null)
                {
                    Rectangle b = new Rectangle((int)position.X, (int)position.Y, image.Width, image.Height);
                    if (b.Contains((int)mousePos.X, (int)mousePos.Y))
                        if (onClick != null)
                            onClick(this);
                }
                else if (Animation != null)
                {
                    Rectangle b = new Rectangle((int)position.X, (int)position.Y, Animation.FrameWidth, Animation.FrameHeight);
                    if (b.Contains((int)mousePos.X, (int)mousePos.Y))
                        if (onClick != null)
                            onClick(this);
                }
            }
        }

        /// <summary>
        /// Draw the image of the object 
        /// </summary>
        /// <param name="batch">The active spritebatch</param>
        public virtual void Draw(SpriteBatch batch)
        {
            if (image != null)
            {
                if (useCustomBounds)
                {
                    // Draw the whole image with custom bounds
                    batch.Draw(image, new Rectangle((int)position.X, (int)position.Y, image.Width, image.Height), Color.White);
                }
                else
                {
                    // no custom bounds mean hitbox contains the position and the image size
                    batch.Draw(image, hitbox, Color.White);
                }
            }
            else if (Animation != null)
            {
                Animation.Draw(batch, position);
            }

            if (Game1.Instance.CreatorMode)
                RectangleRender.Draw(batch, hitbox);
        }

        public void Collide(ICollidAble collider)
        {
            if (collider is ClickableObject)
            {
                ClickableObject obj = collider as ClickableObject;

                if (obj.Info.texturePath.Contains("puddle") || obj.Info.texturePath.Contains("oil"))
                {
                    obj.ChangeMovement(Player.Movement.Current);
                    hidingObject = obj;
                } 

            }
        }

        public int DrawIndex()
        {
            if (hidingObject != null)
                return hidingObject.DrawIndex() + 1;

            return (int)position.Y + image.Height + 30;
        }
    }
}
