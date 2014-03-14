using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace proef_proeven.Components.Game
{
    class Grid
    {
        readonly int width = 30;
        readonly int height = 16;

        Tile[,] grid;

        public Grid(int w, int h)
        {
            grid = new Tile[w, h];
        }

        public Grid()
            :this(30, 16)
        {}

        public void AddTile(Tile tile, int row, int col)
        {
            if (Math.Abs(col) < grid.GetLength(0) && Math.Abs(row) < grid.GetLength(1))
                Console.WriteLine("GRID ERRROR: col or row is to big");

            grid[Math.Abs(col), Math.Abs(row)] = tile;
        }

        public void Draw(SpriteBatch batch)
        {
            for (int row = 0; row < height; row++)
            {
                for (int col = 0; col < width; col++)
                {
                    grid[col, row].Draw(batch);
                }
            }
        }
    }
}
