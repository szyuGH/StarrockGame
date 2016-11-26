using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StarrockGame.InputManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StarrockGame.SceneManagement.Scenes
{
    public class SceneIntro : Scene
    {
        const float INTRO_TIME = 2;
        protected override Color ClearColor
        {
            get
            {
                return Color.CornflowerBlue;
            }
        }

        private Texture2D logo;
        private float timer;

        public SceneIntro(Game1 game) : base(game)
        {
            logo = Content.Load<Texture2D>("Graphics/logo");
            timer = 0;
        }

        public override void Update(float elapsed)
        {
            timer += elapsed;

            if (timer >= INTRO_TIME || Input.Device.MenuSelect())
            {
                SceneManager.Set<SceneTitle>();
            }
        }
        

        public override void Render(GameTime gameTime)
        {
            base.Render(gameTime);
            SpriteBatch.Begin();
            Vector2 screenCenter = new Vector2(Device.Viewport.Width, Device.Viewport.Height) * .5f;
            Vector2 logoCenter = new Vector2(logo.Width, logo.Height) * .5f;
            
            SpriteBatch.Draw(logo, screenCenter, null, Color.White, 0, logoCenter, 0.5f, SpriteEffects.None, 1);
            SpriteBatch.End();
        }
        
    }
}
