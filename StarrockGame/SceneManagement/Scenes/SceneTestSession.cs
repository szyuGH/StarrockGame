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

namespace StarrockGame.SceneManagement.Scenes
{
    public class SceneTestSession : Scene
    {
        Spaceship ship, ship2;
        World world;
        Camera2D cam;

        public SceneTestSession(Game1 game) : base(game)
        {
            world = new World(Vector2.Zero);
            ship = new Spaceship(world, "Spaceship");
            ship.Initialize<PlayerController>(new Vector2(200, 200), 0, Vector2.Zero);

            ship2 = new Spaceship(world, "Spaceship");
            ship2.Initialize<NoController>(new Vector2(400, 300), 0, Vector2.Zero);

            cam = new Camera2D(Device);
            cam.TrackingBody = ship.Body;
            cam.Update();
        }

        public override void Update(GameTime gameTime)
        {
            float elapsed = (float)gameTime.ElapsedGameTime.TotalSeconds;
            world.Step(Math.Min(elapsed, (1f / 60f)));
            ship.Update(gameTime);
            ship2.Update(gameTime);
            cam.Update();

            Particles.Emit<TrailParticleSystem>(new Vector2(400, 400), new Vector2(0, 500));
        }

        public override void Render(GameTime gameTime)
        {
            base.Render(gameTime);

            SpriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, null, null, null, null, cam.Translation);
            ship.Render(SpriteBatch, gameTime);
            ship2.Render(SpriteBatch, gameTime);
            SpriteBatch.End();

            SpriteBatch.Begin();
            SpriteBatch.DrawString(Cache.LoadFont("GameFont"), string.Format("Position: {0}", ConvertUnits.ToDisplayUnits(ship.Body.Position)), new Vector2(10), Color.White);

            SpriteBatch.End();

            Particles.SetCamera(cam.Translation, cam.Projection);
        }
    }
}
