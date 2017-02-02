using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FarseerPhysics.Dynamics;
using TData.TemplateData;
using StarrockGame.Caching;
using Microsoft.Xna.Framework;

namespace StarrockGame.Entities
{
    public class Mine : WeaponEntity
    {
        const float EXISTENCE_TIME = 25;

        private float existenceTimer;

        public float ExplosionRange;


        public Mine(World world, string type) 
            : base(world, type)
        {
            Body.IsBullet = true;
        }

        public override void Initialize(Vector2 position, float rotation, Vector2 initialVelocity, float initialAngularVelocity = 0)
        {
            base.Initialize(position, rotation, initialVelocity, initialAngularVelocity);
            MineTemplate tmp = (Template as MineTemplate);
            
            ExplosionRange = tmp.ExplosionRange;
            Body.LinearDamping = tmp.LinearDampening;

            existenceTimer = EXISTENCE_TIME;
        }

        protected override EntityTemplate LoadTemplate(string type)
        {
            return Cache.LoadTemplate<MineTemplate>(type);
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            existenceTimer -= (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (existenceTimer <= 0)
                Destroy(false);
        }
    }
}
