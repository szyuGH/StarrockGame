using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TData.TemplateData
{
    [Serializable]
    public class WreckageTemplate : EntityTemplate
    {
        public float MinFuel;
        public float MaxFuel;
        public float MinEnergy;
        public float MaxEnergy;
        public float MinStructure;
        public float MaxStructure;
        //public Blueprint blueprint;
    }
}