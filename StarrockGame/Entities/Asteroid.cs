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
            float velocityMultiplier = MathHelper.Lerp((Template as AsteroidTemplateData).MinSpeed, (Template as AsteroidTemplateData).MaxSpeed, (float)Program.Random.NextDouble());
            base.Initialize<T>(position, rotation, 
                initialVelocity * velocityMultiplier, 
                initialAngularVelocity);
        }

        protected override EntityTemplateData LoadTemplate(string type)
        {
            return Cache.LoadTemplate<AsteroidTemplateData>(type);
        }
    }
}
