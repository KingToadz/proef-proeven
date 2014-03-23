using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using proef_proeven.Components.LoadData;
using proef_proeven.Screens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace proef_proeven.Components.LevelCreator
{
    class BoundingboxEditorLayer : BaseLayer
    {
        Texture2D overlay;
        List<Tuple<string, Texture2D>> list;
        Dictionary<Texture2D, Texture2D> backgrounds;

        public BoundingboxEditorLayer(List<Tuple<string, Texture2D>> objects)
        {

        }

        public override void LoadContent(ContentManager content)
        {
            overlay = new Texture2D(Game1.Instance.GraphicsDevice, 1, 1);
            overlay.SetData<Color>(new Color[1] { Color.FromNonPremultiplied(0, 0, 0, 150) });

            // Create backgrounds
            foreach()

            base.LoadContent(content);
        }

        public override void LoadLevel(LevelFormat level)
        {
            base.LoadLevel(level);
        }

        public override void Update(GameTime time)
        {
            base.Update(time);
        }

        public override void Draw(SpriteBatch batch)
        {

            base.Draw(batch);
        }
    }
}
