using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using StarrockGame.Entities;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Collision;
using FarseerPhysics;
using StarrockGame.Audio;

namespace StarrockGame.AI
{
    public class DestroyerController : AbstractController
    {

        private int waypointIndex = -1;
        private Vector2 targetPos;

        public override void Act(Entity entity, GameTime gameTime)
        {
            Spaceship ship = entity as Spaceship;

            float elapsed = (float)gameTime.ElapsedGameTime.TotalSeconds;
            Entity target = entity.Target;
            
            if (target != null)
            {
                targetPos = target.Body.Position;
            }
            else if (waypointIndex == -1)
            {
                float wpx = Program.Random.NextFloat(-EntityManager.Border.Width * .5f, EntityManager.Border.Width * .5f);
                float wpy = Program.Random.NextFloat(-EntityManager.Border.Height * .5f, EntityManager.Border.Height * .5f);
                targetPos = ConvertUnits.ToSimUnits(new Vector2(wpx, wpy));
                waypointIndex = 0;
            }
            float distanceSquared = Vector2.DistanceSquared(targetPos, entity.Body.Position);
            const float minDistanceSquared = 10;
            if (target == null && Vector2.DistanceSquared(EntityManager.PlayerShip.Body.Position, entity.Body.Position) <= ConvertUnits.ToSimUnits(ship.RadarRange * ship.RadarRange))
            {
                entity.Target = EntityManager.PlayerShip;
                targetPos = EntityManager.PlayerShip.Body.Position;
                waypointIndex = -1;
                Sound.Instance.PlaySe("Spotted");
            } // one could put the out of range path here, where the entity loses the target
            else if (waypointIndex != -1 && distanceSquared <= minDistanceSquared * minDistanceSquared)
            {
                waypointIndex = -1;
                return;
            }


            // calculate the direction this entity needs to be turning to

            Vector2 targetDir;
            if (target != null && distanceSquared <= minDistanceSquared)
            {
                ship.Decelerate(2, elapsed);
                //targetDir = entity.Body.Position - targetPos;
                targetDir = targetPos - entity.Body.Position;
            }
            else
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
                if (target != null && Math.Abs(rotDif) < MathHelper.ToRadians(10))
                {
                    ship.FirePrimary();
                    //ship.FireSecondary();
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
