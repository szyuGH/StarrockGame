using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TData.TemplateData
{
    [Serializable]
    public class ModuleTemplateData
    {
        public String Name;
        public ModuleEffectData[] ModuleEffect;
        public float Price;
    }
}
