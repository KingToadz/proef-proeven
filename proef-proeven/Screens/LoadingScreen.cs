using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace proef_proeven.Screens
{
    class LoadingScreen : BaseScreen
    {
        Texture2D background;

        public LoadingScreen(ContentManager content)
        {
            // Load some stuff
            background = Game1.Instance.Content.Load<Texture2D>("mouse");

            isContentLoaded = true;
        }

        public override void Draw(SpriteBatch batch)
        {
            if(background != null)
                batch.Draw(background, Game1.Instance.ScreenRect, Color.White);

            base.Draw(batch);
        }
    }
}
