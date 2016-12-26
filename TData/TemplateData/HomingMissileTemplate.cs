using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TData.TemplateData
{
    [Serializable]
    public class HomingMissileTemplate : WeaponTemplate
    {
        public float ExplosionRange;
        public float Fuel;
        public EngineData[] Engines;
    }
}
