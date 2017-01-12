using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using StarrockGame.GUI;
using Microsoft.Xna.Framework.Graphics;
using StarrockGame.Caching;
using StarrockGame.SceneManagement.Scenes;

namespace StarrockGame.SceneManagement.Popups
{
    public class PopupGameover : Popup
    {
        private Menu menu;
        private Label gameoverLabel;

        private Color[] textColors = new Color[] { Color.White, Color.Red };
        private float textColorProgress = 0;
        private int progressDirection = 1;
        const float PROGRESS_TIME = 1;

        public PopupGameover(Game1 game) : base(game)
        {
        }

        public override void Initialize()
        {
            SpriteFont font = Cache.LoadFont("MenuFont");
            menu = new Menu(font, null);

            Vector2 screenCenter = new Vector2(Device.Viewport.Width * .5f, Device.Viewport.Height * .5f);

            gameoverLabel = new Label(menu, "Game Over", screenCenter, 3, Color.White);
            new ButtonLabel(menu, "Accept Death", screenCenter + new Vector2(0, font.LineSpacing * 2.5f), 1, Color.White, OnReturnToTitle);
            new ButtonLabel(menu, string.Format("Show found blueprints ({0})", SessionManager.FoundBlueprints.Count),
                screenCenter + new Vector2(0,  font.LineSpacing * 4f), 1, Color.White, OnShowFoundBlueprints)
            {
                Active = SessionManager.FoundBlueprints.Count > 0
            };

            new Label(menu, "Statistics", screenCenter - new Vector2(60, font.LineSpacing * 5), 1, Color.LightSlateGray, 0);
            new Label(menu, "", screenCenter - new Vector2(60, font.LineSpacing * 4), 1, Color.White, 0) { CaptionMonitor = () => { return string.Format("Elapsed Time: {0:hh\\:mm\\:ss}", SessionManager.ElapsedTime); } };
            new Label(menu, "", screenCenter - new Vector2(60, font.LineSpacing * 3), 1, Color.White, 0) { CaptionMonitor = () => { return string.Format("Score: {0}", SessionManager.Score); } };
            //new Label(menu, "", screenCenter - new Vector2(60, font.LineSpacing * 2), 1, Color.White, 0) { CaptionMonitor = () => { return string.Format("Credits: {0}", XXX); } };

            menu.SelectNext();
        }

        public override void Update(GameTime gameTime)
        {
            UpdateGameoverColor((float)gameTime.ElapsedGameTime.TotalSeconds);
            
            menu.Update(gameTime);
        }

        private void UpdateGameoverColor(float elapsed)
        {
            textColorProgress += elapsed * progressDirection;
            if (progressDirection == 1)
            {
                if (textColorProgress >= PROGRESS_TIME)
                {
                    textColorProgress = PROGRESS_TIME;
                    progressDirection = -1;
                }
            }
            else if (progressDirection == -1)
            {
                if (textColorProgress <= 0)
                {
                    textColorProgress = 0;
                    progressDirection = 1;
                }
            }
            gameoverLabel.Color = Color.Lerp(textColors[0], textColors[1], textColorProgress / PROGRESS_TIME);
        }

        public override void Render(GameTime gameTime)
        {
            SpriteBatch.Begin();
            menu.Render(SpriteBatch);
            SpriteBatch.End();
        }

        private void OnReturnToTitle()
        {
            Close();
            SceneManager.ReturnUntil<ScenePrepareSession>();
            SessionManager.Reset();
        }

        private void OnShowFoundBlueprints()
        {
            SceneManager.CallPopup<PopupFoundBlueprints>();
        }
    }
}
