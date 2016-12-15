using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TData.TemplateData;

namespace StarrockGame.Entities
{
    public class Module
    {
        public Spaceship Ship { get; private set; }
        public ModuleTemplate Template { get; private set; }

        public Module(Spaceship ship, ModuleTemplate template)
        {
            Ship = ship;
            Template = template;
            ApplyModule();
        }

        private void ApplyModule()
        {
            foreach (ModuleEffectData effect in Template.ModuleEffects)
            {
                ApplyEffect((ModuleEffectType)effect.EffectType, effect.Value);
            }
        }

        private void ApplyEffect(ModuleEffectType type, float amp)
        {
            switch (type)
            {
                case ModuleEffectType.StructureCapacity:
                    (Ship.Template as SpaceshipTemplate).Structure *= amp;
                    break;
                case ModuleEffectType.FuelCapacity:
                    (Ship.Template as SpaceshipTemplate).Fuel *= amp;
                    break;
                case ModuleEffectType.EnergyCapacity:
                    (Ship.Template as SpaceshipTemplate).Energy *= amp;
                    break;
                case ModuleEffectType.ShieldCapacity:
                    (Ship.Template as SpaceshipTemplate).ShieldCapacity *= amp;
                    break;
                case ModuleEffectType.RadarRange:
                    (Ship.Template as SpaceshipTemplate).RadarRange *= amp;
                    break;
                case ModuleEffectType.ScavengePower:
                    (Ship.Template as SpaceshipTemplate).ScavengePower *= amp;
                    break;
                case ModuleEffectType.ScavengeRange:
                    (Ship.Template as SpaceshipTemplate).ScavengeRange *= amp;
                    break;
                case ModuleEffectType.EnergyRecoveryPerSecond:
                    (Ship.Template as SpaceshipTemplate).EnergyRecoveryPerSecond *= amp;
                    break;
                case ModuleEffectType.FuelRecoveryPerSecond:
                    (Ship.Template as SpaceshipTemplate).FuelRecoveryPerSecond *= amp;
                    break;
                case ModuleEffectType.ShieldRecoveryPerSecond:
                    (Ship.Template as SpaceshipTemplate).ShieldRecoveryPerSecond *= amp;
                    break;
                case ModuleEffectType.Damage:
                    Ship.DamageAmplifier *= amp;
                    break;
            }
        }

        public static Module[] FromTemplate(Spaceship ship,ModuleTemplate[] templates)
        {
            Module[] result = new Module[templates.Length];
            for (int i = 0; i < result.Length; i++)
            {
                Module mod = new Module(ship, templates[i]);
                result[i] = mod;
            }

            return result;
        }


    }
}
