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
                center = Menu.Font.MeasureString(_caption) * .5f;
            }
        }
        public bool IsSelected { get; private set; }
        public Action Select { get; private set; }
        public float PulseSpeed = 2;
        public float PulseRestoreSpeed = 10;
        public float PulseSize = 1.4f;

        private Vector2 center;
        

        private float selectedSize;
        private float timer;

        public ButtonLabel(Menu menu, string caption, Vector2 position, float size, Color color, Action onSelect)
            :base(menu, position, size, color)
        {
            Caption = caption;
            Select = onSelect;
        }
        

        public override void Update(float elapsed, bool isSelected)
        {

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
            batch.DrawString(Menu.Font, Caption, Position, Color, 0, center, selectedSize, SpriteEffects.None, 1);
        }

        public void OnSelect()
        {
            Select?.Invoke();
        }
    }
}
