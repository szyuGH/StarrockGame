using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StarrockGame
{
    public static class Extensions
    {
        public static Vector2 ToVector2(this float val)
        {
            return new Vector2((float)Math.Cos(val), (float)Math.Sin(val));
        }
    }
}
