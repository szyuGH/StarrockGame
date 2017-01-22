using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using StarrockGame.Entities;

namespace StarrockGame.AI
{
    public class HaulerController : AbstractController
    {
        private Wreckage target;

        public override void Act(Entity entity, GameTime gameTime)
        {
            Spaceship ship = entity as Spaceship;
            float elapsed = (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (target == null)
            {
                target = EntityManager.GetAllEntities(null, -1).Where(e => e is Wreckage).FirstOrDefault() as Wreckage;
            } else
            {
                if (!target.IsAlive)
                {
                    target = null;
                    return;
                }

                float rangeSquared = Vector2.DistanceSquared(entity.Body.Position, target.Body.Position);
                if (rangeSquared <= ship.Scavenging.Range * ship.Scavenging.Range)
                {
                    ship.Scavenge(true);
                } else 
                {
                    float velMultiplier = 1;
                    if (rangeSquared < ship.Scavenging.Range * ship.Scavenging.Range * ship.Body.LinearVelocity.LengthSquared())
                    {
                        velMultiplier = 0.45f;
                    }

                    Vector2 targetDir = target.Body.Position - entity.Body.Position;
                    float targetRotation = (float)(Math.Atan2(targetDir.Y, targetDir.X));
                    float curRot = (float)(Math.Atan2(entity.Direction.Y, entity.Direction.X));
                    float rotDif = MathHelper.WrapAngle(targetRotation - curRot);

                    if (Math.Abs(rotDif) > MathHelper.ToRadians(35))
                    {
                        if (Vector2.DistanceSquared(target.Body.Position, entity.Body.Position) < 4)
                            entity.Accelerate(velMultiplier, elapsed);
                        else
                            entity.Accelerate(0.35f * velMultiplier, elapsed);
                    }
                    else if (Math.Abs(rotDif) > MathHelper.ToRadians(10))
                        entity.Accelerate(0.75f * velMultiplier, elapsed);
                    else
                        entity.Accelerate(velMultiplier, elapsed);
                    if (rotDif < 0)
                        entity.Rotate(Math.Max(rotDif, -1), elapsed);
                    else if (rotDif > 0)
                        entity.Rotate(Math.Min(rotDif, 1), elapsed);
                }
            }

        }
    }
}
