using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FarseerPhysics.Dynamics;
using Microsoft.Xna.Framework;
using StarrockGame.AI;

namespace StarrockGame.Entities.Weaponry
{
    public class LaserBulletBase : WeaponBase
    {
        public LaserBulletBase(Body body, string wName, Vector2 localPosition, float localAngle) 
            : base(body, wName, localPosition, localAngle)
        {
        }

        public override void Fire()
        {
            WeaponTransform transform = Transform;
            EntityManager.Add<LaserBullet, NoController>(Template.Name, transform.Position, transform.Angle, transform.Direction);

            // TODO: add internal cooldown
        }
    }
}
