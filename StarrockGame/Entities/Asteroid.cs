using FarseerPhysics.Dynamics;
using Microsoft.Xna.Framework.Graphics;
using StarrockGame.Caching;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TData.TemplateData;
using Microsoft.Xna.Framework;

namespace StarrockGame.Entities
{
    public class Asteroid : Entity
    {
        public Asteroid(World world, string type)
            :base(world, type)
        {
            Body.AngularDamping = 0;
            Body.LinearDamping = 0;
        }

        public override void Initialize<T>(Vector2 position, float rotation, Vector2 initialVelocity, float initialAngularVelocity = 0)
        {
            float velocityMultiplier = MathHelper.Lerp((Template as AsteroidTemplate).MinSpeed, (Template as AsteroidTemplate).MaxSpeed, (float)Program.Random.NextDouble());
            base.Initialize<T>(position, rotation,
                initialVelocity * velocityMultiplier,
                initialAngularVelocity);
        }

        protected override EntityTemplate LoadTemplate(string type)
        {
            return Cache.LoadTemplate<AsteroidTemplate>(type);
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            if (Vector2.DistanceSquared(EntityManager.Border.Center, Body.Position) > EntityManager.Border.Width * 1.5f)
            {
                Destroy(true);
            }
        }
    }
}
