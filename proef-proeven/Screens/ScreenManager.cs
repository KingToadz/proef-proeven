using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace proef_proeven.Screens
{
    class ScreenManager
    {
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

        private ScreenManager()
        {
            screenStack = new List<BaseScreen>();
        }

        public void SetLoadingScreen(LoadingScreen loadingScreen)
        {
            this.loadingScreen = loadingScreen;
        }

        public void SetScreen(BaseScreen screen)
        {
            screenStack.Add(screen);

            Task.Factory.StartNew(() =>
            {
                screen.LoadContent(Game1.Instance.Content);
            });
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

            if(screenStack[lastScreenIndex].isContentLoaded)
            {
                screenStack[lastScreenIndex].Update(dt);
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
            }
            else
            {
                loadingScreen.Draw(batch);
            }
        }
    }
}
