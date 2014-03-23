using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using proef_proeven.Components;
using proef_proeven.Components.Level;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace proef_proeven.Screens
{
    class LevelSelectScreen : BaseScreen
    {
        List<Button> lvlButtons;
        List<LevelPreview> previews;

        public LevelSelectScreen()
        {
            lvlButtons = new List<Button>();
        }

        public override void LoadContent(ContentManager content)
        {
            LevelManager.Instance.LoadData();
            LevelManager.Instance.UnlockLevel(0);

            // load preview buttons
            previews = new List<LevelPreview>();

            int id = 0;
            for (int row = 0; row < 2; row++ )
            {
                for (int col = 0; col < 4; col++)
                {
                    // TEMP NEEDED FOR SIZE
                    Button btn = new Button();
                    btn.LoadImage(@"quickview\level-tile");

                    LevelPreview preview = new LevelPreview(LevelManager.Instance.GetLevel(id++));
                    preview.LoadContent(content);
                    preview.button.OnClick += button_OnClick;
                    preview.Position = new Vector2(col * (btn.Hitbox.Width + 25), row * (btn.Hitbox.Height + 25));
                    preview.button.Tag = preview.LevelID;
                    previews.Add(preview);
                }
            }

            base.LoadContent(content);
        }

        void button_OnClick(object sender)
        {
            if (sender is Button)
            {
                Button b = sender as Button;

                if (Game1.Instance.CreatorMode)
                {
                    ScreenManager.Instance.SetScreen(new LevelCreator((int)b.Tag));
                }
                else if (LevelManager.Instance.IsUnlocked((int)b.Tag))
                {
                    ScreenManager.Instance.SetScreen(new GameScreen((int)b.Tag));
                }
            }
        }


        public override void Update(GameTime dt)
        {
            foreach (LevelPreview p in previews)
            {
                p.Update(dt);
            }

            base.Update(dt);
        }

        public override void Draw(SpriteBatch batch)
        {
            foreach (LevelPreview p in previews)
            {
                p.Draw(batch);
            }

            if(Game1.Instance.CreatorMode)
            {
                Game1.Instance.fontRenderer.DrawText(batch, new Vector2(5, 5), "Level creator Mode enabled");
            }

            base.Draw(batch);
        }
    }
}
