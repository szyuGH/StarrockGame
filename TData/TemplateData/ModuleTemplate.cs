using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TData.TemplateData
{
    [Serializable]
    public class ModuleTemplate : AbstractTemplate
    {
        public ModuleEffectData[] ModuleEffects;
        public int Price;
    }
}
