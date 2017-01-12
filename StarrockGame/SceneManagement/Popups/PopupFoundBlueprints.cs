using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using StarrockGame.GUI;
using StarrockGame.Caching;
using Microsoft.Xna.Framework.Graphics;

namespace StarrockGame.SceneManagement.Popups
{
    public class PopupFoundBlueprints : Popup
    {
        Menu menu;

        public PopupFoundBlueprints(Game1 game) : base(game)
        {
        }

        public override void Initialize()
        {
            SpriteFont font = Cache.LoadFont("MenuFont");
            menu = new Menu(font, OnReturn);

            Vector2 screenCenter = new Vector2(Device.Viewport.Width * .5f, (Device.Viewport.Height - font.LineSpacing * SessionManager.FoundBlueprints.Count) * .5f);

            for (int i = 0; i < SessionManager.FoundBlueprints.Count; i++)
            {
                new Label(menu, SessionManager.FoundBlueprints[i], 
                    screenCenter + new Vector2(0, i * font.LineSpacing), 
                    1, Color.White);
            }
        }

        public override void Render(GameTime gameTime)
        {
            Device.Clear(Color.Black);
            SpriteBatch.Begin();
            menu.Render(SpriteBatch);
            SpriteBatch.End();
        }

        public override void Update(GameTime gameTime)
        {
            menu.Update(gameTime);
        }

        private void OnReturn()
        {
            Close();
        }
    }
}
