using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using proef_proeven.Components;
using proef_proeven.Components.Game;
using proef_proeven.Components.Game.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace proef_proeven.Screens
{
    class GameScreen : BaseScreen
    {
        Grid grid;
        Player player;

        List<object> GameObjects;

        /// <summary>
        /// Clickable objects will set an objective to true if it's clicked
        /// </summary>
        Dictionary<string, bool> objectives;

        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="level">The level number that should be loaded</param>
        public GameScreen(int level)
        {
            GameObjects = new List<object>();
            player = new Player();
            objectives = new Dictionary<string, bool>();
        }

        public override void LoadContent(ContentManager content)
        {
            Texture2D tileSheet = content.Load<Texture2D>("tiles");
            grid = new Grid();

            int tileWidth = 16;
            int tileHeight = 16;
            int cols = 30;
            int rows = 16;

            for (int row = 0; row < rows; row++)
            {
                for (int col = 0; col < cols; col++)
                {
                    grid.AddTile(new Tile(tileSheet, new Vector2(col * 16, row * 16), new Rectangle(col * 16, row * 16, 16, 16)), row, col);
                }
            }
            GameObjects.Add(grid);

            player.LoadContent(content);
            GameObjects.Add(player);

           base.LoadContent(content);
        }

        public void OnClickHandler(object sender)
        {
            Console.WriteLine("Fuck yeah iet works");
        }

        public override void Update(GameTime dt)
        {
            if(InputHelper.Instance.IsKeyDown(Keys.Left))
            {
                player.ChangeMovement(Player.Movement.Left);
            }
            else if (InputHelper.Instance.IsKeyDown(Keys.Right))
            {
                player.ChangeMovement(Player.Movement.Right);
            }
            else if (InputHelper.Instance.IsKeyDown(Keys.Up))
            {
                player.ChangeMovement(Player.Movement.Up);
            }
            else if (InputHelper.Instance.IsKeyDown(Keys.Down))
            {
                player.ChangeMovement(Player.Movement.Down);
            }

            foreach(object o in GameObjects)
            {
                if(o is IUpdateAble)
                {
                    (o as IUpdateAble).Update(dt);
                }
            }

            base.Update(dt);
        }

        public override void Draw(SpriteBatch batch)
        {

            foreach (object o in GameObjects)
            {
                if (o is IDrawAble)
                {
                    (o as IDrawAble).Draw(batch);
                }
            }

            Game1.Instance.fontRenderer.DrawText(batch, new Vector2(300, 200), "Game Screen!", Color.ForestGreen);

            base.Draw(batch);
        }
    }
}
