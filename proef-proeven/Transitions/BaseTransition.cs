using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace proef_proeven.Transitions
{
    class BaseTransition
    {
        protected TimeSpan timeLeft;
        protected TimeSpan startTime;

        public bool Started { get; protected set; }
        public bool Done { get; protected set; }

        public virtual void Reset()
        {
            timeLeft = startTime;
            Done = false;
            Started = false;
        }

        public void Start()
        {
            Started = true;
        }

        public virtual void Update(GameTime time) {

            if (Started && !Done)
            {
                timeLeft -= time.ElapsedGameTime;

                if (timeLeft.Milliseconds <= 0)
                {
                    Done = true;
                }
            }
        }

        public virtual void Draw(SpriteBatch batch) { }
    }
}
