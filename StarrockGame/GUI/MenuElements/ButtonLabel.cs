using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StarrockGame.GUI
{
    public class ButtonLabel : GUIElement, ISelectable
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
        public bool IsSelected { get; private set; }
        public Action Select { get; private set; }
        public float PulseSpeed = 2;
        public float PulseRestoreSpeed = 10;
        public float PulseSize = 1.4f;
        public Func<string> CaptionMonitor;
        

        private Vector2 center;
        

        private float selectedSize;
        private float timer;

        public ButtonLabel(Menu menu, string caption, Vector2 position, float size, Color color, Action onSelect, int alignment=1)
            :base(menu, position, size, color)
        {
            Caption = caption;
            Select = onSelect;
            Alignment = alignment;
        }
        

        public override void Update(GameTime gameTime, bool isSelected)
        {
            float elapsed = (float)gameTime.ElapsedGameTime.TotalSeconds;
            IsSelected = isSelected;
            if (isSelected)
            {
                timer += elapsed * PulseSpeed;
                if (timer > Math.PI)
                    timer -= (float)Math.PI;
                selectedSize = Size * MathHelper.Lerp(1, PulseSize, (float)Math.Sin(timer));
            } else
            {
                selectedSize = MathHelper.Lerp(selectedSize, Size, elapsed * PulseRestoreSpeed);
                timer = 0;
            }
        }

        public override void Render(SpriteBatch batch)
        {
            if (CaptionMonitor != null)
            {
                Caption = CaptionMonitor();
            }
            batch.DrawString(Menu.Font, Caption, Position, Active ? Color : Color.Gray, 0, center, selectedSize, SpriteEffects.None, 1);
        }

        public void OnSelect()
        {
            Select?.Invoke();
        }

        private void CalculateCenter()
        {
            Vector2 measure = Menu.Font.MeasureString(_caption);
            center = new Vector2((Alignment * .5f) * measure.X, measure.Y*.5f);
        }
    }
}
