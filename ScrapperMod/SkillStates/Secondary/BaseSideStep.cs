﻿using EntityStates;
using RoR2;
using Scrapper.Content;
using UnityEngine;
using UnityEngine.Networking;

namespace Scrapper.SkillStates.Secondary
{
    public abstract class BaseSideStep : BaseScrapperSkillState
    {
        protected Vector3 slipVector = Vector3.zero;
        public float duration = 0.3f;
        public float speedCoefficient = 7f;
        private Vector3 cachedForward;

        public override void OnEnter()
        {
            base.OnEnter();
            this.slipVector = ((base.inputBank.moveVector == Vector3.zero) ? base.characterDirection.forward : base.inputBank.moveVector).normalized;
            this.cachedForward = this.characterDirection.forward;

            Animator anim = this.GetModelAnimator();
            Vector3 rhs = base.characterDirection ? base.characterDirection.forward : this.slipVector;
            Vector3 rhs2 = Vector3.Cross(Vector3.up, rhs);
            
            float num = Vector3.Dot(this.slipVector, rhs);
            float num2 = Vector3.Dot(this.slipVector, rhs2);

            anim.SetFloat(StaticValues.DASH_F, num);
            anim.SetFloat(StaticValues.DASH_R, num2);

            base.PlayBodyCrossfade(StaticValues.DASH, StaticValues.DASH_RATE, this.duration * 1.5f, 0.05f);
            base.PlayGestureDefault();

            Util.PlaySound("sfx_driver_dash", this.gameObject);

            this.ApplyBuff();
            this.CreateDashEffect();
        }

        public virtual void ApplyBuff()
        {
            if (this.scrapCtrl)
                this.scrapCtrl.Prepare();
        }

        public virtual void CreateDashEffect()
        {/*
            EffectData effectData = new EffectData();
            effectData.rotation = Util.QuaternionSafeLookRotation(this.slipVector);
            effectData.origin = base.characterBody.corePosition;

            EffectManager.SpawnEffect(Assets.dashFX, effectData, false);*/
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();
            base.characterMotor.velocity = Vector3.zero;
            base.characterMotor.rootMotion = this.slipVector * (this.moveSpeedStat * this.speedCoefficient * Time.fixedDeltaTime) * Mathf.Cos(base.fixedAge / this.duration * 1.57079637f);

            if (base.isAuthority)
            {
                if (base.characterDirection)
                {
                    base.characterDirection.forward = this.cachedForward;
                }
            }

            if (base.isAuthority && base.fixedAge >= this.duration)
            {
                this.outer.SetNextStateToMain();
            }
        }

        public virtual void DampenVelocity()
        {
            base.characterMotor.velocity *= 0.8f;
        }

        public override void OnExit()
        {
            this.DampenVelocity();
            //base.PlayAnimation("FullBody, Override", "BufferEmpty");

            base.OnExit();
        }

        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.Frozen;
        }
    }
}