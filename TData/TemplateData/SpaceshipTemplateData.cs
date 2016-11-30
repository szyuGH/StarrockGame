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
        public float Fuel;
        public float Energy;
        public float ShieldCapacity;
        public float ScavengePower;
        public float ShieldReplenishCostPerSecond;
        public float EnergyRecoveryPerSecond;
        public float ShieldRecoveryPerSecond;
        public float FuelRecoveryperSecond;
        public int ModuleCount;
        public String PrimaryWeapon;
        public String SecondaryWeapon;
        public EngineData[] Engines;

    }
}
