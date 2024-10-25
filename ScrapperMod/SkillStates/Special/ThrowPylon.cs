using EntityStates;
using RoR2;
using RoR2.Projectile;
using UnityEngine;

namespace Scrapper.SkillStates.Special
{
    public class ThrowPylon : BaseState
    {
        public static GameObject projectilePrefab;

        public static float baseDuration;

        public static float damageCoefficient;

        public static string muzzleString;

        public static GameObject muzzleflashObject;

        public static string soundString;

        private float duration;

        public override void OnEnter()
        {
            base.OnEnter();
            this.duration = ThrowPylon.baseDuration / base.attackSpeedStat;
            if (base.isAuthority)
            {
                var aimRay = base.GetAimRay();
                FireProjectileInfo fireProjectileInfo = new()
                {
                    crit = base.RollCrit(),
                    damage = base.damageStat * ThrowPylon.damageCoefficient,
                    damageColorIndex = DamageColorIndex.Default,
                    force = 0f,
                    owner = base.gameObject,
                    position = aimRay.origin,
                    procChainMask = default,
                    projectilePrefab = ThrowPylon.projectilePrefab,
                    rotation = Quaternion.LookRotation(aimRay.direction),
                    target = null
                };
                ProjectileManager.instance.FireProjectile(fireProjectileInfo);
            }
            EffectManager.SimpleMuzzleFlash(ThrowPylon.muzzleflashObject, base.gameObject, ThrowPylon.muzzleString, transmit: false);
            Util.PlaySound(ThrowPylon.soundString, base.gameObject);
        }

        public override void FixedUpdate()
        {
            if (this.inputBank.skill4.justPressed)
                base.FixedUpdate();
            if (base.isAuthority && this.duration <= base.age)
            {
                base.outer.SetNextStateToMain();
            }
        }

        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.Frozen;
        }
    }
}
