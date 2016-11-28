using FarseerPhysics.Dynamics;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StarrockGame.Templating;
using StarrockGame.Caching;
using Microsoft.Xna.Framework;

namespace StarrockGame.Entities
{
    class Spaceship : Entity
    {
        public float ShieldCapacity { get; private set; }
        public float Energy { get; private set; }
        public float Fuel { get; private set; }

        //public Module[] Modules { get; private set; }
        //public Weapon[] Weapons { get; private set; }
        //public Dictionary<MovementType, Engine> Engines { get; private set; } // will be used to emit particles based on the moving direction
            // @Dom: Engines hold data like propulsion power and one of 4 directions like stated in the enum MovementType
            //      They also hold the 2 colors of the emitting particles (min color and max color, between which will be lerped)

        public Spaceship(World world, string type)
            :base(world, type)
        {

        }

        public override void Initialize<T>(Vector2 position, float rotation, Vector2 initialVelocity, float initialAngularVelocity = 0)
        {
            base.Initialize<T>(position, rotation, initialVelocity, initialAngularVelocity);
            //ShieldCapacity = Template.ShieldCapacity;
            //Energy = Template.Energy;
            //Fuel = Template.Fuel;

            //Modules = new Module[Template.ModuleCount]; // only array initialization, because the actual modules come from the preperation
            //Weapons = Weapon.FromTemplate(Template.Weapons);
            //Engines = Engine.FromTemplate(Template.Engines);
        }


        public override void Update(float elapsed)
        {
            base.Update(elapsed);

            //Update Modules, Weapons and Engines
        }

        public override void Accelerate(float val)
        {
            //base.Accelerate(val * Engines.Where(e => e.MovementType == MovementType.Forward).Sum(e => e.PropulsionPower));
        }
    }
}
