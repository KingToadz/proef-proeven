using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using proef_proeven.Components.Game.Interfaces;
using proef_proeven.Components.LevelCreator.Layers;
using proef_proeven.Components.LoadData;
using proef_proeven.Components.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace proef_proeven.Components.Game
{
    class Grid : IDrawAble
    {
        int width;
        int height;

        int[,] grid;

        public List<Rectangle> clipRects { get; set; }

        public int currentClipRect { get; set; }

        public Texture2D tiles { get; set; }

        public GridSize gridSize { get; set; }

        public Grid()
        {
            if (IOHelper.Instance.DoesDirectoryExists(Constants.LEVEL_CREATOR_DIR + "grid") && IOHelper.Instance.DoesFileExist(Constants.LEVEL_CREATOR_DIR + @"grid\tiles.json"))
            {
                gridSize = Newtonsoft.Json.JsonConvert.DeserializeObject<GridSize>(IOHelper.Instance.ReadFile(Constants.LEVEL_CREATOR_DIR + @"grid\tiles.json"));
                tiles = Game1.Instance.Content.Load<Texture2D>(@"level-editor\grid\tiles");

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
        }

        public void LoadFromLevelInfo(LevelFormat lvl)
        {
            if (lvl.Grid != null && lvl.Grid.Count > 0)
            {
                foreach (GridTileInfo info in lvl.Grid)
                {
                    grid[info.column, info.row] = info.cliprect;
                }
            }
        }

        public void Draw(SpriteBatch batch)
        {
            for (int row = 0; row < grid.GetLength(1); row++)
            {
                for (int col = 0; col < grid.GetLength(0); col++)
                {
                    if (grid[col, row] > -1)
                    {
                        //                      The vector2 in the position                          the cliprect of the tile
                        batch.Draw(tiles, new Vector2(col * gridSize.Width, row * gridSize.Height), clipRects[grid[col, row]], Color.White);
                    }
                }
            }
        }
    }
}
