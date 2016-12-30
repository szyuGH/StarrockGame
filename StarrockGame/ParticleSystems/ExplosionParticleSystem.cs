using GPart;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StarrockGame.ParticleSystems
{
    public class ExplosionParticleSystem : ParticleSystem
    {
        public ExplosionParticleSystem(Game game, ContentManager content)
            : base(game, content)
        { }


        protected override void InitializeSettings(ParticleSettings settings)
        {
            settings.TextureName = "Graphics/Particles/Explosion";
            settings.EffectName = "Effects/ParticleEffect";

            settings.MaxParticles = 500;

            settings.Duration = TimeSpan.FromSeconds(2);
            settings.DurationRandomness = 1;

            settings.EmitterVelocitySensitivity = 0.1f;
            settings.MinDirectionDistortion = -(float)Math.PI * 0.01f;
            settings.MaxDirectionDistortion = (float)Math.PI * 0.01f;

            settings.EndVelocity = 0;

            settings.MinColor = Color.DarkGray;
            settings.MaxColor = Color.Gray;

            settings.MinRotateSpeed = -1;
            settings.MaxRotateSpeed = 1;

            settings.MinStartSize = 7;
            settings.MaxStartSize = 7;

            settings.MinEndSize = 70;
            settings.MaxEndSize = 140;

            // Use additive blending.
            settings.BlendState = BlendState.Additive;
        }
    }
}
