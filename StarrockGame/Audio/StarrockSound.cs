using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StarrockGame.Audio
{
    public class StarrockSound : GameSound
    {
        public override void PlayCancel()
        {
            Sound.Instance.PlaySe("cancel");
        }

        public override void PlayCursor()
        {
            Sound.Instance.PlaySe("cursor");
        }

        public override void PlaySelection()
        {
            Sound.Instance.PlaySe("decision1");
        }
    }
}

