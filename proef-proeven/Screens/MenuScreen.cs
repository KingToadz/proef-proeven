using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using proef_proeven.Components;
using proef_proeven.Components.Animations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace proef_proeven.Screens
{
    class MenuScreen : BaseScreen
    {
        Button start;
        Button help;

        public MenuScreen()
        {
            start = new Button();
            help = new Button();
        }

        public override void LoadContent(ContentManager content)
        {
            start.LoadImage(@"buttons\start");
            start.OnClick += button_OnClick;
            start.Position = Game1.Instance.ScreenCenter;
            start.Position -= new Vector2(start.Hitbox.Width / 2, start.Hitbox.Height / 2);

            help.LoadImage(@"buttons\help");
            //help.OnClick += button_OnClick;
            help.Position = start.Position + new Vector2(0, 100);

            base.LoadContent(content);
        }

        /// <summary>
        /// Onclick handler for all the buttons on this screen
        /// </summary>
        /// <param name="sender"></param>
        void button_OnClick(object sender)
        {
            if (sender == start)
            {
                ScreenManager.Instance.SetScreen(new LevelSelectScreen());
            }
            else if(sender == help)
            {
                throw new NotImplementedException();
            }
        }

        public override void Update(GameTime dt)
        {
            start.Update(dt);
            //help.Update(dt);

            base.Update(dt);
        }

        public override void Draw(SpriteBatch batch)
        {
            start.Draw(batch);
            //help.Draw(batch);

            base.Draw(batch);
        }
    }
}
