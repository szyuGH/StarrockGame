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
        }

        //Selects a random ship from all available ones, uses the dropchance to calculate if the blueprint contains the ship or not
        private void chooseBlueprint(TemplateType type)
        { 
            List<Tuple<string, float>> templates = Cache.Templates[type];
            Random rand = new Random();
            var k = templates[rand.Next(templates.Count)];

            if (k.Item2 > rand.NextDouble())
            {
                Name = k.Item1;
            }
        }
    }
}
