using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace proef_proeven.Components.Util
{
    static class ExtensionMethods
    {
        public static Vector2 toVector2(this Point p)
        {
            return new Vector2(p.X, p.Y);
        }

        public static Point toPoint(this Vector2 p)
        {
            return new Point((int)p.X, (int)p.Y);
        }
    }
}
