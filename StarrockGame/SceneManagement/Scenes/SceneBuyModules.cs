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
        private MatrixMenu moduleMenu;
        private List<ModuleTemplate> moduleTemplates;
        private int lastSelected = 0;
        

        public SceneBuyModules(Game1 game) : base(game)
        {
        }

        public override void Initialize()
        {
            moduleTemplates = Player.Get().GetTemplates<ModuleTemplate>();
            SpriteFont font = Cache.LoadFont("MenuFont");

            menu = new Menu(font, () => { SceneManager.Return(); });
            moduleMenu = new MatrixMenu(font, new Vector2(150, 100), Device.Viewport.Width / TILE_WIDTH_REAL, TILE_WIDTH, TILE_HEIGHT, TILE_SPACE, OnBuyModulesCancel);
            moduleMenu.IsActive = false;

            for (int i = 0; i < moduleTemplates.Count; i++)
            {
                moduleMenu.AddButtonLabel(moduleTemplates[i].Name, Color.White, OnModuleSelected);
            }

            new ButtonLabel(menu, "Select Modules", new Vector2(Device.Viewport.Width * .5f, Device.Viewport.Height - 40 - 3 * font.LineSpacing), 1, Color.White, OnBuyModules) { Active = moduleTemplates.Count > 0 };
            new ButtonLabel(menu, "Start Session", new Vector2(Device.Viewport.Width * .5f, Device.Viewport.Height - 40 - 2 * font.LineSpacing), 1, Color.White, OnStartSession);
            new ButtonLabel(menu, "Back", new Vector2(Device.Viewport.Width * .5f, Device.Viewport.Height - 40 - 1 * font.LineSpacing), 1, Color.White, () => { SceneManager.Return(); });
            new Label(menu, "", new Vector2(10, 10), 1, Color.White, 0) { CaptionMonitor = () => { return "Credits: " + Player.Get().Credits; } };
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
            ModuleTemplate mt = moduleTemplates[moduleMenu.SelectedIndex];
            if (SessionManager.ModuleTemplates.Count < SessionManager.UsedShipTemplate.ModuleCount && Player.Get().Credits >= mt.Price)
            {
                Player.Get().Credits -= mt.Price;
                SessionManager.ModuleTemplates.Add(mt);
            }
        }

        private void RemoveLastModule()
        {
            if (SessionManager.ModuleTemplates.Count > 0)
            {
                Player.Get().Credits += SessionManager.ModuleTemplates[SessionManager.ModuleTemplates.Count - 1].Price;
                SessionManager.ModuleTemplates.RemoveAt(SessionManager.ModuleTemplates.Count - 1);
            }
        }

        private void OnStartSession()
        {
            SessionManager.Score = 0;
            SceneManager.Call<SceneSession>();
        }
    }
}
