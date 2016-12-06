using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
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
        public IEnumerable LivingThings;
        private Texture2D renderTex;
        public Color BorderColor = Color.White;
        public Color Color { get; private set; }
        public Rectangle Bounding { get; private set; }
        public int Difficulty { get; private set; }


        public Radar(int difficulty)
        {
            Difficulty = difficulty;
            //TODO: Display Entities in range, specific color for entity
            LivingThings = EntityManager.GetAllLiving();
        }

        public void Render(SpriteBatch batch)
        {
            if (renderTex == null)
            {
                renderTex = new Texture2D(batch.GraphicsDevice, 1, 1);
                renderTex.SetData(new Color[] { Color.White });
            }
            int width = 200;
            int BorderStrength = 4;
            Rectangle bgBounding = new Rectangle(Bounding.X, Bounding.Y, width, Bounding.Height);
            Rectangle fgBounding = new Rectangle(Bounding.X + BorderStrength, Bounding.Y + BorderStrength, width - BorderStrength, Bounding.Height - BorderStrength);

            batch.Draw(renderTex, bgBounding, BorderColor); // Background
            batch.Draw(renderTex, fgBounding, Color); // Foreground
            foreach (Entity entity in LivingThings)
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
                if (entity.GetType() == typeof(Wreckage))
                {
                    //Yellow
                }
            }
        }
    }
}