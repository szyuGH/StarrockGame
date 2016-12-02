using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using StarrockGame.InputManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StarrockGame.SceneManagement.Scenes
{
    public class SceneControls : Scene
    {
        const float TIMER_INTERVAL = 2;

        SpriteFont font;

        float acceleration;
        float deceleration;
        float rotation;

        bool firingPrimary;
        bool firingSecondary;
        bool scavenging;
        bool replenishingShield;

        bool showingStats;

        float openMenuTimer;
        float menuUpTimer;
        float menuDownTimer;
        float menuLeftTimer;
        float menuRightTimer;
        float menuSelectTimer;
        float menuCancelTimer;

        public SceneControls(Game1 game) : base(game)
        {
            font = Content.Load<SpriteFont>("Fonts/MenuFont");
            
        }

        public override void Update(GameTime gameTime)
        {
            if (Keyboard.GetState().IsKeyDown(Keys.Enter))
                SceneManager.Return();

            acceleration = Input.Device.Acceleration();
            deceleration = Input.Device.Deceleration();
            rotation = Input.Device.Rotation();
            firingPrimary = Input.Device.FirePrimary();
            firingSecondary = Input.Device.FireSecondary();
            replenishingShield = Input.Device.ReplenishingShield();
            scavenging = Input.Device.Scavenging();
            showingStats = Input.Device.ShowingStats();

            if (Input.Device.OpenMenu()) openMenuTimer = TIMER_INTERVAL;
            if (Input.Device.MenuUp()) menuUpTimer = TIMER_INTERVAL;
            if (Input.Device.MenuDown()) menuDownTimer = TIMER_INTERVAL;
            if (Input.Device.MenuLeft()) menuLeftTimer = TIMER_INTERVAL;
            if (Input.Device.MenuRight()) menuRightTimer = TIMER_INTERVAL;
            if (Input.Device.MenuSelect()) menuSelectTimer = TIMER_INTERVAL;
            if (Input.Device.MenuCancel()) menuCancelTimer = TIMER_INTERVAL;

            UpdateTimers(gameTime);
        }

        private void UpdateTimers(float elapsed)
        {
            if (openMenuTimer > 0)
                openMenuTimer -= elapsed;
            if (menuUpTimer > 0)
                menuUpTimer -= elapsed;
            if (menuDownTimer > 0)
                menuDownTimer -= elapsed;
            if (menuLeftTimer > 0)
                menuLeftTimer -= elapsed;
            if (menuRightTimer > 0)
                menuRightTimer -= elapsed;
            if (menuSelectTimer > 0)
                menuSelectTimer -= elapsed;
            if (menuCancelTimer > 0)
                menuCancelTimer -= elapsed;
        }

        public override void Render(GameTime gameTime)
        {
            base.Render(gameTime);
            SpriteBatch.Begin();

            SpriteBatch.DrawString(font, string.Format("Acceleration: {0}", acceleration), new Vector2(10, 10), Color.White);
            SpriteBatch.DrawString(font, string.Format("Deceleration: {0}", deceleration), new Vector2(10, 30), Color.White);
            SpriteBatch.DrawString(font, string.Format("Rotation Left: {0}", rotation), new Vector2(10, 50), Color.White);
            SpriteBatch.DrawString(font, string.Format("Rotation Right: {0}", rotationRight), new Vector2(10, 70), Color.White);
                                                      
            SpriteBatch.DrawString(font, string.Format("Firing Primary: {0}", firingPrimary), new Vector2(10, 90), Color.White);
            SpriteBatch.DrawString(font, string.Format("Firing Secondary: {0}", firingSecondary), new Vector2(10, 110), Color.White);
            SpriteBatch.DrawString(font, string.Format("Replenishing Shield: {0}", replenishingShield), new Vector2(10, 130), Color.White);
            SpriteBatch.DrawString(font, string.Format("Scavenging: {0}", scavenging), new Vector2(10, 150), Color.White);

            SpriteBatch.DrawString(font, string.Format("Showing Stats: {0}", showingStats), new Vector2(10, 170), Color.White);

            SpriteBatch.DrawString(font, string.Format("Device Type: {0}", Input.Device.GetType().Name), new Vector2(10, 220), Color.White);

            SpriteBatch.DrawString(font, string.Format("Opened Menu: {0}", (openMenuTimer > 0 ? "O" : "-")), new Vector2(300, 10), Color.White);
            SpriteBatch.DrawString(font, string.Format("Menu Up: {0}", (menuUpTimer > 0 ? "O" : "-")), new Vector2(300, 30), Color.White);
            SpriteBatch.DrawString(font, string.Format("Menu Down: {0}", (menuDownTimer > 0 ? "O" : "-")), new Vector2(300, 50), Color.White);
            SpriteBatch.DrawString(font, string.Format("Menu Left: {0}", (menuLeftTimer > 0 ? "O" : "-")), new Vector2(300, 70), Color.White);
            SpriteBatch.DrawString(font, string.Format("Menu Right: {0}", (menuRightTimer > 0 ? "O" : "-")), new Vector2(300, 90), Color.White);
            SpriteBatch.DrawString(font, string.Format("Menu Select: {0}", (menuSelectTimer > 0 ? "O" : "-")), new Vector2(300, 110), Color.White);
            SpriteBatch.DrawString(font, string.Format("Menu Cancel: {0}", (menuCancelTimer > 0 ? "O" : "-")), new Vector2(300, 130), Color.White);


            SpriteBatch.End();
        }
    }
}
