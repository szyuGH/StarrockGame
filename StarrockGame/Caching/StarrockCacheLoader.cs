using StarrockGame.Caching;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;

namespace StarrockGame.Caching
{
    public class StarrockCacheLoader : ICacheLoader
    {
        public void Preload(ContentManager content)
        {
            //Cache.LoadSe("cursor");
            //Cache.LoadSe("cancel");
            //Cache.LoadSe("decision1");
            //Cache.Load<Texture2D>("Graphics/Devastator");
        }
    }
}
