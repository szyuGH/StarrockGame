using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TData.TemplateData
{
    [Serializable]
    public class WreckageTemplateData : EntityTemplateData
    {
        public float minFuel;
        public float maxFuel;
        public float minEnergy;
        public float maxEnergy;
        //public Blueprint blueprint;
    }
}