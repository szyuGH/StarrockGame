using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StarrockGame.Entities;
using StarrockGame.InputManagement;
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
        private Gauge scavengeGauge;
        private Radar radar;

        private SessionDifficulty difficulty;

        private Label scoreLabel;
        private Label creditsLabel;
        private Label timeLabel;

        private bool showingStats;

        public IngameInterface(GraphicsDevice device, Spaceship entity, SessionDifficulty difficulty=SessionDifficulty.Easy)
        {
            Ship = entity;
            this.difficulty = difficulty;
            SpaceshipTemplate shipTemplate = Ship.Template as SpaceshipTemplate;


            const int X_OFFSET = 20;
            const int Y_OFFSET = 20;
            int realGaugeWidth = (device.Viewport.Width-5*X_OFFSET) / (4);
            const int GAUGE_HEIGHT = 12;
            const int RADAR_SIZE = 150;
            const int SCAVENGE_WIDTH = 96;
            const int SCAVENGE_HEIGHT = 8;

            structureGauge = new Gauge(shipTemplate.Structure,      new Rectangle(X_OFFSET + (X_OFFSET + realGaugeWidth) * 0, Y_OFFSET,  realGaugeWidth, GAUGE_HEIGHT), Color.Red);
            energyGauge = new Gauge(shipTemplate.Energy,            new Rectangle(X_OFFSET + (X_OFFSET + realGaugeWidth) * 1, Y_OFFSET, realGaugeWidth, GAUGE_HEIGHT), Color.Green);
            fuelGauge = new Gauge(shipTemplate.Fuel,                new Rectangle(X_OFFSET + (X_OFFSET + realGaugeWidth) * 2, Y_OFFSET, realGaugeWidth, GAUGE_HEIGHT), Color.Yellow);
            shieldGauge = new Gauge(shipTemplate.ShieldCapacity,    new Rectangle(X_OFFSET + (X_OFFSET + realGaugeWidth) * 3, Y_OFFSET, realGaugeWidth, GAUGE_HEIGHT), Color.Blue);
            scavengeGauge = new Gauge(1f,                         new Rectangle((device.Viewport.Width - SCAVENGE_WIDTH) / 2, device.Viewport.Height/2+ 80, SCAVENGE_WIDTH, SCAVENGE_HEIGHT), Color.Azure);

            if (difficulty != SessionDifficulty.Lost)
                radar = new Radar(entity, 0, new Rectangle(X_OFFSET,device.Viewport.Height - RADAR_SIZE - Y_OFFSET,RADAR_SIZE,RADAR_SIZE));

            timeLabel = new Label(null, "", new Vector2(X_OFFSET, radar.Bounding.Y - 3 * 24), 1, Color.White, 0) { Visible = false, CaptionMonitor = () => { return string.Format("Elapsed Time: {0:hh\\:mm\\:ss}", SessionManager.ElapsedTime); } };
            scoreLabel = new Label(null, "", new Vector2(X_OFFSET, radar.Bounding.Y - 2 * 24), 1, Color.White, 0) { Visible = false, CaptionMonitor = () => { return string.Format("Score: {0}", SessionManager.Score); } };
            //creditsLabel = new Label(null, "", new Vector2(), 1, Color.White, 2) { Visible = false, CaptionMonitor = () => { return string.Format("Credits: {0}", CREDITS); } };
        }

        public void Update()
        {
            structureGauge.Value    = Ship.Structure;
            energyGauge.Value       = Ship.Energy;
            fuelGauge.Value         = Ship.Fuel;
            shieldGauge.Value = Ship.ShieldCapacity;
            if (difficulty != SessionDifficulty.Lost)
                radar.LivingThings = EntityManager.GetAllEntities(Ship, Ship.RadarRange);

            if (Ship.Scavenging.Active)
                scavengeGauge.Value = Ship.Scavenging.Progress;

            showingStats = Input.Device.ShowingStats();
        }

        public void Render(SpriteBatch batch)
        { 
            structureGauge.Render(batch);
            energyGauge.Render(batch);
            fuelGauge.Render(batch);
            shieldGauge.Render(batch);
            if (difficulty != SessionDifficulty.Lost)
                radar.Render(batch);
            if (Ship.Scavenging.Active)
            {
                scavengeGauge.Render(batch);
            }

            if (showingStats)
            {
                timeLabel.Render(batch);
                scoreLabel.Render(batch);
                //creditsLabel.Render(batch);
            }
        }
    }
}
