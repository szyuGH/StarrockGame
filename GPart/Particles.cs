using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GPart
{
    public static class Particles
    {
        private static Dictionary<Type, ParticleSystem> systems = new Dictionary<Type, ParticleSystem>();


        public static ParticleSystem Get<T>() where T : ParticleSystem
        {
            return (systems.ContainsKey(typeof(T))) ? systems[typeof(T)] : null;
        }

        public static T Add<T>(Game game) where T : ParticleSystem
        {
            ParticleSystem system;
            if ((system = Get<T>()) == null)
            {
                system = (ParticleSystem)Activator.CreateInstance(typeof(T), game, game.Content);
                systems[typeof(T)] = system;
                game.Components.Add(system);
            }
            return system as T;
        }

        public static void SetCamera(Matrix view, Matrix projection, float zoom=1f)
        {
            foreach (ParticleSystem system in systems.Values)
            {
                system?.SetZoom(zoom);
                system?.SetCamera(view, projection);
            }
        }

        public static void Emit<T>(Vector2 position, Vector2 velocity, float size = 1f) where T : ParticleSystem
        {
            systems[typeof(T)]?.AddParticle(position, velocity, size);
        }
    }
}
