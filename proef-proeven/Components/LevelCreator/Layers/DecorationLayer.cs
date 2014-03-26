using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using proef_proeven.Components.Game;
using proef_proeven.Components.LoadData;
using proef_proeven.Components.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace proef_proeven.Components.LevelCreator.Layers
{
    class DecorationLayer : BaseLayer
    {
        List<Decoration> tiles;

        List<Tuple<string, Texture2D>> decorationImages;
        int currentTile;
        
        public DecorationLayer()
        {
            tiles = new List<Decoration>();
            decorationImages = new List<Tuple<string, Texture2D>>();
            currentTile = 0;

            layerInfo = "Place decoration objects. They won't have any collision";
        }

        public override void LoadContent(ContentManager content)
        {
            List<string> backgroundPath = IOHelper.Instance.FilesInDirectory(@"\Content\level-editor\decoration", "*.png");

            decorationImages = new List<Tuple<string, Texture2D>>();

            for (int i = 0; i < backgroundPath.Count; i++)
            {
                backgroundPath[i] = backgroundPath[i].Remove(backgroundPath[i].LastIndexOf('.'), backgroundPath[i].Length - backgroundPath[i].LastIndexOf('.'));

                if (backgroundPath[i].Contains(@"\Content\"))
                    backgroundPath[i] = backgroundPath[i].Remove(0, @"\Content\".Length);

                decorationImages.Add(new Tuple<string, Texture2D>(backgroundPath[i], content.Load<Texture2D>(backgroundPath[i])));
            }

            base.LoadContent(content);
        }

        public override void LoadLevel(LevelFormat level)
        {
            foreach(DecorationInfo d in level.decoration)
            {
                Decoration decoration = new Decoration(d.ImagePath);
                decoration.Position = d.position;

                for (int i = 0; i < decorationImages.Count; i++)
                {
                    if (decorationImages[i].Item1 == d.ImagePath)
                    {
                        decoration.Image = decorationImages[i].Item2;
                        break;
                    }
                }
                
                tiles.Add(decoration);
            }

            base.LoadLevel(level);
        }

        public override void Update(GameTime time)
        {
            if (InputHelper.Instance.IsKeyPressed(Keys.A) && currentTile > 0)
                currentTile--;
            else if (InputHelper.Instance.IsKeyPressed(Keys.D) && currentTile < tiles.Count - 1)
                currentTile++;

            if(InputHelper.Instance.IsLeftMousePressed())
            {
                Decoration deco = new Decoration(decorationImages[currentTile].Item1);
                deco.Image = decorationImages[currentTile].Item2;
                deco.Position = InputHelper.Instance.MousePos();
                tiles.Add(deco);
            }
            else if(InputHelper.Instance.IsRightMousePressed())
            {
                for(int i = 0; i < tiles.Count; i++)
                {
                    if(tiles[i].Bounds.Contains(InputHelper.Instance.MousePos().toPoint()))
                    {
                        tiles.RemoveAt(i);
                    }
                }
            }
            

            base.Update(time);
        }

        public override void Draw(SpriteBatch batch)
        {
            if(ActiveLayer)
            {
                batch.Draw(decorationImages[currentTile].Item2, InputHelper.Instance.MousePos(), Color.FromNonPremultiplied(255, 255, 255, 100));
            }

            foreach(Decoration d in tiles)
            {
                d.Draw(batch);
            }

            base.Draw(batch);
        }

        public override List<object> getObjects()
        {
            return tiles.ToList<object>();
        }
    }
}
