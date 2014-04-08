/*

  
 
  
  
 
                             _______  __    __       ___      .______       _______   __       ___      .__   __. 
                            /  _____||  |  |  |     /   \     |   _  \     |       \ |  |     /   \     |  \ |  | 
                           |  |  __  |  |  |  |    /  ^  \    |  |_)  |    |  .--.  ||  |    /  ^  \    |   \|  | 
                           |  | |_ | |  |  |  |   /  /_\  \   |      /     |  |  |  ||  |   /  /_\  \   |  . `  | 
                           |  |__| | |  `--'  |  /  _____  \  |  |\  \----.|  '--'  ||  |  /  _____  \  |  |\   | 
                            \______|  \______/  /__/     \__\ | _| `._____||_______/ |__| /__/     \__\ |__| \__| 
                                                                                       
                                             ______    _______       .___________. __    __   _______              
                                            /  __  \  |   ____|      |           ||  |  |  | |   ____|             
                                           |  |  |  | |  |__         `---|  |----`|  |__|  | |  |__                
                                           |  |  |  | |   __|            |  |     |   __   | |   __|               
                                           |  `--'  | |  |               |  |     |  |  |  | |  |____              
                                            \______/  |__|               |__|     |__|  |__| |_______|             
                                                                                       
                                ______     ___           _______. __    __       ___       __          _______.    
                               /      |   /   \         /       ||  |  |  |     /   \     |  |        /       |   
                              |  ,----'  /  ^  \       |   (----`|  |  |  |    /  ^  \    |  |       |   (----`   
                              |  |      /  /_\  \       \   \    |  |  |  |   /  /_\  \   |  |        \   \       
                              |  `----./  _____  \  .----)   |   |  `--'  |  /  _____  \  |  `----.----)   |      
                               \______/__/     \__\ |_______/     \______/  /__/     \__\ |_______|_______/       
                                                                                           
     
 

 

  
  
  
  
  
  
  
 
 */

#region Using Statements
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Storage;
using Microsoft.Xna.Framework.GamerServices;
using proef_proeven.Screens;
using proef_proeven.Components.Fonts;
using System.IO;
using proef_proeven.Components;
using proef_proeven.Components.Animations;
using proef_proeven.Components.Util;
using System.Runtime.InteropServices;
#endregion

namespace proef_proeven
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        private static Game1 instance;
        public static Game1 Instance
        {
            get { return instance; }
        }

        public Rectangle ScreenRect
        {
            get { return new Rectangle(0, 0, graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight);  }
        }

        public Vector2 ScreenCenter
        {
            get { return new Vector2(graphics.PreferredBackBufferWidth / 2, graphics.PreferredBackBufferHeight / 2); }
        }

        public FontRenderer fontRenderer;

        public bool CreatorMode = false;

        public Game1()
            : base()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            // pretty ugly.. 
            instance = this;
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            graphics.PreferredBackBufferWidth = 1280;
            graphics.PreferredBackBufferHeight = 720;
            graphics.ApplyChanges();

            Window.Title = "Guardian of the Casuals";

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);

            /* 1 game 1 font */
            var fontFilePath = Path.Combine(Content.RootDirectory, @"fonts\segoe32.fnt");
            var fontFile = FontLoader.Load(fontFilePath);
            var fontTexture = Content.Load<Texture2D>(@"fonts\segoe32_0.png");

            fontRenderer = new FontRenderer(fontFile, fontTexture);

            LoadingScreen loading = new LoadingScreen(Content);
            ScreenManager.Instance.SetLoadingScreen(loading);


            ScreenManager.Instance.SetScreen(new MenuScreen());
            //ScreenManager.Instance.SetScreen(new LevelCreator(0));
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
            // Always update the mouse before the screen
            InputHelper.Instance.Update();

            if (InputHelper.Instance.IsKeyPressed(Keys.C))
                CreatorMode = !CreatorMode;

            ScreenManager.Instance.Update(gameTime);

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            spriteBatch.Begin();

            ScreenManager.Instance.Draw(spriteBatch);

            spriteBatch.End();

            base.Draw(gameTime);
        }

        /// <summary>
        /// Creates an screenshot of the game. It will call the draw method once and save that to an png
        /// </summary>
        /// <param name="levelID">The level id to save to</param>
        public void ScreenShot(int levelID)
        {

            RenderTarget2D renderTarget = new RenderTarget2D(
               GraphicsDevice,
               GraphicsDevice.PresentationParameters.BackBufferWidth,
               GraphicsDevice.PresentationParameters.BackBufferHeight);

            GraphicsDevice.SetRenderTarget(renderTarget);

            Draw(new GameTime());

            GraphicsDevice.SetRenderTarget(null);

            Stream stream = File.Create(System.Environment.CurrentDirectory + Constants.CONTENT_DIR + "level-preview\\" + levelID + ".png"); ;
            if (stream != null)
            {
                SaveRenderTargetAsPng(stream, renderTarget.Width, renderTarget.Height, renderTarget);
            }
            stream.Dispose();
        }

        /// <summary>
        /// Save an RenderTarget2D as an PNG. MonoGame hasn't implemented this yet.
        /// I stole the code from https://github.com/mono/MonoGame/blob/29378026d803a8bbce61abd08b8dba2ebb6b2096/MonoGame.Framework/Graphics/Texture2D.OpenGL.cs#L559
        /// </summary>
        /// <param name="stream">The stream to save the file to</param>
        /// <param name="width">The width of the image</param>
        /// <param name="height">The height of the image</param>
        /// <param name="target">The RenderTarget to save</param>
        public void SaveRenderTargetAsPng(Stream stream, int width, int height, RenderTarget2D target)
        {
            byte[] data = null;
            GCHandle? handle = null;
            System.Drawing.Bitmap bitmap = null;
            try
            {
                data = new byte[width * height * 4];
                handle = GCHandle.Alloc(data, GCHandleType.Pinned);
                target.GetData(data);

                // internal structure is BGR while bitmap expects RGB
                for (int i = 0; i < data.Length; i += 4)
                {
                    byte temp = data[i + 0];
                    data[i + 0] = data[i + 2];
                    data[i + 2] = temp;
                }

                bitmap = new System.Drawing.Bitmap(width, height, width * 4, System.Drawing.Imaging.PixelFormat.Format32bppArgb, handle.Value.AddrOfPinnedObject());

                bitmap.Save(stream, System.Drawing.Imaging.ImageFormat.Png);
            }
            finally
            {
                if (bitmap != null)
                {
                    bitmap.Dispose();
                }
                if (handle.HasValue)
                {
                    handle.Value.Free();
                }
                if (data != null)
                {
                    data = null;
                }
            }
        }
    }
}
