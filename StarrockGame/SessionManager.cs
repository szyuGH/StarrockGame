using Microsoft.Xna.Framework;
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
            ElapsedTime += gameTime.ElapsedGameTime;
        }



        private static void SpawnAsteroids()
        {

        }

        private static void SpawnEnemyShips()
        {

        }

        private static void SpawnBoss()
        {

        }
    }
}
