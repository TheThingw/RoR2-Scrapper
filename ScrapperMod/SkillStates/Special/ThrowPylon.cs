using EntityStates;
using RoR2;
using RoR2.Projectile;
using Scrapper.Content;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Scrapper.SkillStates.Special
{
    public class ThrowPylon : BaseState
    {
        public static GameObject projectilePrefab = Addressables.LoadAssetAsync<GameObject>("RoR2/Base/Loader/LoaderPylon.prefab").WaitForCompletion();

        public static float baseDuration = 1f;

        public static float damageCoefficient = 1f;

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
            EffectManager.SimpleMuzzleFlash(EntityStates.Loader.ThrowPylon.muzzleflashObject, base.gameObject, StaticValues.MUZZLE, transmit: false);
            Util.PlaySound(ThrowPylon.soundString, base.gameObject);
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();

            if (base.isAuthority && base.fixedAge > this.duration)
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
