using FarseerPhysics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StarrockGame.Caching;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StarrockGame.Entities
{
    public class Scavenging
    {
        private AnimatedTexture scavengeAtlas;
        private Spaceship ship;

        public Wreckage Target;
        public float progressTimer;

        public bool Active { get { return Target != null; } }
        public float Progress { get { return progressTimer / Target.ScavengeTime; } }
        public float Range { get; private set; }

        private Action onSuccessAction;

        public Scavenging(Spaceship ship, float range, Action onSuccessAction)
        {
            this.ship = ship;
            Range = range;
            this.onSuccessAction = onSuccessAction;

            scavengeAtlas = new AnimatedTexture("tractor_beam_atlas", new Vector2(0, 16), 4, 1);
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
                if (Vector2.DistanceSquared(ship.Body.Position, Target.Body.Position) > Range * Range)
                {
                    Reset();
                }
                else
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

        public void Render(SpriteBatch batch, GameTime gameTime)
        {
            if (Active)
            {
                Vector2 mypos = ConvertUnits.ToDisplayUnits(ship.Body.Position);
                Vector2 dif = ConvertUnits.ToDisplayUnits(Target.Body.Position) - mypos;

                int size = (int)dif.Length();
                Rectangle bounds = new Rectangle((int)mypos.X, (int)mypos.Y, size, (int)scavengeAtlas.TileCenter.Y * 2);
                float rot = (float)Math.Atan2(dif.Y, dif.X);

                scavengeAtlas.Draw(batch, gameTime, bounds, Color.White, rot);
            }
        }
    }
}
