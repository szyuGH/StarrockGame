using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TData.TemplateData
{
    [Serializable]
    public class ModuleTemplateData : TemplateData
    {
        public ModuleEffectData[] ModuleEffects;
        public float Price;
    }
}
