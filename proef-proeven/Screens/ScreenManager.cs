using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
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
        Texture2D mouseTex;

        BaseTransition transition;

        private ScreenManager()
        {
            screenStack = new List<BaseScreen>();
            mouseTex = Game1.Instance.Content.Load<Texture2D>("mouse");

            transition = new FadeInTransition();
        }

        public void SetLoadingScreen(LoadingScreen loadingScreen)
        {
            this.loadingScreen = loadingScreen;
        }

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

        public void PopScreen()
        {
            if(lastScreenIndex > 0)
                screenStack.RemoveAt(lastScreenIndex);
        }

        public void Update(GameTime dt)
        {
            // No screens so why update..
            if(lastScreenIndex < 0){
                return; 
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

        public void Draw(SpriteBatch batch)
        {
            // No screens draw the loading screen?
            if (lastScreenIndex < 0)
            {
                loadingScreen.Draw(batch);
                return;
            }

            // TODO: Add popup screen support
            if (screenStack[lastScreenIndex].isContentLoaded)
            {

                screenStack[lastScreenIndex].Draw(batch);

                batch.Draw(mouseTex, new Vector2(Mouse.GetState().X - mouseTex.Width / 2, Mouse.GetState().Y - mouseTex.Height / 2), Color.White);

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
