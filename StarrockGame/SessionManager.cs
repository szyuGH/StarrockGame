using System;
using System.Collections.Generic;
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
    }
}
