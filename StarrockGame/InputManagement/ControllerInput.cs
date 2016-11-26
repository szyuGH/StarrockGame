using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StarrockGame.InputManagement
{
    public class ControllerInput : IInputDevice
    {
        private GamePadState gpState, gpStateOld;

        public void Update()
        {
            gpStateOld = gpState;
            gpState = GamePad.GetState(0);
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
            return gpState.IsButtonDown(controllerMapping[type]);
        }

        private bool IsKeyTriggered(KeyboardInputType type)
        {
            return IsKeyDown(type) && gpStateOld.IsButtonUp(controllerMapping[type]);
        }

        private float LerpInput(KeyboardInputType type)
        {
            switch (controllerMapping[type])
            {
                case Buttons.RightTrigger:
                    return gpState.Triggers.Right;
                case Buttons.LeftTrigger:
                    return gpState.Triggers.Left;
                case Buttons.LeftThumbstickLeft:
                case Buttons.LeftThumbstickRight:
                    return gpState.ThumbSticks.Left.X;
                default:
                    return IsKeyDown(type) ? 1 : 0;
            }
        }

        public string InputTypeName(KeyboardInputType type)
        {
            return controllerMapping[type].ToString();
        }

        private static Dictionary<KeyboardInputType, Buttons> controllerMapping = new Dictionary<KeyboardInputType, Buttons>()
        {
            { KeyboardInputType.Accelerate, Buttons.RightTrigger },
            { KeyboardInputType.Decelerate, Buttons.LeftTrigger },
            { KeyboardInputType.TurnLeft, Buttons.LeftThumbstickLeft },
            { KeyboardInputType.TurnRight, Buttons.LeftThumbstickRight },
            { KeyboardInputType.PrimaryWeapon, Buttons.X },
            { KeyboardInputType.SecondaryWeapon, Buttons.Y },
            { KeyboardInputType.ReplenishShield, Buttons.B },
            { KeyboardInputType.Scavenge, Buttons.A },
            { KeyboardInputType.ShowStats, Buttons.Back },
            { KeyboardInputType.Menu, Buttons.Start },

            { KeyboardInputType.MenuUp,     Buttons.DPadUp },
            { KeyboardInputType.MenuDown,   Buttons.DPadDown},
            { KeyboardInputType.MenuLeft,   Buttons.DPadLeft},
            { KeyboardInputType.MenuRight,  Buttons.DPadRight},
            { KeyboardInputType.MenuSelect, Buttons.A },
            { KeyboardInputType.MenuCancel, Buttons.B },
        };
    }
}
