using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using proef_proeven.Components.Game;
using proef_proeven.Components.LoadData;
using proef_proeven.Components.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace proef_proeven.Components.LevelCreator
{
    class WinTileLayer : BaseLayer
    {
        List<WinTile> tiles;
        Texture2D pixel;
        Rectangle wSize;
        Rectangle winTileSize;

        bool dragging;

        public WinTileLayer()
        {
            tiles = new List<WinTile>();
            wSize = Game1.Instance.fontRenderer.CharSize;
            dragging = false;
        }

        public override List<object> getObjects()
        {
            return tiles.ToList<object>();
        }

        public override void LoadContent(ContentManager content)
        {
            pixel = new Texture2D(Game1.Instance.GraphicsDevice, 1, 1);
            pixel.SetData<Color>(new Color[1] { Color.Black });
            base.LoadContent(content);
        }

        public override void LoadLevel(LoadData.LevelFormat level)
        {
            foreach (MovementTileInfo info in level.moveTiles)
            {
                if (info.WinningTile)
                    tiles.Add(new WinTile(new Rectangle(info.X, info.Y, info.Width, info.Height)));
            }

            base.LoadLevel(level);
        }

        public override void Update(GameTime time)
        {
            if (InputHelper.Instance.IsLeftMousePressed())
            {
                if (!dragging)
                {
                    dragging = true;
                    winTileSize.X = (int)InputHelper.Instance.MousePos().X;
                    winTileSize.Y = (int)InputHelper.Instance.MousePos().Y;
                    winTileSize.Width = (int)Math.Abs(InputHelper.Instance.MousePos().X - winTileSize.X);
                    winTileSize.Height = (int)Math.Abs(InputHelper.Instance.MousePos().Y - winTileSize.Y);
                }
            }
            else if (InputHelper.Instance.LeftMouseDown())
            {
                winTileSize.Width = (int)Math.Abs(InputHelper.Instance.MousePos().X - winTileSize.X);
                winTileSize.Height = (int)Math.Abs(InputHelper.Instance.MousePos().Y - winTileSize.Y);
            }
            else if (InputHelper.Instance.IsLeftMouseReleased() && dragging)
            {
                WinTile win = new WinTile(winTileSize);
                tiles.Add(win);
                dragging = false;
            }
            else if (InputHelper.Instance.IsRightMousePressed() && !dragging)
            {
                for (int i = 0; i < tiles.Count; i++)
                {
                    WinTile o = tiles[i];

                    if (o.Boundingbox.Contains(InputHelper.Instance.MousePos().toPoint()))
                    {
                        tiles.Remove(o);
                    }
                }
            }

            // check for left mouse up
            if(!InputHelper.Instance.LeftMouseDown())
            {
                dragging = false;
            }


            if (dragging)
            {
                blockLayerChange = true;
                winTileSize = GetInvertedRectangle(winTileSize);
            }
            else
            {
                blockLayerChange = false;
            }
            base.Update(time);
        }

        Rectangle GetInvertedRectangle(Rectangle checkRect)
        {
            Rectangle r = new Rectangle();

            r.X = checkRect.X;
            r.Y = checkRect.Y;

            if (checkRect.Width < 0)
            {
                r.X += checkRect.Width;
                r.Width = -checkRect.Width;
            }
            else
            {
                r.Width = checkRect.Width;
            }

            if (checkRect.Height < 0)
            {
                r.Y += checkRect.Height;
                r.Height = -checkRect.Height;
            }
            else
            {
                r.Height = checkRect.Height;
            }

            return r;
        }

        public override void Draw(SpriteBatch batch)
        {
            foreach(WinTile t in tiles)
            {
                batch.Draw(pixel, new Rectangle(t.Bounds.X, t.Bounds.Y, t.Bounds.Width, 1), Color.White);
                batch.Draw(pixel, new Rectangle(t.Bounds.X, t.Bounds.Y, 1, t.Bounds.Height), Color.White);
                batch.Draw(pixel, new Rectangle(t.Bounds.X + t.Bounds.Width, t.Bounds.Y, 1, t.Bounds.Height), Color.White);
                batch.Draw(pixel, new Rectangle(t.Bounds.X, t.Bounds.Y + t.Bounds.Height, t.Bounds.Width, 1), Color.White);
                Game1.Instance.fontRenderer.DrawText(batch, new Vector2((t.Bounds.X + t.Bounds.Width / 2) - wSize.Width / 2, (t.Bounds.Y + t.Bounds.Height / 2) - wSize.Height), "W");
            }

            if (dragging && ActiveLayer)
            {
                batch.Draw(pixel, new Rectangle(winTileSize.X, winTileSize.Y, winTileSize.Width, 1), Color.White);
                batch.Draw(pixel, new Rectangle(winTileSize.X, winTileSize.Y, 1, winTileSize.Height), Color.White);
                batch.Draw(pixel, new Rectangle(winTileSize.X + winTileSize.Width, winTileSize.Y, 1, winTileSize.Height), Color.White);
                batch.Draw(pixel, new Rectangle(winTileSize.X, winTileSize.Y + winTileSize.Height, winTileSize.Width, 1), Color.White);
                Game1.Instance.fontRenderer.DrawText(batch, new Vector2((winTileSize.X + winTileSize.Width / 2) - wSize.Width / 2, (winTileSize.Y + winTileSize.Height / 2) - wSize.Height), "W");
            }

            base.Draw(batch);
        }
    }
}
