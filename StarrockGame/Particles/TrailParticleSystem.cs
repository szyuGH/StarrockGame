#region File Description
//-----------------------------------------------------------------------------
// SmokePlumeParticleSystem.cs
//
// Microsoft XNA Community Game Platform
// Copyright (C) Microsoft Corporation. All rights reserved.
//-----------------------------------------------------------------------------
#endregion

#region Using Statements
using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
#endregion

namespace StarrockGame.Particles
{
    /// <summary>
    /// Custom particle system for creating a giant plume of long lasting smoke.
    /// </summary>
    class TrailParticleSystem : ParticleSystem
    {
        public TrailParticleSystem(Game game, ContentManager content)
            : base(game, content)
        { }


        protected override void InitializeSettings(ParticleSettings settings)
        {
            settings.TextureName = "particle";

            settings.MaxParticles = 600;

            settings.Duration = TimeSpan.FromSeconds(4);

            settings.MinDistortion = -(float)Math.PI / 90;
            settings.MaxDistortion = (float)Math.PI / 90;

            // Create a wind effect by tilting the gravity vector sideways.
            settings.Gravity = new Vector2(0, 0);

            settings.EndVelocity = 0.75f;

            settings.MinRotateSpeed = -1;
            settings.MaxRotateSpeed = 1;

            settings.MinStartSize = 50;
            settings.MaxStartSize = 50;

            settings.MinEndSize = 0;
            settings.MaxEndSize = 0;

            settings.MinColor = Color.GreenYellow;
            settings.MaxColor = Color.Transparent;
        }
    }
}
