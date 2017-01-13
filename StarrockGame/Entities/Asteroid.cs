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
            DrawOrder = 0.5f;
            Body.AngularDamping = 0;
            Body.LinearDamping = 0;
        }

        public override void Initialize(Vector2 position, float rotation, Vector2 initialVelocity, float initialAngularVelocity = 0)
        {
            float velocityMultiplier = MathHelper.Lerp((Template as AsteroidTemplate).MinSpeed, (Template as AsteroidTemplate).MaxSpeed, (float)Program.Random.NextDouble());
            base.Initialize(position, rotation,
                initialVelocity * velocityMultiplier,
                initialAngularVelocity);
        }

        protected override void SetCollisionGroup()
        {
            Body.CollisionCategories = Category.Cat1;
            Body.CollidesWith = Category.Cat1 | Category.Cat2 | Category.Cat3 | Category.Cat4 | Category.Cat5;
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

        protected override void HandleCollisionResponse(Body with)
        {
            Structure = 0;
        }
    }
}
