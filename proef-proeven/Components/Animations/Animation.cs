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
            :this(sheet, frameWidth, frameHeight, cols, rows, cols*rows, 30)
        {}

        public Animation(Texture2D sheet, int frameWidth, int frameHeight, int cols, int rows, int totalFrames, int fps)
        {
            spritesheet = sheet;
            currentFrame = 0;
            loop = true;

            this.fps = fps;
            this.timePerFrame = 1.0f / fps;
            timer = TimeSpan.FromSeconds(timePerFrame);

            frames = new List<Rectangle>();


            int curFrame = 0;
            for (int row = 0; row < rows; row++)
            {
                for (int col = 0; col < cols; col++)
                {
                    if (curFrame >= totalFrames)
                        break;

                    Rectangle rect = new Rectangle();
                    rect.X = col * frameWidth;
                    rect.Y = row * frameHeight;
                    rect.Width = frameWidth;
                    rect.Height = frameHeight;
                    frames.Add(rect);

                    curFrame++;
                }
            }
        }

        public void SetFPS(int fps)
        {
            this.fps = fps;
            this.timePerFrame = 1.0f / fps;
            timer = TimeSpan.FromSeconds(timePerFrame);
        }

        public void SetLoop(bool loop)
        {
            this.loop = loop;
        }

        public void Update(GameTime dt)
        {
            if (isDone && !loop)
                return;

            timer -= dt.ElapsedGameTime;

            if(timer.Milliseconds < 0.0f)
            {
                currentFrame++;
                timer = TimeSpan.FromSeconds(timePerFrame);

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

        public void Reset()
        {
            currentFrame = 0;
            isDone = false;
        }
    }
}
