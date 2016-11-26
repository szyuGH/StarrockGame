using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StarrockGame.Audio
{
    public abstract class GameSound
    {
        public abstract void PlayCancel(); 

        public abstract void PlayCursor();

        public abstract void PlaySelection();
    }
}
