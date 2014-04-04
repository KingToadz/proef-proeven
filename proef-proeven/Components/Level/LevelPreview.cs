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
        public Button button;

        Vector2 position;

        Texture2D previewImg;

        public int LevelID
        {
            get
            {
                return level.ID;
            }
        }

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
            button.Hitbox = new Rectangle((int)position.X, (int)position.Y + 50, 256, 256);

            previewImg = content.Load<Texture2D>(@"level-preview\" + level.ID);
        }

        public void Update(GameTime dt)
        {
            button.Update(dt);
        }

        public void Draw(SpriteBatch batch)
        {
            if (level.Unlocked)
            {
                batch.Draw(previewImg, new Rectangle((int)position.X, (int)position.Y + 30, 256, 256), previewImg.Bounds, Color.White);

                Game1.Instance.fontRenderer.DrawText(batch, position, level.Name);

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
