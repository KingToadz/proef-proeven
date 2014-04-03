using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using proef_proeven.Components.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace proef_proeven.Components.LevelCreator.Layers
{
    class GridSize
    {
        public int Width;
        public int Height;
        public int Columns;
        public int Rows;

        public int tileColumns;
        public int tileRows;
        public int totalTiles;
    }

    class GridTileInfo
    {
        public int row;
        public int column;
        public int cliprect;
    }

    class GridLayer : BaseLayer
    {
        GridSize gridSize;
        Texture2D tiles;
        List<Rectangle> clipRects;
        int[,] grid;

        int currentClipRect;

        public override void LoadContent(ContentManager content)
        {
            if (IOHelper.Instance.DoesDirectoryExists(Constants.LEVEL_CREATOR_DIR + "grid") && IOHelper.Instance.DoesFileExist(Constants.LEVEL_CREATOR_DIR + @"grid\tiles.json"))
            {
                gridSize = Newtonsoft.Json.JsonConvert.DeserializeObject<GridSize>(IOHelper.Instance.ReadFile(Constants.LEVEL_CREATOR_DIR + @"grid\tiles.json"));
                tiles = content.Load<Texture2D>(@"level-editor\grid\tiles");

                clipRects = new List<Rectangle>();
                currentClipRect = 0;
                grid = new int[Game1.Instance.ScreenRect.Width / gridSize.Width, Game1.Instance.ScreenRect.Height / gridSize.Rows];

                for (int row = 0; row < grid.GetLength(1); row++)
                {
                    for (int col = 0; col < grid.GetLength(0); col++)
                    {
                        grid[col, row] = -1;
                    }
                }

                for (int row = 0; row < gridSize.tileRows; row++)
                {
                    for (int col = 0; col < gridSize.tileColumns; col++)
                    {
                        clipRects.Add(new Rectangle(col * gridSize.Width, row * gridSize.Height, gridSize.Width, gridSize.Height));
                    }
                }
            }

            base.LoadContent(content);
        }

        public override void LoadLevel(LoadData.LevelFormat level)
        {
            if(level.Grid != null && level.Grid.Count > 0)
            {
                foreach(GridTileInfo info in level.Grid)
                {
                    grid[info.column, info.row] = info.cliprect;
                }
            }

            base.LoadLevel(level);
        }

        public override void Update(GameTime time)
        {
            if(InputHelper.Instance.IsKeyPressed(Keys.A) && currentClipRect > 0)
            {
                currentClipRect--;
            }
            else if(InputHelper.Instance.IsKeyPressed(Keys.D) && currentClipRect + 1 < clipRects.Count)
            {
                currentClipRect++;
            }

            if(InputHelper.Instance.LeftMouseDown())
            {
                Vector2 mpos = InputHelper.Instance.MousePos();

                int col = 0;
                int row = 0;

                // the while loops will place the tile at the right position
                while((int)mpos.X % gridSize.Width != 0)
                {
                    mpos.X--;
                }                

                while ((int)mpos.Y % gridSize.Height != 0)
                {
                    mpos.Y--;
                }

                col = (int)mpos.X / gridSize.Width;
                row = (int)mpos.Y / gridSize.Height;


                if(col >= 0 && row >= 0 && col < grid.GetLength(0) && row < grid.GetLength(1))
                    grid[col, row] = currentClipRect;
            }
            else if(InputHelper.Instance.IsRightMousePressed())
            {
                Vector2 mpos = InputHelper.Instance.MousePos();

                int col = 0;
                int row = 0;

                // the while loops will place the tile at the right position
                while ((int)mpos.X % gridSize.Width != 0)
                {
                    mpos.X--;
                }

                while ((int)mpos.Y % gridSize.Height != 0)
                {
                    mpos.Y--;
                }

                col = (int)mpos.X / gridSize.Width;
                row = (int)mpos.Y / gridSize.Height;

                if (col >= 0 && row >= 0 && col < grid.GetLength(0) && row < grid.GetLength(1))
                    grid[col, row] = -1;
            }

            base.Update(time);
        }

        public override void Draw(SpriteBatch batch)
        {
            for (int row = 0; row < grid.GetLength(1); row++)
            {
                for (int col = 0; col < grid.GetLength(0); col++)
                {
                    if(grid[col, row] > -1)
                    {
                        //                      The vector2 in the position                          the cliprect of the tile
                        batch.Draw(tiles, new Vector2(col * gridSize.Width, row * gridSize.Height), clipRects[grid[col, row]], Color.White);
                    }
                }
            }

            if(ActiveLayer)
            {
                batch.Draw(tiles, InputHelper.Instance.MousePos(), clipRects[currentClipRect], Color.White);
            }

            base.Draw(batch);
        }

        public override List<object> getObjects()
        {
            List<GridTileInfo> cliprects = new List<GridTileInfo>();
            for (int row = 0; row < grid.GetLength(1); row++)
            {
                for (int col = 0; col < grid.GetLength(0); col++)
                {
                    cliprects.Add(new GridTileInfo() { column = col, row = row, cliprect = grid[col, row] });
                }
            }
            
            return cliprects.ToList<object>();
        }
    }
}
