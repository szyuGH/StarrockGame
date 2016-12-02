#region File Description
//-----------------------------------------------------------------------------
// ParticleEmitter.cs
//
// Microsoft XNA Community Game Platform
// Copyright (C) Microsoft Corporation. All rights reserved.
//-----------------------------------------------------------------------------
#endregion

#region Using Statements
using System;
using Microsoft.Xna.Framework;
using FarseerPhysics.Dynamics;
using FarseerPhysics;
#endregion

namespace StarrockGame.Particles
{
    /// <summary>
    /// Helper for objects that want to leave particles behind them as they
    /// move around the world. This emitter implementation solves two related
    /// problems:
    /// 
    /// If an object wants to create particles very slowly, less than once per
    /// frame, it can be a pain to keep track of which updates ought to create
    /// a new particle versus which should not.
    /// 
    /// If an object is moving quickly and is creating many particles per frame,
    /// it will look ugly if these particles are all bunched up together. Much
    /// better if they can be spread out along a line between where the object
    /// is now and where it was on the previous frame. This is particularly
    /// important for leaving trails behind fast moving objects such as rockets.
    /// 
    /// This emitter class keeps track of a moving object, remembering its
    /// previous position so it can calculate the velocity of the object. It
    /// works out the perfect locations for creating particles at any frequency
    /// you specify, regardless of whether this is faster or slower than the
    /// game update rate.
    /// </summary>
    public class ParticleEmitter
    {
        #region Fields

        ParticleSystem particleSystem;
        float timeBetweenParticles;
        float timeLeftOver;

        Body body;
        float relativeAngle;
        float propulsionPower;
        

        public bool Emitting { get; set; }
        public bool ResetEmittingState = false;



        #endregion


        /// <summary>
        /// Constructs a new particle emitter object.
        /// </summary>
        public ParticleEmitter(ParticleSystem particleSystem,
                               float particlesPerSecond, Body body, float relativeAngle, float propulsionPower)
        {
            this.particleSystem = particleSystem;
            this.body = body;
            this.relativeAngle = relativeAngle;
            this.propulsionPower = propulsionPower;

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
                    float bodyRot = body.Rotation + MathHelper.ToRadians(relativeAngle);
                    Vector2 velocity = new Vector2((float)Math.Cos(bodyRot), (float)Math.Sin(bodyRot));

                    // Create particles as long as we have a big enough time interval.
                    while (timeToSpend > timeBetweenParticles)
                    {
                        currentTime += timeBetweenParticles;
                        timeToSpend -= timeBetweenParticles;

                        // Create the particle.
                        particleSystem.AddParticle(ConvertUnits.ToSimUnits(body.Position), velocity * propulsionPower);
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
