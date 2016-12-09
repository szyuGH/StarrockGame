using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StarrockGame.GUI
{
    public interface ISelectable
    {
        Action Select { get; }
        void OnSelect();
    }
}
