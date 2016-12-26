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
            Entity target = entity.Target;
            if (target == null)
                return;

            // calculate the direction this entity needs to be turning to
            Vector2 targetDir = target.Body.Position - entity.Body.Position;
            // get the rotation difference
            float targetRotation = (float)(Math.Atan2(targetDir.Y, targetDir.X));
            float curRot = (float)(Math.Atan2(entity.Direction.Y, entity.Direction.X));
            float rotDif = MathHelper.WrapAngle(targetRotation - curRot);

            // if difference is > 35°, acceleration depends on the distance
            if (Math.Abs(rotDif) > MathHelper.ToRadians(35))
            {
                if (Vector2.DistanceSquared(target.Body.Position, entity.Body.Position) < 4)
                    entity.Accelerate(1f, elapsed);
                else
                    entity.Accelerate(0.35f, elapsed);
            }
            // if difference is > 10°, this entity may accelerate a little bit faster
            else if (Math.Abs(rotDif) > MathHelper.ToRadians(10))
                entity.Accelerate(0.75f, elapsed);
            else
                entity.Accelerate(1, elapsed);

            // rotate towards the destined position (could be split to rotate less when difference is small)
            if (rotDif < 0)
                entity.Rotate(Math.Max(rotDif, -1), elapsed);
            else if (rotDif > 0)
                entity.Rotate(Math.Min(rotDif, 1), elapsed);

        }
    }
}
