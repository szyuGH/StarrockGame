using FarseerPhysics.Dynamics;
using GPart;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using StarrockGame.Audio;
using StarrockGame.Caching;
using StarrockGame.ParticleSystems;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TData.TemplateData;

namespace StarrockGame.Entities
{
    public class Engine
    {
        public MovementType Direction { get; private set; }
        public float PropulsionPower { get; private set; }
        public float FuelPerSeconds { get; private set; }
        public Vector2 LocalPosition { get; private set; }

        public bool Emitting { get { return emitter.Emitting; } set { emitter.Emitting = value; } }

        private ParticleEmitter emitter;

        private SoundEffectInstance sound;


        public Engine(Body body, Vector2 localPos, MovementType dir, float power, float fps, float pps, float psize=1)
        {
            Direction = dir;
            PropulsionPower = power;
            FuelPerSeconds = fps;
            LocalPosition = localPos;

            emitter = new ParticleEmitter(Particles.Get<TrailParticleSystem>(), pps, body, LocalPosition, GetRelativeAngle(dir), power * EmittingFactor(dir), psize);
            emitter.ResetEmittingState = true;

            sound = Cache.LoadSe("Thruster").CreateInstance();
        }

        public void Update(GameTime gameTime)
        {
            if (emitter.Emitting)
            {
                sound.Play();
            } else if (sound.State == SoundState.Playing)
            {
                sound.Stop();
            }
            emitter.Update(gameTime);
        }

        private float GetRelativeAngle(MovementType dir)
        {
            switch (dir)
            {
                case MovementType.Forward: return (float)Math.PI;
                case MovementType.Brake: return 0;
                case MovementType.RotateLeft: return (float)Math.PI * .5f;
                case MovementType.RotateRight: return -(float)Math.PI * .5f;
            }
            return 0;
        }

        private float EmittingFactor(MovementType dir)
        {
            switch (dir)
            {
                case MovementType.Forward: return 1;
                case MovementType.Brake: return 1;
                case MovementType.RotateLeft:
                case MovementType.RotateRight: return 1;
            }
            return 0;
        }

        internal static Dictionary<MovementType, List<Engine>> FromTemplate(Body body, EngineData[] engines)
        {
            Dictionary<MovementType, List<Engine>> res = new Dictionary<MovementType, List<Engine>>();
            res[MovementType.Forward] = new List<Engine>();
            res[MovementType.Brake] = new List<Engine>();
            res[MovementType.RotateLeft] = new List<Engine>();
            res[MovementType.RotateRight] = new List<Engine>();

            foreach (EngineData data in engines)
            {
                MovementType mtype = (MovementType)data.Direction;
                if (!res.ContainsKey(mtype))
                    res[mtype] = new List<Engine>();
                res[mtype].Add(
                    new Engine(body, data.LocalPosition, mtype, data.PropulsionPower, data.FuelCostPerSecond, data.ParticlesPerSecond, data.ParticleSize));
            }
            return res;
        }
    }
}
