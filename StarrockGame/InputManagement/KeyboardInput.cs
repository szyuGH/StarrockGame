using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StarrockGame.InputManagement
{
    public class KeyboardInput : IInputDevice
    {
        private KeyboardState kbState, kbStateOld;

        public void Update()
        {
            kbStateOld = kbState;
            kbState = Keyboard.GetState();
        }

        public float Acceleration()
        {
            return LerpInput(KeyboardInputType.Accelerate);
        }

        public float Deceleration()
        {
            return LerpInput(KeyboardInputType.Decelerate);
        }

        public float RotationLeft()
        {
            return LerpInput(KeyboardInputType.TurnLeft);
        }

        public float RotationRight()
        {
            return LerpInput(KeyboardInputType.TurnRight);
        }

        public bool FirePrimary()
        {
            return IsKeyDown(KeyboardInputType.PrimaryWeapon);
        }

        public bool FireSecondary()
        {
            return IsKeyDown(KeyboardInputType.SecondaryWeapon);
        }

        public bool ReplenishingShield()
        {
            return IsKeyDown(KeyboardInputType.ReplenishShield);
        }

        public bool Scavenging()
        {
            return IsKeyDown(KeyboardInputType.Scavenge);
        }

        public bool ShowingStats()
        {
            return IsKeyDown(KeyboardInputType.ShowStats);
        }

        public bool OpenMenu()
        {
            return IsKeyTriggered(KeyboardInputType.Menu);
        }

        public bool MenuUp()
        {
            return IsKeyTriggered(KeyboardInputType.MenuUp);
        }

        public bool MenuDown()
        {
            return IsKeyTriggered(KeyboardInputType.MenuDown);
        }

        public bool MenuLeft()
        {
            return IsKeyTriggered(KeyboardInputType.MenuLeft);
        }

        public bool MenuRight()
        {
            return IsKeyTriggered(KeyboardInputType.MenuRight);
        }

        public bool MenuSelect()
        {
            return IsKeyTriggered(KeyboardInputType.MenuSelect);
        }

        public bool MenuCancel()
        {
            return IsKeyTriggered(KeyboardInputType.MenuCancel);
        }

        private bool IsKeyDown(KeyboardInputType type)
        {
            return kbState.IsKeyDown(keyboardMapping[type]);
        }

        private bool IsKeyTriggered(KeyboardInputType type)
        {
            return IsKeyDown(type) && kbStateOld.IsKeyUp(keyboardMapping[type]);
        }

        private float LerpInput(KeyboardInputType type)
        {
            if (IsKeyDown(type))
                return 1;
            else
                return 0;
        }

        private static Dictionary<KeyboardInputType, Keys> keyboardMapping = new Dictionary<KeyboardInputType, Keys>()
        {
            { KeyboardInputType.Accelerate, Keys.W },
            { KeyboardInputType.Decelerate, Keys.S },
            { KeyboardInputType.TurnLeft, Keys.A },
            { KeyboardInputType.TurnRight, Keys.D },
            { KeyboardInputType.PrimaryWeapon, Keys.J },
            { KeyboardInputType.SecondaryWeapon, Keys.L },
            { KeyboardInputType.ReplenishShield, Keys.K },
            { KeyboardInputType.Scavenge, Keys.Space },
            { KeyboardInputType.ShowStats, Keys.LeftAlt },
            { KeyboardInputType.Menu, Keys.Escape },

            { KeyboardInputType.MenuUp, Keys.W },
            { KeyboardInputType.MenuDown, Keys.S},
            { KeyboardInputType.MenuLeft, Keys.A },
            { KeyboardInputType.MenuRight, Keys.D },
            { KeyboardInputType.MenuSelect, Keys.Space },
            { KeyboardInputType.MenuCancel, Keys.Escape },
        };
    }
}
