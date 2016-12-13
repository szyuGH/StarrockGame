using FarseerPhysics;
using FarseerPhysics.Dynamics;
using GPart;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StarrockGame.ParticleSystems
{
    public class ParticleEmitter
    {
        #region Fields

        ParticleSystem particleSystem;
        float timeBetweenParticles;
        float timeLeftOver;

        Body body;
        float relativeAngle;
        float propulsionPower;
        Vector2 localPosition;

        public bool Emitting { get; set; }
        public bool ResetEmittingState = false;



        #endregion


        /// <summary>
        /// Constructs a new particle emitter object.
        /// </summary>
        public ParticleEmitter(ParticleSystem particleSystem,
                               float particlesPerSecond, Body body, Vector2 localPosition, float relativeAngle, float propulsionPower)
        {
            this.particleSystem = particleSystem;
            this.body = body;
            this.relativeAngle = relativeAngle;
            this.propulsionPower = propulsionPower;
            this.localPosition = localPosition;

            timeBetweenParticles = 1.0f / particlesPerSecond;
        }


        /// <summary>
        /// Updates the emitter, creating the appropriate number of particles
        /// in the appropriate positions.
        /// </summary>
        public void Update(GameTime gameTime)
        {
            if (gameTime == null)
                throw new ArgumentNullException("gameTime");

            // Work out how much time has passed since the previous update.
            float elapsedTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (elapsedTime > 0)
            {

                // If we had any time left over that we didn't use during the
                // previous update, add that to the current elapsed time.
                float timeToSpend = timeLeftOver + elapsedTime;

                // Counter for looping over the time interval.
                float currentTime = -timeLeftOver;


                if (Emitting)
                {
                    float bodyRot = body.Rotation + relativeAngle;
                    Vector2 velocity = new Vector2((float)Math.Cos(bodyRot), (float)Math.Sin(bodyRot));

                    Matrix trMatrix = Matrix.CreateTranslation(new Vector3(-ConvertUnits.ToDisplayUnits(body.Position), 0))
                        * Matrix.CreateTranslation(new Vector3(localPosition, 0))
                        * Matrix.CreateRotationZ(body.Rotation)
                        * Matrix.CreateTranslation(new Vector3(ConvertUnits.ToDisplayUnits(body.Position), 0));
                    Vector2 emitPosition = Vector2.Transform(ConvertUnits.ToDisplayUnits(body.Position), trMatrix);


                    //Matrix tr = Matrix.CreateTranslation(new Vector3(-ConvertUnits.ToDisplayUnits(Body.Position), 0))
                    //* Matrix.CreateTranslation(new Vector3(_relativeTrailBays[i], 0))
                    //* Matrix.CreateRotationZ((float)(Body.Rotation))
                    //* Matrix.CreateTranslation(new Vector3(ConvertUnits.ToDisplayUnits(Body.Position), 0));
                    //TrailBays[i] = Vector2.Transform(ConvertUnits.ToDisplayUnits(Body.Position), tr);

                    // Create particles as long as we have a big enough time interval.
                    while (timeToSpend > timeBetweenParticles)
                    {
                        currentTime += timeBetweenParticles;
                        timeToSpend -= timeBetweenParticles;

                        // Create the particle.
                        particleSystem.AddParticle(emitPosition, velocity * propulsionPower * 0.25f);
                    }

                    if (ResetEmittingState)
                        Emitting = false;
                }
                // Store any time we didn't use, so it can be part of the next update.
                timeLeftOver = timeToSpend;
            }
        }
    }
}