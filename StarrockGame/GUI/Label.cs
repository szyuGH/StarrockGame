using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;

namespace StarrockGame.GUI
{
    public class Label : GUIElement
    {
        private string _caption;
        public string Caption
        {
            get { return _caption; }
            set
            {
                _caption = value;
                center = Menu.Font.MeasureString(_caption) * .5f;
            }
        }

        private Vector2 center;

        public Label(Menu menu, string caption, Vector2 position, float size, Color color)
            :base(menu, position, size, color)
        {
            Caption = caption;
        }


        public override void Update(float elapsed, bool selected)
        {
        }

        public override void Render(SpriteBatch batch)
        {
            batch.DrawString(Menu.Font, Caption, Position, Color, 0, center, Size, SpriteEffects.None, 1);
        }
    }
}
