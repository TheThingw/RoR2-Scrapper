using EntityStates;
using RoR2;
using UnityEngine;

namespace Scrapper.SkillStates
{
    public class BasicScrapperMeleeAttack : BasicMeleeAttack
    {
        public static GameObject overchargeImpactEffectPrefab;

        public static float barrierPercentagePerHit;

        public override void AuthorityModifyOverlapAttack(OverlapAttack overlapAttack)
        {
            base.AuthorityModifyOverlapAttack(overlapAttack);
            if (HasBuff(JunkContent.Buffs.LoaderOvercharged))
            {
                overlapAttack.damage *= 2f;
                overlapAttack.hitEffectPrefab = overchargeImpactEffectPrefab;
                overlapAttack.damageType |= (DamageTypeCombo)DamageType.Stun1s;
            }
        }

        public override void OnMeleeHitAuthority()
        {
            base.OnMeleeHitAuthority();
            healthComponent.AddBarrierAuthority(barrierPercentagePerHit * healthComponent.fullBarrier);
        }
    }
}
