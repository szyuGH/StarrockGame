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
    public class SceneTest : Scene
    {
        SpriteFont font;

        public SceneTest(Game1 game) : base(game)
        {
            font = Content.Load<SpriteFont>("GameFont");
        }

        public override void Update(float elapsed)
        {
            if (Input.Device.MenuRight())
                SceneManager.Return();
        }

        public override void Render()
        {
            Device.Clear(Color.CornflowerBlue);
            SpriteBatch.Begin();
            SpriteBatch.DrawString(font, "This is the TestScene", new Vector2(200, 130), Color.White);
            SpriteBatch.End();
        }

        
    }
}
