using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Dynamics.Contacts;
using Microsoft.Xna.Framework;

namespace StarrockGame.Entities
{
    public abstract class WeaponEntity : Entity
    {
        public Body EmitterBody;

        public WeaponEntity(World world, string type) : base(world, type)
        {
            Body.LinearDamping = 0;
            Body.AngularDamping = 0;
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            if (Vector2.DistanceSquared(EntityManager.Border.Center, Body.Position) > EntityManager.Border.Width * 1f)
            {
                Destroy();
            }
        }

        protected override bool Body_OnCollision(Fixture fixtureA, Fixture fixtureB, Contact contact)
        {
            Body other;
            if (fixtureA.Body.Equals(Body))
                other = fixtureB.Body;
            else
                other = fixtureA.Body;

            if (other.Equals(EmitterBody)) return false;
            else if (other.UserData is WeaponEntity && (other.UserData as WeaponEntity).EmitterBody.Equals(EmitterBody)) return false;
            return base.Body_OnCollision(fixtureA, fixtureB, contact);
        }
    }
}
