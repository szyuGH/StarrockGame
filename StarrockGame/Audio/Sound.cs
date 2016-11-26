using StarrockGame.Caching;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;

namespace StarrockGame.Audio
{
    public class Sound
    {
        public float FadeTime = 2; // Fade time in seconds

        private SoundEffectInstance currentBgm;
        private SoundEffectInstance nextBgm;

        public FadingType State { get; private set; }

        private static Sound instance = new Sound();
        public static Sound Instance { get { return instance; } }
        
        public static GameSound GameSound { get; set; }

        protected Sound() { }
        

        /// <summary>
        /// Plays the specified background music file from the "Audio/BGM/" content directory.
        /// </summary>
        public void PlayBgm(string name, float volume = 0, float pitch = 0, float pan = 0, bool loop=true)
        {
            nextBgm = Cache.LoadBgm(name).CreateInstance();
            nextBgm.Volume = volume;
            nextBgm.Pitch = pitch;
            nextBgm.Pan = pan;
            nextBgm.IsLooped = loop;
            
            FadeBgm();
        }

        /// <summary>
        /// Fade out the old bgm and fade in the new bgm.
        /// </summary>
        private void FadeBgm()
        {
            if (currentBgm?.State == SoundState.Playing)
            {
                FadeOut();
            }
            else
            {
                FadeIn();
            }
        }

        private void FadeOut()
        {
            Thread fadeoutThread = new Thread(new ThreadStart(() =>
            {
                State = FadingType.FadeOut;
                Stopwatch sw = new Stopwatch();
                sw.Start();
                while (sw.Elapsed.TotalSeconds < FadeTime)
                {
                    currentBgm.Volume = MathHelper.Lerp(0, 1, 1-(float)sw.Elapsed.TotalSeconds / FadeTime);
                }
                sw.Stop();
                currentBgm.Dispose();
                if (nextBgm != null)
                    FadeIn();
                else
                {
                    currentBgm = null;
                    State = FadingType.None;
                }
            }));
            fadeoutThread.IsBackground = true;
            fadeoutThread.Start();
        }

        private void FadeIn()
        {
            Thread fadeinThread = new Thread(new ThreadStart(() =>
            {
                State = FadingType.FadeIn;
                currentBgm = nextBgm;
                currentBgm.Play();
                Stopwatch sw = new Stopwatch();
                sw.Start();
                while (sw.Elapsed.TotalSeconds < FadeTime)
                {
                    currentBgm.Volume = MathHelper.Lerp(0, 1, (float)sw.Elapsed.TotalSeconds / FadeTime);
                }
                sw.Stop();
                nextBgm = null;
                State = FadingType.None;
            }));
            fadeinThread.IsBackground = true;
            fadeinThread.Start();
        }

        /// <summary>
        /// Plays the specified sound file from the "Audio/SE/" content directory.
        /// </summary>
        public void PlaySe(string name, float volume=1, float pitch=0, float pan = 0)
        {
            SoundEffectInstance se = Cache.LoadSe(name).CreateInstance();
            se.Volume = volume;
            se.Pitch = pitch;
            se.Pan = pan;

            se.Play();
        }

        


        public enum FadingType
        {
            None, FadeIn, FadeOut
        }
    }
}
