using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace proef_proeven.Components.Fonts
{
    public class FontRenderer
    {
        private Dictionary<char, FontChar> _characterMap;
        private FontFile _fontFile;
        private Texture2D _texture;

        public FontRenderer(FontFile fontFile, Texture2D fontTexture)
        {
            _fontFile = fontFile;
            _texture = fontTexture;
            _characterMap = new Dictionary<char, FontChar>();

            foreach (var fontCharacter in _fontFile.Chars)
            {
                char c = (char)fontCharacter.ID;
                _characterMap.Add(c, fontCharacter);
            }
        }

        /// <summary>
        /// Draws the Text
        /// </summary>
        /// <param name="spriteBatch">The spritebatch to draw to</param>
        /// <param name="pos">The start position</param>
        /// <param name="text">The text to draw</param>
        public void DrawText(SpriteBatch spriteBatch, Vector2 pos, string text)
        {
            this.DrawText(spriteBatch, (int)pos.X, (int)pos.Y, text, Color.White);
        }

        /// <summary>
        /// Draws the Text
        /// </summary>
        /// <param name="spriteBatch">The spritebatch to draw to</param>
        /// <param name="pos">The start position</param>
        /// <param name="text">The text to draw</param>
        /// <param name="color">The color the text should have</param>
        public void DrawText(SpriteBatch spriteBatch, Vector2 pos, string text, Color color)
        {
            this.DrawText(spriteBatch, (int)pos.X, (int)pos.Y, text, color);
        }

        /// <summary>
        /// Draws the Text
        /// </summary>
        /// <param name="spriteBatch">The spritebatch to draw to</param>
        /// <param name="x">The x start position</param>
        /// <param name="y">The y start position</param>
        /// <param name="text">The text to draw</param>
        public void DrawText(SpriteBatch spriteBatch, int x, int y, string text)
        {
            this.DrawText(spriteBatch, x, y, text, Color.White);
        }

        /// <summary>
        /// Draws the Text
        /// </summary>
        /// <param name="spriteBatch">The spritebatch to draw to</param>
        /// <param name="x">The x start position</param>
        /// <param name="y">The y start position</param>
        /// <param name="text">The text to draw</param>
        /// <param name="color">The color the text should have</param>
        public void DrawText(SpriteBatch spriteBatch, int x, int y, string text, Color color)
        {
            int dx = x;
            int dy = y;
            foreach (char c in text)
            {
                FontChar fc;
                if (_characterMap.TryGetValue(c, out fc))
                {
                    var sourceRectangle = new Rectangle(fc.X, fc.Y, fc.Width, fc.Height);
                    var position = new Vector2(dx + fc.XOffset, dy + fc.YOffset);

                    spriteBatch.Draw(_texture, position, sourceRectangle, color);
                    dx += fc.XAdvance;
                }
            }
        }

        /// <summary>
        /// Only returns the width and height of the first line
        /// Still need to implement new lines
        /// </summary>
        /// <param name="text">The text to messure</param>
        /// <returns>The bounds of the string</returns>
        public Rectangle StringSize(string text)
        {
            Rectangle ret = new Rectangle();

            foreach (char c in text)
            {
                FontChar fc;
                if (_characterMap.TryGetValue(c, out fc))
                {
                    ret.Width += fc.XAdvance;

                    if (ret.Height < fc.Height)
                        ret.Height = fc.Height;
                }
            }

            return ret;
        }
    }
}
