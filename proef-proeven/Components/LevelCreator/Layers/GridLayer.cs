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
    class GridLayer : BaseLayer
    {
        struct GridSize
        {
            public int Width = 32;
            public int Height = 32;
        }

        GridSize gridSize;
        Texture2D tiles;
        List<Rectangle> clipRects;

        public override void LoadContent(ContentManager content)
        {
            if (IOHelper.Instance.DoesDirectoryExists(Constants.LEVEL_CREATOR_DIR + @"gird\tiles.json"))
            {
                gridSize = Newtonsoft.Json.JsonConvert.DeserializeObject<GridSize>(Constants.LEVEL_CREATOR_DIR + @"gird\tiles.json");
                tiles = content.Load<Texture2D>(@"gird\tiles");
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

        public override List<object> getObjects()
        {
            throw new NotImplementedException();
        }
    }
}
