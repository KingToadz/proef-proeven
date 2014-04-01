using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using proef_proeven.Components.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace proef_proeven.Components.LevelCreator.Layers
{
    class GridSize
    {
        public int Width;
        public int Height;
        public int Columns;
        public int Rows;
    }

    class GridLayer : BaseLayer
    {
        GridSize gridSize;
        Texture2D tiles;
        List<Rectangle> clipRects;
        int[,] grid;

        public override void LoadContent(ContentManager content)
        {
            if (IOHelper.Instance.DoesDirectoryExists(Constants.LEVEL_CREATOR_DIR + @"gird\tiles.json"))
            {
                gridSize = Newtonsoft.Json.JsonConvert.DeserializeObject<GridSize>(Constants.LEVEL_CREATOR_DIR + @"gird\tiles.json");
                tiles = content.Load<Texture2D>(@"gird\tiles");

                clipRects = new List<Rectangle>();
                grid = new int[gridSize.Columns, gridSize.Rows];

                for(int row = 0; row < gridSize.Rows; row++)
                {
                    for (int col = 0; col < gridSize.Columns; col++)
                    {
                        clipRects.Add(new Rectangle(col * gridSize.Width, row * gridSize.Height, gridSize.Width, gridSize.Height));
                        grid[col, row] = -1;
                    }
                }
            }

            base.LoadContent(content);
        }

        public override void LoadLevel(LoadData.LevelFormat level)
        {
            if(level.Grid != null && level.Grid.Count > 0)
            {
                // Laad de tiles

            }

            base.LoadLevel(level);
        }

        public override void Update(GameTime time)
        {
            base.Update(time);
        }

        public override void Draw(SpriteBatch batch)
        {
            for (int row = 0; row < gridSize.Rows; row++)
            {
                for (int col = 0; col < gridSize.Columns; col++)
                {
                    if(grid[col, row] > -1)
                    {
                        batch.Draw(tiles, new Vector2(col * gridSize.Width, row * gridSize.Height), clipRects[grid[col, row]], Color.White);
                    }
                }
            }

            base.Draw(batch);
        }

        public override List<object> getObjects()
        {
            throw new NotImplementedException();
        }
    }
}
