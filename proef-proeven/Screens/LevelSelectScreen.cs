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
    class LevelSelectScreen : BaseScreen
    {
        List<Button> lvlButtons;

        public LevelSelectScreen()
        {
            lvlButtons = new List<Button>();
        }

        public override void LoadContent(ContentManager content)
        {
            // load preview buttons
            base.LoadContent(content);
        }

        public void onClick(object sender)
        {
            if(sender is Button)
            {
                Button btn = sender as Button;

                // goto gamescreen with the level id
            }
        }

        public override void Update(GameTime dt)
        {
            foreach(Button b in lvlButtons)
            {
                b.Update(dt);
            }

            base.Update(dt);
        }

        public override void Draw(SpriteBatch batch)
        {
            foreach (Button b in lvlButtons)
            {
                b.Draw(batch);
            }

            base.Draw(batch);
        }
    }
}
