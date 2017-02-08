using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using StarrockGame.GUI;
using Microsoft.Xna.Framework.Graphics;
using StarrockGame.Caching;

namespace StarrockGame.SceneManagement.Popups
{
    public class PopupNoLBConnection : Popup
    {
        Menu menu;
        Texture2D darkTex;

        public PopupNoLBConnection(Game1 game) : base(game)
        {
        }

        public override void Initialize()
        {
            SpriteFont font = Cache.LoadFont("MenuFont");
            menu = new Menu(font, null);
            Vector2 screenCenter = new Vector2(Device.Viewport.Width * .5f, Device.Viewport.Height * .5f);

            new Label(menu, "Connection to Leaderboard could not be established!", screenCenter, 1, Color.White);
            new ButtonLabel(menu, "Ok", screenCenter + new Vector2(0, font.LineSpacing), 1, Color.White, () => { Close(); });
            menu.SelectNext();
        }

        public override void Update(GameTime gameTime)
        {
            menu.Update(gameTime);
        }

        public override void Render(GameTime gameTime)
        {
            SpriteBatch.GraphicsDevice.Clear(Color.Black);
            SpriteBatch.Begin();
            menu.Render(SpriteBatch);
            SpriteBatch.End();
        }

        
    }
}
