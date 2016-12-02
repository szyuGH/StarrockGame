using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StarrockGame.GUI
{
    public abstract class GUIElement
    {
        public Vector2 Position { get; private set; }
        public float Size { get; private set; }
        public Color Color { get; private set; }
        public Menu Menu { get; private set; }

        public GUIElement(Menu menu, Vector2 position, float size, Color color)
        {
            Menu = menu;
            Menu += this;

            Position = position;
            Size = size;
            Color = color;
        }
        ~GUIElement()
        {
            Menu -= this;
        }

        public abstract void Update(GameTime gameTime, bool selected);
        public abstract void Render(SpriteBatch batch);
    }
}
