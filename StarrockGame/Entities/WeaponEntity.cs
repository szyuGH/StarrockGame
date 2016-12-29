using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Dynamics.Contacts;
using Microsoft.Xna.Framework;
using StarrockGame.AI;
using TData.TemplateData;

namespace StarrockGame.Entities
{
    public abstract class WeaponEntity : Entity
    {
        private Body _emitterBody;
        public Body EmitterBody
        {
            get { return _emitterBody; }
            set {
                _emitterBody = value;
                ResetCollisionCategory();
            }
        }

        public float Damage { get { return (Template as WeaponTemplate).Damage; } }

        public WeaponEntity(World world, string type) : base(world, type)
        {
            Body.LinearDamping = 0;
            Body.AngularDamping = 0;
        }

        public override void Initialize<T>(Vector2 position, float rotation, Vector2 initialVelocity, float initialAngularVelocity = 0)
        {
            base.Initialize<T>(position, rotation, initialVelocity, initialAngularVelocity);
        }

        private void ResetCollisionCategory()
        {
            if ((EmitterBody.UserData as Entity).Controller is PlayerController)
            {
                Body.CollisionCategories = Category.Cat4;
                Body.CollidesWith = Category.Cat1 | Category.Cat3;
            }
            else
            {
                Body.CollisionCategories = Category.Cat5;
                Body.CollidesWith = Category.Cat1 | Category.Cat2;
            }
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            if (Vector2.DistanceSquared(EntityManager.Border.Center, Body.Position) > EntityManager.Border.Width * 1f)
            {
                Destroy();
            }
        }


        protected override void HandleCollisionResponse(Body with)
        {
            Structure = 0;
        }
    }
}
