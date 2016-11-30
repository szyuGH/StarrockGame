using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TData.TemplateData
{
    [Serializable]
    public struct EngineData
    {
        public Byte Direction;
        public Vector2 LocalPosition;
        public String ParticleColorMin;
        public String ParticleColorMax;
        public float PropulsionPower;
        public float FuelCostPerSecond;

    }
}
