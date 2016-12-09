using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TData.TemplateData
{
    [Serializable]
    public struct WeaponBaseData
    {
        public string WeaponType;
        public LocalBase[] Bases;

        [Serializable]
        public struct LocalBase
        {
            public Vector2 LocalPosition;
            public float LocalAngle;
        }
    }
}
