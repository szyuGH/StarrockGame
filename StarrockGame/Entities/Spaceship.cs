using FarseerPhysics.Dynamics;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StarrockGame.Caching;
using Microsoft.Xna.Framework;
using TData.TemplateData;
using StarrockGame.AI;

namespace StarrockGame.Entities
{
    public class Spaceship : Entity
    {
        private SpaceshipTemplateData ShipTemplate { get { return Template as SpaceshipTemplateData; } }

        public float ShieldCapacity { get; private set; }
        public float Energy { get; private set; }
        public float Fuel { get; private set; }

        //public Module[] Modules { get; private set; }
        //public Weapon[] Weapons { get; private set; }
        public Dictionary<MovementType, List<Engine>> Engines { get; private set; } // will be used to emit particles based on the moving direction

        public Spaceship(World world, string type)
            :base(world, type)
        {

        }

        protected override EntityTemplateData LoadTemplate(string type)
        {
            return Cache.LoadTemplate<SpaceshipTemplateData>(type);
        }

        public override void Initialize<T>(Vector2 position, float rotation, Vector2 initialVelocity, float initialAngularVelocity = 0)
        {
            base.Initialize<T>(position, rotation, initialVelocity, initialAngularVelocity);
            ShieldCapacity = ShipTemplate.ShieldCapacity;
            Energy = ShipTemplate.Energy;
            Fuel = ShipTemplate.Fuel;

            //Modules = new Module[Template.ModuleCount]; // only array initialization, because the actual modules come from the preperation
            //Weapons = Weapon.FromTemplate(ShipTemplate.Weapons);
            Engines = Engine.FromTemplate(Body, ShipTemplate.Engines);
        }


        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            foreach (Engine engine in Engines.Values.SelectMany(e => e).ToList())
            {
                engine.Update(gameTime);
            }
            //Update Modules, Weapons and Engines
        }

        public override void Accelerate(float val)
        {
            float prop = Engines[MovementType.Forward].Sum(e => e.PropulsionPower);
            base.Accelerate(val * prop);
            if (val != 0)
                foreach (Engine engine in Engines[MovementType.Forward])
                    engine.Emitting = true;
        }

        public override void Decelerate(float val)
        {
            float prop = Engines[MovementType.Brake].Sum(e => e.PropulsionPower);
            base.Decelerate(val * prop);
            if (val != 0)
                foreach (Engine engine in Engines[MovementType.Brake])
                    engine.Emitting = true;
        }

        public override void Rotate(float val)
        {
            float mul = 1;
            if (val > 0)
            {
                mul = Engines[MovementType.RotateRight].Sum(e => e.PropulsionPower);
                foreach (Engine engine in Engines[MovementType.RotateRight])
                    engine.Emitting = true;
            }
            else if (val < 0)
            {
                mul = Engines[MovementType.RotateLeft].Sum(e => e.PropulsionPower);
                foreach (Engine engine in Engines[MovementType.RotateLeft])
                    engine.Emitting = true;
            }
            base.Rotate(val * mul);

        }

        //Returns true if the ship is controlled by a player
        public bool IsPlayer()
        {
            return  (this.Controller.GetType()==typeof(PlayerController));
        }
    }
}
