using System.Collections.Generic;
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
using StarrockGame.InputManagement;
using StarrockGame.SceneManagement.Popups;
using TData.TemplateData;

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
            SessionManager.Reset();
            SessionManager.ModuleTemplates.Add(Cache.LoadTemplate<ModuleTemplate>("FuelCapacityboost"));
            EntityManager.Clear();
            ship = EntityManager.Add<Spaceship, PlayerController>("Spaceship", new Vector2(0, 0), (float)Math.PI * -.35f, Vector2.Zero,0, true);
            ship2 = EntityManager.Add<Spaceship, NoController>("BS_Caine", new Vector2(400, 100), (float)Math.PI * .5f, Vector2.Zero);
            wreckage = EntityManager.Add<Wreckage, NoController>("SpaceshipWreckage",new Vector2(0, 100), 0, Vector2.Zero);
            ship.Target = ship2;

            cam = new Camera2D(Device);
            cam.TrackingBody = ship.Body;
            cam.Update();
            

            ingameInterface = new IngameInterface(Game.GraphicsDevice, ship);

            Line.Initialize(Device);
            EntityManager.Border = new GameBorder(EntityManager.World, Device, 2000, 2000);

            bg = new Background();
            SessionManager.ElapsedTime = TimeSpan.FromSeconds(0);
            SessionManager.Score = 0;
        }

        public override void Update(GameTime gameTime)
        {
            if (ship.IsAlive == false)
            {
                SceneManager.CallPopup<PopupGameover>();
            }

            SessionManager.ElapsedTime += gameTime.ElapsedGameTime;
            float elapsed = (float)gameTime.ElapsedGameTime.TotalSeconds;
            asteroidSpawnTimer -= elapsed;
            if (asteroidSpawnTimer <= 0)
            {
                asteroidSpawnTimer += MathHelper.Lerp(15,25, (float)Program.Random.NextDouble());
                for (int i=0;i<10;i++)
                SpawnAsteroid();
            }
            
            if (Input.Device.OpenMenu())
            {
                SceneManager.CallPopup<PopupPause>();
            }

            EntityManager.Update(gameTime);
            cam.Update();
            ingameInterface.Update();
        }

        public override void Render(GameTime gameTime)
        {
            base.Render(gameTime);

            bg.Render(SpriteBatch, gameTime, cam);


            SpriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, null, null, null, EntityManager.OutlineEffect, cam.Translation);
            
            EntityManager.Render(SpriteBatch, gameTime);
            SpriteBatch.End();

            SpriteBatch.Begin();
            ingameInterface.Render(SpriteBatch);
            SpriteBatch.End();

            Particles.SetCamera(cam.Translation, cam.Projection, cam.Zoom);
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
