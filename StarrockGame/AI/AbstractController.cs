using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using StarrockGame.Entities;

namespace StarrockGame.AI
{
    public abstract class AbstractController : IController
    {
        

        public abstract void Act(Entity entity, GameTime gameTime);
    }
}
