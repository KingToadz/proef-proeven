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

        Button start;
        Button help;

        public MenuScreen()
        {
            start = new Button();
            help = new Button();
        }

        public override void LoadContent(ContentManager content)
        {
            bg = content.Load<Texture2D>("menu");
            start.LoadImage(@"buttons\start");
            start.OnClick += button_OnClick;
            start.Position = Game1.Instance.ScreenCenter;
            start.Position -= new Vector2(start.Hitbox.Width / 2, 200);

            help.LoadImage(@"buttons\help");
            help.OnClick += button_OnClick;
            help.Position = start.Position + new Vector2(0, 100);

            base.LoadContent(content);
        }

        void button_OnClick(object sender)
        {
            if (sender == start)
            {
                // MSG
                Console.WriteLine("Test clicked");
            }
            else if(sender == help)
            {
                Console.WriteLine("Help clicked");
            }
        }

        public override void Update(GameTime dt)
        {
            start.Update(dt);
            help.Update(dt);
            base.Update(dt);
        }

        public override void Draw(SpriteBatch batch)
        {
            //batch.Draw(bg, Vector2.Zero, Color.White);
            start.Draw(batch);
            help.Draw(batch);

            base.Draw(batch);
        }
    }
}
