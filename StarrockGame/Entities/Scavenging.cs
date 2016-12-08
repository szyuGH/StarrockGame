using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StarrockGame.Entities
{
    public class Scavenging
    {
        
        public Wreckage Target;
        public float progressTimer;

        public bool Active { get { return Target != null; } }
        public float Progress { get { return progressTimer / Target.ScavengeTime; } }
        public float Range { get; private set; }

        private Action onSuccessAction;

        public Scavenging(float range, Action onSuccessAction)
        {
            Range = range;
            this.onSuccessAction = onSuccessAction;
        }

        public void Reset()
        {
            progressTimer = 0;
            Target = null;
        }

        public void Update(GameTime gameTime)
        {
            if (Active)
            {
                progressTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;
                if (progressTimer >= Target.ScavengeTime)
                {
                    progressTimer = Target.ScavengeTime;
                    onSuccessAction?.Invoke();
                }
            }
        }
    }
}
