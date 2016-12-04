using System;
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

namespace StarrockGame.SceneManagement.Scenes
{
    public class SceneTestSession : Scene
    {
        Spaceship ship, ship2;
        Camera2D cam;
        Gauge gauge;
        

        public SceneTestSession(Game1 game) : base(game)
        {
            EntityManager.Clear();
            ship = EntityManager.Add<Spaceship, PlayerController>("Spaceship", new Vector2(200, 200), 0, Vector2.Zero);
            ship2 = EntityManager.Add<Spaceship, NoController>("Spaceship", new Vector2(400, 300), 10, Vector2.Zero);
            

            cam = new Camera2D(Device);
            cam.TrackingBody = ship.Body;
            cam.Update();
            gauge = new Gauge(100, new Rectangle(10, 30, 150, 24), Color.Red);
            
        }

        public override void Update(GameTime gameTime)
        {
            float elapsed = (float)gameTime.ElapsedGameTime.TotalSeconds;
            
            EntityManager.Update(gameTime);
            cam.Update();

            ship.Structure -= 0.25f;
            gauge.Value = ship.Structure;

            Particles.Emit<TrailParticleSystem>(new Vector2(400, 400), new Vector2(0, 500));
        }

        public override void Render(GameTime gameTime)
        {
            base.Render(gameTime);

            SpriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, null, null, null, null, cam.Translation);
            EntityManager.Render(SpriteBatch, gameTime);
            SpriteBatch.End();

            SpriteBatch.Begin();
            SpriteBatch.DrawString(Cache.LoadFont("GameFont"), string.Format("Position: {0}", ConvertUnits.ToDisplayUnits(ship.Body.Position)), new Vector2(10), Color.White);
            gauge.Render(SpriteBatch);
            SpriteBatch.End();

            Particles.SetCamera(cam.Translation, cam.Projection);
        }
    }
}
