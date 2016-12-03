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

namespace StarrockGame.SceneManagement.Scenes
{
    public class SceneTestSession : Scene
    {
        Spaceship ship;
        World world;

        public SceneTestSession(Game1 game) : base(game)
        {
            world = new World(Vector2.Zero);
            ship = new Spaceship(world, "Spaceship");
            ship.Initialize<PlayerController>(new Vector2(200,200), 0, Vector2.Zero);
        }

        public override void Update(GameTime gameTime)
        {
            float elapsed = (float)gameTime.ElapsedGameTime.TotalSeconds;
            world.Step(Math.Min(elapsed, (1f / 60f)));
            ship.Update(gameTime);

            Particles.Emit<TrailParticleSystem>(Vector2.Zero, new Vector2(0, 500));
        }

        public override void Render(GameTime gameTime)
        {
            base.Render(gameTime);

            SpriteBatch.Begin();
            ship.Render(SpriteBatch, gameTime);
            SpriteBatch.End();


            Matrix view = Matrix.CreateTranslation(0, 0, 0) *
                         Matrix.CreateLookAt(new Vector3(0, 0, -1),
                                             new Vector3(0, 0, 0), Vector3.Up);

            Matrix projection = Matrix.CreateOrthographic(Device.Viewport.Width, Device.Viewport.Height, -1, 1);


            Particles.SetCamera(view, projection);
        }
    }
}
