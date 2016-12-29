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
    public class MineBase : WeaponBase
    {
        public MineBase(Entity parent, WeaponBaseData baseTemplate, WeaponTemplate weaponTemplate, Vector2 localPosition, float localAngle) 
            : base(parent, baseTemplate, weaponTemplate, localPosition, localAngle)
        {
        }

        protected override void DoFire()
        {
            if (CanShoot)
            {
                WeaponTransform transform = Transform;
                transform.Direction.Normalize();
                Mine r = EntityManager.Add<Mine, NoController>(BaseTemplate.WeaponType, transform.Position, transform.Angle, -transform.Direction*.25f, Program.Random.NextFloat(-2, 2));
                r.EmitterBody = Parent.Body;
            }
        }
    }
}
