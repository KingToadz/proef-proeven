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
                    preview.Position = new Vector2(col * (btn.Hitbox.Width + 25), row * (btn.Hitbox.Height + 25));
                    previews.Add(preview);
                }
            }

            base.LoadContent(content);
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

            base.Draw(batch);
        }
    }
}
