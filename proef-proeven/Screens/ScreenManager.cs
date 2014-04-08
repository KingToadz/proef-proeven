using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using proef_proeven.Components;
using proef_proeven.Transitions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace proef_proeven.Screens
{
    class ScreenManager
    {
        /// <summary>
        /// The states of the screen manager
        /// </summary>
        enum State
        {
            Running,
            Loading,
            Transitioning
        }

        private State state;

        private static ScreenManager instance;
        public static ScreenManager Instance
        {
            get
            {
                if (instance == null)
                    instance = new ScreenManager();

                return instance;
            }

            protected set { instance = value; }
        }

        private int lastScreenIndex
        {
            get { return screenStack.Count - 1; }
        }

        List<BaseScreen> screenStack;
        LoadingScreen loadingScreen;

        BaseTransition transition;

        /// <summary>
        /// Is there an screen loading
        /// </summary>
        public bool IsLoading
        {
            get
            {
                return state != State.Running;
            }
        }

        private ScreenManager()
        {
            screenStack = new List<BaseScreen>();

            transition = new FadeInTransition();
        }

        /// <summary>
        /// Set the loading screen.
        /// This screen should have already loaded it's content
        /// </summary>
        /// <param name="loadingScreen">The loadingscreen of the game</param>
        public void SetLoadingScreen(LoadingScreen loadingScreen)
        {
            this.loadingScreen = loadingScreen;
        }

        /// <summary>
        /// Change the current screen. 
        /// This will load the content of the screen if necessary
        /// </summary>
        /// <param name="screen">The screen to add to the stack</param>
        public void SetScreen(BaseScreen screen)
        {
            screenStack.Add(screen);

            if (!screen.isContentLoaded)
            {
                Task.Factory.StartNew(() =>
                {
                    this.state = State.Loading;
                    screen.LoadContent(Game1.Instance.Content);
                    this.transition = TransitionFactory.GetTransition(screen.transitionKind);
                    this.transition.Start();
                    this.state = State.Transitioning;
                });
            }
        }

        /// <summary>
        /// Change the current screen. 
        /// But don't show an transition
        /// This will load the content of the screen if necessary
        /// </summary>
        /// <param name="screen">The screen to add to the stack</param>
        public void SetScreenNoTransition(BaseScreen screen)
        {
            screenStack.Add(screen);

            if (!screen.isContentLoaded)
            {
                Task.Factory.StartNew(() =>
                {
                    this.state = State.Loading;
                    screen.LoadContent(Game1.Instance.Content);
                    this.state = State.Running;
                });
            }
        }

        /// <summary>
        /// Remove the current screen and go back to the last one
        /// </summary>
        public void PopScreen()
        {
            if(lastScreenIndex > 0)
                screenStack.RemoveAt(lastScreenIndex);
        }

        /// <summary>
        /// Updates the screen or transition that needs to be updated 
        /// </summary>
        /// <param name="dt">GameTime from Game1 instance</param>
        public void Update(GameTime dt)
        {
            // No screens so why update..
            if(lastScreenIndex < 0){
                return; 
            }

            if(InputHelper.Instance.IsKeyReleased(Keys.Back) && lastScreenIndex > 0)
            {
                if(!screenStack[lastScreenIndex].HandledBackbutton())
                    PopScreen();
            }

            if(screenStack[lastScreenIndex].isContentLoaded && state == State.Running)
            {
                screenStack[lastScreenIndex].Update(dt);
            }
            else if(state == State.Transitioning)
            {
                transition.Update(dt);

                if (transition.Done)
                {
                    state = State.Running;
                }
            }
        }

        /// <summary>
        /// Draw the current screen or the loading screen if needed
        /// </summary>
        /// <param name="batch">An active spritebatch</param>
        public void Draw(SpriteBatch batch)
        {
            // No screens draw the loading screen?
            if (lastScreenIndex < 0)
            {
                loadingScreen.Draw(batch);
                return;
            }

            if (screenStack[lastScreenIndex].isContentLoaded)
            {

                screenStack[lastScreenIndex].Draw(batch);
                if(state == State.Transitioning)
                {
                    transition.Draw(batch);
                }
            }
            else
            {
                loadingScreen.Draw(batch);
            }
        }
    }
}
