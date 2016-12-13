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
                CalculateCenter();
            }
        }

        private int _alignment;
        public int Alignment
        {
            get { return _alignment; }
            set
            {
                _alignment = value;
                CalculateCenter();
            }
        }

        private Vector2 center;

        public Func<string> CaptionMonitor;

        public Label(Menu menu, string caption, Vector2 position, float size, Color color, int alignment = 1)
            :base(menu, position, size, color)
        {
            Caption = caption;
            Alignment = alignment;
        }


        public override void Update(GameTime gameTime, bool selected)
        {
        }

        public override void Render(SpriteBatch batch)
        {
            if (CaptionMonitor != null)
            {
                Caption = CaptionMonitor();
            }
            batch.DrawString(Menu.Font, Caption, Position, Color, 0, center, Size, SpriteEffects.None, 1);
        }


        private void CalculateCenter()
        {
            Vector2 measure = Menu.Font.MeasureString(_caption);
            center = new Vector2((Alignment * .5f) * measure.X, 0);
        }
    }
}
