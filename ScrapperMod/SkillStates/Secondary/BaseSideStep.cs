using EntityStates;
using RoR2;
using Scrapper.Content;
using UnityEngine;
using UnityEngine.Networking;

namespace Scrapper.SkillStates.Secondary
{
    public abstract class BaseSideStep : BaseScrapperSkillState
    {
        private float duration = 1f;

        private Vector3 forwardDirection;

        private GameObject slideEffectInstance;

        private bool startedStateGrounded;

        private Animator animator;

        private bool playedSFX = false;

        private float baseJumpPower;

        private GameObject kickEffectInstance;

        public override void OnEnter()
        {
            base.OnEnter();
            this.animator = base.GetModelAnimator();
            this.duration = 1f;
            this.baseJumpPower = base.characterBody.baseJumpPower;
            base.characterBody.baseJumpPower *= 1.75f;
            base.characterBody.isSprinting = true;
            base.moveSpeedStat = base.characterBody.moveSpeed;
            base.characterBody.RecalculateStats();

            if (base.inputBank && base.characterDirection)
            {
                base.characterDirection.forward = ((base.inputBank.moveVector == Vector3.zero) ? base.characterDirection.forward : base.inputBank.moveVector).normalized;
            }
            if (base.characterMotor)
            {
                this.startedStateGrounded = base.characterMotor.isGrounded;
            }

            base.characterBody.SetSpreadBloom(0f, canOnlyIncreaseBloom: false);
            this.animator.SetBool("canCancelSlide", value: false);
            if (!this.startedStateGrounded)
            {
                this.duration = 0.75f;
                EffectManager.SpawnEffect(LegacyResourcesAPI.Load<GameObject>("Prefabs/Effects/ImpactEffects/CharacterLandImpact"), new EffectData
                {
                    origin = base.characterBody.footPosition,
                    scale = base.characterBody.radius
                }, transmit: true);
                //base.PlayAnimation("FullBody, Override", "AirKick", "Slide.playbackRate", this.duration + 0.75f * base.belmont.slideAnimValue);
                this.forwardDirection = base.GetAimRay().direction;
                base.characterMotor.velocity = Vector3.zero;
                base.StartAimMode(0.9f);
            }
            else
            {
                //base.PlayAnimation("FullBody, Override", "SlideKick", "Slide.playbackRate", this.duration + base.belmont.slideAnimValue);
                if (EntityStates.Commando.SlideState.slideEffectPrefab)
                {
                    this.slideEffectInstance = Object.Instantiate(parent: base.FindModelChild("Root"), original: EntityStates.Commando.SlideState.slideEffectPrefab);
                }
            }
        }

        public override void Update()
        {
            base.Update();
            if ((bool)this.animator)
            {
                this.animator.SetBool("isGrounded", base.isGrounded);
            }
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();
            if ((bool)this.animator)
            {
                this.animator.SetBool("isGrounded", base.isGrounded);
            }
            base.characterBody.isSprinting = true;
            if (!base.isGrounded && (bool)this.slideEffectInstance)
            {
                EntityState.Destroy(this.slideEffectInstance);
            }
            if (base.fixedAge >= 0.2f && !this.playedSFX)
            {
                this.playedSFX = true;
                Util.PlaySound("sfx_belmont_slidekick", base.gameObject);
                this.CreateDashEffect();
                //this.kickEffectInstance = Object.Instantiate(BelmontAssets.slideEffect, base.FindModelChild("FootL"));
            }
            if (!base.isAuthority)
            {
                return;
            }
            if ((bool)base.inputBank && (bool)base.characterDirection)
            {
                base.characterDirection.moveVector = base.inputBank.moveVector;
                if (this.startedStateGrounded)
                {
                    this.forwardDirection = base.characterDirection.forward;
                }
            }
            if ((bool)base.characterMotor)
            {
                float num;
                if (!this.startedStateGrounded)
                {
                    num = ((!(base.fixedAge >= 0.2f)) ? 0.1f : (EntityStates.Commando.SlideState.forwardSpeedCoefficientCurve.Evaluate((base.fixedAge - 0.2f) / this.duration) * 1.5f));
                }
                else
                {
                    num = EntityStates.Commando.SlideState.forwardSpeedCoefficientCurve.Evaluate(base.fixedAge / this.duration) * 1.25f;
                    if (base.fixedAge < 0.2f)
                    {
                        num *= 0.1f;
                    }
                }
                base.characterMotor.rootMotion += num * base.moveSpeedStat * this.forwardDirection * Time.fixedDeltaTime;
                if (!this.startedStateGrounded)
                {
                    base.characterMotor.velocity = Vector3.zero;
                }
            }
            if (base.fixedAge >= this.duration)
            {
                base.characterMotor.Motor.ForceUnground();
                if (base.characterMotor.velocity.y <= 10f)
                {
                    base.characterMotor.velocity.y = 10f;
                }
                else
                {
                    base.characterMotor.velocity.y += 9f;
                }
                base.outer.SetNextStateToMain();
            }
        }

        public virtual void CreateDashEffect()
        {
            EffectData effectData = new EffectData();
            effectData.rotation = Util.QuaternionSafeLookRotation(this.forwardDirection);
            effectData.origin = base.characterBody.corePosition;
            //EffectManager.SpawnEffect(BelmontAssets.dashEffect, effectData, transmit: false);
            float num = 3f;
            if (base.isAuthority)
            {
                base.AddRecoil(-1f * num, -2f * num, -0.5f * num, 0.5f * num);
            }
            //Transform transform = base.belmont.childLocator.FindChild("JetL");
            //Transform transform2 = base.belmont.childLocator.FindChild("JetR");
            if ((bool)transform)
            {
                //Object.Instantiate(BelmontAssets.jetEffect, transform);
            }
            //if ((bool)transform2)
            //{
                //Object.Instantiate(BelmontAssets.jetEffect, transform2);
            //}
        }

        public override void OnExit()
        {
            if (this.startedStateGrounded)
            {
                this.PlayImpactAnimation();
            }
            if ((bool)this.slideEffectInstance)
            {
                EntityState.Destroy(this.slideEffectInstance);
            }
            if ((bool)this.kickEffectInstance)
            {
                EntityState.Destroy(this.kickEffectInstance);
            }
            base.OnExit();
            base.characterBody.baseJumpPower = this.baseJumpPower;
            base.characterBody.RecalculateStats();
            //base.belmont.EndSlide();
        }

        private void PlayImpactAnimation()
        {
            Animator modelAnimator = base.GetModelAnimator();
            int layerIndex = modelAnimator.GetLayerIndex("Impact");
            if (layerIndex >= 0)
            {
                modelAnimator.SetLayerWeight(layerIndex, 1f);
            }
        }

        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.Pain;
        }
    }
}
