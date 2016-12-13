using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace StarrockGame.GUI.MenuElements
{
    public class ButtonImage : GUIElement, ISelectable
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
        public int Width { get; private set; }
        public int Height { get; private set; }
        public Texture2D Texture;
        public Color SelectionColor = Color.White;

        private Vector2 center;
        private static Texture2D borderTex;


        public ButtonImage(Menu menu, Texture2D image, string caption, Vector2 position, int width, int height, Color color, Action select=null) 
            : base(menu, position, 1, color)
        {
            Texture = image;
            Caption = caption;
            Width = width;
            Height = height;
            Select = select;

            
        }

        public override void Update(GameTime gameTime, bool selected)
        {
            IsSelected = selected;
        }

        public override void Render(SpriteBatch batch)
        {
            if (borderTex == null)
            {
                borderTex = new Texture2D(batch.GraphicsDevice, 1, 1);
                borderTex.SetData(new Color[] { Color.White });
            }

            // TODO: calculate aspect ratio of image and scale image, so it isn't blurred
            batch.Draw(Texture, new Rectangle((int)Position.X, (int)Position.Y, Width, Height - (int)(Menu.Font.LineSpacing*.5f)), Color.White);
            batch.DrawString(Menu.Font, Caption, Position+new Vector2(Width*.5f, Height + 2 - (int)(Menu.Font.LineSpacing * .5f)), Color, 0, center, 0.5f, SpriteEffects.None, 1);
            // draw border if selected
            if (IsSelected)
            {
                batch.Draw(borderTex, new Rectangle((int)Position.X, (int)Position.Y, Width, 1), SelectionColor);
                batch.Draw(borderTex, new Rectangle((int)Position.X, (int)Position.Y + Height - 1, Width, 1), SelectionColor);
                batch.Draw(borderTex, new Rectangle((int)Position.X, (int)Position.Y + 1, 1, Height - 2), SelectionColor);
                batch.Draw(borderTex, new Rectangle((int)Position.X + Width - 1, (int)Position.Y + 1, 1, Height - 2), SelectionColor);
            }
        }

        public void OnSelect()
        {
            Select?.Invoke();
        }
    }
}
