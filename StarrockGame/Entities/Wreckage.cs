using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FarseerPhysics.Dynamics;
using TData.TemplateData;
using StarrockGame.Caching;
using Microsoft.Xna.Framework;
using TData;

namespace StarrockGame.Entities
{
    public class Wreckage : Entity
    {
        public float ScavengeTime { get; private set; }
        public float GainEnergy { get; private set; }
        public float GainFuel { get; private set; }
        public float GainStructure { get; private set; }
        public Blueprint SpaceshipBlueprint;
        public Blueprint ModuleBlueprint;

        public Wreckage(World world, string type) : base(world, type)
        {
            DrawOrder = 0;
            Center = new Vector2(Graphic.Width * .5f, Graphic.Height * .5f);
            
        }

        protected override void SetCollisionGroup()
        {
            Body.CollisionCategories = Category.None;
        }

        protected override EntityTemplate LoadTemplate(string type)
        {
            return Cache.LoadTemplate<WreckageTemplate>(type);
        }

        public override void Initialize(Vector2 position, float rotation, Vector2 initialVelocity, float initialAngularVelocity = 0)
        {
            base.Initialize(position, rotation, initialVelocity, initialAngularVelocity);

            WreckageTemplate wtd = Template as WreckageTemplate;
            GainEnergy = MathHelper.Lerp(wtd.MinEnergy, wtd.MaxEnergy, (float)Program.Random.NextDouble());
            GainFuel = MathHelper.Lerp(wtd.MinFuel, wtd.MaxFuel, (float)Program.Random.NextDouble());
            GainStructure = MathHelper.Lerp(wtd.MinStructure, wtd.MaxStructure, (float)Program.Random.NextDouble());
            SpaceshipBlueprint = new Blueprint(TemplateType.Spaceship);
            ModuleBlueprint = new Blueprint(TemplateType.Module);
            this.ScavengeTime = (GainEnergy + GainEnergy + GainStructure) / 100;
        }

        protected override void HandleCollisionResponse(Body with)
        {
        }
    }
}