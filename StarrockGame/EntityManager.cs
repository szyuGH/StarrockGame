using FarseerPhysics.Dynamics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StarrockGame.AI;
using StarrockGame.Caching;
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
        private static List<Entity> entities;
        public static World World;
        public static Entity PlayerShip { get; private set; }
        public static GameBorder Border;

        public static Effect OutlineEffect;
        private static EffectParameterCollection outlineParams;

        public static void Initialize()
        {
            entities = new List<Entity>();
            World = new World(Vector2.Zero);
            OutlineEffect = Cache.LoadEffect("OutlineEffect");
            outlineParams = OutlineEffect.Parameters;
        }

        public static void Update(GameTime gameTime)
        {
            World.Step(Math.Min((float)gameTime.ElapsedGameTime.TotalSeconds, (1f / 60f)));
            for (int i = entities.Count-1; i >= 0; i--)
            {
                if (entities[i].IsAlive)
                {
                    entities[i].Update(gameTime);
                }
            }
        }

        public static void Render(SpriteBatch batch, GameTime gameTime)
        {
            foreach (Entity e in entities.Where(e => e.IsAlive).OrderBy(e => e.DrawOrder))
            {
                e.Render(batch, gameTime, outlineParams);
            }
        }

        public static T Add<T, B>(string type, Vector2 pos, float rot, Vector2 initialVelocity, float initialAngularVelocity = 0, bool player = false)
            where T : Entity
            where B : IController
        {

            Entity entity = FindFreeEntity<T>(type, player);
            entity.Initialize<B>(pos, rot, initialVelocity, initialAngularVelocity);
            
                
            return entity as T;
        }

        public static T Add<T>(string type, Vector2 pos, float rot, Vector2 initialVelocity, float initialAngularVelocity = 0, bool player=false)
            where T : Entity
        {
            Entity entity = FindFreeEntity<T>(type, player);
            entity.Initialize(pos, rot, initialVelocity, initialAngularVelocity);
            return entity as T;
        }

        private static T FindFreeEntity<T>(string type, bool player = false)
            where T : Entity
        {
            Entity entity = entities.Find(e => e is T && e.IsAlive == false && e.Template.File.Equals(type));
            if (entity == null)
            {
                entity = (Entity)Activator.CreateInstance(typeof(T), World, type);
                entities.Add(entity);
            }
            if (player)
            {
                (entity as Spaceship).SetModules(SessionManager.ModuleTemplates.ToArray());
                PlayerShip = entity;
            }
            else if (typeof(T) == typeof(WeaponEntity))
            {
                entity.Target = ((entity as WeaponEntity).EmitterBody.UserData as Entity).Target;
            }
            return entity as T;
        }

        public static void Clear()
        {
            entities.Clear();
            World.Clear();
        }

        public static List<Entity> GetAllEntities(Entity enquirer, float range=-1)
        {
            if (enquirer == null)
                return entities;
            if (range == -1)
                return entities.Where(e => e.IsAlive).ToList();
            else
                return entities.Where(e => e.IsAlive && Vector2.DistanceSquared(enquirer.Body.Position, e.Body.Position) <= range * range).ToList();
        }

        public static void AddExplosion()
        {

        }
    }
}
