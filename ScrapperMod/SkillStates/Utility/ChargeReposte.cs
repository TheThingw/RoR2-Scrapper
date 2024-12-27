using System;
using EntityStates;
using EntityStates.Loader;
using RoR2;
using RoR2.UI;
using UnityEngine;

namespace Scrapper.SkillStates.Utility
{
    public class ChargeReposte : BaseSkillState
    {
        private class ArcVisualizer : IDisposable
        {
            private readonly Vector3[] points;

            private readonly float duration;

            private readonly GameObject arcVisualizerInstance;

            private readonly LineRenderer lineRenderer;

            public ArcVisualizer(GameObject arcVisualizerPrefab, float duration, int vertexCount)
            {
                this.arcVisualizerInstance = UnityEngine.Object.Instantiate(arcVisualizerPrefab);
                this.lineRenderer = this.arcVisualizerInstance.GetComponent<LineRenderer>();
                this.lineRenderer.positionCount = vertexCount;
                this.points = new Vector3[vertexCount];
                this.duration = duration;
            }

            public void Dispose()
            {
                EntityState.Destroy(this.arcVisualizerInstance);
            }

            public void SetParameters(Vector3 origin, Vector3 initialVelocity, float characterMaxSpeed, float characterAcceleration)
            {
                this.arcVisualizerInstance.transform.position = origin;
                if (!this.lineRenderer.useWorldSpace)
                {
                    Vector3 eulerAngles = Quaternion.LookRotation(initialVelocity).eulerAngles;
                    eulerAngles.x = 0f;
                    eulerAngles.z = 0f;
                    Quaternion rotation = Quaternion.Euler(eulerAngles);
                    this.arcVisualizerInstance.transform.rotation = rotation;
                    origin = Vector3.zero;
                    initialVelocity = Quaternion.Inverse(rotation) * initialVelocity;
                }
                else
                {
                    this.arcVisualizerInstance.transform.rotation = Quaternion.LookRotation(Vector3.Cross(initialVelocity, Vector3.up));
                }
                float y = Physics.gravity.y;
                float num = this.duration / (float)this.points.Length;
                Vector3 vector = origin;
                Vector3 vector2 = initialVelocity;
                float num2 = num;
                float num3 = y * num2;
                float maxDistanceDelta = characterAcceleration * num2;
                for (int i = 0; i < this.points.Length; i++)
                {
                    this.points[i] = vector;
                    Vector2 vector3 = Vector2.MoveTowards(Util.Vector3XZToVector2XY(vector2), Vector3.zero, maxDistanceDelta);
                    vector2.x = vector3.x;
                    vector2.z = vector3.y;
                    vector2.y += num3;
                    vector += vector2 * num2;
                }
                this.lineRenderer.SetPositions(this.points);
            }
        }

        public static GameObject arcVisualizerPrefab;

        public static float arcVisualizerSimulationLength;

        public static int arcVisualizerVertexCount;

        [SerializeField]
        public float baseChargeDuration = 1f;

        public static float minChargeForChargedAttack;

        public static GameObject chargeVfxPrefab;

        public static string chargeVfxChildLocatorName;

        public static GameObject crosshairOverridePrefab;

        public static float walkSpeedCoefficient;

        public static string startChargeLoopSFXString;

        public static string endChargeLoopSFXString;

        public static string enterSFXString;

        private CrosshairUtils.OverrideRequest crosshairOverrideRequest;

        private Transform chargeVfxInstanceTransform;

        //private int gauntlet;

        private uint soundID;

        private static int EmptyStateHash = Animator.StringToHash("Empty");

        private static int ChargePunchIntroStateHash = Animator.StringToHash("ChargePunchIntro");

        private static int ChargePunchIntroParamHash = Animator.StringToHash("ChargePunchIntro.playbackRate");

        protected float chargeDuration { get; private set; }

        protected float charge { get; private set; }

        public override void OnEnter()
        {
            base.OnEnter();
            this.chargeDuration = this.baseChargeDuration / base.attackSpeedStat;
            Util.PlaySound(ChargeReposte.enterSFXString, base.gameObject);
            this.soundID = Util.PlaySound(ChargeReposte.startChargeLoopSFXString, base.gameObject);
        }

        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.PrioritySkill;
        }

        public override void OnExit()
        {
            if ((bool)this.chargeVfxInstanceTransform)
            {
                EntityState.Destroy(this.chargeVfxInstanceTransform.gameObject);
                this.PlayAnimation("Gesture, Additive", ChargeReposte.EmptyStateHash);
                this.PlayAnimation("Gesture, Override", ChargeReposte.EmptyStateHash);
                this.crosshairOverrideRequest?.Dispose();
                this.chargeVfxInstanceTransform = null;
            }
            base.characterMotor.walkSpeedPenaltyCoefficient = 1f;
            Util.PlaySound(ChargeReposte.endChargeLoopSFXString, base.gameObject);
            base.OnExit();
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();
            this.charge = Mathf.Clamp01(base.fixedAge / this.chargeDuration);
            AkSoundEngine.SetRTPCValueByPlayingID("loaderShift_chargeAmount", this.charge * 100f, this.soundID);
            base.characterBody.SetSpreadBloom(this.charge);
            base.characterBody.SetAimTimer(3f);
            if (this.charge >= ChargeReposte.minChargeForChargedAttack && !this.chargeVfxInstanceTransform && (bool)ChargeReposte.chargeVfxPrefab)
            {
                if ((bool)ChargeReposte.crosshairOverridePrefab && this.crosshairOverrideRequest == null)
                {
                    this.crosshairOverrideRequest = CrosshairUtils.RequestOverrideForBody(base.characterBody, ChargeReposte.crosshairOverridePrefab, CrosshairUtils.OverridePriority.Skill);
                }
                Transform transform = base.FindModelChild(ChargeReposte.chargeVfxChildLocatorName);
                if ((bool)transform)
                {
                    this.chargeVfxInstanceTransform = UnityEngine.Object.Instantiate(ChargeReposte.chargeVfxPrefab, transform).transform;
                    ScaleParticleSystemDuration component = this.chargeVfxInstanceTransform.GetComponent<ScaleParticleSystemDuration>();
                    if ((bool)component)
                    {
                        component.newDuration = (1f - ChargeReposte.minChargeForChargedAttack) * this.chargeDuration;
                    }
                }
                base.PlayCrossfade("Gesture, Additive", ChargeReposte.ChargePunchIntroStateHash, ChargeReposte.ChargePunchIntroParamHash, this.chargeDuration, 0.1f);
                base.PlayCrossfade("Gesture, Override", ChargeReposte.ChargePunchIntroStateHash, ChargeReposte.ChargePunchIntroParamHash, this.chargeDuration, 0.1f);
            }
            if ((bool)this.chargeVfxInstanceTransform)
            {
                base.characterMotor.walkSpeedPenaltyCoefficient = ChargeReposte.walkSpeedCoefficient;
            }
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
            return new SwingChargedFist
            {
                charge = this.charge
            };
        }
    }
}
