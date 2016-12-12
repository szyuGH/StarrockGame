using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StarrockGame.Caching;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StarrockGame
{
    public class AnimatedTexture
    {
        public Texture2D Atlas { get; private set; }
        public Vector2 TileCenter { get; set; }

        public float FramesPerSecond { get; set; } = 30;

        public int Columns { get; private set; }
        public int Rows { get; private set; }
        public int TileWidth { get; private set; }
        public int TileHeight { get; private set; }

        private int index;
        private float frameTimer;

        public AnimatedTexture(string graphicName, Vector2 tileCenter, int columns, int rows)
        {
            Atlas = Cache.LoadGraphic(graphicName);
            TileCenter = tileCenter;
            Columns = columns;
            Rows = rows;
            TileWidth = Atlas.Width / Columns;
            TileHeight = Atlas.Height / Rows;

            index = 0;
            frameTimer = 0;
        }

        public void Draw(SpriteBatch batch, GameTime gameTime, Rectangle rect, Color color, float rot, float layerDepth=1)
        {
            frameTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (frameTimer >= 1f / FramesPerSecond)
            {
                index = (index + 1) % (Rows * Columns);
                frameTimer -= 1f / FramesPerSecond;
            }
            Rectangle src = new Rectangle(
                TileWidth * (index % Columns),
                TileHeight * (index / Columns),
                TileWidth, TileHeight
                );
            batch.Draw(Atlas, rect, src, color, rot, TileCenter, SpriteEffects.None, layerDepth);
        }
    }
}
