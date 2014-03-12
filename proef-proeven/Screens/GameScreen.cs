using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace proef_proeven.Screens
{
    class GameScreen : BaseScreen
    {
        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="level">The level number that should be loaded</param>
        public GameScreen(int level)
        {

        }

        public override void LoadContent(ContentManager content)
        {
            base.LoadContent(content);
        }

        public override void Update(GameTime dt)
        {
            base.Update(dt);
        }

        public override void Draw(SpriteBatch batch)
        {
            Game1.Instance.fontRenderer.DrawText(batch, new Vector2(300, 200), "Game Screen!", Color.ForestGreen);

            base.Draw(batch);
        }
    }
}
