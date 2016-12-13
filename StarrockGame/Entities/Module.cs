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
            // TODO: add other types
            foreach (ModuleEffectData effect in Template.ModuleEffects)
            {
                switch ((ModuleEffectType)effect.Buff)
                {
                    case ModuleEffectType.StructureCapacity:
                        (Ship.Template as SpaceshipTemplate).Structure *= effect.Buff;
                        break;
                }
                switch ((ModuleEffectType)effect.Debuff)
                {
                    case ModuleEffectType.StructureCapacity:
                        (Ship.Template as SpaceshipTemplate).Structure *= effect.Debuff;
                        break;
                }
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
