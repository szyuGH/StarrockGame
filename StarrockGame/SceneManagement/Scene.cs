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
    public abstract class Scene
    {
        public SceneState State;
        protected float FadeSpeed = 0.75f; // in seconds
        protected float FadeProgress = 0f; // set this to a negative value to stay in "black screen" for a short time until fadein starts

        protected SpriteBatch SpriteBatch;
        protected ContentManager Content;
        protected GraphicsDevice Device;

        public Scene(Game1 game)
        {
            Content = game.Content;
            Device = game.GraphicsDevice;
            SpriteBatch = new SpriteBatch(Device);
        }
        ~Scene()
        {

        }


        public abstract void Update(float elapsed);

        public virtual void UpdateFade(float elapsed)
        {
            switch (State)
            {
                case SceneState.FadingIn:
                    DefaultUpdateFadeIn(elapsed);
                    break;
                case SceneState.FadingOut:
                    DefaultUpdateFadeOut(elapsed);
                    break;
            }
        }
        private void DefaultUpdateFadeIn(float elapsed)
        {
            FadeProgress += elapsed;
            if (FadeProgress >= FadeSpeed)
            {
                FadeProgress = FadeSpeed;
                State = SceneState.Open;
            }
        }

        private void DefaultUpdateFadeOut(float elapsed)
        {
            FadeProgress -= elapsed;
            if (FadeProgress <= 0)
            {
                FadeProgress = 0;
                State = SceneState.Closed;
            }
        }

        public abstract void Render();

        public virtual void RenderFade()
        {
            RenderTarget2D rt = new RenderTarget2D(Device, Device.Viewport.Width, Device.Viewport.Height);
            Device.SetRenderTarget(rt);
            Device.Clear(Color.Transparent);
            Render();
            Device.SetRenderTarget(null);

            SpriteBatch.Begin();
            int a = (int)(255 * (FadeProgress / FadeSpeed));
            SpriteBatch.Draw(rt, new Vector2(0, 0), new Color(a, a, a, a));
            SpriteBatch.End();
        }
    }
}
