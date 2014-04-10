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

        GameScreen loadingLevel;

        public LoadingScreen(ContentManager content)
        {
            center = Game1.Instance.ScreenCenter;
            Rectangle rect = Game1.Instance.fontRenderer.StringSize(loadingString);
            center.X -= rect.Width / 2;
            center.Y -= rect.Height / 2;

            loadingLevel = new GameScreen(0);
            loadingLevel.LoadContent(content);

            isContentLoaded = true;
        }

        public override void Update(GameTime dt)
        {
            loadingLevel.Update(dt);
            base.Update(dt);
        }

        public override void Draw(SpriteBatch batch)
        {
            loadingLevel.Draw(batch);

            Game1.Instance.fontRenderer.DrawText(batch, center, loadingString);

            base.Draw(batch);
        }
    }
}
