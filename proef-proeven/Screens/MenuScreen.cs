using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace proef_proeven.Screens
{
    class MenuScreen : BaseScreen
    {
        Texture2D bg;

        public MenuScreen()
        {

        }

        public override void LoadContent(ContentManager content)
        {
            bg = content.Load<Texture2D>("menu");

            base.LoadContent(content);
        }

        public override void Update(GameTime dt)
        {
            base.Update(dt);
        }

        public override void Draw(SpriteBatch batch)
        {
            batch.Draw(bg, Vector2.Zero, Color.White);

            base.Draw(batch);
        }
    }
}
