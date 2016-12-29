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


        public static Vector2 ExtractVector2(this Vector4 v4, int index)
        {
            switch (index)
            {
                case 0: return new Vector2(v4.X, v4.Y);
                case 1: return new Vector2(v4.Z, v4.W);
                default: throw new Exception("Failed to retrieve Vector2 for index " + index);
            }
        }

        public static T Remove<T>(this Stack<T> stack, T element)
        {
            T obj = stack.Pop();
            if (obj.Equals(element))
            {
                return obj;
            }
            else
            {
                T toReturn = stack.Remove(element);
                stack.Push(obj);
                return toReturn;
            }
        }

        public static float NextFloat(this Random rand, float min, float max)
        {
            return (float)rand.NextDouble() * (max - min) + min;
        }
    }
}
