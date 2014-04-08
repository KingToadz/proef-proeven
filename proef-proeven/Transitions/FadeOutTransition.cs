using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace proef_proeven.Transitions
{
    class FadeOutTransition : BaseTransition
    {
        float alpha;
        float deltaAlpha;
        Texture2D pixel;

        public FadeOutTransition()
        {
            pixel = new Texture2D(Game1.Instance.GraphicsDevice, 1, 1);
            Color[] data = new Color[] { Color.Black };
            pixel.SetData<Color>(data);

            alpha = 0;

            startTime = TimeSpan.FromSeconds(1);
            timeLeft = startTime;

            deltaAlpha = alpha / (float)startTime.TotalMilliseconds;
        }

        /// <summary>
        /// This will reset the transition.
        /// </summary>
        public override void Reset()
        {
            alpha = 0;

            base.Reset();
        }

        /// <summary>
        /// Update the transition
        /// </summary>
        /// <param name="time">The elapsed gametime</param>
        public override void Update(GameTime time)
        {
            if (!Done && Started)
            {
                float dtAlpha = (float)time.ElapsedGameTime.TotalMilliseconds * deltaAlpha;

                if (alpha + dtAlpha > 255)
                {
                    alpha = 255;
                }
                else
                {
                    alpha += dtAlpha;
                }
            }

            base.Update(time);
        }

        /// <summary>
        /// Draw the transition
        /// </summary>
        /// <param name="batch">The active spritebatch</param>
        public override void Draw(SpriteBatch batch)
        {
            batch.Draw(pixel, Game1.Instance.ScreenRect, Color.FromNonPremultiplied(255, 255, 255, (int)alpha));

            base.Draw(batch);
        }
    }
}
