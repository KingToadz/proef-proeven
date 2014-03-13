using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace proef_proeven.Components.Game
{
    class ClickAbleObject
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

        public delegate void OnClick(object sender);
        public event OnClick onClick;

        public ClickAbleObject()
        {
            hitbox = new Rectangle();
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
    }
}
