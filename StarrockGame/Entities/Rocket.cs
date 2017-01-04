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
    public class Rocket : WeaponEntity
    {
        public Engine Engine { get; private set; } // will be used to emit particles based on the moving direction
        private float _fuel;
        public float Fuel
        {
            get { return _fuel; }
            set { _fuel = MathHelper.Clamp(value, 0, (Template as RocketTemplate).Fuel); }
        }

        public float ExplosionRange;

        public Rocket(World world, string type) 
            : base(world, type)
        {
            Body.IsBullet = true;
        }

        public override void Initialize<T>(Vector2 position, float rotation, Vector2 initialVelocity, float initialAngularVelocity = 0)
        {
            base.Initialize<T>(position, rotation, initialVelocity, initialAngularVelocity);
            RocketTemplate tmp = (Template as RocketTemplate);

            Fuel = tmp.Fuel;
            ExplosionRange = tmp.ExplosionRange;


            Engine = new Engine(Body, tmp.Engine.LocalPosition, (MovementType)tmp.Engine.Direction, tmp.Engine.PropulsionPower, tmp.Engine.FuelCostPerSecond, tmp.Engine.ParticlesPerSecond, tmp.Engine.ParticleSize);
        }

        protected override EntityTemplate LoadTemplate(string type)
        {
            return Cache.LoadTemplate<RocketTemplate>(type);
        }

        public override void Update(GameTime gameTime)
        {
            if (!IsAlive)
                return;
            base.Update(gameTime);
            Accelerate(1, (float)gameTime.ElapsedGameTime.TotalSeconds);
            Engine.Update(gameTime);
        }


        public override void Accelerate(float val, float elapsed)
        {
            if (Fuel >= Engine.FuelPerSeconds * elapsed)
            {
                base.Accelerate(val * Engine.PropulsionPower, elapsed);
                if (val != 0)
                {
                    Engine.Emitting = true;
                    Fuel -= Engine.FuelPerSeconds * elapsed;
                }
            }
        }


        public override void Destroy(bool ignoreScore = false)
        {
            base.Destroy(ignoreScore);
            Engine.Deinit();
            Sound.Instance.PlaySe("Explosion1", 1 - MathHelper.Clamp(Vector2.Distance(EntityManager.PlayerShip.Body.Position, Body.Position) / SoundEmitter.MAX_RANGE, 0, 1));
        }
    }
}
