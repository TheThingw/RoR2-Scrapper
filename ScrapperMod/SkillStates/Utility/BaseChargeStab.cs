using EntityStates;
using Scrapper.Content;
using UnityEngine;

namespace Scrapper.SkillStates.Utility
{
    public class BaseChargeStab : BaseScrapperSkillState
    {
        public float baseChargeDuration = 1f;

        public static float minChargeForChargedAttack = 1f;

        public static float walkSpeedCoefficient;

        protected float chargeDuration { get; private set; }

        protected float charge { get; private set; }

        public override void OnEnter()
        {
            base.OnEnter();
            this.PlayAnimation("Gesture, Override", AnimatorStates.StabStart.GetName());
            this.chargeDuration = this.baseChargeDuration / base.attackSpeedStat;
        }

        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.PrioritySkill;
        }

        public override void OnExit()
        {
            this.PlayAnimation("Gesture, Override", AnimatorStates.BufferEmpty.GetName());

            base.characterMotor.walkSpeedPenaltyCoefficient = 1f;

            base.OnExit();
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();
            this.charge = Mathf.Clamp01(base.fixedAge / this.chargeDuration);

            base.characterBody.SetSpreadBloom(this.charge);
            base.characterBody.SetAimTimer(3f);
            if (this.charge >= ChargeRiposte.minChargeForChargedAttack)
            {
                base.PlayCrossfade("Gesture, Override", AnimatorStates.StabHold.GetName(), AnimatorParams.Stab.GetName(), this.chargeDuration, 0.1f);
            }
            base.characterMotor.walkSpeedPenaltyCoefficient = ChargeRiposte.walkSpeedCoefficient;
            if (base.isAuthority)
            {
                this.AuthorityFixedUpdate();
            }
        }

        public override void Update()
        {
            base.Update();
            Mathf.Clamp01(base.age / this.chargeDuration);
        }

        private void AuthorityFixedUpdate()
        {
            if (!this.ShouldKeepChargingAuthority())
            {
                base.outer.SetNextState(this.GetNextStateAuthority());
            }
        }

        protected virtual bool ShouldKeepChargingAuthority()
        {
            return base.IsKeyDownAuthority();
        }

        protected virtual EntityState GetNextStateAuthority()
        {
            return new BaseReleaseStab
            {
                charge = this.charge
            };
        }
    }
}
