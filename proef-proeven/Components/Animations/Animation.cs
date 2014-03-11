using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace proef_proeven.Components.Animations
{
    class Animation
    {
        Texture2D spritesheet;
        List<Rectangle> frames;

        int currentFrame;

        int fps = 30;
        float timePerFrame = 1.0f / 30;
        TimeSpan timer;

        bool loop;
        public bool isDone { get; private set; }

        public Animation(Texture2D sheet, int frameWidth, int frameHeight, int cols, int rows)
        {
            spritesheet = sheet;
            currentFrame = 0;
            timer = TimeSpan.FromSeconds(timePerFrame);
            loop = true;

            for (int row = 0; row < rows; row++)
            {
                for (int col = 0; col < cols; col++)
                {
                    Rectangle rect = new Rectangle();
                    rect.X = col * frameWidth;
                    rect.Y = row * frameHeight;
                    rect.Width = frameWidth;
                    rect.Height = frameHeight;
                }
            }
        }

        public void Update(GameTime dt)
        {
            if (isDone)
                return;

            timer -= dt.ElapsedGameTime;

            if(timer.Milliseconds < 0.0f)
            {
                currentFrame++;

                if (currentFrame >= frames.Count)
                {
                    if (loop)
                    {
                        currentFrame = 0;
                    }
                    else
                    {
                        isDone = true;
                    }
                }
            }
        }

        public void Draw(SpriteBatch batch, Vector2 position)
        {
            batch.Draw(spritesheet, position, frames[currentFrame], Color.White);
        }
    }
}
