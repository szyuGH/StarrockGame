using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StarrockGame.SceneManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StarrockGame
{
    public class StarrockGraphicsDeviceManager : GraphicsDeviceManager
    {
        readonly string[] supportedDisplayModes = new string[] {
            "800x600",
            "1024x768",
            "1280x960",
            "1280x1024",
            "1920x1080"
        };

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
            IEnumerable<DisplayMode> dmc = GetSupportedResolutions();
            DisplayMode res = dmc.ElementAt(resolutionId);
            this.PreferredBackBufferWidth = res.Width;
            this.PreferredBackBufferHeight = res.Height;
            this.ApplyChanges();
            SceneManager.SceneRenderTarget = new RenderTarget2D(GraphicsDevice, res.Width, res.Height);
        }

        public IEnumerable<DisplayMode> GetSupportedResolutions()
        {
            IEnumerable<DisplayMode> modes = GraphicsAdapter.DefaultAdapter.SupportedDisplayModes.Where(dm => dm.Format == SurfaceFormat.Color && supportedDisplayModes.Contains(string.Format("{0}x{1}", dm.Width, dm.Height)));
            return modes;
        }
    }
}
