using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using StarrockGame.GUI;
using StarrockGame.Caching;
using Microsoft.Xna.Framework.Graphics;
using StarrockGame.SceneManagement.Scenes;

namespace StarrockGame.SceneManagement.Popups
{
    public class PopupPause : Popup
    {
        Menu menu;

        public PopupPause(Game1 game) : base(game)
        {
        }

        public override void Initialize()
        {
            SpriteFont font = Cache.LoadFont("MenuFont");
            menu = new Menu(font, Close);

            Vector2 screenCenter = new Vector2(Device.Viewport.Width * .5f, Device.Viewport.Height * .5f);

            new Label(menu, "- Pause -", screenCenter, 2, Color.White);
            new ButtonLabel(menu, "Resume", screenCenter + new Vector2(0, font.LineSpacing * 3), 1, Color.White, Close);
            new ButtonLabel(menu, string.Format("Show found blueprints ({0})", SessionManager.FoundBlueprints.Count),
                screenCenter + new Vector2(0, font.LineSpacing * 4f), 1, Color.White, OnShowFoundBlueprints)
            {
                Active = SessionManager.FoundBlueprints.Count > 0
            };
            new ButtonLabel(menu, "End Session", screenCenter + new Vector2(0, font.LineSpacing * 5), 1, Color.White, OnEndSession);
        }

        public override void Update(GameTime gameTime)
        {
            menu.Update(gameTime);
        }

        public override void Render(GameTime gameTime)
        {
            
            SpriteBatch.Begin();
            menu.Render(SpriteBatch);
            SpriteBatch.End();
        }

        private void OnEndSession()
        {
            Close();
            SceneManager.Set<SceneTitle>();
            SessionManager.Reset();
        }

        private void OnShowFoundBlueprints()
        {
            SceneManager.CallPopup<PopupFoundBlueprints>();
        }
    }
}
