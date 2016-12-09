using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FarseerPhysics.Dynamics;
using Microsoft.Xna.Framework;
using StarrockGame.AI;
using TData.TemplateData;

namespace StarrockGame.Entities.Weaponry
{
    public class LaserBulletBase : WeaponBase
    {
        public LaserBulletBase(Body body, WeaponBaseData baseTemplate, WeaponTemplateData weaponTemplate, Vector2 localPosition, float localAngle) 
            : base(body, baseTemplate, weaponTemplate, localPosition, localAngle)
        {
            Body.IsBullet = true;
        }

        protected override void DoFire()
        {
            if (CanShoot)
            {
                WeaponTransform transform = Transform;
                LaserBullet lb = EntityManager.Add<LaserBullet, NoController>(BaseTemplate.WeaponType, transform.Position, transform.Angle, transform.Direction);
                lb.EmitterBody = Body;
            }
            // TODO: add internal cooldown
        }
    }
}
