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
using StarrockGame.Audio;

namespace StarrockGame.Entities
{
    public class Spaceship : Entity
    {
        public static Texture2D SpawnTexture;

        private SpaceshipTemplate shipTemplate { get { return Template as SpaceshipTemplate; } }
        private Dictionary<MovementType, float> fuelCostPerSecond;
        private float spawnTimer;
        private float spawnSize;

        private float _shieldCapacity;
        public float ShieldCapacity
        {
            get { return _shieldCapacity; }
            set { _shieldCapacity = MathHelper.Clamp(value, 0, (Template as SpaceshipTemplate).ShieldCapacity); }
        }

        private float _energy;
        public float Energy
        {
            get { return _energy; }
            set { _energy = MathHelper.Clamp(value, 0, (Template as SpaceshipTemplate).Energy); }
        }

        private float _fuel;
        public float Fuel
        {
            get { return _fuel; }
            set { _fuel = MathHelper.Clamp(value, 0, (Template as SpaceshipTemplate).Fuel); }
        }

        public float RadarRange { get; private set; }

        public bool IsPlayer
        {
            get { return ( Controller != null && Controller.GetType() == typeof(PlayerController)); }
        }

        public Scavenging Scavenging;
        public bool ReplenishingShield;

        public Module[] Modules { get; private set; }
        public WeaponBase[] PrimaryWeapons { get; private set; }
        public WeaponBase[] SecondaryWeapons { get; private set; }
        public Dictionary<MovementType, List<Engine>> Engines { get; private set; } // will be used to emit particles based on the moving direction

        public float DamageAmplifier { get; set; } = 1;

        public Spaceship(World world, string type)
            :base(world, type)
        {
            Scavenging = new Scavenging(this, ConvertUnits.ToSimUnits(shipTemplate.ScavengeRange), OnScavengeSuccess);
            fuelCostPerSecond = new Dictionary<MovementType, float>();
        }

        protected override EntityTemplate LoadTemplate(string type)
        {
            return Cache.LoadTemplate<SpaceshipTemplate>(type);
        }

        public override void Initialize(Vector2 position, float rotation, Vector2 initialVelocity, float initialAngularVelocity = 0)
        {
            base.Initialize(position, rotation, initialVelocity, initialAngularVelocity);
            ShieldCapacity = shipTemplate.ShieldCapacity;
            Energy = shipTemplate.Energy;
            Fuel = shipTemplate.Fuel;
            RadarRange = shipTemplate.RadarRange;
            Scavenging.Reset();
            
            PrimaryWeapons = WeaponBase.FromTemplate(this, shipTemplate.PrimaryWeaponBases);
            SecondaryWeapons = WeaponBase.FromTemplate(this, shipTemplate.SecondaryWeaponBases);

            Engines = Engine.FromTemplate(Body, shipTemplate.Engines);
            fuelCostPerSecond[MovementType.Forward] = Engines[MovementType.Forward].Sum(e => e.FuelPerSeconds);
            fuelCostPerSecond[MovementType.Brake] = Engines[MovementType.Brake].Sum(e => e.FuelPerSeconds);
            fuelCostPerSecond[MovementType.RotateLeft] = Engines[MovementType.RotateLeft].Sum(e => e.FuelPerSeconds);
            fuelCostPerSecond[MovementType.RotateRight] = Engines[MovementType.RotateRight].Sum(e => e.FuelPerSeconds);

            spawnTimer = shipTemplate.SpawnTime;
        }

        protected override void SetCollisionGroup()
        {
            if (IsPlayer)
            {
                Body.CollisionCategories = Category.Cat2;
                Body.CollidesWith = Category.Cat1 | Category.Cat3 | Category.Cat5 | Category.Cat6;
            }
            else
            {
                Body.CollisionCategories = Category.Cat3;
                Body.CollidesWith = Category.Cat1 | Category.Cat2 | Category.Cat4;
            }
        }

        public void SetModules(params ModuleTemplate[] modules)
        {
            Modules = Module.FromTemplate(this, modules);
        }

        public override void Destroy(bool ignoreScore=false)
        {
            Vector2 position = Body.Position;
            float rotation = Body.Rotation;
            base.Destroy();
            if(!IsPlayer)
            {
                TurnIntoWreckage(ConvertUnits.ToDisplayUnits(position),rotation);
            }
            
        }

        public void TurnIntoWreckage(Vector2 position, float rotation)
        {            
            string wreckagename = shipTemplate.Name + "Wreckage";
            EntityManager.Add<Wreckage, NoController>(wreckagename, position, rotation, Vector2.Zero);
        }

        public override void Update(GameTime gameTime)
        {
            float elapsed = (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (spawnTimer > -1)
            {
                spawnTimer -= elapsed;
                if (spawnTimer < -1)
                    spawnTimer = -1;
                return;
            }
            base.Update(gameTime);


            foreach (Engine engine in Engines.Values.SelectMany(e => e).ToList())
            {
                engine.Update(gameTime);
            }

            foreach (WeaponBase pwb in PrimaryWeapons) pwb.Update(gameTime);
            foreach (WeaponBase swb in SecondaryWeapons) swb.Update(gameTime);
            

            Scavenging.Update(gameTime);
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
            Fuel += shipTemplate.FuelRecoveryPerSecond * elapsed;
        }

        public override void Render(SpriteBatch spriteBatch, GameTime gameTime)
        {
            if (spawnTimer != -1)
            {
                float spawnRatio;
                if (spawnTimer >= 0)
                    spawnRatio = 1 - spawnTimer / shipTemplate.SpawnTime;
                else
                    spawnRatio = (spawnTimer+1) / 1f;
                spriteBatch.Draw(SpawnTexture, 
                    ConvertUnits.ToDisplayUnits(Body.Position)-(Graphic.Width*.5f)*Direction, null, shipTemplate.SpawnColor,
                    spawnTimer * 8, 
                    new Vector2(SpawnTexture.Bounds.Center.X, SpawnTexture.Bounds.Center.Y),
                    shipTemplate.SpawnSize * spawnRatio,
                    SpriteEffects.None, 0.5f);
                if (spawnRatio >= 0.5f || spawnTimer < 0)
                {
                    if (spawnTimer < 0)
                        spawnRatio = 1;
                    int drw = (int)((spawnRatio - 0.5f) * 2 * Graphic.Width);
                    spriteBatch.Draw(Graphic,
                        ConvertUnits.ToDisplayUnits(Body.Position),
                        new Rectangle(Graphic.Width-drw, 0,drw , Graphic.Height),
                        Color.White,
                        Body.Rotation,
                        Center,
                        Scale,
                        SpriteEffects.None,
                        DrawOrder);
                }
            }
            else
            {
                base.Render(spriteBatch, gameTime);
                Scavenging.Render(spriteBatch, gameTime);
            }
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
               Energy -= pwb.Fire(Energy);
            }
        }

        public void FireSecondary()
        {
            foreach (WeaponBase swb in SecondaryWeapons)
            {
                Energy -= swb.Fire(Energy);
            }
        }

        private void OnScavengeSuccess()
        {
            Energy += Scavenging.Target.GainEnergy;
            Fuel += Scavenging.Target.GainFuel;
            Structure += Scavenging.Target.GainStructure;
            checkBlueprints();
            Scavenging.Target.Destroy();
            Scavenging.Reset();
        }

        private void checkBlueprints()
        {
            String spaceshipName = Scavenging.Target.SpaceshipBlueprint.Name;
            String moduleName = Scavenging.Target.ModuleBlueprint.Name;
            if (spaceshipName == null && moduleName == null)
            {
                return;
            }
            else if (spaceshipName != null && moduleName != null)
            {
                
                unlockModule();
                unlockSpaceship();

            }
            else if (spaceshipName == null && moduleName != null)
            {
                unlockModule();
            }
            else
            {
                unlockSpaceship();
            }
        }

        private void unlockSpaceship()
        {
            Player.Get().UnlockTemplates(Scavenging.Target.SpaceshipBlueprint.Name);
        }

        private void unlockModule()
        {
            Player.Get().UnlockTemplates(Scavenging.Target.ModuleBlueprint.Name);
        }

        protected override void HandleCollisionResponse(Body with)
        {
            float structureDmg=0;
            float shieldDmg=0;
            if (with.UserData is WeaponEntity)
            {
                float dmg = (with.UserData as WeaponEntity).Damage * ((with.UserData as WeaponEntity).EmitterBody.UserData as Spaceship).DamageAmplifier;
                if (with.UserData is LaserBullet) // || with.UserData is LaserBeam
                {
                    shieldDmg = Math.Min(dmg, ShieldCapacity);
                    dmg -= shieldDmg;
                }
                structureDmg = Math.Max(0, dmg);
                
            } else if (with.UserData is GameBorder)
            {
                // TODO: add collision response to gameborder
            }
            else // Spaceship or Asteroid
            {
                Entity e = with.UserData as Entity;
                float velAvg = ((e.Body.LinearVelocity + Body.LinearVelocity) * 1.5f).LengthSquared();
                float massAvg = (e.Body.Mass + Body.Mass) * 1.5f;

                structureDmg = (float)Math.Sqrt(velAvg * massAvg) * .4f;
            }
            
            ShieldCapacity -= shieldDmg;
            Structure -= structureDmg;
        }
    }
}
