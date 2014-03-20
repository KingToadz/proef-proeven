using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace proef_proeven.Components.Util
{
    class RectangleRender
    {
        /// <summary>
        /// May cause leak
        /// </summary>
        static Texture2D pixel;
        static Texture2D Pixel
        {
            get
            {
                if(pixel == null)
                {
                    pixel = new Texture2D(Game1.Instance.GraphicsDevice, 1, 1);
                    pixel.SetData<Color>(new Color[1]{ Color.Black });
                }
                return pixel;
            }
        }

        public Rectangle rectangle;

        public static void Draw(SpriteBatch batch, Rectangle rect)
        {
            batch.Draw(Pixel, new Rectangle(rect.X, rect.Y, rect.Width, 1), Color.White);
            batch.Draw(Pixel, new Rectangle(rect.X, rect.Y, 1, rect.Height), Color.White);
            batch.Draw(Pixel, new Rectangle(rect.X + rect.Width, rect.Y, 1, rect.Height), Color.White);
            batch.Draw(Pixel, new Rectangle(rect.X, rect.Y + rect.Height, rect.Width, 1), Color.White);
        }

        public void Draw(SpriteBatch batch)
        {
            if (!rectangle.IsEmpty){
                batch.Draw(Pixel, new Rectangle(rectangle.X, rectangle.Y, rectangle.Width, 1), Color.White);
                batch.Draw(Pixel, new Rectangle(rectangle.X, rectangle.Y, 1, rectangle.Height), Color.White);
                batch.Draw(Pixel, new Rectangle(rectangle.X + rectangle.Width, rectangle.Y, 1, rectangle.Height), Color.White);
                batch.Draw(Pixel, new Rectangle(rectangle.X, rectangle.Y + rectangle.Height, rectangle.Width, 1), Color.White);
            }
        }
    }
}
