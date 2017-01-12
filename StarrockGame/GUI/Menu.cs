using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StarrockGame.Audio;
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
        public bool NotSelectable;

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
                    Sound.Instance.PlaySe("Cursor");
                    SelectNext();
                }
                else if (Input.Device.MenuUp())
                {
                    Sound.Instance.PlaySe("Cursor");
                    SelectPrevious();
                }
                if (SelectedIndex != -1 && Elements[SelectedIndex] is ISelectable && Input.Device.MenuSelect())
                {
                    Sound.Instance.PlaySe("Decision");
                    IgnoreNextInput = true;
                    (Elements[SelectedIndex] as ISelectable).OnSelect();
                }
                else if (Input.Device.MenuCancel())
                {
                    if (Cancel != null)
                    {
                        Sound.Instance.PlaySe("Cancel");
                        IgnoreNextInput = true;
                        Cancel.Invoke();
                    }
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
            if (!NotSelectable && Elements.Where(e => e is ISelectable).Count() > 0)
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
            if (!NotSelectable && Elements.Where(e => e is ISelectable).Count() > 0)
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
