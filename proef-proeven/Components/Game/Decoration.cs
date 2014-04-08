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
    class Decoration : BaseTile, IDrawAble
    {
        private Texture2D image;
        public Texture2D Image
        {
            get
            {
                return image;
            }

            set
            {
                image = value;
                Bounds = new Rectangle((int)Position.X, (int)Position.Y, image.Bounds.Width, image.Bounds.Height);
            }
        }

        public string ImagePath;

        /// <summary>
        /// Start information of the decoration.
        /// </summary>
        public DecorationInfo Info
        {
            get
            {
                DecorationInfo info = new DecorationInfo();
                info.ImagePath = ImagePath;
                info.position = Position;
                return info;
            }
        }

        /// <summary>
        /// Constructor used in the levelcreator
        /// </summary>
        /// <param name="imagePath">The path to the image</param>
        public Decoration(string imagePath)
            :base(new Rectangle())
        {
            this.ImagePath = imagePath;
        }

        /// <summary>
        /// Constructor used in the GameScreen
        /// </summary>
        public Decoration()
            :base(new Rectangle())
        {
            this.ImagePath = "";
        }



        public void Draw(SpriteBatch batch)
        {
            if (image != null)
                batch.Draw(image, Position, image.Bounds, Color.White);
        }

        public int DrawIndex()
        {
            return Bounds.Y + Bounds.Height;
        }
    }
}
