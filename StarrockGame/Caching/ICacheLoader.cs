using Microsoft.Xna.Framework.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StarrockGame.Caching
{
    public interface ICacheLoader
    {
        void Preload(ContentManager content);
    }
}
