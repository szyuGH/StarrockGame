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
    public class SceneSelectResolution : Scene
    {
        Menu menu;
        public SceneSelectResolution(Game1 game) : base(game)
        {
        }

        public override void Initialize()
        {
            SpriteFont font = Cache.LoadFont("MenuFont");
            menu = new Menu(font, () => { SceneManager.Return(); });
            float size = 1;

            IEnumerable<DisplayMode> dmc = Game.Graphics.GetSupportedResolutions();
            Vector2 screenCenter = new Vector2(Device.Viewport.Width, (Device.Viewport.Height - font.LineSpacing * dmc.Count())) * .5f;

            for (int i = 0; i < dmc.Count(); i++)
            {
                DisplayMode dm = dmc.ElementAt(i);
                new ButtonLabel(menu,
                    string.Format("{0}x{1}", dm.Width, dm.Height),
                    screenCenter + new Vector2(0, i * font.LineSpacing), size, Color.White,
                    () =>
                    {
                        Game.Graphics.SetResolution(menu.SelectedIndex);
                        Initialize();
                    });
            }
            new ButtonLabel(menu, "Back", screenCenter + new Vector2(0, dmc.Count() * font.LineSpacing), size, Color.White, () => { SceneManager.Return(); });
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
    }
}
