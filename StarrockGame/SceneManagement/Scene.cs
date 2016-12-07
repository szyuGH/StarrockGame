using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StarrockGame.SceneManagement
{
    public abstract class Scene : IDisposable
    {
        protected virtual Color ClearColor { get { return Color.Black; } }

        public SceneState State;
        protected float FadeSpeed = 0.75f; // in seconds
        protected float FadeProgress = 0f; // set this to a negative value to stay in "black screen" for a short time until fadein starts

        protected SpriteBatch SpriteBatch;
        protected ContentManager Content;
        protected GraphicsDevice Device;
        protected Game1 Game;

        private bool disposed;

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
            if (!disposed)
            {
                Dispose();
            }
        }

        public abstract void Initialize();
        public virtual void Dispose() {
            disposed = true;
        }

        public abstract void Update(GameTime gameTime);

        public virtual void UpdateFade(GameTime gameTime)
        {
            switch (State)
            {
                case SceneState.FadingIn:
                    //State = SceneState.Open;
                    DefaultUpdateFadeIn(gameTime);
                    break;
                case SceneState.FadingOut:
                    //State = SceneState.Closed;
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
            Device.SetRenderTarget(SceneManager.SceneRenderTarget);
            Device.Clear(Color.Transparent);
            Render(gameTime);
            Device.SetRenderTarget(null);

            SpriteBatch.Begin();
            int a = (int)(255 * (FadeProgress / FadeSpeed));
            SpriteBatch.Draw(SceneManager.SceneRenderTarget, new Vector2(0, 0), new Color(a, a, a, a));
            SpriteBatch.End();
        }
    }
}
