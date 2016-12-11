using Microsoft.Xna.Framework;
using StarrockGame.Entities;
using StarrockGame.InputManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StarrockGame.AI
{
    public class PlayerController : IController
    {


        public void Act(Entity entity, GameTime gameTime)
        {
            float elapsed = (float)gameTime.ElapsedGameTime.TotalSeconds;
            float acceleration = Input.Device.Acceleration();
            float deceleration = Input.Device.Deceleration();
            float rotation = Input.Device.Rotation();
            

            entity.Accelerate(acceleration, elapsed);
            entity.Decelerate(deceleration, elapsed);
            entity.Rotate(rotation, elapsed);


            (entity as Spaceship).Scavenge(Input.Device.Scavenging());
            (entity as Spaceship).ReplenishingShield = Input.Device.ReplenishingShield();

            if (Input.Device.FirePrimary())
            {
                (entity as Spaceship).FirePrimary();
            }
            if (Input.Device.FireSecondary())
            {
                (entity as Spaceship).FireSecondary();
            }
        }
    }
}
