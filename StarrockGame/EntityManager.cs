using FarseerPhysics.Dynamics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StarrockGame.AI;
using StarrockGame.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StarrockGame
{
    public static class EntityManager
    {
        private static List<Entity> entities = new List<Entity>();
        public static readonly World World = new World(Vector2.Zero);


        public static void Update(GameTime gameTime)
        {
            World.Step(Math.Min((float)gameTime.ElapsedGameTime.TotalSeconds, (1f / 60f)));
            foreach (Entity e in entities.Where(e => e.IsAlive))
            {
                e.Update(gameTime);
            }
        }

        public static void Render(SpriteBatch batch, GameTime gameTime)
        {
            foreach (Entity e in entities.Where(e => e.IsAlive))
            {
                e.Render(batch, gameTime);
            }
        }

        public static T Add<T, B>(string type, Vector2 pos, float rot, Vector2 initialVelocity, float initialAngularVelocity = 0)
            where T : Entity
            where B : IBehavior
        {

            Entity entity = entities.Find(e => e is T && e.IsAlive == false);
            if (entity == null)
            {
                entity = (Entity)Activator.CreateInstance(typeof(T), World, type);
                entities.Add(entity);
            }
            entity.Initialize<B>(pos, rot, initialVelocity, initialAngularVelocity);
            return entity as T;
        }

        public static void Clear()
        {
            entities.Clear();
            World.Clear();
        }

        public static List<Entity> GetAllLiving()
        {
            return entities.Where(e => e.IsAlive).ToList();
        }
    }
}
