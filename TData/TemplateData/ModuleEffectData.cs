using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TData.TemplateData
{
    [Serializable]
    public struct ModuleEffectData
    {
        static readonly string[] VTS = {
            "ERpS", "SRpS", "FRpS", "Damage",
            "Scavenge Power", "Scavenge Range", "Radar Range",
            "Structure", "Energy Capacity", "Shield Capacity", "Fuel Capacity"
    };

        public int EffectType;
        public float Value;

        public override string ToString()
        {
            
            string eff;
            if (EffectType >= 0 && EffectType <= 10)
                eff = VTS[EffectType];
            else
                eff = "";

            return string.Format("{0} x{1:0.##}", eff, Value);
        }
    }
}
