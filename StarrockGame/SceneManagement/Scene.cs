﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StarrockGame.SceneManagement
{
    public abstract class Scene
    {
        protected virtual Color ClearColor { get { return Color.Black; } }

        public SceneState State;
        protected float FadeSpeed = 0.75f; // in seconds
        protected float FadeProgress = 0f; // set this to a negative value to stay in "black screen" for a short time until fadein starts

        protected SpriteBatch SpriteBatch;
        protected ContentManager Content;
        protected GraphicsDevice Device;
        protected Game1 Game;

        public Scene(Game1 game)
        {
            Game = game;
            Content = game.Content;
            Device = game.GraphicsDevice;
            SpriteBatch = new SpriteBatch(Device);
            Initialize();
        }
        ~Scene()
        {

        }

        public abstract void Initialize();

        public abstract void Update(GameTime gameTime);

        public virtual void UpdateFade(GameTime gameTime)
        {
            switch (State)
            {
                case SceneState.FadingIn:
                    DefaultUpdateFadeIn(gameTime);
                    break;
                case SceneState.FadingOut:
                    DefaultUpdateFadeOut(gameTime);
                    break;
            }
        }
        private void DefaultUpdateFadeIn(GameTime gameTime)
        {
            float elapsed = (float)gameTime.ElapsedGameTime.TotalSeconds;
            FadeProgress += elapsed;
            if (FadeProgress >= FadeSpeed)
            {
                FadeProgress = FadeSpeed;
                State = SceneState.Open;
            }
        }

        private void DefaultUpdateFadeOut(GameTime gameTime)
        {
            float elapsed = (float)gameTime.ElapsedGameTime.TotalSeconds;
            FadeProgress -= elapsed;
            if (FadeProgress <= 0)
            {
                FadeProgress = 0;
                State = SceneState.Closed;
            }
        }


        public virtual void Render(GameTime gameTime)
        {
            Device.Clear(ClearColor);
        }

        public virtual void RenderFade(GameTime gameTime)
        {
            RenderTarget2D rt = new RenderTarget2D(Device, Device.Viewport.Width, Device.Viewport.Height);
            Device.SetRenderTarget(rt);
            Device.Clear(Color.Transparent);
            Render(gameTime);
            Device.SetRenderTarget(null);

            SpriteBatch.Begin();
            int a = (int)(255 * (FadeProgress / FadeSpeed));
            SpriteBatch.Draw(rt, new Vector2(0, 0), new Color(a, a, a, a));
            SpriteBatch.End();
        }
    }
}
