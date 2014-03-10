using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace proef_proeven.Components
{
    class Button
    {
        protected Rectangle hitbox;
        public Rectangle Hitbox
        {
            get { return hitbox; }

            protected set
            {
                hitbox = value;
                position.X = hitbox.X;
                position.Y = hitbox.Y;
            }
        }

        private Vector2 position;
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

        public delegate void onClick(object sender);
        public event onClick OnClick;

        public object Tag;
        
        protected Texture2D image;
        protected MouseState ms, lms;

        /// <summary>
        /// Load an new image 
        /// </summary>
        /// <param name="img">The content string for the image</param>
        public void LoadImage(string img)
        {
            image = Game1.Instance.Content.Load<Texture2D>(img);
            hitbox.Width = image.Width;
            hitbox.Height = image.Height;
        }

        /// <summary>
        /// Update the button.
        /// Check if it was pressed
        /// </summary>
        /// <param name="dt"></param>
        public void Update(GameTime dt)
        {
            ms = Mouse.GetState();

            Vector2 mousePos = new Vector2(Mouse.GetState().X, Mouse.GetState().Y);

            // Mouse last frame pressed this frame released
            if(hitbox.Contains((int)mousePos.X, (int)mousePos.Y) && ms.LeftButton == ButtonState.Released && lms.LeftButton == ButtonState.Pressed)
            {
                if (OnClick != null)
                    OnClick(this);
            }

            lms = ms;
        }

        /// <summary>
        /// Draw the button
        /// </summary>
        /// <param name="batch">Batch to draw to</param>
        public virtual void Draw(SpriteBatch batch)
        {
            batch.Draw(image, hitbox, Color.White);
        }
    }
}
