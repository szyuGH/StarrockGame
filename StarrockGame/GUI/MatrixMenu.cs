using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using StarrockGame.InputManagement;
using StarrockGame.GUI.MenuElements;
using StarrockGame.Audio;

namespace StarrockGame.GUI
{
    public class MatrixMenu : Menu
    {
        public int Columns { get; private set; }
        public Vector2 Position { get; private set; }
        public int TileWidth { get; private set; }
        public int TileHeight { get; private set; }
        public int TileSpace { get; private set; }

        public MatrixMenu(SpriteFont font, Vector2 position, int columns, int tileWidth, int tileHeight, int tileSpace, Action onCancel) 
            : base(font, onCancel)
        {
            Columns = columns;
            Position = position;
            TileWidth = tileWidth;
            TileHeight = tileHeight;
            TileSpace = tileSpace;
        }

        public override void Update(GameTime gameTime)
        {
            if (IsActive && !IgnoreNextInput)
            {
                if (Input.Device.MenuDown())
                {
                    Sound.Instance.PlaySe("Cursor");
                    SelectDown();
                }
                else if (Input.Device.MenuUp())
                {
                    Sound.Instance.PlaySe("Cursor");
                    SelectUp();
                }
                if (Input.Device.MenuLeft())
                {
                    Sound.Instance.PlaySe("Cursor");
                    SelectLeft();
                }
                else if (Input.Device.MenuRight())
                {
                    Sound.Instance.PlaySe("Cursor");
                    SelectRight();
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
                        Sound.Instance.PlaySe("Cursor");
                        IgnoreNextInput = true;
                        Cancel.Invoke();
                    }
                }
            }
            for (int i = 0; i < Elements.Count; i++)
            {
                Elements[i].Update(gameTime, i == SelectedIndex && (IsActive || Elements[i] is ButtonImage));
            }
        }

        private void SelectRight()
        {
            if (Elements.Count > 0)
            {
                if (SelectedIndex == -1)
                    SelectedIndex = 0;
                else if (Elements.Count > 1)
                {
                    if ((SelectedIndex + 1) % Columns == 0 || SelectedIndex + 1 == Elements.Count)
                        SelectedIndex = (SelectedIndex / Columns) * Columns;
                    else
                        SelectedIndex++;
                }
            }
        }

        private void SelectLeft()
        {
            if (Elements.Count > 0)
            {
                if (SelectedIndex == -1)
                    SelectedIndex = Math.Min(Columns, Elements.Count) - 1;
                else if (Elements.Count > 1)
                {
                    if (SelectedIndex % Columns == 0)
                        SelectedIndex = Math.Min(SelectedIndex + Columns, Elements.Count) - 1;
                    else
                        SelectedIndex--;
                }
            }
        }

        private void SelectDown()
        {
            if (Elements.Count > 0)
            {
                if (SelectedIndex == -1)
                    SelectedIndex = 0;
                else if (Elements.Count >= Columns)
                {
                    if ((SelectedIndex + Columns) >= Elements.Count)
                        SelectedIndex -= (SelectedIndex / Columns) * Columns;
                    else
                        SelectedIndex += Columns;
                }
            }
        }

        private void SelectUp()
        {
            if (Elements.Count > 0)
            {
                if (SelectedIndex == -1)
                    SelectedIndex = Elements.Count - 1;
                else if (Elements.Count >= Columns)
                {
                    if ((SelectedIndex - Columns) < 0)
                    {
                        SelectedIndex += ((Elements.Count - 1) / Columns) * Columns;
                        if ((SelectedIndex + Columns) >= Elements.Count)
                            SelectedIndex -= Columns;
                    }
                    else
                        SelectedIndex -= Columns;
                }
            }
        }

        public GUIElement AddButtonImage(Texture2D img, string caption, Color color, Action selectedAction)
        {
            int index = Elements.Count;
            int ix = index % Columns;
            int iy = index / Columns;
            return new ButtonImage(this, img, caption,
                new Vector2(
                    Position.X + (TileWidth + TileSpace) * ix,
                    Position.Y + (TileHeight + TileSpace) * iy),
                TileWidth, TileHeight, color, selectedAction);
        }

        public GUIElement AddButtonLabel(string caption, Color color, Action selectedAction)
        {
            int index = Elements.Count;
            int ix = index % Columns;
            int iy = index / Columns;
            return new ButtonLabel(this, caption,
                new Vector2(
                    Position.X + (TileWidth + TileSpace) * ix,
                    Position.Y + (TileHeight + TileSpace) * iy),
                0.75f, color, selectedAction);
        }
    }
}
