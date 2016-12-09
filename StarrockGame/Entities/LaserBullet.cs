using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FarseerPhysics.Dynamics;
using TData.TemplateData;
using StarrockGame.Caching;
using FarseerPhysics.Dynamics.Contacts;
using Microsoft.Xna.Framework;

namespace StarrockGame.Entities
{
    public class LaserBullet : WeaponEntity
    {
        

        public LaserBullet(World world, string type) : base(world, type)
        {
            
        }

        public override void Initialize<T>(Vector2 position, float rotation, Vector2 initialVelocity, float initialAngularVelocity = 0)
        {
            base.Initialize<T>(position, rotation, initialVelocity * (Template as LaserBulletTemplate).Velocity, initialAngularVelocity);
        }

        protected override EntityTemplate LoadTemplate(string type)
        {
            return Cache.LoadTemplate<LaserBulletTemplate>(type);
        }

        
    }
}
