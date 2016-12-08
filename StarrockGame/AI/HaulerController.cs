using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using StarrockGame.Entities;

namespace StarrockGame.AI
{
    public class HaulerController : IBehavior
    {
        private Wreckage target;

        public void Act(Entity entity, GameTime gameTime)
        {
            Spaceship ship = entity as Spaceship;
            if (target == null)
            {
                target = EntityManager.GetAllEntities(null, -1).Where(e => e is Wreckage).FirstOrDefault() as Wreckage;
            } else if (Vector2.DistanceSquared(entity.Body.Position, target.Body.Position) > ship.Scavenging.Range* ship.Scavenging.Range)
            {
                Vector2 targetDir = target.Body.Position - entity.Body.Position;
                float targetRotation = (float)(Math.Atan2(targetDir.Y, targetDir.X));
                float curRot = (float)(Math.Atan2(entity.Direction.Y, entity.Direction.X));
                float rotDif = MathHelper.WrapAngle(targetRotation - curRot);

                if (Math.Abs(rotDif) > MathHelper.ToRadians(35))
                {
                    if (Vector2.DistanceSquared(target.Body.Position, entity.Body.Position) < 4)
                        entity.Accelerate(1f);
                    else
                        entity.Accelerate(0.35f);
                }
                else if (Math.Abs(rotDif) > MathHelper.ToRadians(10))
                    entity.Accelerate(0.75f);
                else
                    entity.Accelerate(1);
                if (rotDif < 0)
                    entity.Rotate(Math.Max(rotDif, -1));
                else if (rotDif > 0)
                    entity.Rotate(Math.Min(rotDif, 1));
                
            } else
            {
                ship.Scavenge(true);
            }

        }
    }
}
