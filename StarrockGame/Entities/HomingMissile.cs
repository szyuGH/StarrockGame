using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FarseerPhysics.Dynamics;
using TData.TemplateData;
using StarrockGame.Caching;
using Microsoft.Xna.Framework;
using StarrockGame.Audio;

namespace StarrockGame.Entities
{
    public class HomingMissile : WeaponEntity
    {
        public Dictionary<MovementType, List<Engine>> Engines { get; private set; } // will be used to emit particles based on the moving direction
        private float _fuel;
        public float Fuel
        {
            get { return _fuel; }
            set { _fuel = MathHelper.Clamp(value, 0, (Template as HomingMissileTemplate).Fuel); }
        }
        private Dictionary<MovementType, float> fuelCostPerSecond;

        public float ExplosionRange;

        public HomingMissile(World world, string type) 
            : base(world, type)
        {
            Body.IsBullet = true;
            fuelCostPerSecond = new Dictionary<MovementType, float>();
        }

        public override void Initialize(Vector2 position, float rotation, Vector2 initialVelocity, float initialAngularVelocity = 0)
        {
            base.Initialize(position, rotation, initialVelocity, initialAngularVelocity);
            Fuel = (Template as HomingMissileTemplate).Fuel;
            ExplosionRange = (Template as HomingMissileTemplate).ExplosionRange;

            Engines = Engine.FromTemplate(Body, (Template as HomingMissileTemplate).Engines);
            fuelCostPerSecond[MovementType.Forward] = Engines[MovementType.Forward].Sum(e => e.FuelPerSeconds);
            fuelCostPerSecond[MovementType.Brake] = Engines[MovementType.Brake].Sum(e => e.FuelPerSeconds);
            fuelCostPerSecond[MovementType.RotateLeft] = Engines[MovementType.RotateLeft].Sum(e => e.FuelPerSeconds);
            fuelCostPerSecond[MovementType.RotateRight] = Engines[MovementType.RotateRight].Sum(e => e.FuelPerSeconds);
        }

        protected override EntityTemplate LoadTemplate(string type)
        {
            return Cache.LoadTemplate<HomingMissileTemplate>(type);
        }

        public override void Update(GameTime gameTime)
        {
            if (!IsAlive)
                return;
            base.Update(gameTime);

            foreach (Engine engine in Engines.Values.SelectMany(e => e).ToList())
            {
                engine.Update(gameTime);
            }

            if (Fuel <= 0)
            {
                Destroy();
            }
        }

        public override void Destroy(bool ignoreScore = false)
        {
            base.Destroy(ignoreScore);
        }


        public override void Accelerate(float val, float elapsed)
        {
            if (Fuel >= fuelCostPerSecond[MovementType.Forward] * elapsed)
            {
                float prop = Engines[MovementType.Forward].Sum(e => e.PropulsionPower);
                base.Accelerate(val * prop, elapsed);
                if (val != 0)
                    foreach (Engine engine in Engines[MovementType.Forward])
                    {
                        engine.Emitting = true;
                        Fuel -= engine.FuelPerSeconds * elapsed;
                    }
            }
        }

        public override void Decelerate(float val, float elapsed)
        {
            if (Fuel >= fuelCostPerSecond[MovementType.Brake])
            {
                float prop = Engines[MovementType.Brake].Sum(e => e.PropulsionPower);
                base.Decelerate(val * prop, elapsed);
                if (val != 0)
                    foreach (Engine engine in Engines[MovementType.Brake])
                    {
                        engine.Emitting = true;
                        Fuel -= engine.FuelPerSeconds * elapsed;
                    }
            }
        }

        public override void Rotate(float val, float elapsed)
        {
            float mul = 0;
            if (val > 0 && Fuel >= fuelCostPerSecond[MovementType.RotateRight])
            {
                mul = Engines[MovementType.RotateRight].Sum(e => e.PropulsionPower);
                foreach (Engine engine in Engines[MovementType.RotateRight])
                {
                    engine.Emitting = true;
                    Fuel -= engine.FuelPerSeconds * elapsed;
                }

            }
            else if (val < 0 && Fuel >= fuelCostPerSecond[MovementType.RotateLeft])
            {
                mul = Engines[MovementType.RotateLeft].Sum(e => e.PropulsionPower);
                foreach (Engine engine in Engines[MovementType.RotateLeft])
                {
                    engine.Emitting = true;
                    Fuel -= engine.FuelPerSeconds * elapsed;
                }

            }
            base.Rotate(val * mul, elapsed);
        }
    }
}
