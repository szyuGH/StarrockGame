using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using static StarrockGame.Caching.Cache;

namespace StarrockGame.Caching
{
    public class DefaultContentLocator : IContentLocator
    {
        public string AudioContent { get { return "Audio"; } }

        public string FontContent { get { return "Fonts"; } }

        public string GraphicContent { get { return "Graphics"; } }

        public string SystemContent { get { return "Graphics/System"; } }

        public string ParticleContent { get { return "Graphics/Particles"; } }

        public string TemplateContent { get { return "Data/Templates"; } }

        public string EffectContent { get { return "Effects"; } }
    }
}
