﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StarrockGame.Caching;
using StarrockGame.GUI;
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
            SpriteFont font = Cache.LoadFont("MenuFont");
            menu = new Menu(font, null);

            Vector2 screenCenter = new Vector2(Device.Viewport.Width, Device.Viewport.Height) * .5f;
            float size = 1;

            new ButtonLabel(menu, "New Session",    screenCenter + new Vector2(0, 0 * font.LineSpacing), size, Color.White, OnNewSession);
            new ButtonLabel(menu, "Leaderboard",    screenCenter + new Vector2(0, 1 * font.LineSpacing), size, Color.White, OnLeaderboard);
            new ButtonLabel(menu, "Controls",       screenCenter + new Vector2(0, 2 * font.LineSpacing), size, Color.White, OnControls);
            new ButtonLabel(menu, "Exit",           screenCenter + new Vector2(0, 3 * font.LineSpacing), size, Color.White, OnExit);
        }

        public override void Update(float elapsed)
        {
            menu.Update(elapsed);
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
            SceneManager.Call<SceneLeaderboard>();
        }

        private void OnControls()
        {
            SceneManager.Call<SceneControls>();
        }

        private void OnExit()
        {
            SceneManager.Exit();
        }
    }
}
