using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FarseerPhysics.Dynamics;
using TData.TemplateData;
using StarrockGame.Caching;
using Microsoft.Xna.Framework;

namespace StarrockGame.Entities
{
    public class Wreckage : Entity
    {
        public float ScavengeTime { get; private set; }
        public float GainEnergy { get; private set; }
        public float GainFuel { get; private set; }
        public float GainStructure { get; private set; }

        public Wreckage(World world, string type) : base(world, type)
        {
            DrawOrder = 0;
            Body.CollisionCategories = Category.None;
            Center = new Vector2(Graphic.Width * .5f, Graphic.Height * .5f);
        }

        protected override EntityTemplateData LoadTemplate(string type)
        {
            return Cache.LoadTemplate<WreckageTemplateData>(type);
        }

        public override void Initialize<T>(Vector2 position, float rotation, Vector2 initialVelocity, float initialAngularVelocity = 0)
        {
            base.Initialize<T>(position, rotation, initialVelocity, initialAngularVelocity);
            // TODO: calculate scavenge time based on resources
            float time = 5;
            this.ScavengeTime = time;

            WreckageTemplateData wtd = Template as WreckageTemplateData;
            GainEnergy = MathHelper.Lerp(wtd.MinEnergy, wtd.MaxEnergy, (float)Program.Random.NextDouble());
            GainFuel = MathHelper.Lerp(wtd.MinFuel, wtd.MaxFuel, (float)Program.Random.NextDouble());
            GainStructure = MathHelper.Lerp(wtd.MinStructure, wtd.MaxStructure, (float)Program.Random.NextDouble());
        }
    }
}