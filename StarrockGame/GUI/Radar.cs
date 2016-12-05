using StarrockGame.Entities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StarrockGame.GUI
{
    public class Radar
    {
        private IEnumerable livingThings;
        public Radar()
        {
            //TODO: Display Entities in range, specific color for entity
            livingThings = EntityManager.GetAllLiving();
        }
        
        public void Update()
        {
            livingThings = EntityManager.GetAllLiving();
            foreach (Entity entity in livingThings)
            {
                Draw(entity);
            }
        }

        public void Draw(Entity entity)
        {
            if (entity.GetType() == typeof(Asteroid))
            {
                //Grey
            }
            if (entity.GetType() == typeof(Spaceship))
                {
                    if ((entity as Spaceship).IsPlayer())
                    {
                        //Green
                    } 
                    else
                    {
                        //Red
                    }
                }
            //TODO: Wreckages
        }
    }
}