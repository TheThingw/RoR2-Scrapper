using EntityStates;
using RoR2;
using Scrapper.Content;
using UnityEngine;
using UnityEngine.Networking;

namespace Scrapper.SkillStates.Secondary
{
    public class BaseLungeAttack : BasicScrapperMeleeAttack
    {
        public float speedCoefficientOnExit = 0.2f;

        public float speedCoefficient = 1f;

        public float exitSmallHop = 0.5f;

        public float enterOverlayDuration = 0.7f;

        private Vector3 dashVector;

        private int originalLayer;

        private Vector3 dashVelocity => dashVector * moveSpeedStat * speedCoefficient;

        public override void OnEnter()
        {
            base.OnEnter();

            dashVector = inputBank.aimDirection;
            dashVector.Normalize();

            originalLayer = gameObject.layer;
            gameObject.layer = LayerIndex.GetAppropriateFakeLayerForTeam(teamComponent.teamIndex).intVal;
            characterMotor.Motor.RebuildCollidableLayers();

            characterMotor.Motor.ForceUnground();
            characterMotor.velocity = Vector3.zero;

            characterDirection.forward = characterMotor.velocity.normalized;
            if (NetworkServer.active)
            {
                characterBody.AddBuff(RoR2Content.Buffs.HiddenInvincibility);
            }
        }

        public override void OnExit()
        {
            if (NetworkServer.active)
            {
                characterBody.RemoveBuff(RoR2Content.Buffs.HiddenInvincibility);
            }

            characterMotor.velocity *= speedCoefficientOnExit;
            SmallHop(characterMotor, exitSmallHop);

            gameObject.layer = originalLayer;
            characterMotor.Motor.RebuildCollidableLayers();
            base.PlayAnimation(LAYER_GESTURE, AnimatorStates.BufferEmpty.GetName());
            base.OnExit();
        }

        public override void PlayAnimation()
        {
            base.PlayAnimation(LAYER_GESTURE, AnimatorStates.StabStart.GetName(), AnimatorParams.Stab.GetName(), duration);
        }

        public override void AuthorityFixedUpdate()
        {
            base.AuthorityFixedUpdate();
            if (!authorityInHitPause)
            {
                characterMotor.rootMotion += dashVelocity * GetDeltaTime();
                characterDirection.forward = dashVelocity;
                characterDirection.moveVector = dashVelocity;
                characterBody.isSprinting = true;
            }
        }

        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.PrioritySkill;
        }
    }
}
