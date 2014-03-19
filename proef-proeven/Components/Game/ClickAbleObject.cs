using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using proef_proeven.Components.Game.Interfaces;
using proef_proeven.Components.LoadData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace proef_proeven.Components.Game
{
    class ClickAbleObject : IUpdateAble, IDrawAble, ICollidable, IResetAble
    {
        private Rectangle hitbox;
        /// <summary>
        /// The Hitbox of the object. 
        /// TODO: Test if this alters the private hitbox. It shouldn't do that
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
                hitbox.Width = value.Width;
                hitbox.Height = value.Height;
            }
        }

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

                hitbox.X = (int)value.X;
                hitbox.Y = (int)value.Y;
            }
        }

        /// <summary>
        /// The position the object needs to move to when clicked
        /// </summary>
        public Vector2 moveToPosition = Vector2.Zero;

        public Rectangle Boundingbox
        {
            get { return hitbox; }
        }

        public Vector2 Delta
        {
            get { return Vector2.Zero; }
        }

        public Player.Movement CurMovement
        {
            get { return Player.Movement.Dead; }
        }

        /// <summary>
        /// The objective id of this clickable object
        /// </summary>
        public int ObjectiveID = -1;

        public delegate void OnClick(object sender);
        public event OnClick onClick;

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
                return info;
            }
        }

        public ClickAbleObject()
        {
            hitbox = new Rectangle();
        }

        /// <summary>
        /// Reset the object to the state of the beginning of the level
        /// </summary>
        public void Reset()
        {
            // use the public one to set the boundingbox
            Position = StartPosition;
        }

        /// <summary>
        /// Update the button
        /// This will fire an delegate that can be set with setOnClickDelegate if the object is pressed
        /// </summary>
        /// <param name="dt"></param>
        public virtual void Update(GameTime dt)
        {
            Vector2 mousePos = InputHelper.Instance.MousePos();

            if(hitbox.Contains((int)mousePos.X, (int)mousePos.Y) && InputHelper.Instance.IsLeftMouseReleased())
            {
                if (onClick != null)
                    onClick(this);
            }
        }

        /// <summary>
        /// Draw the image of the object 
        /// </summary>
        /// <param name="batch">The active spritebatch</param>
        public virtual void Draw(SpriteBatch batch)
        {
            if (image == null)
                return;

            batch.Draw(image, hitbox, Color.White);
        }

        public void Collide(ICollidable collider)
        {
            // Shouldn't be called because delta is zero
        }
    }
}
