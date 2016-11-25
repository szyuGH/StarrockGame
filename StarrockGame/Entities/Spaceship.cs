using FarseerPhysics.Dynamics;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StarrockGame.Entities
{
    class Spaceship : Entity
    {
        private int energy;
        private int shieldCapcity;
        public Spaceship(Texture2D texture, World world, float x, float y, float mass,int energy, int shieldCapcity) : base(texture, world, x, y, mass)
        {
            this.energy = energy;
            this.shieldCapcity = shieldCapcity;
        }
                   
        
    }
}
