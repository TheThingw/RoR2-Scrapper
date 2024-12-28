using EntityStates;
using R2API;
using RoR2;
using Scrapper.Components;
using Scrapper.Content;
using UnityEngine;

namespace Scrapper.SkillStates
{
    public class BasicScrapperMeleeAttack : BasicMeleeAttack
    {
        public static GameObject overchargeImpactEffectPrefab;

        public static float barrierPercentagePerHit;

        protected ScrapCtrl scrapCtrl;

        public override void OnEnter()
        {
            this.baseDuration = 1f;
            this.duration = 1f;
            hitBoxGroupName = "StabHitboxGroup";
            base.OnEnter();

            RefreshState();

            /*
            Standard melee attack flow
            ** -> empty method/does nothing without override


            OnEnter
                CalcDuration
                GetHitboxGroupName **
                new OverlapAttack
                PlayAnimation **

            FixedUpdate
                BeginMeleeAttackEffect
                Authority - FixedUpdate
                    AuthFireAttack
                        AuthModifyOverlapAttack **
                        AuthTriggerHitPause
                        OnMeleeHitAuth **
                    AuthExitHitPause
                    AuthOnFinish **
                
            OnExit
                BeginMeleeAttackEffect
                AuthFireAttack
                AuthExitHitPause
                
             */
        }

        public void RefreshState()
        {
            if (!scrapCtrl)
                scrapCtrl = GetComponent<ScrapCtrl>();
        }

        public override void AuthorityModifyOverlapAttack(OverlapAttack overlapAttack)
        {
            if (scrapCtrl)
            {
                var stacks = scrapCtrl.OpportunistStacks;

                overlapAttack.damage *= 1f + stacks * StaticValues.OPPORTUNIST_DMG_MULT;
                overlapAttack.hitEffectPrefab = overchargeImpactEffectPrefab;
                overlapAttack.damageType |= (DamageTypeCombo)DamageType.Stun1s;
                overlapAttack.AddModdedDamageType(DamageTypes.ImpaleDamageType);
            }
        }

        public override void OnMeleeHitAuthority()
        {
            base.OnMeleeHitAuthority();

            healthComponent.AddBarrierAuthority(barrierPercentagePerHit * healthComponent.fullBarrier);
        }
    }
}
