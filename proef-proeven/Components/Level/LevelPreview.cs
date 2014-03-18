using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using proef_proeven.Screens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace proef_proeven.Components.Level
{
    class LevelPreview
    {
        LevelData level;
        Button button;

        Vector2 position;

        Texture2D previewImg;

        public Vector2 Position
        {
            get
            {
                return position;
            }

            set
            {
                position = value;
                button.Position = value;
            }
        }

        public LevelPreview(LevelData level)
        {
            this.level = level;
        }

        public void LoadContent(ContentManager content)
        {
            button = new Button();
            button.OnClick += button_OnClick;
            button.Hitbox = new Rectangle((int)position.X, (int)position.Y, 256, 256);

            previewImg = content.Load<Texture2D>(@"level-preview\" + level.ID);
        }

        void button_OnClick(object sender)
        {
            if (sender == button)
            {
                if (LevelManager.Instance.IsUnlocked(level.ID))
                    ScreenManager.Instance.SetScreen(new GameScreen(level.ID));
            }
        }

        public void Update(GameTime dt)
        {
            button.Update(dt);
        }

        public void Draw(SpriteBatch batch)
        {
            if (level.Unlocked)
            {
                batch.Draw(previewImg, button.Hitbox, Color.White);

                Game1.Instance.fontRenderer.DrawText(batch, position + new Vector2(5, 5), level.Name);

                if (level.Beaten)
                    Game1.Instance.fontRenderer.DrawText(batch, position + new Vector2(5, 240), "Best " + level.Tries);
            }
            else
            {
                batch.Draw(previewImg, button.Hitbox, Color.Black);
            }
        }
    }
}
