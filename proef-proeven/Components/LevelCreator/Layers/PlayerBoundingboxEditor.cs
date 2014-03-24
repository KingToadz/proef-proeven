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
using proef_proeven.Components.Game;

namespace proef_proeven.Components.LevelCreator
{
    class PlayerBoundingboxEditor
    {
        Texture2D overlay;

        bool dragging = false;

        Rectangle boundingboxDraw;
        Rectangle boundingboxInfo;

        Player player;
        PlayerLayer parent;

        public bool Showing;

        public PlayerBoundingboxEditor(PlayerLayer parent)
        {
            this.parent = parent;
            overlay = new Texture2D(Game1.Instance.GraphicsDevice, 1, 1);
            overlay.SetData<Color>(new Color[1] { Color.FromNonPremultiplied(0, 0, 0, 150) });
        }

        public void Update(GameTime time)
        {
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
                boundingboxInfo.X = boundingboxDraw.X - player.Boundingbox.X;
                boundingboxInfo.Y = boundingboxDraw.Y - player.Boundingbox.Y;
                boundingboxInfo.Width = boundingboxDraw.Width;
                boundingboxInfo.Height = boundingboxDraw.Height;

                parent.ChangePlayerBounds(boundingboxInfo);
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
        }


        public void Draw(SpriteBatch batch)
        {
            batch.Draw(overlay, Game1.Instance.ScreenRect, Color.White);

            if (dragging)
                RectangleRender.Draw(batch, boundingboxDraw);

            if (player != null)
                player.Draw(batch);
        }   

        public void InvertRectangle()
        {
            boundingboxDraw.X = Math.Min(boundingboxDraw.X, boundingboxDraw.X + boundingboxDraw.Width);
            boundingboxDraw.Y = Math.Min(boundingboxDraw.Y, boundingboxDraw.Y + boundingboxDraw.Height);
            boundingboxDraw.Width = Math.Abs(boundingboxDraw.X + boundingboxDraw.Width - boundingboxDraw.X);
            boundingboxDraw.Height = Math.Abs(boundingboxDraw.Y + boundingboxDraw.Height - boundingboxDraw.Y);
        }

        public void SetPlayer(Player player)
        {
            this.player = player;
        }
    }
}
