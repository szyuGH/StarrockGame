using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StarrockGame.InputManagement
{
    public static class Input
    {
        public static IInputDevice Device { get; set; }
        public static event EventHandler ControllerDisconnectedEvent;

        /// <summary>
        /// Initializes player input with a game controller as default device if present, else keyboard
        /// </summary>
        public static void Initialize()
        {
            if (GamePad.GetState(0).IsConnected)
            {
                Device = new ControllerInput();
            } else
            {
                Device = new KeyboardInput();
            }
        }

        /// <summary>
        /// Updates the set device if present
        /// </summary>
        public static void Update()
        {
            Device?.Update();
            // check if controller is still connected when device is controller
            if (typeof(ControllerInput).Equals(Device.GetType()))
            {
                if (!GamePad.GetState(0).IsConnected)
                {
                    Device = new KeyboardInput();
                    ControllerDisconnectedEvent?.Invoke(null, null);
                }
                else if (Keyboard.GetState().GetPressedKeys().Length > 0)
                {
                    Device = new KeyboardInput();
                }

            }
            else if (typeof(KeyboardInput).Equals(Device.GetType()))
            {
                if (GamePad.GetState(0).IsButtonDown(Buttons.Start))
                {
                    Device = new ControllerInput();
                }
            }
        }
    }
}
