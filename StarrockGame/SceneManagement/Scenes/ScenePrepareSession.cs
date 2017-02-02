using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StarrockGame.Audio;
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
        private Menu statsMenu;
        private MatrixMenu shipMenu;
        private int lastSelectedShip = 0;
        private List<SpaceshipTemplate> unlockedShips;
        ButtonLabel continueButton;

        private SpaceshipTemplate currentTemplate { get { return shipMenu.SelectedIndex == -1 ? null : unlockedShips[shipMenu.SelectedIndex]; } }

        public ScenePrepareSession(Game1 game) : base(game)
        {
        }

        public override void Initialize()
        {
            CreateMenu();
            CreateShipMenu();
            CreateStatsMenu();

            if (SessionManager.UsedShipTemplate != null)
            {
                shipMenu.SelectedIndex = unlockedShips.IndexOf(SessionManager.UsedShipTemplate);
                continueButton.Active = true;
            }
            else
                shipMenu.SelectedIndex = -1;
        }

        private void CreateMenu()
        {
            SpriteFont font = Cache.LoadFont("MenuFont");
            menu = new Menu(font, () => { SceneManager.Return(); });

            Vector2 menuPos = new Vector2(100 + Device.Viewport.Width * .5f, Device.Viewport.Height - 40 - 4 * font.LineSpacing);
            new ButtonLabel(menu, "Select Ship", menuPos + new Vector2(0, font.LineSpacing * 0), 1, Color.White, OnSelectShip);
            continueButton = new ButtonLabel(menu, "Next", menuPos + new Vector2(0, font.LineSpacing * 1), 1, Color.White, OnBuyModules) { Active = false };
            new ButtonLabel(menu, "Back", menuPos + new Vector2(0, font.LineSpacing * 2), 1, Color.White, () => { OnMenuBack(); });

            new Label(menu, "", new Vector2(20, 40), 1, Color.White, 0) { CaptionMonitor = () => { return string.Format("Credits: {0} C", Player.Get().Credits); } };
        }

        private void CreateShipMenu()
        {
            SpriteFont font = Cache.LoadFont("MenuFont");
            shipMenu = new MatrixMenu(font, new Vector2(60, 100), Device.Viewport.Width / 160, TILE_WIDTH, TILE_HEIGHT, TILE_SPACE, OnShipMenuCancel);
            shipMenu.IsActive = false;

            unlockedShips = Player.Get().GetTemplates<SpaceshipTemplate>();
            for (int i = 0; i < unlockedShips.Count; i++)
            {
                shipMenu.AddButtonImage(Cache.LoadGraphic(unlockedShips[i].TextureName), unlockedShips[i].Name, Color.White, OnShipSelected);
            }
        }

        private void CreateStatsMenu()
        {
            SpriteFont statsFont = Cache.LoadFont("StatsFont");
            statsMenu = new Menu(statsFont, null);
            statsMenu.NotSelectable = true;

            Vector2 statsPosition = new Vector2(20, Device.Viewport.Height - statsFont.LineSpacing * 7);
            new Label(statsMenu, "Ship Stats:", statsPosition, 1, Color.White, 0);
            new Label(statsMenu, "", statsPosition + new Vector2(0, statsFont.LineSpacing * 1), 1, Color.White, 0) { CaptionMonitor = () => { return string.Format("Ship Name: {0}", (currentTemplate == null ? "" : currentTemplate.Name)); } };
            new Label(statsMenu, "", statsPosition + new Vector2(0, statsFont.LineSpacing * 2), 1, Color.White, 0) { CaptionMonitor = () => { return string.Format("Price: {0}", (currentTemplate == null ? "" : currentTemplate.Price.ToString())); } };
            new Label(statsMenu, "", statsPosition + new Vector2(0, statsFont.LineSpacing * 3), 1, Color.White, 0) { CaptionMonitor = () => { return string.Format("Module Count: {0}", (currentTemplate == null ? "" : currentTemplate.ModuleCount.ToString())); } };
            new Label(statsMenu, "", statsPosition + new Vector2(0, statsFont.LineSpacing * 4), 1, Color.White, 0) { CaptionMonitor = () => { return string.Format("Primary Weapon: {0}", (currentTemplate == null ? "" : GetWeaponName(currentTemplate.PrimaryWeaponBases.WeaponType))); } };
            new Label(statsMenu, "", statsPosition + new Vector2(0, statsFont.LineSpacing * 5), 1, Color.White, 0) { CaptionMonitor = () => { return string.Format("Secondary Weapon: {0}", (currentTemplate == null ? "" : GetWeaponName(currentTemplate.SecondaryWeaponBases.WeaponType))); } };
        }

        private string GetWeaponName(string wtype)
        {
            WeaponTemplate wt = Cache.LoadTemplate<WeaponTemplate>(wtype);
            return wt.Name;
        }

        public override void Update(GameTime gameTime)
        {

            shipMenu.Update(gameTime);
            menu.Update(gameTime);
            statsMenu.Update(gameTime);
        }

        public override void Render(GameTime gameTime)
        {
            base.Render(gameTime);

            SpriteBatch.Begin();
            menu.Render(SpriteBatch);
            shipMenu.Render(SpriteBatch);
            statsMenu.Render(SpriteBatch);
            SpriteBatch.End();
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
            if (shipMenu.SelectedIndex != -1)
            {
                SpaceshipTemplate selectedTemplate = unlockedShips[shipMenu.SelectedIndex];
                if (Player.Get().Credits >= selectedTemplate.Price)
                {
                    Sound.Instance.PlaySe("Buy");
                    if (SessionManager.UsedShipTemplate != null)
                        Player.Get().Credits += SessionManager.UsedShipTemplate.Price;

                    SessionManager.UsedShipTemplate = selectedTemplate;
                    Player.Get().Credits -= SessionManager.UsedShipTemplate.Price;
                    shipMenu.IsActive = false;
                    menu.IsActive = true;
                    lastSelectedShip = shipMenu.SelectedIndex;
                    continueButton.Active = true;
                }
                else
                {
                    Sound.Instance.PlaySe("Fail");
                }
            }
        }

        private void OnBuyModules()
        {
            SceneManager.Call<SceneBuyModules>();
        }

        private void OnMenuBack()
        {
            if (SessionManager.UsedShipTemplate != null)
            {
                Player.Get().Credits += SessionManager.UsedShipTemplate.Price;
                SessionManager.Reset();
            }
            SceneManager.Return();
        }
    }
}
