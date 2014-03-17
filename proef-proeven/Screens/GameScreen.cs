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

        ClickAbleObject objective1;

        /// <summary>
        /// Clickable objects will set an objective to true if it's clicked
        /// </summary>
        List<Objective> objectives;

        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="level">The level number that should be loaded</param>
        public GameScreen(int level)
        {
            GameObjects = new List<object>();
            player = new Player();
            objectives = new List<Objective>();
        }

        public override void LoadContent(ContentManager content)
        {
            objectives.Add(new Objective("objective 1"));
            objectives.Add(new Objective("Objective 2"));
            objectives.Add(new Objective("Objective 3"));

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

            objective1 = new ClickAbleObject();
            objective1.Image = content.Load<Texture2D>(@"buttons\button");
            objective1.onClick += OnClickHandler;
            objective1.ObjectiveID = 1;
            objective1.Position = new Vector2(500, 500);

            GameObjects.Add(objective1);

           base.LoadContent(content);
        }

        public void OnClickHandler(object sender)
        {
            if(sender is ClickAbleObject)
            {
                ClickAbleObject s = sender as ClickAbleObject;

                if(s.ObjectiveID >= 0 && s.ObjectiveID < objectives.Count)
                    SetObjective(s.ObjectiveID, true);
            }
        }

        public void SetObjective(int id, bool newState)
        {
            objectives[id].Done = newState;
        }

        public override void Update(GameTime dt)
        {
            foreach(object o in GameObjects)
            {
                if(o is IUpdateAble)
                {
                    (o as IUpdateAble).Update(dt);
                }
            }

            foreach (object o in GameObjects)
            {
                if (o is ICollidable)
                {
                    ICollidable collideable = o as ICollidable;

                    if(collideable.Delta != Vector2.Zero)
                    {
                        foreach (object o2 in GameObjects)
                        {
                            if (o2 is ICollidable && o != o2)
                            {
                                collideable.Collide(o2 as ICollidable);
                            }
                        }
                    }
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

#if DEBUG
            float yMargin = 0;
            // Just use one letter to find the height and add some extra margin
            float deltaMargin = Game1.Instance.fontRenderer.StringSize("H").Height + 5;
            foreach (Objective objective in objectives)
            {
                Game1.Instance.fontRenderer.DrawText(batch, new Vector2(10, 10 + yMargin), objective.Name + ": " + objective.Done, Color.Black);
                yMargin += deltaMargin;
            }
#endif

            base.Draw(batch);
        }
    }
}
