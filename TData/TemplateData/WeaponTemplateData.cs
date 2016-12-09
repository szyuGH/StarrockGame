using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TData.TemplateData
{
    [Serializable]
    public class WeaponTemplateData : EntityTemplateData
    {
        public int WeaponType;
        public float EnergyCost;
        public float Damage;
        public float Cooldown;
    }
}
