﻿using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using StarrockGame.Entities;
using FarseerPhysics.Dynamics;
using StarrockGame.AI;
using GPart;
using StarrockGame.ParticleSystems;
using Microsoft.Xna.Framework.Graphics;
using StarrockGame.Caching;
using FarseerPhysics;
using StarrockGame.GUI;
using System;

namespace StarrockGame.SceneManagement.Scenes
{
    public class SceneTestSession : Scene
    {
        Spaceship ship, ship2;
        Wreckage wreckage;
        Camera2D cam;
        IngameInterface ingameInterface;
        float asteroidSpawnTimer = 4;
        Background bg;

        

        public SceneTestSession(Game1 game) : base(game)
        {            
        }

        public override void Initialize()
        {
            EntityManager.Clear();
            ship = EntityManager.Add<Spaceship, PlayerController>("Spaceship", new Vector2(900, 350), (float)Math.PI * -.5f, Vector2.Zero);
            ship2 = EntityManager.Add<Spaceship, NoController>("BS_Caine", new Vector2(400, 100), (float)Math.PI * .5f, Vector2.Zero);
            wreckage = EntityManager.Add<Wreckage, NoController>("Wreckage",new Vector2(900, 500), 0, Vector2.Zero);

            cam = new Camera2D(Device);
            cam.TrackingBody = ship.Body;
            cam.Update();

            ingameInterface = new IngameInterface(Game.GraphicsDevice, ship);

            Line.Initialize(Device);
            EntityManager.Border = new GameBorder(EntityManager.World, Device, 2000, 2000);

            bg = new Background();
        }

        public override void Update(GameTime gameTime)
        {
            float elapsed = (float)gameTime.ElapsedGameTime.TotalSeconds;
            asteroidSpawnTimer -= elapsed;
            if (asteroidSpawnTimer <= 0)
            {
                asteroidSpawnTimer += MathHelper.Lerp(15,25, (float)Program.Random.NextDouble());
                for (int i=0;i<10;i++)
                SpawnAsteroid();
            }
            

            EntityManager.Update(gameTime);
            cam.Update();
            ingameInterface.Update();
        }

        public override void Render(GameTime gameTime)
        {
            base.Render(gameTime);

            bg.Render(SpriteBatch, gameTime, cam);


            SpriteBatch.Begin(SpriteSortMode.FrontToBack, BlendState.AlphaBlend, null, null, null, null, cam.Translation);
            
            EntityManager.Render(SpriteBatch, gameTime);
            SpriteBatch.End();

            SpriteBatch.Begin();
            ingameInterface.Render(SpriteBatch);
            SpriteBatch.End();

            Particles.SetCamera(cam.Translation, cam.Projection);
            EntityManager.Border.Render(SpriteBatch, gameTime, cam);
        }


        private void SpawnAsteroid()
        {
            float x, y;
            Vector2 min, max;
            int spawnType = Program.Random.Next(2);

            if (spawnType == 0)
            {
                //horizontal spawn
                int verticalAlign = Program.Random.Next(2);
                x = MathHelper.Lerp(-1f, 1f, (float)Program.Random.NextDouble()) * EntityManager.Border.Width + EntityManager.Border.Center.X;
                y = (verticalAlign == 0 ? -1f : 1f) * EntityManager.Border.Height + EntityManager.Border.Center.Y;
                if (verticalAlign == 0)
                {
                    min = new Vector2(EntityManager.Border.Center.X - EntityManager.Border.Width * .5f, EntityManager.Border.Center.Y - EntityManager.Border.Height * .5f);
                    max = new Vector2(EntityManager.Border.Center.X + EntityManager.Border.Width * .5f, EntityManager.Border.Center.Y - EntityManager.Border.Height * .5f);
                } else
                {
                    min = new Vector2(EntityManager.Border.Center.X - EntityManager.Border.Width * .5f, EntityManager.Border.Center.Y + EntityManager.Border.Height * .5f);
                    max = new Vector2(EntityManager.Border.Center.X + EntityManager.Border.Width * .5f, EntityManager.Border.Center.Y + EntityManager.Border.Height * .5f);
                }
                
            } else
            {
                // vertical spawn
                int horizontalAlign = Program.Random.Next(2);
                y = MathHelper.Lerp(-1f, 1f, (float)Program.Random.NextDouble()) * EntityManager.Border.Height + EntityManager.Border.Center.Y;
                x = (horizontalAlign == 0 ? -1f : 1f) * EntityManager.Border.Width + EntityManager.Border.Center.X;
                if (horizontalAlign == 0)
                {
                    min = new Vector2(EntityManager.Border.Center.X - EntityManager.Border.Width * .5f, EntityManager.Border.Center.Y - EntityManager.Border.Height * .5f);
                    max = new Vector2(EntityManager.Border.Center.X - EntityManager.Border.Width * .5f, EntityManager.Border.Center.Y + EntityManager.Border.Height * .5f);
                }
                else
                {
                    min = new Vector2(EntityManager.Border.Center.X + EntityManager.Border.Width * .5f, EntityManager.Border.Center.Y - EntityManager.Border.Height * .5f);
                    max = new Vector2(EntityManager.Border.Center.X + EntityManager.Border.Width * .5f, EntityManager.Border.Center.Y + EntityManager.Border.Height * .5f);
                }
            }
            Vector2 spawnPos = new Vector2(x, y);
            Vector2 dir = Vector2.Lerp(min, max, (float)Program.Random.NextDouble());
            dir.Normalize();
            float rot = MathHelper.Lerp(-(float)Math.PI, (float)Math.PI, (float)Program.Random.NextDouble());

            EntityManager.Add<Asteroid, NoController>("Asteroid", spawnPos, rot, -dir, 0);
        }
    }

}
