using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using proef_proeven.Components.Game;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace proef_proeven.Screens
{
    class GameScreen : BaseScreen
    {
        ClickAbleObject test;

        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="level">The level number that should be loaded</param>
        public GameScreen(int level)
        {

        }

        public override void LoadContent(ContentManager content)
        {
            Texture2D img = content.Load<Texture2D>("animtest");
            test = new ClickAbleObject();
            test.Image = img;
            test.Position = new Vector2(100, 100);
            test.setOnClickDelegate(new ClickAbleObject.OnClick(OnClickHandler));

            base.LoadContent(content);
        }

        public void OnClickHandler(object sender)
        {
            Console.WriteLine("Fuck yeah iet works");
        }

        public override void Update(GameTime dt)
        {
            test.Update(dt);

            base.Update(dt);
        }

        public override void Draw(SpriteBatch batch)
        {
            Game1.Instance.fontRenderer.DrawText(batch, new Vector2(300, 200), "Game Screen!", Color.ForestGreen);

            test.Draw(batch);

            base.Draw(batch);
        }
    }
}
