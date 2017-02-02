using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using StarrockGame.GUI;
using StarrockGame.Caching;
using Microsoft.Xna.Framework.Graphics;
using StarrockGame.Audio;

namespace StarrockGame.SceneManagement.Scenes
{
    public class SceneSelectDifficulty : Scene
    {
        private Menu menu;

        public SceneSelectDifficulty(Game1 game) : base(game)
        {
        }

        public override void Initialize()
        {
            SpriteFont font = Cache.LoadFont("MenuFont");
            menu = new Menu(font, () => { OnMenuBack(); });

            Vector2 menuPos = new Vector2(100 + Device.Viewport.Width * .5f, Device.Viewport.Height * .5f + 4 * font.LineSpacing);
            new ButtonLabel(menu, "", menuPos + new Vector2(0, font.LineSpacing * 0), 1, Color.White, OnDifficultySelect) {
                CaptionMonitor = () => {
                    return string.Format("Difficulty: {0} ({1} C)", SessionManager.Difficulty.ToString(), SessionManager.SessionCost);
                }
            };
            new ButtonLabel(menu, "Start", menuPos + new Vector2(0, font.LineSpacing * 1), 1, Color.White, OnSessionStart);
            new ButtonLabel(menu, "Back", menuPos + new Vector2(0, font.LineSpacing * 2), 1, Color.White, OnMenuBack);

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


        private void OnMenuBack()
        {
            SceneManager.Return();
        }

        private void OnDifficultySelect()
        {
            if (SessionManager.Difficulty == SessionDifficulty.Lost)
                SessionManager.Difficulty = SessionDifficulty.Easy;
            else
                SessionManager.Difficulty++;

        }

        private void OnSessionStart()
        {
            if (Player.Get().Credits >= SessionManager.SessionCost)
            {
                Player.Get().Credits -= SessionManager.SessionCost;
                SessionManager.Score = 0;
                SceneManager.Call<SceneSession>();
                Sound.Instance.PlaySe("Buy");
            } else
            {
                Sound.Instance.PlaySe("Fail");
            }
        }
    }
}
