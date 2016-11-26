using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StarrockGame.GUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StarrockGame.SceneManagement.Scenes
{
    public class SceneLeaderboard : Scene
    {
        const float RETRIEVE_INTERVAL = 10;

        Menu menu;
        private float verticalMeasure;

        float retrieveTimer = RETRIEVE_INTERVAL;
        

        public SceneLeaderboard(Game1 game) : base(game)
        {
            SpriteFont font = Content.Load<SpriteFont>("Fonts/MenuFont");
            menu = new Menu(font, () => { SceneManager.Return(); });
            verticalMeasure = font.MeasureString("WYjy|").Y;
            
            RetrieveLeaderboard();
        }

        public override void Update(float elapsed)
        {
            if (retrieveTimer <= 0)
            {
                RetrieveLeaderboard();
                retrieveTimer = RETRIEVE_INTERVAL;
            }
            menu.Update(elapsed);
        }

        public override void Render(GameTime gameTime)
        {
            SpriteBatch.Begin();
            menu.Render(SpriteBatch);
            SpriteBatch.End();
        }

        private void RetrieveLeaderboard()
        {

        }
    }
}
