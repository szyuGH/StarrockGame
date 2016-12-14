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
    public abstract class Popup : IDisposable
    {
        private bool disposed;

        protected SpriteBatch SpriteBatch;
        protected ContentManager Content;
        protected GraphicsDevice Device;
        protected Game1 Game;

        public Popup(Game1 game)
        {
            Game = game;
            Content = game.Content;
            Device = game.GraphicsDevice;
            SpriteBatch = new SpriteBatch(Device);
            Initialize();
        }
        ~Popup()
        {
            if (!disposed)
            {
                Dispose();
            }
        }

        public abstract void Initialize();
        public virtual void Dispose()
        {
            disposed = true;
        }

        public abstract void Update(GameTime gameTime);
        public abstract void Render(GameTime gameTime);

        public virtual void Close()
        {
            SceneManager.ClosePopup(this);
        }
    }
}
