using EntityStates;
using RoR2;
using Scrapper.Content;
using UnityEngine;
using UnityEngine.Networking;

namespace Scrapper.SkillStates.Secondary
{
    public abstract class BaseSideStep : BaseScrapperSkillState
    {
        public Animator modelAnimator;

        public static float minimumDuration = 0.5f;
        public static float maxY = 0.2f;
        public static float msMult = 1f;
        public static float upMult = 1f;
        public static float finalMult = 1f;


        public override void OnEnter()
        {
            base.OnEnter();

            this.modelAnimator = this.GetModelAnimator();
            
            Vector3 direction = base.GetAimRay().direction;
            if (base.isAuthority)
            {
                base.characterBody.isSprinting = true;
                direction.y = Mathf.Max(direction.y, maxY);
                Vector3 vector = direction.normalized * base.moveSpeedStat * msMult;
                Vector3 vector2 = Vector3.up * upMult;
                Vector3 vector3 = new Vector3(direction.x, 0f, direction.z).normalized * finalMult;
                base.characterMotor.Motor.ForceUnground();
                base.characterMotor.velocity = vector + vector2 + vector3;
            }

            if (NetworkServer.active)
            {
                base.characterBody.AddTimedBuff(JunkContent.Buffs.IgnoreFallDamage, 0.25f, 1);
            }

            base.PlayCrossfade("Gesture, Override", "Leap", 0.1f);

            base.characterDirection.moveVector = direction;
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();
            if (base.isAuthority && base.fixedAge >= minimumDuration && base.characterMotor)
            {
                base.characterMotor.moveDirection = base.inputBank.moveVector;
                if (base.characterMotor.Motor.GroundingStatus.IsStableOnGround && !base.characterMotor.Motor.LastGroundingStatus.IsStableOnGround)
                {
                    base.outer.SetNextStateToMain();
                }
            }
            if (NetworkServer.active)
            {
                base.characterBody.AddTimedBuff(JunkContent.Buffs.IgnoreFallDamage, 0.25f, 1);
            }
        }

        public override void OnExit()
        {
            base.characterBody.isSprinting = false;
            /*int layerIndex = this.modelAnimator.GetLayerIndex("Impact");
            if (layerIndex >= 0)
            {
                //this.modelAnimator.SetLayerWeight(layerIndex, 2f);
                //this.PlayAnimation("Impact", BaseLeap.LightImpactStateHash);
            }*/
            base.PlayCrossfade("Gesture, Override", AnimatorStates.BufferEmpty.GetName(), 0.1f);

            base.OnExit();
        }

        protected virtual EntityState GetNextStateAuthority()
        {
            return new BaseLungeAttack();
        }

        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.PrioritySkill;
        }
    }
}
