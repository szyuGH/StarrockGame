using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FarseerPhysics.Dynamics;
using Microsoft.Xna.Framework;
using TData.TemplateData;
using StarrockGame.AI;

namespace StarrockGame.Entities.Weaponry
{
    public class RocketBase : WeaponBase
    {
        public RocketBase(Entity parent, WeaponBaseData baseTemplate, WeaponTemplate weaponTemplate, Vector2 localPosition, float localAngle) 
            : base(parent, baseTemplate, weaponTemplate, localPosition, localAngle)
        {
        }

        protected override void DoFire()
        {
            if (CanShoot)
            {
                WeaponTransform transform = Transform;
                Rocket r = EntityManager.Add<Rocket, NoController>(BaseTemplate.WeaponType, transform.Position, transform.Angle, transform.Direction);
                r.EmitterBody = Parent.Body;
            }
        }
    }
}
