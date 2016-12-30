using FarseerPhysics.Dynamics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using StarrockGame.InputManagement;
using StarrockGame.SceneManagement;
using StarrockGame.SceneManagement.Scenes;
using System.Collections.Generic;
using System;
using StarrockGame.Entities;
using FarseerPhysics.Collision.Shapes;
using StarrockGame.Caching;
using GPart;
using StarrockGame.ParticleSystems;
using StarrockGame.GUI;

namespace StarrockGame
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        public StarrockGraphicsDeviceManager Graphics { get; private set; }
        SpriteBatch spriteBatch;

        

        public Game1()
        {
            Graphics = new StarrockGraphicsDeviceManager(this, false);
            Graphics.PreferredBackBufferWidth = 800;
            Graphics.PreferredBackBufferHeight = 600;
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            Input.Initialize();
            Cache.Initialize(Content, null, new StarrockCacheLoader());
            Particles.Add<TrailParticleSystem>(this);
            Particles.Add<ExplosionParticleSystem>(this);
            Player.Get();

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            SceneManager.Initialize<SceneIntro>(this);
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            // TODO: use this.Content to load your game content here
            
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            float elapsed = (float)gameTime.ElapsedGameTime.TotalSeconds;
            Input.Update();
            SceneManager.Update(gameTime);

            base.Update(gameTime);
            Menu.IgnoreNextInput = false;

            //Particles.Emit<TrailParticleSystem>(Vector2.Zero, Vector2.Zero);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);
            SceneManager.Render(gameTime);
            

            base.Draw(gameTime);
        }

        protected override void OnExiting(object sender, EventArgs args)
        {
            Cache.Dispose();
            SceneManager.Dispose();
            base.OnExiting(sender, args);
        }
    }
}
