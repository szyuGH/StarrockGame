using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MySql.Data.MySqlClient;
using StarrockGame.Caching;
using StarrockGame.GUI;
using StarrockGame.SceneManagement.Popups;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StarrockGame.SceneManagement.Scenes
{
    public class SceneTitle : Scene
    {
        Menu menu;

        public SceneTitle(Game1 game) : base(game)
        {
            Player.Get().Save();
        }

        public override void Initialize()
        {
            SpriteFont font = Cache.LoadFont("MenuFont");
            menu = new Menu(font, null);

            Vector2 screenCenter = new Vector2(Device.Viewport.Width, Device.Viewport.Height) * .5f;
            float size = 1;

            // for testing issues
#if DEBUG
            new ButtonLabel(menu, "Test Session", screenCenter + new Vector2(0, -1 * font.LineSpacing), size, Color.White, () => { SceneManager.Call<SceneTestSession>(); });
#endif
            new ButtonLabel(menu, "New Session", screenCenter + new Vector2(0, 0 * font.LineSpacing), size, Color.White, OnNewSession);
            new ButtonLabel(menu, "Leaderboard", screenCenter + new Vector2(0, 1 * font.LineSpacing), size, Color.White, OnLeaderboard);
            new ButtonLabel(menu, "Controls", screenCenter + new Vector2(0, 2 * font.LineSpacing), size, Color.White, OnControls);
            new ButtonLabel(menu, "Options", screenCenter + new Vector2(0, 3 * font.LineSpacing), size, Color.White, OnOptions);
            new ButtonLabel(menu, "Exit", screenCenter + new Vector2(0, 4 * font.LineSpacing), size, Color.White, OnExit);

            new Label(menu, "", new Vector2(20, 40), 1, Color.White, 0) { CaptionMonitor = () => { return string.Format("Credits: {0} C", Player.Get().Credits); } };
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

        private void OnNewSession()
        {
            SceneManager.Call<ScenePrepareSession>();
        }

        private void OnLeaderboard()
        {
            if (CheckConnection())
                SceneManager.Call<SceneLeaderboard>();
        }

        private bool CheckConnection()
        {
            MySqlConnection connection = new MySqlConnection(SceneLeaderboard.ConnectionString);
            try
            {
                connection.Open();
                return true;
            }
            catch (Exception)
            {
                SceneManager.CallPopup<PopupNoLBConnection>();
            }
            return false;
        }

        private void OnControls()
        {
            SceneManager.Call<SceneControls>();
        }

        private void OnOptions()
        {
            SceneManager.Call<SceneOptions>();
        }

        private void OnExit()
        {
            SceneManager.Exit();
        }
    }
}
