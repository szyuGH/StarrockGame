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
        public LaserBulletBase(Entity parent, WeaponBaseData baseTemplate, WeaponTemplate weaponTemplate, Vector2 localPosition, float localAngle) 
            : base(parent, baseTemplate, weaponTemplate, localPosition, localAngle)
        {
            
        }

        protected override void DoFire()
        {
            if (CanShoot)
            {
                WeaponTransform transform = Transform;
                LaserBullet lb = EntityManager.Add<LaserBullet, HomingController>(BaseTemplate.WeaponType, transform.Position, transform.Angle, transform.Direction);
                lb.EmitterBody = Parent.Body;
            }
        }
    }
}
