using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StarrockGame.InputManagement
{
    public enum KeyboardInputType : byte
    {
        Accelerate,
        Decelerate,
        TurnLeft,
        TurnRight,
        PrimaryWeapon,
        SecondaryWeapon,
        RecoverShield,
        Scavenge,
        ShowStats,
        Menu,

        MenuUp,
        MenuDown,
        MenuLeft,
        MenuRight,
        MenuSelect,
        MenuCancel
    }
}
