﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using proef_proeven.Transitions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace proef_proeven.Screens
{
    class BaseScreen
    {
        /// <summary>
        /// bool to check if the content was loaded
        /// if isConentLoaded is true the Update and Draw will be called
        /// </summary>
        public bool isContentLoaded { get; protected set; }

        /// <summary>
        /// Not used right now. Not really needed
        /// </summary>
        public Vector2 position { get; protected set; }

        /// <summary>
        /// Is this screen fullscreen or just an popup
        /// </summary>
        public bool isFullscreen { get; protected set; }

        /// <summary>
        /// The kind of transition this screen will have
        /// </summary>
        public TransitionKind transitionKind { get; protected set; }

        public BaseScreen(TransitionKind transitionKind = TransitionKind.FadeIn)
        {
            this.position = Vector2.Zero;
            this.isFullscreen = true;
            this.transitionKind = transitionKind;
        }

        public BaseScreen(Vector2 position, TransitionKind transitionKind)
        {
            this.position = position;
            this.isFullscreen = false;
            this.transitionKind = transitionKind;
        }

        /// <summary>
        /// Called when the will be the new top of the stack
        /// </summary>
        public virtual void Enter() { }

        /// <summary>
        /// Called when an other screen will be the top
        /// </summary>
        public virtual void Leave() { }

        /// <summary>
        /// Function that will load the content
        /// </summary>
        /// <param name="content">The ContentManager the game uses</param>
        public virtual void LoadContent(ContentManager content)
        {
            isContentLoaded = true;
        }

        /// <summary>
        /// Updates the screen. Called every frame
        /// </summary>
        /// <param name="dt"></param>
        public virtual void Update(GameTime dt){ }

        /// <summary>
        /// Draws the screen.
        /// </summary>
        /// <param name="batch">The started spritebatch</param>
        public virtual void Draw(SpriteBatch batch){ }

        /// <summary>
        /// This will be called when the back button is pressed
        /// </summary>
        /// <returns>Return wether or not this screen handled the backbutton</returns>
        public virtual bool HandledBackbutton()
        {
            return false;
        }
    }
}
