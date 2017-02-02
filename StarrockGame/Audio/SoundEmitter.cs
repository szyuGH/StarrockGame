using FarseerPhysics.Dynamics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace StarrockGame.Audio
{
    public class SoundEmitter
    {
        public const float MAX_RANGE = 10;

        private SoundEffectInstance effectInstance;
        private Body emitter;

        //public bool Emitting { get; set; }
        public bool IsLooped
        {
            get { return effectInstance.IsLooped; }
            set
            {
                effectInstance.IsLooped = value;
            }
        }

        public float Pan { set { effectInstance.Pan = value; } }
        public float Pitch { set { effectInstance.Pitch = value; } }

        public SoundEmitter(SoundEffect effect, Body emitter)
        {
            effectInstance = effect.CreateInstance();
            this.emitter = emitter;
        }

        public void Update(bool emitting)
        {
            if (emitting)
            {
                float distanceRatio = 1-MathHelper.Clamp(Vector2.Distance(EntityManager.PlayerShip.Body.Position, emitter.Position) / MAX_RANGE, 0, 1);
                effectInstance.Volume = distanceRatio;
                if (!IsLooped || effectInstance.State != SoundState.Playing)
                    effectInstance.Play();
            } else if (effectInstance.State == SoundState.Playing)
            {
                effectInstance.Volume = MathHelper.Clamp(effectInstance.Volume - 0.05f, 0, 1);
                if (effectInstance.Volume == 0)
                {
                    effectInstance.Volume = 0;
                    effectInstance.Stop();
                }
            }
        }

        internal void Stop()
        {
            effectInstance.Stop();
        }
    }
}
