using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using StarrockGame.Entities;

namespace StarrockGame.AI
{
    public class HomingController : IController
    {
        public void Act(Entity entity, GameTime gameTime)
        {
            float elapsed = (float)gameTime.ElapsedGameTime.TotalSeconds;
            Entity target = EntityManager.PlayerShip;
            /*
             if entity must rotate > 135°, stop acceleration and focus on turning
             else keep accelerating till the engines explode

            always check the needed rotation and apply torque until rotation hits a threshold
            */

            Vector2 targetDir = target.Body.Position - entity.Body.Position;
            float targetRotation =(float)(Math.Atan2(targetDir.Y, targetDir.X));
            float curRot = (float)(Math.Atan2(entity.Direction.Y, entity.Direction.X));
            float rotDif = MathHelper.WrapAngle(targetRotation - curRot);


            //if (rotDif < MathHelper.ToRadians(-90))
            //    entity.Rotate(-1f);
            //else if (rotDif < MathHelper.ToRadians(-40))
            //    entity.Rotate(-0.8f);
            //else if (rotDif < MathHelper.ToRadians(-25))
            //    entity.Rotate(-0.65f);
            //else if (rotDif < MathHelper.ToRadians(-10))
            //    entity.Rotate(-0.35f);
            //else if (rotDif < MathHelper.ToRadians(-4))
            //entity.Rotate(-0.05f);

            //else if (rotDif > MathHelper.ToRadians(90))
            //    entity.Rotate(1f);
            //else if (rotDif > MathHelper.ToRadians(40))
            //    entity.Rotate(0.8f);
            //else if (rotDif > MathHelper.ToRadians(25))
            //    entity.Rotate(0.65f);
            //else if (rotDif > MathHelper.ToRadians(10))
            //    entity.Rotate(0.35f);
            //else if (rotDif > MathHelper.ToRadians(4))
            //    entity.Rotate(0.05f);

            //float ratio = Math.Max(Math.Min(90, Vector2.DistanceSquared(target.Body.Position, entity.Body.Position)/1.2f/1.2f), 5); 
            //if (Math.Abs(rotDif) < MathHelper.ToRadians(ratio))
            //    entity.Accelerate(1);

            if (Math.Abs(rotDif) > MathHelper.ToRadians(35))
            {
                if (Vector2.DistanceSquared(target.Body.Position, entity.Body.Position) < 4)
                    entity.Accelerate(1f, elapsed);
                else
                    entity.Accelerate(0.35f, elapsed);
            }
            else if (Math.Abs(rotDif) > MathHelper.ToRadians(10))
                entity.Accelerate(0.75f, elapsed);
            else
                entity.Accelerate(1, elapsed);
            if (rotDif < 0)
                entity.Rotate(Math.Max(rotDif, -1), elapsed);
            else if (rotDif > 0)
                entity.Rotate(Math.Min(rotDif, 1), elapsed);
            
        }
    }
}
