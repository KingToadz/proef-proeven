using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using proef_proeven.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace proef_proeven.Screens
{
    class MenuScreen : BaseScreen
    {
        Texture2D bg;

        Button test;

        public MenuScreen()
        {
            test = new Button();
        }

        public override void LoadContent(ContentManager content)
        {
            bg = content.Load<Texture2D>("menu");
            test.LoadImage("button");
            test.OnClick += button_OnClick;
            test.Position = new Vector2(100, 100);

            System.Threading.Thread.Sleep(3000);

            base.LoadContent(content);
        }

        void button_OnClick(object sender)
        {
            if(sender == test)
            {
                // MSG
                Console.WriteLine("Test clicked");
            }
        }

        public override void Update(GameTime dt)
        {
            test.Update(dt);
            base.Update(dt);
        }

        public override void Draw(SpriteBatch batch)
        {
            batch.Draw(bg, Vector2.Zero, Color.White);
            test.Draw(batch);

            base.Draw(batch);
        }
    }
}
