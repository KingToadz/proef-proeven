using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace proef_proeven.Transitions
{
    class FadeInTransition : BaseTransition
    {
        float alpha;
        float deltaAlpha;
        Texture2D pixel;

        public FadeInTransition()
        {
            pixel = new Texture2D(Game1.Instance.GraphicsDevice, 1, 1);
            Color[] data = new Color[] { Color.Black };
            pixel.SetData<Color>(data);

            alpha = 255;

            startTime = TimeSpan.FromSeconds(1);
            timeLeft = startTime;

            deltaAlpha = alpha / (float)startTime.TotalMilliseconds;
        }

        public override void Reset()
        {
            alpha = 255;

            base.Reset();
        }

        public override void Update(GameTime time)
        {
            if (!Done && Started)
            {
                float dtAlpha = (float)time.ElapsedGameTime.TotalMilliseconds * deltaAlpha;

                if (alpha - dtAlpha < 0)
                {
                    alpha = 0;
                }
                else
                {
                    alpha -= dtAlpha;
                }
            }

            base.Update(time);
        }

        public override void Draw(SpriteBatch batch)
        {
            batch.Draw(pixel, Game1.Instance.ScreenRect, Color.FromNonPremultiplied(255, 255, 255, (int)alpha));

            base.Draw(batch);
        }
    }
}
