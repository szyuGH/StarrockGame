using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StarrockGame.InputManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StarrockGame.GUI
{
    public class Menu
    {
        public List<GUIElement> Elements { get; private set; }
        public SpriteFont Font { get; set; }

        public int SelectedIndex { get; set; }

        private Action cancel;

        public Menu(SpriteFont font, Action onCancel)
        {
            Font = font;
            Elements = new List<GUIElement>();
            SelectedIndex = -1;
            cancel = onCancel;
        }

        public virtual void Update(GameTime gameTime)
        {
            if (Input.Device.MenuDown())
            {
                SelectNext();
            } else if (Input.Device.MenuUp())
            {
                SelectPrevious();
            }
            if (SelectedIndex != -1 && Elements[SelectedIndex] is ISelectable && Input.Device.MenuSelect())
            {
                (Elements[SelectedIndex] as ISelectable).OnSelect();
            } else if (Input.Device.MenuCancel())
            {
                cancel?.Invoke();
            }

            for (int i=0;i<Elements.Count;i++)
            {
                Elements[i].Update(gameTime, i == SelectedIndex);
            }
        }

        public virtual void Render(SpriteBatch batch)
        {
            foreach (GUIElement element in Elements)
            {
                element.Render(batch);
            }
        }

        public void SelectNext()
        {
            if (SelectedIndex == -1)
            {
                SelectedIndex = 0;
            }
            else if (Elements.Count > 1)
            {
                SelectedIndex += 1;
                if (SelectedIndex == Elements.Count)
                    SelectedIndex = 0;

                if (!(Elements[SelectedIndex] is ISelectable))
                    SelectNext();
            }
        }

        public void SelectPrevious()
        {
            if (SelectedIndex == -1)
            {
                SelectedIndex = Elements.Count - 1;
            }
            else if (Elements.Count > 1)
            {
                SelectedIndex -= 1;
                if (SelectedIndex == -1)
                    SelectedIndex = Elements.Count-1;

                if (!(Elements[SelectedIndex] is ISelectable))
                    SelectPrevious();
            }
        }

        public void Clear()
        {
            Elements.Clear();
            SelectedIndex = -1;
        }

        public static Menu operator +(Menu menu, GUIElement element)
        {
            menu.Elements.Add(element);
            return menu;
        }

        public static Menu operator -(Menu menu, GUIElement element)
        {
            menu.Elements.Remove(element);
            return menu;
        }
    }
}
