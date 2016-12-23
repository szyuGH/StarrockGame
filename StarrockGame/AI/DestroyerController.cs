using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using StarrockGame.Entities;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Collision;

namespace StarrockGame.AI
{
    public class DestroyerController : IController
    {
        public void Act(Entity entity, GameTime gameTime)
        {
            Spaceship ship = entity as Spaceship;

            float elapsed = (float)gameTime.ElapsedGameTime.TotalSeconds;
            Entity target = EntityManager.PlayerShip;
            float distanceSquared = Vector2.DistanceSquared(target.Body.Position, entity.Body.Position);
            const float minDistanceSquard = 7;

            // calculate the direction this entity needs to be turning to
            Vector2 targetPos = target.Body.Position;
            Vector2 targetDir;
            if (distanceSquared <= minDistanceSquard)
            {
                ship.Decelerate(1, elapsed);
                targetDir = entity.Body.Position - targetPos;
            } else
            {
                targetDir = targetPos - entity.Body.Position;
            }

            
            // get the rotation difference
            float targetRotation = (float)(Math.Atan2(targetDir.Y, targetDir.X));
            float curRot = (float)(Math.Atan2(entity.Direction.Y, entity.Direction.X));
            float rotDif = MathHelper.WrapAngle(targetRotation - curRot);

            // if difference is > 35°, acceleration depends on the distance
            if (Math.Abs(rotDif) > MathHelper.ToRadians(35))
            {
                if (distanceSquared < 4)
                    entity.Accelerate(1f, elapsed);
                else
                    entity.Accelerate(0.35f, elapsed);
            }
            // if difference is > 10°, this entity may accelerate a little bit faster
            else if (Math.Abs(rotDif) > MathHelper.ToRadians(10))
                entity.Accelerate(0.75f, elapsed);
            else
            {
                entity.Accelerate(1, elapsed);
                if (Math.Abs(rotDif) < MathHelper.ToRadians(5))
                {
                    ship.FirePrimary();
                    ship.FireSecondary();
                }
            }

            // rotate towards the destined position (could be split to rotate less when difference is small)
            if (rotDif < 0)
                entity.Rotate(Math.Max(rotDif, -1), elapsed);
            else if (rotDif > 0)
                entity.Rotate(Math.Min(rotDif, 1), elapsed);

        }
    }
}
