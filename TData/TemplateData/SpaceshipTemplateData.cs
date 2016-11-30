using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TData.TemplateData
{
    [Serializable]
    public class SpaceshipTemplateData : EntityTemplateData
    {
        public EngineData [] Engines;
        public float ShieldCapacity;
        public float Energy;
        public float Fuel;
    }
}
