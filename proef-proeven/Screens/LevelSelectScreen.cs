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
            int id = 1;
            for (int row = 0; row < 2; row++ )
            {
                for (int col = 0; col < 4; col++)
                {
                    Button btn = new Button();
                    btn.LoadImage(@"quickview\level-tile");
                    btn.OnClick += onClick;
                    btn.Position = new Vector2(col * (btn.Hitbox.Width + 25), row * (btn.Hitbox.Height + 25));

                    // Should first assign Tag to id and then add one to it
                    btn.Tag = id++;

                    lvlButtons.Add(btn);
                }
            }

            base.LoadContent(content);
        }

        public void onClick(object sender)
        {
            if(sender is Button)
            {
                Button btn = sender as Button;

                if (btn.Tag is int)
                {
                    int id = (int)btn.Tag;

                    if(LevelManager.Instance.IsUnlocked(id))
                        ScreenManager.Instance.SetScreen(new GameScreen(id));
                }
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
                Game1.Instance.fontRenderer.DrawText(batch, b.Position, b.Tag.ToString());
            }

            base.Draw(batch);
        }
    }
}
