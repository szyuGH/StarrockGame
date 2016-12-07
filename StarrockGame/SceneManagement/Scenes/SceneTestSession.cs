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

namespace StarrockGame.SceneManagement.Scenes
{
    public class SceneTestSession : Scene
    {
        Spaceship ship, ship2;
        Wreckage wreckage;
        Camera2D cam;
        IngameInterface ingameInterface;
        GameBorder border;
        

        public SceneTestSession(Game1 game) : base(game)
        {            
        }

        public override void Initialize()
        {
            EntityManager.Clear();
            ship = EntityManager.Add<Spaceship, PlayerController>("Spaceship", new Vector2(200, 200), 0, Vector2.Zero);
            ship2 = EntityManager.Add<Spaceship, NoController>("Spaceship", new Vector2(400, 300), 10, Vector2.Zero);
            wreckage = EntityManager.Add<Wreckage, NoController>("Wreckage",new Vector2(400, 200), 0, Vector2.Zero);


            cam = new Camera2D(Device);
            cam.TrackingBody = ship.Body;
            cam.Update();

            ingameInterface = new IngameInterface(Game.GraphicsDevice, ship);

            Line.Initialize(Device);
            border = new GameBorder(EntityManager.World, Device, 8000, 3000);
        }

        public override void Update(GameTime gameTime)
        {
            float elapsed = (float)gameTime.ElapsedGameTime.TotalSeconds;
            
            EntityManager.Update(gameTime);
            cam.Update();
            ingameInterface.Update();

            Particles.Emit<TrailParticleSystem>(new Vector2(400, 400), new Vector2(0, 500));
        }

        public override void Render(GameTime gameTime)
        {
            base.Render(gameTime);

            border.Render(SpriteBatch, gameTime, cam);

            SpriteBatch.Begin(SpriteSortMode.FrontToBack, BlendState.AlphaBlend, null, null, null, null, cam.Translation);
            EntityManager.Render(SpriteBatch, gameTime);
            SpriteBatch.End();

            SpriteBatch.Begin();
            ingameInterface.Render(SpriteBatch);
            SpriteBatch.End();

            Particles.SetCamera(cam.Translation, cam.Projection);
        }
    }
}
