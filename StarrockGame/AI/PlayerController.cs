using StarrockGame.Entities;
using StarrockGame.InputManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StarrockGame.AI
{
    public class PlayerController : IBehavior
    {


        public void Act(Entity entity, float elapsed)
        {
            float acceleration = Input.Device.Acceleration();
            float deceleration = Input.Device.Deceleration();
            float rotation = Input.Device.Rotation();
            

            entity.Accelerate(acceleration);
            entity.Decelerate(deceleration);
            entity.Rotate(rotation);
            // check rotation
        }
    }
}
