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

        /// <summary>
        /// This will reset the transition.
        /// </summary>
        public virtual void Reset()
        {
            timeLeft = startTime;
            Done = false;
            Started = false;
        }

        /// <summary>
        /// Start the transition
        /// </summary>
        public void Start()
        {
            Started = true;
        }

        /// <summary>
        /// Update the transition
        /// </summary>
        /// <param name="time">The elapsed gametime</param>
        public virtual void Update(GameTime time) 
        {
            if (Started && !Done)
            {
                timeLeft -= time.ElapsedGameTime;

                if (timeLeft.Milliseconds <= 0)
                {
                    Done = true;
                }
            }
        }

        /// <summary>
        /// Draw the transition
        /// </summary>
        /// <param name="batch">The active spritebatch</param>
        public virtual void Draw(SpriteBatch batch) { }
    }
}
