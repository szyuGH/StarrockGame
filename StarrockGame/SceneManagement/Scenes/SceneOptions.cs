using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using StarrockGame.GUI;
using StarrockGame.Caching;
using Microsoft.Xna.Framework.Graphics;

namespace StarrockGame.SceneManagement.Scenes
{
    public class SceneOptions : Scene
    {
        Menu menu;

        public SceneOptions(Game1 game) : base(game)
        {
        }

        public override void Initialize()
        {
            SpriteFont font = Cache.LoadFont("MenuFont");
            menu = new Menu(font, null);

            Vector2 screenCenter = new Vector2(Device.Viewport.Width, Device.Viewport.Height) * .5f;
            float size = 1;

            new ButtonLabel(menu,
                string.Format("Resolution: {0}x{1}", Game.GraphicsDevice.Viewport.Width, Game.GraphicsDevice.Viewport.Height),
                screenCenter + new Vector2(0, 0 * font.LineSpacing), size, Color.White, OnResolution)
            { Tag = 0 };
            new ButtonLabel(menu, "Toggle Fullscreen", screenCenter + new Vector2(0, 1 * font.LineSpacing), size, Color.White, OnToggleFullscreen);
            new ButtonLabel(menu, "Back", screenCenter + new Vector2(0, 2 * font.LineSpacing), size, Color.White, () => { SceneManager.Return(); });
        }

        public override void Dispose()
        {
            base.Dispose();
            menu.Dispose();
            menu = null;
        }

        public override void Update(GameTime gameTime)
        {
            menu.Update(gameTime);
        }

        public override void Render(GameTime gameTime)
        {
            base.Render(gameTime);
            SpriteBatch.Begin();
            menu.Render(SpriteBatch);
            SpriteBatch.End();
        }

        private void OnResolution()
        {
            SceneManager.Call<SceneSelectResolution>();
        }

        private void OnToggleFullscreen()
        {
            Game.Graphics.ToggleFullScreen();
        }
    }
}
