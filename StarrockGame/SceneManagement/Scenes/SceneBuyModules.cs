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
        private int creditsToGo;

        private ModuleTemplate currentTemplate { get { return moduleMenu.SelectedIndex == -1 ? null : unlockedModules[moduleMenu.SelectedIndex]; } }


        public SceneBuyModules(Game1 game) : base(game)
        {
        }

        public override void Initialize()
        {
            creditsToGo = Player.Get().Credits;
            unlockedModules = Player.Get().GetTemplates<ModuleTemplate>();

            CreateMenu();
            CreateModuleMenu();
            CreateStatsMenu();
            
        }

        private void CreateMenu()
        {
            SpriteFont font = Cache.LoadFont("MenuFont");
            menu = new Menu(font, () => { OnMenuBack(); });

            Vector2 menuPos = new Vector2(100 + Device.Viewport.Width * .5f, Device.Viewport.Height - 40 - 4 * font.LineSpacing);
            new ButtonLabel(menu, "Select Modules", menuPos + new Vector2(0, font.LineSpacing * 0), 1, Color.White, OnBuyModules) { Active = unlockedModules.Count > 0 };
            new ButtonLabel(menu, "Next", menuPos + new Vector2(0, font.LineSpacing * 1), 1, Color.White, OnSelectDifficulty);
            new ButtonLabel(menu, "Back", menuPos + new Vector2(0, font.LineSpacing * 2), 1, Color.White, () => { OnMenuBack(); });
            new Label(menu, "", new Vector2(20, 40), 1, Color.White, 0) { CaptionMonitor = () => { return string.Format("Credits: {0} C", creditsToGo); } };

            menu.SelectNext();
        }

        private void CreateModuleMenu()
        {
            SpriteFont font = Cache.LoadFont("MenuFont");

            moduleMenu = new MatrixMenu(font, new Vector2(150, 100), Device.Viewport.Width / TILE_WIDTH_REAL, TILE_WIDTH, TILE_HEIGHT, TILE_SPACE, OnBuyModulesCancel);
            moduleMenu.IsActive = false;
            moduleMenu.SelectedIndex = -1;

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
            new Label(statsMenu, "Modules:", statsPosition, 1, Color.White, 0);
            for (int i= 0; i < SessionManager.UsedShipTemplate.ModuleCount; i++)
            {
                Label l = new Label(statsMenu, "", statsPosition + new Vector2(0, statsFont.LineSpacing * (i+1)), 1, Color.White, 0) { Tag = i };
                l.CaptionMonitor = () => GetModuleName((int)l.Tag);
            }


            for (int i = 0; i < 8; i++)
            {
                Label meLabel = new Label(statsMenu, "", statsPosition + new Vector2(150, statsFont.LineSpacing * i), 1, Color.White, 0) { Tag = i };
                meLabel.CaptionMonitor = () => GetModuleEffectString((int)meLabel.Tag);
            }
        }

        private string GetModuleName(int slot)
        {
            if (SessionManager.ModuleTemplates.Count >= slot + 1 && SessionManager.ModuleTemplates[slot] != null)
                return SessionManager.ModuleTemplates[slot].Name;
            else
                return "---";
        }

        private string GetModuleEffectString(int i)
        {
            if (currentTemplate?.ModuleEffects.Length >= i + 1)
            {
                return currentTemplate.ModuleEffects[i].ToString();
            }
            else
                return "";
        }

        public override void Update(GameTime gameTime)
        {
            menu.Update(gameTime);
            moduleMenu.Update(gameTime);
            statsMenu.Update(gameTime);

            if (Input.Device.FirePrimary())
            {
                RemoveLastModule();
            }
            if (creditsToGo != Player.Get().Credits)
            {
                creditsToGo = (int)MathHelper.Lerp(creditsToGo, Player.Get().Credits, (float)creditsToGo / Player.Get().Credits);
            }
        }

        public override void Render(GameTime gameTime)
        {
            base.Render(gameTime);
            SpriteBatch.Begin();
            menu.Render(SpriteBatch);
            moduleMenu.Render(SpriteBatch);
            statsMenu.Render(SpriteBatch);
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

        private void OnSelectDifficulty()
        {
            SceneManager.Call<SceneSelectDifficulty>();
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
