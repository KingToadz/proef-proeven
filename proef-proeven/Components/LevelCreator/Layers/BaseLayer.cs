using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using proef_proeven.Components.LoadData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace proef_proeven.Components.LevelCreator
{
    abstract class BaseLayer
    {
        protected bool blockLayerChange;
        public bool BlockLayerChange
        {
            get
            {
                return blockLayerChange;
            }
        }

        private bool activeLayer;
        public bool ActiveLayer
        {
            get { return activeLayer;  }
        }

        protected string layerInfo = "Layer info needs to be updated!";
        public string LayerInfo
        {
            get { return layerInfo; }
        }

        public abstract List<object> getObjects();

        public virtual void ChangeActive(bool active)
        {
            activeLayer = active;
        }

        public virtual void LoadContent(ContentManager content)
        {

        }

        public virtual void LoadLevel(LevelFormat level)
        {

        }

        public virtual void Update(GameTime time)
        {

        }

        public virtual void Draw(SpriteBatch batch)
        {

        }
    }
}
