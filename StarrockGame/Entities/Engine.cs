using FarseerPhysics.Dynamics;
using Microsoft.Xna.Framework;
using StarrockGame.Particles;
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

        public bool Emitting { get { return emitter.Emitting; } set { emitter.Emitting = value; } }

        private ParticleEmitter emitter;


        public Engine(Body body, MovementType dir, float power, float fps)
        {
            Direction = dir;
            PropulsionPower = power;
            FuelPerSeconds = fps;

            emitter = new ParticleEmitter(ParticleManager.Get.GetSystem<TrailParticleSystem>(), 10, body, 180, power);
            emitter.ResetEmittingState = true;
        }

        public void Update(GameTime gameTime)
        {
            emitter.Update(gameTime);
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
                res[(MovementType)data.Direction].Add(
                    new Engine(body, (MovementType)data.Direction, data.PropulsionPower, data.FuelCostPerSecond));
            }
            return res;
        }
    }
}
