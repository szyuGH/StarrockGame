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
using FarseerPhysics;
using StarrockGame.GUI;
using StarrockGame.Entities.Weaponry;

namespace StarrockGame.Entities
{
    public class Spaceship : Entity
    {
        private SpaceshipTemplateData shipTemplate { get { return Template as SpaceshipTemplateData; } }
        

        private float _shieldCapacity;
        public float ShieldCapacity
        {
            get { return _shieldCapacity; }
            set { _shieldCapacity = MathHelper.Clamp(value, 0, (Template as SpaceshipTemplateData).ShieldCapacity); }
        }

        private float _energy;
        public float Energy
        {
            get { return _energy; }
            set { _energy = MathHelper.Clamp(value, 0, (Template as SpaceshipTemplateData).Energy); }
        }

        private float _fuel;
        public float Fuel
        {
            get { return _fuel; }
            set { _fuel = MathHelper.Clamp(value, 0, (Template as SpaceshipTemplateData).Fuel); }
        }

        public float RadarRange { get; private set; }

        public bool IsPlayer
        {
            get { return (this.Controller.GetType() == typeof(PlayerController)); }
        }

        public Scavenging Scavenging;
        public bool ReplenishingShield;

        //public Module[] Modules { get; private set; }
        public WeaponBase[] PrimaryWeapons { get; private set; }
        public WeaponBase[] SecondaryWeapons { get; private set; }
        public Dictionary<MovementType, List<Engine>> Engines { get; private set; } // will be used to emit particles based on the moving direction

        public Spaceship(World world, string type)
            :base(world, type)
        {
            Scavenging = new Scavenging(1f, OnScavengeSuccess); // TODO: add scavenge range to ship
        }

        protected override EntityTemplateData LoadTemplate(string type)
        {
            return Cache.LoadTemplate<SpaceshipTemplateData>(type);
        }

        public override void Initialize<T>(Vector2 position, float rotation, Vector2 initialVelocity, float initialAngularVelocity = 0)
        {
            base.Initialize<T>(position, rotation, initialVelocity, initialAngularVelocity);
            ShieldCapacity = shipTemplate.ShieldCapacity;
            Energy = shipTemplate.Energy;
            Fuel = shipTemplate.Fuel;
            RadarRange = shipTemplate.RadarRange;
            Scavenging.Reset();
            ShieldCapacity = 10;

            //Modules = new Module[Template.ModuleCount]; // only array initialization, because the actual modules come from the preperation
            PrimaryWeapons = WeaponBase.FromTemplate(Body, shipTemplate.PrimaryWeaponBases);
            SecondaryWeapons = WeaponBase.FromTemplate(Body, shipTemplate.SecondaryWeaponBases);

            Engines = Engine.FromTemplate(Body, shipTemplate.Engines);
        }


        public override void Update(GameTime gameTime)
        {
            float elapsed = (float)gameTime.ElapsedGameTime.TotalSeconds;
            base.Update(gameTime);
            
            

            foreach (Engine engine in Engines.Values.SelectMany(e => e).ToList())
            {
                engine.Update(gameTime);
            }
            foreach (WeaponBase pwb in PrimaryWeapons) pwb.Update(gameTime);
            foreach (WeaponBase swb in SecondaryWeapons) swb.Update(gameTime);
            //Update Modules, Weapons and Engines

            Scavenging.Update(this, gameTime);
            UpdatePerSecond(elapsed);
            if (ReplenishingShield && ShieldCapacity < shipTemplate.ShieldCapacity)
            {
                float cost = shipTemplate.ShieldReplenishCostPerSecond * elapsed;
                if (Energy >= cost)
                {
                    Energy -= cost;
                    ShieldCapacity += shipTemplate.ShieldReplenishValuePerSecond * elapsed;
                }
            }
        }

        private void UpdatePerSecond(float elapsed)
        {
            Energy += shipTemplate.EnergyRecoveryPerSecond * elapsed;
            ShieldCapacity += shipTemplate.ShieldRecoveryPerSecond * elapsed;
            Fuel += shipTemplate.FuelRecoveryperSecond * elapsed;
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
        
        public void Scavenge(bool active)
        {
            if (active)
            {
                if (!Scavenging.Active)
                {
                    List<Entity> wreckages = EntityManager.GetAllEntities(this, Scavenging.Range).Where(e => e is Wreckage).ToList();
                    if (wreckages.Count > 1)
                    {
                        wreckages.Sort((e1, e2) => (int)Vector2.DistanceSquared(e1.Body.Position, e2.Body.Position));
                        Scavenging.Target = wreckages.First() as Wreckage;
                    }
                    else
                    {
                        Scavenging.Target = wreckages.FirstOrDefault() as Wreckage;
                    }
                }
            } 
            else if (!active && Scavenging.Active)
            {
                Scavenging.Reset();
            }
        }

        public void FirePrimary()
        {
            foreach (WeaponBase pwb in PrimaryWeapons)
            {
                pwb.Fire();
            }
        }

        public void FireSecondary()
        {
            foreach (WeaponBase swb in SecondaryWeapons)
            {
                swb.Fire();
            }
        }

        private void OnScavengeSuccess()
        {
            Energy += Scavenging.Target.GainEnergy;
            Fuel += Scavenging.Target.GainFuel;
            Structure += Scavenging.Target.GainStructure;
            Scavenging.Target.Destroy();
            Scavenging.Reset();
        }
    }
}
