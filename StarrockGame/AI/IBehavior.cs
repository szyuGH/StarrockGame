using Microsoft.Xna.Framework;
using StarrockGame.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StarrockGame.AI
{
    public interface IBehavior
    {
        void Act(Entity entity, GameTime gameTime);
    }
}
