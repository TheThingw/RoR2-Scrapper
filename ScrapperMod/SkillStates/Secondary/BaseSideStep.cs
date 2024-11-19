using EntityStates;
using RoR2;
using UnityEngine;
using UnityEngine.Networking;

namespace Scrapper.SkillStates.Secondary
{
    public abstract class BaseSideStep : BaseState
    {
        public static GameObject dashPrefab;

        public static float smallHopVelocity;

        public static float dashPrepDuration;

        public static float dashDuration = 0.3f;

        public static float speedCoefficient = 25f;

        public static string beginSoundString;

        private float stopwatch;

        private Vector3 dashVector = Vector3.zero;

        private ChildLocator childLocator;

        private bool isDashing;

        private CameraTargetParams.AimRequest aimRequest;

        private int originalLayer;

        public override void OnEnter()
        {
            base.OnEnter();
            Util.PlaySound(beginSoundString, gameObject);
            PlayAnimation("FullBody, Override", "StepBrothersPrep", "StepBrothersPrep.playbackRate", dashPrepDuration);
            dashVector = inputBank.aimDirection;
            if (NetworkServer.active)
            {
                characterBody.AddBuff(RoR2Content.Buffs.HiddenInvincibility.buffIndex);
            }
        }

        protected virtual void CreateDashEffect()
        {
            Transform transform = childLocator.FindChild("DashCenter");
            if ((bool)transform && (bool)dashPrefab)
            {
                Object.Instantiate(dashPrefab, transform.position, Util.QuaternionSafeLookRotation(dashVector), transform);
            }
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();
            characterDirection.forward = dashVector;
            if (stopwatch > dashPrepDuration / attackSpeedStat && !isDashing)
            {
                isDashing = true;
                dashVector = inputBank.aimDirection;
                CreateDashEffect();
                PlayCrossfade("FullBody, Override", "StepBrothersLoop", 0.1f);
                originalLayer = gameObject.layer;
                gameObject.layer = LayerIndex.GetAppropriateFakeLayerForTeam(teamComponent.teamIndex).intVal;
                characterMotor.Motor.RebuildCollidableLayers();
            }
            if (!isDashing)
            {
                stopwatch += GetDeltaTime();
            }
            else if (isAuthority)
            {
                characterMotor.velocity = Vector3.zero;
            }
            if (stopwatch >= dashDuration + dashPrepDuration / attackSpeedStat && isAuthority)
            {
                outer.SetNextStateToMain();
            }
        }

        public override void OnExit()
        {
            gameObject.layer = originalLayer;
            characterMotor.Motor.RebuildCollidableLayers();
            PlayAnimation("FullBody, Override", "StepBrothersLoopExit");
            if (NetworkServer.active)
            {
                characterBody.RemoveBuff(RoR2Content.Buffs.HiddenInvincibility.buffIndex);
            }
            base.OnExit();
        }

        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.PrioritySkill;
        }
    }
}
