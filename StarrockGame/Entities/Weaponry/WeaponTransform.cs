using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StarrockGame.Entities.Weaponry
{
    public struct WeaponTransform
    {
        public Vector2 Position;
        public float Angle;
        public Vector2 Direction;

        public WeaponTransform(Vector2 pos, float angle, Vector2 dir)
        {
            Position = pos;
            Angle = angle;
            Direction = dir;
        }
    }
}
