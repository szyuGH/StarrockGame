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
        public SessionEnemyData[] Enemies;
    }

    [Serializable]
    public struct SessionEnemyData
    {
        public string Type;
        public float Chance;
    }
}
