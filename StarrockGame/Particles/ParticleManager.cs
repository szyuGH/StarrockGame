using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StarrockGame.Particles
{
    public class ParticleManager
    {
        private List<ParticleSystem> systems;


        private ParticleManager() {
            systems = new List<ParticleSystem>();
        }

        public ParticleSystem AddSystem<T>(Game game) where T : ParticleSystem
        {
            ParticleSystem system = (ParticleSystem)Activator.CreateInstance(typeof(T), game, game.Content);
            game.Components.Add(system);
            systems.Add(system);
            return system;
        }

        public ParticleSystem GetSystem<T>() where T : ParticleSystem
        {
            return systems.Find(s => s is T);
        }

        private static ParticleManager instance;
        public static ParticleManager Get
        {
            get
            {
                if (instance == null)
                    instance = new ParticleManager();
                return instance;
            }
        }
    }
}
