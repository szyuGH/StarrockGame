using GPart;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StarrockGame.ParticleSystems
{
    public class TrailParticleSystem : ParticleSystem
    {
        public TrailParticleSystem(Game game, ContentManager content)
            : base(game, content)
        { }


        protected override void InitializeSettings(ParticleSettings settings)
        {
            settings.TextureName = "Graphics/Particles/Particle";
            settings.EffectName = "Effects/ParticleEffect";

            settings.MaxParticles = 1000;

            settings.Duration = TimeSpan.FromSeconds(3);

            settings.DurationRandomness = 1.5f;

            settings.EmitterVelocitySensitivity = 0.1f;

            settings.MinDirectionDistortion = -(float)Math.PI * 0.025f;
            settings.MaxDirectionDistortion = (float)Math.PI * 0.025f;


            settings.MinColor = Color.CadetBlue;// new Color(64, 96, 128, 255);
            settings.MaxColor = Color.White; //new Color(255, 255, 255, 128);

            settings.MinRotateSpeed = -4;
            settings.MaxRotateSpeed = 4;

            settings.MinStartSize = 50;
            settings.MaxStartSize = 80;

            settings.MinEndSize = 0;
            settings.MaxEndSize = 0;
        }
    }
}
