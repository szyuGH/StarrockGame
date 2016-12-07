using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StarrockGame
{
    public class StarrockGraphicsDeviceManager : GraphicsDeviceManager
    {


        public StarrockGraphicsDeviceManager(Game game, bool fullscreen=false) 
            : base(game)
        {
            this.PreferMultiSampling = true;
            this.PreferredBackBufferWidth = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width;
            this.PreferredBackBufferHeight = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height;
            this.PreferredBackBufferFormat = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Format;
            this.IsFullScreen = fullscreen;
        }

        public void SetResolution(int resolutionId)
        {
            DisplayModeCollection dmc = GetSupportedResolutions();
            DisplayMode res = dmc.ElementAt(resolutionId);
            this.PreferredBackBufferWidth = res.Width;
            this.PreferredBackBufferHeight = res.Height;
            this.ApplyChanges();
        }

        public DisplayModeCollection GetSupportedResolutions()
        {
            return GraphicsAdapter.DefaultAdapter.SupportedDisplayModes;
        }
    }
}
