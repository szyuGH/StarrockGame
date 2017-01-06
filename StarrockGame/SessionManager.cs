using Microsoft.Xna.Framework;
using StarrockGame.Caching;
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
        internal static SessionDifficulty Difficulty { get; set; }
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

        public static void Initialize()
        {
            data = Cache.LoadTemplate<SessionData>("Sessions");
        }

        public static void Reset()
        {
            ElapsedTime = TimeSpan.FromSeconds(0);
            LastUsedShipTemplate = UsedShipTemplate;
            UsedShipTemplate = null;
            Score = 0;
            ModuleTemplates.Clear();
        }

        internal static void Update(GameTime gameTime)
        {
            float elapsed = (float)gameTime.ElapsedGameTime.TotalSeconds;
            ElapsedTime += gameTime.ElapsedGameTime;
            if (spawnRatio < 1)
            {
                spawnRatio = (float)ElapsedTime.TotalSeconds / data.Spawns[(int)Difficulty].EstimatedDuration;
                if (spawnRatio > 1)
                    spawnRatio = 1;
            }

            asteroidSpawnTimer += elapsed;
            enemySpawnTimer += elapsed;

            SpawnAsteroids();
            SpawnEnemyShips();
            SpawnBoss();
        }


        
        private static void SpawnAsteroids()
        {

        }

        private static void SpawnEnemyShips()
        {
            if (enemySpawnTimer >= currentEnemySpawnTime)
            {
                // TODO: spawn
                enemySpawnTimer -= currentEnemySpawnTime;
            }
        }

        private static void SpawnBoss()
        {

        }




        private static float currentEnemySpawnTime
        {
            get { return MathHelper.Lerp(spawn.EnemySpawnTimerMin, spawn.EnemySpawnTimerMax, spawnRatio); }
        }
    }
}
