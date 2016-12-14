using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StarrockGame.Caching;
using StarrockGame.Entities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StarrockGame.GUI
{
    public class Radar
    {
        const int DOT_SIZE = 3;

        private Texture2D shape;

        public List<Entity> LivingThings;
        private Texture2D renderTex;
        public Color BorderColor = Color.White;
        public Color Color { get; private set; }
        public Rectangle Bounding { get; private set; }
        public SessionDifficulty Difficulty { get; private set; }
        public Spaceship PlayerShip { get; private set; }


        public Radar(Spaceship player, SessionDifficulty difficulty, Rectangle bounding)
        {
            PlayerShip = player;
            shape = Cache.LoadGraphic("RadarShape");
            Difficulty = difficulty;
            Bounding = bounding;
            LivingThings = EntityManager.GetAllEntities(player, player.RadarRange);
        }

        public void Render(SpriteBatch batch)
        {
            if (renderTex == null)
            {
                renderTex = new Texture2D(batch.GraphicsDevice, DOT_SIZE, DOT_SIZE);
                Color[] dotColors = new Color[DOT_SIZE * DOT_SIZE];
                for (int i = 0; i < dotColors.Length; i++)
                    dotColors[i] = Color.White;
                renderTex.SetData(dotColors);
            }
            
            batch.Draw(shape, 
                Bounding,
                Color.White * .5f);


            Vector2 radarCenter = new Vector2(Bounding.Center.X, Bounding.Center.Y);
            foreach (Entity entity in LivingThings)
            {
                float distanceToPlayer = Vector2.DistanceSquared(entity.Body.Position, PlayerShip.Body.Position);
                if (distanceToPlayer <= PlayerShip.RadarRange)
                {
                    Vector2 relativePosition = entity.Body.Position - PlayerShip.Body.Position;
                    relativePosition.Normalize();
                    relativePosition = relativePosition * (distanceToPlayer / PlayerShip.RadarRange) * (Bounding.Width * .5f);

                    if (Difficulty == SessionDifficulty.Easy && entity.GetType() == typeof(Asteroid))
                    {
                        DrawRadarDot(batch, radarCenter + relativePosition, Color.Gray * .85f);
                    }
                    else if (entity.GetType() == typeof(Spaceship))
                    {
                        if ((entity as Spaceship).IsPlayer)
                        {
                            DrawRadarDot(batch, radarCenter, Color.Green * .85f);
                        }
                        else if (Difficulty == SessionDifficulty.Easy || Difficulty == SessionDifficulty.Medium)
                        {
                            DrawRadarDot(batch, radarCenter + relativePosition, Color.Red * .85f);
                        }
                    }
                    else if (entity.GetType() == typeof(Wreckage))
                    {
                        DrawRadarDot(batch, radarCenter + relativePosition, Color.Yellow * .85f);
                    }
                }
            }
        }

        private void DrawRadarDot(SpriteBatch batch, Vector2 pos, Color color)
        {
            batch.Draw(renderTex, pos, null, color,
                0, new Vector2(renderTex.Width * .5f, renderTex.Height * .5f), 1, SpriteEffects.None, 1);
        }
    }
}