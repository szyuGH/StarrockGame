using FarseerPhysics;
using Microsoft.Xna.Framework;
using StarrockGame.AI;
using StarrockGame.Caching;
using StarrockGame.Entities;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TData.TemplateData;

namespace StarrockGame
{
    internal static class SessionManager
    {
        const float PI = (float)Math.PI;

        private static SessionDifficulty _difficulty;
        internal static SessionDifficulty Difficulty
        {
            get { return _difficulty; }
            set
            {
                _difficulty = value;
                estimatedDuration = (float)TimeSpan.FromMinutes(data.Spawns[(int)Difficulty].EstimatedDuration).TotalSeconds;
            }
        }
        internal static SpaceshipTemplate UsedShipTemplate { get; set; }
        internal static float Score { get; set; }
        internal static List<ModuleTemplate> ModuleTemplates { get; set; } = new List<ModuleTemplate>();
        internal static TimeSpan ElapsedTime;
        internal static SpaceshipTemplate LastUsedShipTemplate { get; private set; }

        private static SessionData data;
        private static float spawnRatio;

        private static float asteroidSpawnTimer;
        private static float enemySpawnTimer;

        private static SpawnData spawn { get { return data.Spawns[(int)Difficulty]; } }
        private static float estimatedDuration;

        internal static List<string> FoundBlueprints { get; private set; } = new List<string>();

        public static void Initialize()
        {
            data = Cache.LoadTemplate<SessionData>("Sessions");
            Difficulty = SessionDifficulty.Easy;
        }

        public static void Reset()
        {
            ElapsedTime = TimeSpan.FromSeconds(0);
            LastUsedShipTemplate = UsedShipTemplate;
            UsedShipTemplate = null;
            Score = 0;
            ModuleTemplates.Clear();
            FoundBlueprints.Clear();

            enemySpawnTimer = 0;
            asteroidSpawnTimer = 0;
        }

        internal static void Update(GameTime gameTime)
        {
            float elapsed = (float)gameTime.ElapsedGameTime.TotalSeconds;
            ElapsedTime += gameTime.ElapsedGameTime;
            if (spawnRatio < 1)
            {
                spawnRatio = (float)ElapsedTime.TotalSeconds / estimatedDuration;
                if (spawnRatio > 1)
                    spawnRatio = 1;
            }

            asteroidSpawnTimer += elapsed;
            enemySpawnTimer += elapsed;

            SpawnAsteroids();
            SpawnEnemyShips();
        }

        internal static void AddBlueprint(string bp)
        {
            if (!FoundBlueprints.Contains(bp))
                FoundBlueprints.Add(bp);
        }
        
        private static void SpawnAsteroids()
        {
            if (asteroidSpawnTimer >= currentAsteroidSpawnTime)
            {
                asteroidSpawnTimer -= currentAsteroidSpawnTime;
                for (int i = 0; i < currentAsteroidSpawnCount; i++)
                {
                    SpawnAsteroid();
                }
            }
        }

        private static void SpawnAsteroid()
        {
            Vector2 spos, sdir;
            float rot;
            CalcAsteroidSpawnData(out spos, out sdir, out rot);
            //Console.WriteLine("Spawning Asteroid at \"{0}\" with direction \"{1}\" and rotation \"{2}\"", ConvertUnits.ToSimUnits(spos), sdir, rot);
            EntityManager.Add<Asteroid, NoController>("Asteroid", spos, 0, sdir, rot);
        }

        private static void SpawnEnemyShips()
        {
            if (enemySpawnTimer >= currentEnemySpawnTime)
            {
                enemySpawnTimer -= currentEnemySpawnTime;
                // roll for boss spawn chance; when boss spawns, timer is 3 times the normal timer for this wave
                if (Program.Random.Next(100) < spawn.BossSpawnChance)
                {
                    SpawnBoss();
                    enemySpawnTimer -= currentEnemySpawnTime * 2;
                }
                else
                {
                    for (int i = 0; i < currentEnemySpawnCount; i++)
                    {
                        SpawnEnemyShip();
                    }
                }
            }
        }

        private static void SpawnEnemyShip()
        {
            Vector2 spos;
            float rot;
            string type;
            CalcEnemyShipSpawnData(out spos, out rot, out type);
            EntityManager.Add<Spaceship>(type, spos, rot, Vector2.Zero, 0, false);
        }

        private static void SpawnBoss()
        {
            // TODO: Spawn Boss
        }


        private static void CalcAsteroidSpawnData(out Vector2 pos, out Vector2 dir, out float rot)
        {
            pos = EntityManager.Border.Center;
            dir = Vector2.Zero;

            // check for vertical or horizontal spawn; 0 - horizontal; 1 - vertical
            if (Program.Random.Next(2) == 0)
            { // horizontal
                pos.X += Program.Random.NextFloat(-0.6f, 0.6f) * EntityManager.Border.Width;
                // check for top or bottom lane
                if (Program.Random.Next(2) == 0)
                { // top
                    pos.Y -= EntityManager.Border.Height * .75f;
                    dir = Vector2.Lerp(
                            EntityManager.Border.Center + new Vector2(-EntityManager.Border.Width, EntityManager.Border.Height) * .5f,
                            EntityManager.Border.Center + new Vector2(EntityManager.Border.Width, EntityManager.Border.Height) * .5f,
                            (float)Program.Random.NextDouble()
                        );
                }
                else
                { // bottom
                    pos.Y += EntityManager.Border.Height * .75f;
                    dir = Vector2.Lerp(
                            EntityManager.Border.Center - new Vector2(-EntityManager.Border.Width, EntityManager.Border.Height) * .5f,
                            EntityManager.Border.Center - new Vector2(EntityManager.Border.Width, EntityManager.Border.Height) * .5f,
                            (float)Program.Random.NextDouble()
                        );
                }
            }
            else
            { // vertical
                pos.Y += Program.Random.NextFloat(-0.6f, 0.6f) * EntityManager.Border.Height;
                // check for left or right lane
                if (Program.Random.Next(2) == 0)
                { // left
                    pos.X -= EntityManager.Border.Width * .75f;
                    dir = Vector2.Lerp(
                            EntityManager.Border.Center + new Vector2(EntityManager.Border.Width, -EntityManager.Border.Height) * .5f,
                            EntityManager.Border.Center + new Vector2(EntityManager.Border.Width, EntityManager.Border.Height) * .5f,
                            (float)Program.Random.NextDouble()
                        );
                } else
                { // right
                    pos.X -= EntityManager.Border.Width * .75f;
                    dir = Vector2.Lerp(
                            EntityManager.Border.Center - new Vector2(EntityManager.Border.Width, -EntityManager.Border.Height) * .5f,
                            EntityManager.Border.Center - new Vector2(EntityManager.Border.Width, EntityManager.Border.Height) * .5f,
                            (float)Program.Random.NextDouble()
                        );
                }
            }
            dir.Normalize();
            rot = Program.Random.NextFloat(-PI, PI);
        }
        
        private static void CalcEnemyShipSpawnData(out Vector2 pos, out float rot, out string type)
        {
            pos = EntityManager.Border.Center;
            do // try to calc a spawn position until range to player is at least 50m
            {
                pos.X += Program.Random.NextFloat(-0.75f, 0.75f) * EntityManager.Border.Width;
                pos.Y += Program.Random.NextFloat(-0.75f, 0.75f) * EntityManager.Border.Height;
            } while (Vector2.DistanceSquared(ConvertUnits.ToSimUnits(pos), EntityManager.PlayerShip.Body.Position) < 50);

            rot = (float)Math.Atan2(EntityManager.Border.Center.Y - pos.Y, EntityManager.Border.Center.X - pos.X);
            type = data.Enemies.Select(d => d.Type).OrderBy(t => Guid.NewGuid()).First();
        }

        private static float currentAsteroidSpawnTime
        {
            get { return MathHelper.Lerp(spawn.AsteroidSpawnTimerMin, spawn.AsteroidSpawnTimerMax, spawnRatio); }
        }

        private static float currentAsteroidSpawnCount
        {
            get { return MathHelper.Lerp(spawn.AsteroidSpawnCountMin, spawn.AsteroidSpawnCountMax, spawnRatio); }
        }

        private static float currentEnemySpawnTime
        {
            get { return MathHelper.Lerp(spawn.EnemySpawnTimerMin, spawn.EnemySpawnTimerMax, spawnRatio); }
        }

        private static float currentEnemySpawnCount
        {
            get { return MathHelper.Lerp(spawn.EnemySpawnCountMin, spawn.EnemySpawnCountMax, spawnRatio); }
        }

        
    }
}
