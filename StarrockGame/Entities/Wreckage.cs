using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FarseerPhysics.Dynamics;
using TData.TemplateData;
using StarrockGame.Caching;

namespace StarrockGame.Entities
{
    public class Wreckage : Entity
    {
        public Wreckage(World world, string type) :base(world, type)
        {
            DrawOrder = 0;
            Body.CollisionCategories = Category.None;
        }

        protected override EntityTemplateData LoadTemplate(string type)
        { 
            return Cache.LoadTemplate<WreckageTemplateData>(type);
        }
    }
}