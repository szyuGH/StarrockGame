using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FarseerPhysics.Dynamics;
using TData.TemplateData;
using StarrockGame.Caching;

namespace StarrockGame.Entities
{
    public class LaserBullet : Entity
    {
        public LaserBullet(World world, string type) : base(world, type)
        {
        }

        protected override EntityTemplateData LoadTemplate(string type)
        {
            return Cache.LoadTemplate<LaserBulletTemplateData>(type);
        }
    }
}
