using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TData.TemplateData
{
    [Serializable]
    public class SessionData
    {

        public SpawnData[] Spawns;
        public SessionEnemyData[] Enemies;
    }

    [Serializable]
    public struct SpawnData
    {
        public int Difficulty;
        public float EstimatedDuration;

        public int EnemySpawnCountMin;
        public int EnemySpawnCountMax;

        public float EnemySpawnTimerMin;
        public float EnemySpawnTimerMax;

        public int AsteroidSpawnCountMin;
        public int AsteroidSpawnCountMax;

        public float AsteroidSpawnTimerMin;
        public float AsteroidSpawnTimerMax;
    }

    [Serializable]
    public struct SessionEnemyData
    {
        public string Type;
        public float Chance;
    }
}
