using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StarrockGame.Caching;
using StarrockGame.GUI;
using StarrockGame.GUI.MenuElements;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TData.TemplateData;

namespace StarrockGame.SceneManagement.Scenes
{
    public class ScenePrepareSession : Scene
    {
        const int TILE_WIDTH = 150;
        const int TILE_HEIGHT = 90;
        const int TILE_SPACE = 10;
        const int TILE_WIDTH_REAL = TILE_WIDTH + TILE_SPACE;
        const int TILE_HEIGHT_REAL = TILE_HEIGHT + TILE_SPACE;

        private Menu menu;
        private MatrixMenu shipMenu;
        private int lastSelectedShip = 0;
        private List<SpaceshipTemplate> unlockedShips;
        ButtonLabel continueButton;

        public ScenePrepareSession(Game1 game) : base(game)
        {
        }

        public override void Initialize()
        {
            SpriteFont font = Cache.LoadFont("MenuFont");
            menu = new Menu(font, () => { SceneManager.Return(); });
            shipMenu = new MatrixMenu(font, new Vector2(60, 100), Device.Viewport.Width / 160, TILE_WIDTH, TILE_HEIGHT, TILE_SPACE, OnShipMenuCancel);
            shipMenu.IsActive = false;

            unlockedShips = Player.Get().GetTemplates<SpaceshipTemplate>();
            
            for (int i = 0; i< unlockedShips.Count; i++)
            {
                shipMenu.AddButtonImage(Cache.LoadGraphic(unlockedShips[i].TextureName), unlockedShips[i].Name, Color.White, OnShipSelected);
                //new ButtonImage(shipMenu, Cache.LoadGraphic(unlockedShips[i].TextureName), unlockedShips[i].Name, new Vector2(i * TILE_WIDTH_REAL + 60, 100), TILE_WIDTH, TILE_HEIGHT, Color.White, OnShipSelected);
            }

            new ButtonLabel(menu, "Select Ship", new Vector2(Device.Viewport.Width * .5f, Device.Viewport.Height - 40 - 4 * font.LineSpacing), 1, Color.White, OnSelectShip);
            ButtonLabel difficultyButton = new ButtonLabel(menu, "", new Vector2(Device.Viewport.Width * .5f, Device.Viewport.Height - 40 - 3 * font.LineSpacing), 1, Color.White, OnDifficultySelect);
            difficultyButton.CaptionMonitor = () => { return "Difficulty: " + SessionManager.Difficulty.ToString(); };
            continueButton = new ButtonLabel(menu, "Continue", new Vector2(Device.Viewport.Width * .5f, Device.Viewport.Height - 40 - 2 * font.LineSpacing), 1, Color.White, OnBuyModules) { Active = false };
            new ButtonLabel(menu, "Back", new Vector2(Device.Viewport.Width*.5f, Device.Viewport.Height - 40 - 1 * font.LineSpacing), 1, Color.White, () => { SceneManager.Return(); });

            if (SessionManager.UsedShipTemplate != null)
            {
                shipMenu.SelectedIndex = unlockedShips.IndexOf(SessionManager.UsedShipTemplate);
                continueButton.Active = true;
            }
        }

        public override void Update(GameTime gameTime)
        {

            shipMenu.Update(gameTime);
            menu.Update(gameTime);
            
        }

        public override void Render(GameTime gameTime)
        {
            base.Render(gameTime);

            SpriteBatch.Begin();
            menu.Render(SpriteBatch);
            shipMenu.Render(SpriteBatch);
            SpriteBatch.End();
        }

        private void OnDifficultySelect()
        {
            if (SessionManager.Difficulty == SessionDifficulty.Lost)
                SessionManager.Difficulty = SessionDifficulty.Easy;
            else
                SessionManager.Difficulty++;
            
        }

        private void OnSelectShip()
        {
            menu.IsActive = false;
            shipMenu.IsActive = true;
            shipMenu.SelectedIndex = lastSelectedShip;
        }

        private void OnShipMenuCancel()
        {
            shipMenu.IsActive = false;
            menu.IsActive = true;
            lastSelectedShip = shipMenu.SelectedIndex;
            shipMenu.SelectedIndex = -1;
        }

        private void OnShipSelected()
        {
            SessionManager.UsedShipTemplate = unlockedShips[shipMenu.SelectedIndex];
            shipMenu.IsActive = false;
            menu.IsActive = true;
            lastSelectedShip = shipMenu.SelectedIndex;
            continueButton.Active = true;
        }

        private void OnBuyModules()
        {
            SceneManager.Call<SceneBuyModules>();
        }
    }
}
