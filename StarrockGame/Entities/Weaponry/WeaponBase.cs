using FarseerPhysics;
using FarseerPhysics.Dynamics;
using Microsoft.Xna.Framework;
using StarrockGame.Audio;
using StarrockGame.Caching;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TData.TemplateData;

namespace StarrockGame.Entities.Weaponry
{
    public abstract class WeaponBase
    {
        private Vector2 localPosition;
        private float localAngle;
        private float cooldown;

        protected Entity Parent { get; private set; }
        protected WeaponBaseData BaseTemplate { get; private set; }
        protected WeaponTemplate WeaponTemplate { get; private set; }

        public bool CanShoot { get { return cooldown <= 0; } }

        /// <summary>
        /// Returns a vector4. X and Y represent the position, Z and W represent the direction
        /// </summary>
        protected WeaponTransform Transform { get
            {
                Matrix trMatrix = Matrix.CreateTranslation(new Vector3(-ConvertUnits.ToDisplayUnits(Parent.Body.Position), 0))
                        * Matrix.CreateTranslation(new Vector3(localPosition, 0))
                        * Matrix.CreateRotationZ(Parent.Body.Rotation)
                        * Matrix.CreateTranslation(new Vector3(ConvertUnits.ToDisplayUnits(Parent.Body.Position), 0));
                Vector2 emitPosition = Vector2.Transform(ConvertUnits.ToDisplayUnits(Parent.Body.Position), trMatrix);

                float rot = MathHelper.WrapAngle(Parent.Body.Rotation + localAngle);
                Vector2 dir = new Vector2((float)Math.Cos(rot), (float)Math.Sin(rot));

                return new WeaponTransform(emitPosition, rot, dir);
            }
        }

        public WeaponBase(Entity parent, WeaponBaseData baseTemplate, WeaponTemplate weaponTemplate, Vector2 localPosition, float localAngle)
        {
            this.Parent = parent;
            this.BaseTemplate = baseTemplate;
            this.WeaponTemplate = weaponTemplate;
            this.localPosition = localPosition;
            this.localAngle = localAngle;
        }

        public void Update(GameTime gameTime)
        {
            if (cooldown > 0)
            {
                cooldown -= (float)gameTime.ElapsedGameTime.TotalSeconds;
            }
        }

        public float Fire(float energy)
        {
            if (CanShoot && energy > WeaponTemplate.EnergyCost)
            {
                Sound.Instance.PlaySe(WeaponTemplate.SoundFile, 1 - MathHelper.Clamp(Vector2.Distance(EntityManager.PlayerShip.Body.Position, Parent.Body.Position) / SoundEmitter.MAX_RANGE, 0, 1));
                DoFire();
                cooldown = WeaponTemplate.Cooldown;
                return WeaponTemplate.EnergyCost;
                
            }
            return 0;
        }
        protected abstract void DoFire();



        internal static WeaponBase[] FromTemplate(Entity parent, WeaponBaseData data)
        {
            // preload weapon template to define type
            WeaponTemplate wt = Cache.LoadTemplate<WeaponTemplate>(data.WeaponType);

            WeaponBase[] bases = new WeaponBase[data.Bases.Length];
            for (int i = 0; i < bases.Length; i++)
            {
                bases[i] = (WeaponBase)Activator.CreateInstance(SelectWeaponClass((WeaponType)wt.WeaponType),
                    parent, data, wt, data.Bases[i].LocalPosition, data.Bases[i].LocalAngle);
            }
            return bases;
        }

        private static Type SelectWeaponClass(WeaponType type)
        {
            switch (type)
            {
                case WeaponType.LaserBullet: return typeof(LaserBulletBase);
                //case WeaponType.LaserBeam: return typeof(LaserBeamBase);
                case WeaponType.Rocket: return typeof(RocketBase);
                case WeaponType.Missile: return typeof(HomingMissileBase);
                case WeaponType.Mine: return typeof(MineBase);
            }
            throw new Exception("Weapon Type \"" + type.ToString() + "\" not defined!");
        }
    }
}
