using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StarrockGame.Entities
{
    [Flags]
    public enum MovementType : byte
    {
        Forward = 0x1,
        Brake = 0x2,
        RotateLeft = 0x4,
        RotateRight = 0x8
    }
}
