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
        public bool IsActive { get; set; }
        public static bool IgnoreNextInput;

        protected Action Cancel;

        public Menu(SpriteFont font, Action onCancel)
        {
            Font = font;
            Elements = new List<GUIElement>();
            SelectedIndex = -1;
            Cancel = onCancel;
            IsActive = true;
        }

        public void Dispose()
        {
            Elements.Clear();
        }

        public virtual void Update(GameTime gameTime)
        {
            if (IsActive && !IgnoreNextInput)
            {
                if (Input.Device.MenuDown())
                {
                    SelectNext();
                }
                else if (Input.Device.MenuUp())
                {
                    SelectPrevious();
                }
                if (SelectedIndex != -1 && Elements[SelectedIndex] is ISelectable && Input.Device.MenuSelect())
                {
                    IgnoreNextInput = true;
                    (Elements[SelectedIndex] as ISelectable).OnSelect();
                }
                else if (Input.Device.MenuCancel())
                {
                    IgnoreNextInput = true;
                    Cancel?.Invoke();
                }
            }
            for (int i=0;i<Elements.Count;i++)
            {
                Elements[i].Update(gameTime, i == SelectedIndex && IsActive);
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
            if (Elements.Count > 0)
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
                }
                if (!(Elements[SelectedIndex] is ISelectable) || !Elements[SelectedIndex].Active)
                    SelectNext();
            }
        }

        public void SelectPrevious()
        {
            if (Elements.Count > 0)
            {
                if (SelectedIndex == -1)
                {
                    SelectedIndex = Elements.Count - 1;
                }
                else if (Elements.Count > 1)
                {
                    SelectedIndex -= 1;
                    if (SelectedIndex == -1)
                        SelectedIndex = Elements.Count - 1;

                }
                if (!(Elements[SelectedIndex] is ISelectable) || !Elements[SelectedIndex].Active)
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
