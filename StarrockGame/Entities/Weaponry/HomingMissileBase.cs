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
    public class HomingMissileBase : WeaponBase
    {
        public HomingMissileBase(Entity parent, WeaponBaseData baseTemplate, WeaponTemplate weaponTemplate, Vector2 localPosition, float localAngle) 
            : base(parent, baseTemplate, weaponTemplate, localPosition, localAngle)
        {
        }

        protected override void DoFire()
        {
            if (CanShoot)
            {
                WeaponTransform transform = Transform;
                HomingMissile hm = EntityManager.Add<HomingMissile, NoController>(BaseTemplate.WeaponType, transform.Position, transform.Angle, transform.Direction);
                hm.EmitterBody = Parent.Body;
                hm.Target = Parent.Target?.Body; 
                // TODO: determine target (how do we determine an enemy target?
                
            }
        }
    }
}
