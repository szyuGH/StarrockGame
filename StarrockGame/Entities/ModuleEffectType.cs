using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StarrockGame.Entities
{
    public enum ModuleEffectType : int
    {
        EnergyRecoveryPerSecond,
        ShieldRecoveryPerSecond,
        FuelRecoveryPerSecond,
        Damage,
        ScavengePower,
        ScavengeRange,
        RadarRange,
        StructureCapacity,
        EnergyCapacity,
        ShieldCapacity,
        FuelCapacity
    }
}
