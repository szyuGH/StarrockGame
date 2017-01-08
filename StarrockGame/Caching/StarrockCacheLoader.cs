using StarrockGame.Caching;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using System.IO;

namespace StarrockGame.Caching
{
    public class StarrockCacheLoader : ICacheLoader
    {
        public void Preload(ContentManager content)
        {
            // Preload audio files to prevent lag on first load
            foreach (string cp in Directory.EnumerateFiles("Content/Audio/Se", "*", SearchOption.AllDirectories).Select(f => Path.GetFileNameWithoutExtension(f)))
            {
                Cache.LoadSe(cp);
            }

            //Cache.LoadSe("cursor");
            //Cache.LoadSe("cancel");
            //Cache.LoadSe("decision1");
            //Cache.Load<Texture2D>("Graphics/Devastator");
        }
    }
}
