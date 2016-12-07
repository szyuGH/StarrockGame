using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StarrockGame.Entities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TData.TemplateData;

namespace StarrockGame.GUI
{
     public class IngameInterface
    {
        public Spaceship Ship{ get; private set; }
        private Gauge structureGauge;
        private Gauge energyGauge;
        private Gauge fuelGauge;
        private Gauge shieldGauge;
        private Radar radar;

        public IngameInterface(Spaceship entity)
        {
            Ship = entity;
            SpaceshipTemplateData shipTemplate = Ship.Template as SpaceshipTemplateData;
            structureGauge = new Gauge(shipTemplate.Structure, new Rectangle(10, 10, 200, 25), Color.Red);
            energyGauge = new Gauge(shipTemplate.Energy, new Rectangle(300, 10, 200, 25), Color.Green);
            fuelGauge = new Gauge(shipTemplate.Fuel, new Rectangle(600, 10, 200, 25), Color.Yellow);
            shieldGauge = new Gauge(shipTemplate.ShieldCapacity, new Rectangle(900, 10, 200, 25), Color.Blue);
            radar = new Radar(0, new Rectangle(20,750,50,150));
        }

        public void Update()
        {
            structureGauge.Value    = Ship.Structure;
            energyGauge.Value       = Ship.Energy;
            fuelGauge.Value         = Ship.Fuel;
            shieldGauge.Value = Ship.ShieldCapacity;
            radar.LivingThings = EntityManager.GetAllLiving();
        }

        public void Render(SpriteBatch sprite)
        { 
            structureGauge.Render(sprite);
            energyGauge.Render(sprite);
            fuelGauge.Render(sprite);
            shieldGauge.Render(sprite);
            radar.Render(sprite);

        }
    }
}
