using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TData.TemplateData
{
    [Serializable]
    public class SpaceshipTemplate : EntityTemplate
    {
        public float Fuel;
        public float Energy;
        public float ShieldCapacity;
        public float ScavengePower;
        public float ScavengeRange;
        public float ShieldReplenishCostPerSecond;
        public float ShieldReplenishValuePerSecond;
        public float EnergyRecoveryPerSecond;
        public float ShieldRecoveryPerSecond;
        public float FuelRecoveryPerSecond;
        public int ModuleCount;
        public EngineData[] Engines;
        public WeaponBaseData PrimaryWeaponBases;
        public WeaponBaseData SecondaryWeaponBases;
        public float RadarRange;
        public int Price;
    }
}
