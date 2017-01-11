using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using StarrockGame.GUI;
using Microsoft.Xna.Framework.Graphics;
using StarrockGame.Caching;
using TData.TemplateData;
using StarrockGame.InputManagement;
using StarrockGame.Audio;

namespace StarrockGame.SceneManagement.Scenes
{
    public class SceneBuyModules : Scene
    {
        const int TILE_WIDTH = 150;
        const int TILE_HEIGHT = 90;
        const int TILE_SPACE = 10;
        const int TILE_WIDTH_REAL = TILE_WIDTH + TILE_SPACE;
        const int TILE_HEIGHT_REAL = TILE_HEIGHT + TILE_SPACE;


        private Menu menu;
        private Menu statsMenu;
        private MatrixMenu moduleMenu;
        private List<ModuleTemplate> unlockedModules;
        private int lastSelected = 0;

        private ModuleTemplate currentTemplate { get { return moduleMenu.SelectedIndex == -1 ? null : unlockedModules[moduleMenu.SelectedIndex]; } }


        public SceneBuyModules(Game1 game) : base(game)
        {
        }

        public override void Initialize()
        {
            unlockedModules = Player.Get().GetTemplates<ModuleTemplate>();

            CreateMenu();
            CreateModuleMenu();
            CreateStatsMenu();
        }

        private void CreateMenu()
        {
            SpriteFont font = Cache.LoadFont("MenuFont");
            menu = new Menu(font, () => { SceneManager.Return(); });

            Vector2 menuPos = new Vector2(100 + Device.Viewport.Width * .5f, Device.Viewport.Height - 40 - 4 * font.LineSpacing);
            new ButtonLabel(menu, "Select Modules", menuPos + new Vector2(0, font.LineSpacing * 0), 1, Color.White, OnBuyModules) { Active = unlockedModules.Count > 0 };
            new ButtonLabel(menu, "Start Session", menuPos + new Vector2(0, font.LineSpacing * 1), 1, Color.White, OnStartSession);
            new ButtonLabel(menu, "Back", menuPos + new Vector2(0, font.LineSpacing * 2), 1, Color.White, () => { OnMenuBack(); });
            new Label(menu, "", new Vector2(10, 10), 1, Color.White, 0) { CaptionMonitor = () => { return "Credits: " + Player.Get().Credits; } };
        }

        private void CreateModuleMenu()
        {
            SpriteFont font = Cache.LoadFont("MenuFont");

            moduleMenu = new MatrixMenu(font, new Vector2(150, 100), Device.Viewport.Width / TILE_WIDTH_REAL, TILE_WIDTH, TILE_HEIGHT, TILE_SPACE, OnBuyModulesCancel);
            moduleMenu.IsActive = false;

            for (int i = 0; i < unlockedModules.Count; i++)
            {
                moduleMenu.AddButtonLabel(unlockedModules[i].Name, Color.White, OnModuleSelected);
            }
        }

        private void CreateStatsMenu()
        {
            SpriteFont statsFont = Cache.LoadFont("StatsFont");
            statsMenu = new Menu(statsFont, null);
            statsMenu.NotSelectable = true;

            Vector2 statsPosition = new Vector2(20, Device.Viewport.Height - statsFont.LineSpacing * 7);
            new Label(statsMenu, "Ship Stats", statsPosition, 1, Color.White, 0);
            new Label(statsMenu, "", statsPosition + new Vector2(0, statsFont.LineSpacing * 1), 1, Color.White, 0) { CaptionMonitor = () => { return string.Format("Ship Name: {0}", (currentTemplate == null ? "" : currentTemplate.Name)); } };
            new Label(statsMenu, "", statsPosition + new Vector2(0, statsFont.LineSpacing * 2), 1, Color.White, 0) { CaptionMonitor = () => { return string.Format("Price: {0}", (currentTemplate == null ? "" : currentTemplate.Price.ToString())); } };
            
            // TODO: display effects
        }

        public override void Update(GameTime gameTime)
        {
            menu.Update(gameTime);
            moduleMenu.Update(gameTime);

            if (Input.Device.FirePrimary())
            {
                RemoveLastModule();
            }
        }

        public override void Render(GameTime gameTime)
        {
            base.Render(gameTime);
            SpriteBatch.Begin();
            menu.Render(SpriteBatch);
            moduleMenu.Render(SpriteBatch);
            SpriteBatch.End();
        }

        private void OnBuyModules()
        {
            menu.IsActive = false;
            moduleMenu.IsActive = true;
            moduleMenu.SelectedIndex = lastSelected;
        }

        private void OnBuyModulesCancel()
        {
            moduleMenu.IsActive = false;
            lastSelected = moduleMenu.SelectedIndex;
            moduleMenu.SelectedIndex = -1;
            menu.IsActive = true;
        }

        private void OnModuleSelected()
        {
            ModuleTemplate mt = unlockedModules[moduleMenu.SelectedIndex];
            if (SessionManager.ModuleTemplates.Count < SessionManager.UsedShipTemplate.ModuleCount && Player.Get().Credits >= mt.Price)
            {
                Sound.Instance.PlaySe("Buy");
                Player.Get().Credits -= mt.Price;
                SessionManager.ModuleTemplates.Add(mt);
            }else
            {
                Sound.Instance.PlaySe("Fail");
            }
        }

        private void RemoveLastModule()
        {
            if (SessionManager.ModuleTemplates.Count > 0)
            {
                Sound.Instance.PlaySe("Sell");
                Player.Get().Credits += SessionManager.ModuleTemplates[SessionManager.ModuleTemplates.Count - 1].Price;
                SessionManager.ModuleTemplates.RemoveAt(SessionManager.ModuleTemplates.Count - 1);
            }
        }

        private void OnStartSession()
        {
            SessionManager.Score = 0;
            SceneManager.Call<SceneSession>();
        }

        private void OnMenuBack()
        {
            while (SessionManager.ModuleTemplates.Count > 0)
            {
                Player.Get().Credits += SessionManager.ModuleTemplates[0].Price;
                SessionManager.ModuleTemplates.RemoveAt(0);
            }
            SceneManager.Return();
        }
    }
}
