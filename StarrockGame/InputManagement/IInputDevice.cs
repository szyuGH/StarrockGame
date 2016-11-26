using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StarrockGame.InputManagement
{
    public interface IInputDevice
    {
        void Update();

        float Acceleration();
        float Deceleration();
        float RotationLeft();
        float RotationRight();

        bool FirePrimary();
        bool FireSecondary();
        bool ReplenishingShield();
        bool Scavenging();

        bool ShowingStats();
        bool OpenMenu();

        bool MenuUp();
        bool MenuDown();
        bool MenuLeft();
        bool MenuRight();
        bool MenuSelect();
        bool MenuCancel();

        string InputTypeName(KeyboardInputType type);
    }
}
