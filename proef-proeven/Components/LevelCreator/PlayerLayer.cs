using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using proef_proeven.Components.Game;
using proef_proeven.Components.LoadData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace proef_proeven.Components.LevelCreator
{
    class PlayerLayer : BaseLayer
    {
        Player player;

        public override void LoadContent(ContentManager content)
        {
            player = new Player();
            player.LoadContent(content);
            base.LoadContent(content);
        }

        public override void LoadLevel(LevelFormat level)
        {
            player = new Player();
            player.StartPosition = level.playerInfo.position;
            player.StartMovement = level.playerInfo.startMovement;
            player.ChangePosition(level.playerInfo.position);
            player.ChangeMovement(level.playerInfo.startMovement);
            base.LoadLevel(level);
        }

        public override List<object> getObjects()
        {
            return new List<object>() { player };
        }

        public override void Update(GameTime time)
        {
            if (InputHelper.Instance.IsKeyPressed(Keys.A))
            {
                player.ChangeMovement(Player.Movement.Left);
                player.StartMovement = Player.Movement.Left;
            }
            else if (InputHelper.Instance.IsKeyPressed(Keys.D))
            {
                player.ChangeMovement(Player.Movement.Right);
                player.StartMovement = Player.Movement.Right;
            }
            else if (InputHelper.Instance.IsKeyPressed(Keys.S))
            {
                player.ChangeMovement(Player.Movement.Down);
                player.StartMovement = Player.Movement.Down;
            }
            else if (InputHelper.Instance.IsKeyPressed(Keys.W))
            {
                player.ChangeMovement(Player.Movement.Up);
                player.StartMovement = Player.Movement.Up;
            }

            if (InputHelper.Instance.LeftMouseDown())
            {
                player.StartPosition = InputHelper.Instance.MousePos();
                player.ChangePosition(player.StartPosition);
            }

            base.Update(time);
        }

        public override void Draw(SpriteBatch batch)
        {
            player.Draw(batch);

            base.Draw(batch);
        }
    }
}
