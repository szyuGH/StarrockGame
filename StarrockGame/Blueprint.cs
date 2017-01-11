using StarrockGame.Caching;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TData;

namespace StarrockGame
{
    public class Blueprint
    { 
        public string Name;

        public Blueprint(TemplateType type)
        { 
            chooseBlueprint(type);
            Name = "Hunter";
        }

        private void chooseBlueprint(TemplateType type)
        {
            //Zwischen modules und spaceships unterscheiden
            Dictionary<TemplateType, List<string>> templates = new Dictionary<TemplateType, List<string>>();
            templates = Cache.Templates;
            Random rand = new Random();
            var k = templates.Keys.ToList()[rand.Next(templates.Count)];
            //select dropchance des elements, -> rand im bereich [0;1] , if rand > dropchance -> Name = select name
        }
    }
}
