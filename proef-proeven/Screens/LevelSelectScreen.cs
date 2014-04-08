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

        int pageIndex;
        int totalPages;

        Button next, prev;
        bool showNext, showPrev;

        public LevelSelectScreen()
        {
            lvlButtons = new List<Button>();
        }

        public override void LoadContent(ContentManager content)
        {
            next = new Button();
            next.LoadImage("buttons/next");
            next.Position = new Vector2(Game1.Instance.ScreenRect.Width - 200, Game1.Instance.ScreenRect.Height - 75);
            next.OnClick += Buttons_OnClick;

            prev = new Button();
            prev.LoadImage("buttons/prev");
            prev.Position = new Vector2(100, Game1.Instance.ScreenRect.Height - 75);
            prev.OnClick += Buttons_OnClick;

            LevelManager.Instance.LoadData();
            LevelManager.Instance.UnlockLevel(0);

            

            // You start on the first page
            showPrev = false;
            // if there are more then 8 there will be an extra page
            showNext = LevelManager.Instance.LevelCount > 8;

            // load preview buttons
            previews = new List<LevelPreview>();

            int id = 0;

            if (LevelManager.Instance.LevelCount % 8 == 0)
            {
                totalPages = LevelManager.Instance.LevelCount / 8;
                totalPages = totalPages > 0 ? totalPages : 1;
            }
            else
            {
                totalPages = (int)Math.Floor((double)LevelManager.Instance.LevelCount / 8);
                totalPages++;
            }

            for (int page = 0; page < totalPages; page++)
            {
                for (int row = 0; row < 2; row++)
                {
                    for (int col = 0; col < 4; col++)
                    {
                        if (id >= LevelManager.Instance.LevelCount)
                            continue;

                        LevelPreview preview = new LevelPreview(LevelManager.Instance.GetLevel(id++));
                        preview.LoadContent(content);
                        preview.button.OnClick += button_OnClick;
                        preview.Position = new Vector2(page * Game1.Instance.ScreenRect.Width + (75 + (col * (preview.Hitbox.Width + 25))), 10 + (row * (preview.Hitbox.Height + 50)));
                        preview.button.Tag = preview.LevelID;
                        previews.Add(preview);
                    }
                }
            }

            base.LoadContent(content);
        }

        void Buttons_OnClick(object sender)
        {
            if (sender == next)
            {
                pageIndex++;

                foreach(LevelPreview preview in previews)
                {
                    preview.Position = new Vector2(preview.Position.X - Game1.Instance.ScreenRect.Width, preview.Position.Y);
                }
            }
            else if (sender == prev)
            {
                pageIndex--;

                foreach (LevelPreview preview in previews)
                {
                    preview.Position = new Vector2(preview.Position.X + Game1.Instance.ScreenRect.Width, preview.Position.Y);
                }
            }


            if(pageIndex == 0)
            {
                // remove prev button
                showPrev = false;
            }
            else
            {
                showPrev = true;
            }

            if (pageIndex < totalPages - 1)
            {
                // remove next button
                showNext = true;
            }
            else
            {
                showNext = false;
            }
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

            if (showNext)
                next.Update(dt);

            if (showPrev)
                prev.Update(dt);

            base.Update(dt);
        }

        public override void Draw(SpriteBatch batch)
        {
            foreach (LevelPreview p in previews)
            {
                p.Draw(batch);
            }

            if (showNext)
                next.Draw(batch);

            if (showPrev)
                prev.Draw(batch);

            if(Game1.Instance.CreatorMode)
            {
                Game1.Instance.fontRenderer.DrawText(batch, new Vector2(5, 5), "Level creator Mode enabled");
            }

            base.Draw(batch);
        }
    }
}
