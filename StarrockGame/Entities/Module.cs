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

        private static Dictionary<ModuleEffectType, float> effectCollector = new Dictionary<ModuleEffectType, float>();

        public Module(Spaceship ship, ModuleTemplate template)
        {
            Ship = ship;
            Template = template;
            //ApplyModule();
        }

        //private void ApplyModule()
        //{
        //    foreach (ModuleEffectData effect in Template.ModuleEffects)
        //    {
        //        ApplyEffect((ModuleEffectType)effect.EffectType, effect.Value);
        //    }
        //}

        private static void ApplyEffect(Spaceship ship, ModuleEffectType type, float amp)
        {
            switch (type)
            {
                case ModuleEffectType.StructureCapacity:
                    (ship.Template as SpaceshipTemplate).Structure *= amp;
                    break;
                case ModuleEffectType.FuelCapacity:
                    (ship.Template as SpaceshipTemplate).Fuel *= amp;
                    break;
                case ModuleEffectType.EnergyCapacity:
                    (ship.Template as SpaceshipTemplate).Energy *= amp;
                    break;
                case ModuleEffectType.ShieldCapacity:
                    (ship.Template as SpaceshipTemplate).ShieldCapacity *= amp;
                    break;
                case ModuleEffectType.RadarRange:
                    (ship.Template as SpaceshipTemplate).RadarRange *= amp;
                    break;
                case ModuleEffectType.ScavengePower:
                    (ship.Template as SpaceshipTemplate).ScavengePower *= amp;
                    break;
                case ModuleEffectType.ScavengeRange:
                    (ship.Template as SpaceshipTemplate).ScavengeRange *= amp;
                    break;
                case ModuleEffectType.EnergyRecoveryPerSecond:
                    if ((ship.Template as SpaceshipTemplate).EnergyRecoveryPerSecond == 0)
                        (ship.Template as SpaceshipTemplate).EnergyRecoveryPerSecond = 0.1f;
                    (ship.Template as SpaceshipTemplate).EnergyRecoveryPerSecond *= amp;
                    break;
                case ModuleEffectType.FuelRecoveryPerSecond:
                    if ((ship.Template as SpaceshipTemplate).FuelRecoveryPerSecond == 0)
                        (ship.Template as SpaceshipTemplate).FuelRecoveryPerSecond = 0.1f;
                    (ship.Template as SpaceshipTemplate).FuelRecoveryPerSecond *= amp;
                    break;
                case ModuleEffectType.ShieldRecoveryPerSecond:
                    if ((ship.Template as SpaceshipTemplate).ShieldRecoveryPerSecond == 0)
                        (ship.Template as SpaceshipTemplate).ShieldRecoveryPerSecond = 0.1f;
                    (ship.Template as SpaceshipTemplate).ShieldRecoveryPerSecond *= amp;
                    break;
                case ModuleEffectType.Damage:
                    ship.DamageAmplifier *= amp;
                    break;
            }
        }

        public static Module[] FromTemplate(Spaceship ship, ModuleTemplate[] templates)
        {
            Module[] result = new Module[templates.Length];
            for (int i = 0; i < result.Length; i++)
            {
                // collect all amplifier first, so multiple same effects won't stack
                foreach (ModuleEffectData data in templates[i].ModuleEffects)
                {
                    if (!effectCollector.ContainsKey((ModuleEffectType)data.EffectType))
                    {
                        effectCollector[(ModuleEffectType)data.EffectType] = 0;
                    }
                    effectCollector[(ModuleEffectType)data.EffectType] += data.Value;
                }

                Module mod = new Module(ship, templates[i]);
                result[i] = mod;
            }

            // apply collected effects
            foreach (KeyValuePair<ModuleEffectType, float> kvp in effectCollector)
            {
                ApplyEffect(ship, kvp.Key, kvp.Value);
            }

            return result;
        }


    }
}
