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
        Vector2 center;

        string loadingString = "Loading...";

        public LoadingScreen(ContentManager content)
        {
            // Load some stuff
            background = Game1.Instance.Content.Load<Texture2D>("mouse");

            center = Game1.Instance.ScreenCenter;
            Rectangle rect = Game1.Instance.fontRenderer.StringSize(loadingString);
            center.X -= rect.Width / 2;
            center.Y -= rect.Height / 2;

            isContentLoaded = true;
        }

        public override void Draw(SpriteBatch batch)
        {
            if(background != null)
                batch.Draw(background, Game1.Instance.ScreenRect, Color.White);

            Game1.Instance.fontRenderer.DrawText(batch, center, loadingString);

            base.Draw(batch);
        }
    }
}
