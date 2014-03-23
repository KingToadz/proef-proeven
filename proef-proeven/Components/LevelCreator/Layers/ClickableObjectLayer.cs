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

namespace proef_proeven.Components.LevelCreator
{
    class ClickableObjectLayer : BaseLayer 
    {
        List<ClickableObject> clickables;
        List<Tuple<string, Texture2D>> textures;
        int clickablesCount;

        int curObject;

        Vector2 startPos;
        bool placing;

        public ClickableObjectLayer()
        {
            clickables = new List<ClickableObject>();
            clickablesCount = 0;
            curObject = 0;
            placing = false;
        }

        public override List<object> getObjects()
        {
            return clickables.ToList<object>();
        }

        public override void LoadContent(ContentManager content)
        {
            List<string> files = IOHelper.Instance.FilesInDirectory(@"\Content\level-editor\moveable", "*.png");
            clickables = new List<ClickableObject>();

            textures = new List<Tuple<string, Texture2D>>();

            for (int i = 0; i < files.Count; i++)
            {
                files[i] = files[i].Remove(files[i].LastIndexOf('.'), files[i].Length - files[i].LastIndexOf('.'));

                if (files[i].Contains(@"\Content\"))
                    files[i] = files[i].Remove(0, @"\Content\".Length);

                textures.Add(new Tuple<string, Texture2D>(files[i], content.Load<Texture2D>(files[i])));
            }
            base.LoadContent(content);
        }

        public override void LoadLevel(LevelFormat level)
        {
            foreach (ClickAbleInfo info in level.clickObjectsInfo)
            {
                ClickableObject clickObj = new ClickableObject();

                bool found = false;
                foreach(Tuple<string,Texture2D> tup in textures)
                {
                    if(tup.Item1 == info.texturePath)
                    {
                        found = true;
                        clickObj.Image = tup.Item2;
                        clickObj.TexturePath = tup.Item1;
                    }
                }

                // Texture not found just drop it
                if (!found)
                    continue;

                if (info.useCustomBounds)
                {
                    clickObj.SetCustomBounds(new Rectangle(info.X, info.Y, info.Width, info.Height));
                }

                clickObj.StartPosition = info.position;
                clickObj.Position = info.position;
                clickObj.moveToPosition = info.moveToPosition;
                clickObj.ObjectiveID = info.objectiveID;
                
                clickables.Add(clickObj);
                clickablesCount++;
            }

            base.LoadLevel(level);
        }

        public override void Update(Microsoft.Xna.Framework.GameTime time)
        {
            if (InputHelper.Instance.IsKeyPressed(Keys.A) && curObject > 0 && !placing)
            { curObject--; }
            else if (InputHelper.Instance.IsKeyPressed(Keys.D) && curObject < textures.Count - 1 && !placing)
            { curObject++; }

            if (InputHelper.Instance.IsLeftMouseReleased())
            {
                if (placing)
                {
                    ClickableObject obj = new ClickableObject();
                    obj.Image = textures[curObject].Item2;
                    obj.StartPosition = obj.Position = startPos;
                    obj.TexturePath = textures[curObject].Item1;
                    obj.moveToPosition = InputHelper.Instance.MousePos();
                    obj.ObjectiveID = clickablesCount;
                    clickables.Add(obj);
                    clickablesCount++;
                    placing = false;
                }
                else
                {
                    placing = true;
                    startPos = InputHelper.Instance.MousePos();
                }
            }
            else if (InputHelper.Instance.IsRightMousePressed() && !placing)
            {
                bool reset = false;

                for (int i = 0; i < clickables.Count; i++)
                {
                    ClickableObject o = clickables[i];

                    if (o.Boundingbox.Contains(InputHelper.Instance.MousePos().toPoint()))
                    {
                        clickables.Remove(o);
                        clickablesCount--;
                        reset = true;
                    }
                }

                if (reset)
                    ResetIDs();
            }

            if (placing)
                blockLayerChange = true;
            else
                blockLayerChange = false;

            base.Update(time);
        }

        private void ResetIDs()
        {
            clickablesCount = 0;
            for (int i = 0; i < clickables.Count; i++)
            {
                clickables[i].ObjectiveID = i;
                clickablesCount++;
            }
        }

        public override void Draw(SpriteBatch batch)
        {
            foreach(ClickableObject click in clickables)
            {
                click.Draw(batch);
                Game1.Instance.fontRenderer.DrawText(batch, click.StartPosition + new Vector2(5, 5), click.ObjectiveID.ToString());
                batch.Draw(click.Image, click.moveToPosition, Color.FromNonPremultiplied(255, 255, 255, 100));
                Game1.Instance.fontRenderer.DrawText(batch, click.moveToPosition + new Vector2(5, 5), click.ObjectiveID.ToString());
            }

            if (ActiveLayer)
            {
                batch.Draw(textures[curObject].Item2, InputHelper.Instance.MousePos(), placing ? Color.FromNonPremultiplied(255, 255, 255, 100) : Color.White);

                if (placing)
                    batch.Draw(textures[curObject].Item2, startPos, Color.White);
            }

            base.Draw(batch);
        }
    }
}
