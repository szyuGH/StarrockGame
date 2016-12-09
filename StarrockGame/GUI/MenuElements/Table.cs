using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace StarrockGame.GUI
{
    public class Table
    {
        public List<Tuple<string, int>> Columns;
        public List<Dictionary<string, string>> Data;

        public SpriteFont Font { get; set; }
        public Rectangle Bounding { get; set; }
        public Color LineColor = Color.White;
        public int WidthBuffer = 40;
        public Color[] ShadeColors = new Color[3] { new Color(15,15,15,255), new Color(30,30,30,255), Color.Black };

        public int RealHeight
        {
            get
            {
                return (1 + Bounding.Height) * Font.LineSpacing + 10;
            }
        }

        private Texture2D lineTex;
        private int scrollValue = 0;

        public Table(SpriteFont font, Rectangle bounding)
        {
            Font = font;
            Bounding = bounding;
            Data = new List<Dictionary<string, string>>();
            Columns = new List<Tuple<string, int>>();
        }

        public Table AddColumn(string header, int width)
        {
            Columns.Add(new Tuple<string, int>(header, width));
            ResizeBounding();
            return this;
        }
        public Table AddColumn(string header)
        {
            return AddColumn(header, (int)Font.MeasureString(header).X + WidthBuffer);
        }

        public void Render(SpriteBatch batch)
        {
            #region setup line texture
            if (lineTex == null) {
                lineTex = new Texture2D(batch.GraphicsDevice, 1, 1);
                lineTex.SetData(new Color[] { Color.White });
            }
            #endregion

            int verticalOffset = Bounding.Y + Font.LineSpacing + 10;

            #region draw grid

            // preshade rows
            batch.Draw(lineTex, new Rectangle(Bounding.X, Bounding.Y, Bounding.Width, Font.LineSpacing + 10), ShadeColors[0]); // Header Shader
            for (int i = 0;i < Bounding.Height;i++)
                batch.Draw(lineTex, new Rectangle(Bounding.X, verticalOffset + (i - scrollValue) * Font.LineSpacing, Bounding.Width, Font.LineSpacing), ShadeColors[1 + i % 2]); // Entry Shader

            // draw main outlines
            batch.Draw(lineTex, new Rectangle(Bounding.X, Bounding.Y, 1, RealHeight), LineColor);
            batch.Draw(lineTex, new Rectangle(Bounding.X, Bounding.Y, Bounding.Width, 1), LineColor);
            batch.Draw(lineTex, new Rectangle(Bounding.X, Bounding.Y + RealHeight, Bounding.Width, 1), LineColor);

            int dx = Bounding.X;
            foreach (Tuple<string, int> column in Columns)
            {

                batch.Draw(lineTex, new Rectangle(dx + column.Item2, Bounding.Y, 1, RealHeight), LineColor);
                
                batch.DrawString(Font, column.Item1, new Vector2(dx + 4, Bounding.Y + 2), Color.White);
                dx += column.Item2;
            }
            batch.Draw(lineTex, new Rectangle(Bounding.X, Bounding.Y + Font.LineSpacing, Bounding.Width, 1), LineColor);
            batch.Draw(lineTex, new Rectangle(Bounding.X, Bounding.Y + Font.LineSpacing + 4, Bounding.Width, 1), LineColor);

            #endregion

           

            for (int i = scrollValue; i < Math.Min(Data.Count, scrollValue+Bounding.Height); i++)
            {
                dx = Bounding.X;
                foreach (Tuple<string, int> header in Columns)
                {
                    string text = "";
                    if (Data[i].ContainsKey(header.Item1))
                        text = Data[i][header.Item1];

                    batch.DrawString(Font, text, new Vector2(dx + 4, verticalOffset + (i-scrollValue) * Font.LineSpacing + 2), Color.White);
                    dx += header.Item2;
                }
            }
        }

        public void ScrollDown()
        {
            scrollValue++;
            if (scrollValue > Math.Max(0, Data.Count - Bounding.Height))
                scrollValue = Math.Max(0, Data.Count - Bounding.Height);
        }

        public void ScrollUp()
        {
            scrollValue--;
            if (scrollValue < 0)
                scrollValue = 0;
        }

        private void ResizeBounding()
        {
            int newWidth = 0;
            foreach (Tuple<string, int> header in Columns)
            {
                newWidth += header.Item2;
            }
            Bounding = new Rectangle(
                Bounding.X, 
                Bounding.Y,
                newWidth, 
                Bounding.Height);
        }

        public void Move(int x, int y)
        {
            Bounding = new Rectangle(x, y, Bounding.Width, Bounding.Height);
            
        }

        
    }
}
