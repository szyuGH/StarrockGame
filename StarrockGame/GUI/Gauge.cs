using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StarrockGame.Caching;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StarrockGame.GUI
{
    public unsafe class Gauge
    {
        private Texture2D renderTex;

        public Color BorderColor = Color.White;
        public Color Color { get; private set; }
        public Rectangle Bounding { get; private set; }

        private float _value;
        public  float Value
        {
            get { return _value; }
            set
            {
                _value = value;
            }
        }
        public float MaxValue { get; private set; }

        public int BorderStrength = 2;


        public Gauge(float maxValue, Rectangle bounding, Color color)
        {
            MaxValue = maxValue;
            Bounding = bounding;
            Color = color;

            
        }
        
        
        public void Render(SpriteBatch batch)
        {
            if (Value > 0)
            {
                if (renderTex == null)
                {
                    renderTex = new Texture2D(batch.GraphicsDevice, 1, 1);
                    renderTex.SetData(new Color[] { Color.White });
                }

                int width = (int)Math.Ceiling(((Value) / MaxValue) * Bounding.Width);
                Rectangle bgBounding = new Rectangle(Bounding.X, Bounding.Y, width, Bounding.Height);
                Rectangle fgBounding = new Rectangle(Bounding.X + BorderStrength, Bounding.Y + BorderStrength, width - 2 * BorderStrength, Bounding.Height - 2 * BorderStrength);

                batch.Draw(renderTex, bgBounding, BorderColor); // Background
                batch.Draw(renderTex, fgBounding, Color); // Foreground
            }
        }
    }
}
