using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StarrockGame.Caching;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StarrockGame
{
    public class Background
    {
        private Texture2D backTex = Cache.LoadGraphic("starfield");
        private Texture2D frontTex = Cache.LoadGraphic("starfield2");

        private float elapsed;

        public Background()
        {

        }
        

        public void Render(SpriteBatch batch, GameTime gameTime, Camera2D cam)
        {
            batch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, null, null, null, null, cam.Translation);
            batch.Draw(frontTex, EntityManager.Border.Center, null, Color.White, (float)SessionManager.ElapsedTime.TotalSeconds * 0.01f, new Vector2(frontTex.Bounds.Center.X, frontTex.Bounds.Center.Y), 2f, SpriteEffects.FlipVertically, 0);
            batch.Draw(frontTex, EntityManager.Border.Center + new Vector2(20, 20), null, Color.White, (float)SessionManager.ElapsedTime.TotalSeconds * 0.005f, new Vector2(frontTex.Bounds.Center.X, frontTex.Bounds.Center.Y), 2f, SpriteEffects.None, 0);
            batch.End();
        }
    }
}
