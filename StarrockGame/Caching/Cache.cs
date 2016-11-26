using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace StarrockGame.Caching
{
    public static class Cache
    {
        private static ContentManager content;
        private static IContentLocator locator;

        public static void Initialize(ContentManager con, IContentLocator loc=null, ICacheLoader loader=null)
        {
            content = con;
            locator = loc ?? new DefaultContentLocator();
            loader?.Preload(content);
        }

        public static void Dispose()
        {
            content.Unload();
            content.Dispose();
        }

        public static Texture2D LoadSystem(string name)
        {
            return content.Load<Texture2D>(string.Format("{0}/{1}", locator.SystemContent, name));
        }

        public static T Load<T>(string path)
        {
            return content.Load<T>(path);
        }

        public static Texture2D LoadParticle(string name)
        {
            return content.Load<Texture2D>(string.Format("{0}/{1}", locator.ParticleContent, name));
        }

        public static SoundEffect LoadSe(string name)
        {
            return content.Load<SoundEffect>(string.Format("{0}/se/{1}", locator.AudioContent, name));
        }

        public static SoundEffect LoadBgm(string name)
        {
            return content.Load<SoundEffect>(string.Format("{0}/bgm/{1}", locator.AudioContent, name));
        }

        public static SpriteFont LoadFont(string name)
        {
            return content.Load<SpriteFont>(string.Format("{0}/{1}", locator.FontContent, name));
        }

        public static T LoadTemplate<T>(string name)
        {
            return content.Load<T>(string.Format("{0}/{1}", locator.TemplateContent, name));
        }

        public static Effect LoadEffect(string name)
        {
            return content.Load<Effect>(string.Format("{0}/{1}", locator.EffectContent, name));
        }

        public interface IContentLocator
        {
            string SystemContent { get; }
            string GraphicContent { get; }
            string AudioContent { get; }
            string FontContent { get; }
            string ParticleContent { get; }
            string TemplateContent { get; }
            string EffectContent { get; }
        }
    }
}
