using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TData.TemplateData;

namespace StarrockGame
{
    [Serializable]
    public struct PlayerData
    {
        public int Credits;
        public string PlayerName;
        public TemplateData[] UnlockedTemplates;
    }
}
