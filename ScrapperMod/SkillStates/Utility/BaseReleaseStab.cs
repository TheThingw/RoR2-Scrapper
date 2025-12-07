using EntityStates;
using RoR2;
using Scrapper.Content;
using UnityEngine;

namespace Scrapper.SkillStates.Utility
{
    public class BaseReleaseStab : BasicScrapperMeleeAttack
    {
        public float charge;

        public static float minLungeSpeed = 5f;
        public static float maxLungeSpeed = 15f;
        public static float minPunchForce;
        public static float maxPunchForce;
        public static float minDuration = 0.5f;
        public static float maxDuration = 1f;

        public static bool disableAirControlUntilCollision = true;

        public static float speedCoefficientOnExit = 0.2f;

        public static float velocityDamageCoefficient = 1f;

        public Vector3 punchVelocity;

        private float bonusDamage;

        public float punchSpeed { get; private set; }

        public override void OnEnter()
        {
            base.OnEnter();
            if (base.isAuthority)
            {
                base.characterMotor.Motor.ForceUnground();
                base.characterMotor.disableAirControlUntilCollision |= Reposte.disableAirControlUntilCollision;
                this.punchVelocity = Reposte.CalculateLungeVelocity(base.characterMotor.velocity, base.GetAimRay().direction, this.charge, minLungeSpeed, maxLungeSpeed);
                base.characterMotor.velocity = this.punchVelocity;
                base.characterDirection.forward = base.characterMotor.velocity.normalized;
                this.punchSpeed = base.characterMotor.velocity.magnitude;
                this.bonusDamage = this.punchSpeed * (Reposte.velocityDamageCoefficient * base.damageStat);
            }
        }

        public override float CalcDuration()
        {
            return Mathf.Lerp(minDuration, maxDuration, this.charge);
        }

        public override void PlayAnimation()
        {
            base.PlayAnimation("FullBody, Override", AnimatorStates.StabEnd.GetName());
        }

        public override void AuthorityFixedUpdate()
        {
            base.AuthorityFixedUpdate();
            if (!base.authorityInHitPause)
            {
                base.characterMotor.velocity = this.punchVelocity;
                base.characterDirection.forward = this.punchVelocity;
                base.characterBody.isSprinting = true;
            }
        }

        public override void AuthorityModifyOverlapAttack(OverlapAttack overlapAttack)
        {
            base.AuthorityModifyOverlapAttack(overlapAttack);
            overlapAttack.damage = base.damageCoefficient * base.damageStat + this.bonusDamage;
            overlapAttack.forceVector = base.characterMotor.velocity + base.GetAimRay().direction * Mathf.Lerp(minPunchForce, maxPunchForce, this.charge);
        }

        public override void OnExit()
        {
            base.OnExit();
            base.characterMotor.velocity *= Reposte.speedCoefficientOnExit;
        }

        public static Vector3 CalculateLungeVelocity(Vector3 currentVelocity, Vector3 aimDirection, float charge, float minLungeSpeed, float maxLungeSpeed)
        {
            currentVelocity = ((Vector3.Dot(currentVelocity, aimDirection) < 0f) ? Vector3.zero : Vector3.Project(currentVelocity, aimDirection));
            return currentVelocity + aimDirection * Mathf.Lerp(minLungeSpeed, maxLungeSpeed, charge);
        }

        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.PrioritySkill;
        }
    }
}
