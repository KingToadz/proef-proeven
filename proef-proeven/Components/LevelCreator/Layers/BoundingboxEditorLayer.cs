using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using proef_proeven.Components.Util;
using proef_proeven.Components.LoadData;
using proef_proeven.Screens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Input;

namespace proef_proeven.Components.LevelCreator
{
    class BoundingboxEditorLayer : BaseLayer
    {
        Texture2D overlay;
        List<Tuple<string, Texture2D>> textures;
        Dictionary<string, Texture2D> backgrounds;

        int current = 0;
        bool dragging = false;

        Rectangle boundingboxDraw;
        Rectangle boundingboxInfo;

        Vector2 center;// = Game1.Instance.ScreenCenter;
        Vector2 leftTopImg;// = new Vector2(center.X - t.Width / 2, center.Y - t.Height / 2);

        public BoundingboxEditorLayer()
        {
        }

        public override void LoadContent(ContentManager content)
        {
            boundingboxDraw = new Rectangle();

            if (ClickableObjectManager.Instance.NoTexturesLoaded)
                ClickableObjectManager.Instance.LoadTextures(content);

            textures = ClickableObjectManager.Instance.textures;

            overlay = new Texture2D(Game1.Instance.GraphicsDevice, 1, 1);
            overlay.SetData<Color>(new Color[1] { Color.FromNonPremultiplied(0, 0, 0, 150) });

            center = Game1.Instance.ScreenCenter;

            leftTopImg = new Vector2(center.X - textures[current].Item2.Width / 2, center.Y - textures[current].Item2.Height / 2);
            boundingboxDraw = ClickableObjectManager.Instance.GetBoundingbox(textures[current].Item1);
            boundingboxDraw.X += (int)leftTopImg.X;
            boundingboxDraw.Y += (int)leftTopImg.Y;

            backgrounds = new Dictionary<string, Texture2D>();
            #region old background
            /*
            foreach (Tuple<string, Texture2D> kv in textures)
            {
                Texture2D background = new Texture2D(Game1.Instance.GraphicsDevice, kv.Item2.Width, kv.Item2.Height);
                Color[] data = new Color[kv.Item2.Width * kv.Item2.Height];

                bool useGray = false;
                bool startGray = false;

                for (int y = 0; y < kv.Item2.Height; y += 16)
			    {
                    for (int x = 0; x < kv.Item2.Width; x++)
                    {
                        if (x % 16 == 0 && x != 0)
                            useGray = !useGray;

                        Color c = Color.White;
                        if(useGray)
                             c = Color.Gray;

                        for (int i = 0; i < 16; i++)
                            if (x * kv.Item2.Width + (y + i) < data.GetLength(0))
                                data[x * kv.Item2.Width + (y + i)] = c;
                    }

                    startGray = !startGray;
                    useGray = startGray;
			    }

                background.SetData<Color>(data);

                backgrounds.Add(kv.Item1, background);
            }*/
            #endregion

            base.LoadContent(content);
        }

        public override void LoadLevel(LevelFormat level)
        {
            base.LoadLevel(level);
        }

        public override void Update(GameTime time)
        {
            if (InputHelper.Instance.IsKeyPressed(Keys.A) && current > 0 && !dragging)
            { 
                current--;
                leftTopImg = new Vector2(center.X - textures[current].Item2.Width / 2, center.Y - textures[current].Item2.Height / 2);
                boundingboxDraw = ClickableObjectManager.Instance.GetBoundingbox(textures[current].Item1);
                boundingboxDraw.X += (int)leftTopImg.X;
                boundingboxDraw.Y += (int)leftTopImg.Y;
            }
            else if (InputHelper.Instance.IsKeyPressed(Keys.D) && current < textures.Count - 1 && !dragging)
            { 
                current++;
                leftTopImg = new Vector2(center.X - textures[current].Item2.Width / 2, center.Y - textures[current].Item2.Height / 2);
                boundingboxDraw = ClickableObjectManager.Instance.GetBoundingbox(textures[current].Item1);
                boundingboxDraw.X += (int)leftTopImg.X;
                boundingboxDraw.Y += (int)leftTopImg.Y;
            }

            leftTopImg = new Vector2(center.X - textures[current].Item2.Width / 2, center.Y - textures[current].Item2.Height / 2);

            if (InputHelper.Instance.IsLeftMousePressed())
            {
                if (!dragging)
                {
                    dragging = true;
                    boundingboxDraw.X = (int)InputHelper.Instance.MousePos().X;
                    boundingboxDraw.Y = (int)InputHelper.Instance.MousePos().Y;
                    boundingboxDraw.Width = 1;
                    boundingboxDraw.Height = 1;
                }
            }
            else if (InputHelper.Instance.LeftMouseDown())
            {
                boundingboxDraw.Width = (int)Math.Abs(InputHelper.Instance.MousePos().X - boundingboxDraw.X);
                boundingboxDraw.Height = (int)Math.Abs(InputHelper.Instance.MousePos().Y - boundingboxDraw.Y);
            }
            else if (InputHelper.Instance.IsLeftMouseReleased() && dragging)
            {
                // new box
                boundingboxInfo = new Rectangle();
                boundingboxInfo.X = boundingboxDraw.X - (int)leftTopImg.X;
                boundingboxInfo.Y = boundingboxDraw.Y - (int)leftTopImg.Y;
                boundingboxInfo.Width = boundingboxDraw.Width;
                boundingboxInfo.Height = boundingboxDraw.Height;

                ClickableObjectManager.Instance.ChangeBox(textures[current].Item1, boundingboxInfo);

                dragging = false;
            }
            else if (InputHelper.Instance.IsRightMousePressed() && !dragging)
            {
                // rest boundingbox
            }

            // check for left mouse up
            if (!InputHelper.Instance.LeftMouseDown())
            {
                dragging = false;
            }


            if (dragging)
            {
                blockLayerChange = true;
            }
            else
            {
                blockLayerChange = false;
            }

            base.Update(time);
        }

        private string GetCurTexString()
        {
            return textures[current].Item1;
        }

        public override void Draw(SpriteBatch batch)
        {
            if(ActiveLayer)
            {
                batch.Draw(overlay, Game1.Instance.ScreenRect, Color.White);

                if (backgrounds.ContainsKey(GetCurTexString()))
                    DrawCenter(backgrounds[GetCurTexString()], batch);

                DrawCenter(textures[current].Item2, batch);
            }

            base.Draw(batch);
        }

        public void DrawCenter(Texture2D t, SpriteBatch batch)
        {
            batch.Draw(t, leftTopImg, Color.White);
            RectangleRender.Draw(batch, boundingboxDraw, Color.White);
        }

        public void InvertRectangle()
        {
            boundingboxDraw.X = Math.Min(boundingboxDraw.X, boundingboxDraw.X + boundingboxDraw.Width);
            boundingboxDraw.Y = Math.Min(boundingboxDraw.Y, boundingboxDraw.Y + boundingboxDraw.Height);
            boundingboxDraw.Width = Math.Abs(boundingboxDraw.X + boundingboxDraw.Width - boundingboxDraw.X);
            boundingboxDraw.Height = Math.Abs(boundingboxDraw.Y + boundingboxDraw.Height - boundingboxDraw.Y);
        }

        public override List<object> getObjects()
        {
            // Probably called when the level is saved or tested
            ClickableObjectManager.Instance.Save();
            // This layer has nothing to return
            return new List<object>();
        }
    }
}
