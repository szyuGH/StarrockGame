using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StarrockGame.Entities
{
    public enum ModuleEffectType : int
    {
        None=-1,
        EnergyRecoveryPerSecond=0,
        ShieldRecoveryPerSecond=1,
        FuelRecoveryPerSecond=2,
        Damage=3,
        ScavengePower=4,
        ScavengeRange=5,
        RadarRange=6,
        StructureCapacity=7,
        EnergyCapacity=8,
        ShieldCapacity=9,
        FuelCapacity=10
    }
}
