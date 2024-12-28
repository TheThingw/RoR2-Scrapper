using EntityStates;
using RoR2;
using RoR2.Networking;
using RoR2.Orbs;
using Scrapper.Content;
using UnityEngine;
using UnityEngine.Networking;

namespace Scrapper.SkillStates.Secondary
{
    public class BaseLungeAttack : BasicScrapperMeleeAttack
    {
        public float speedCoefficientOnExit;

        public float speedCoefficient;

        public string endSoundString;

        public float exitSmallHop;

        public float delayedDamageCoefficient;

        public float delayedProcCoefficient;

        public float delay;

        public Material enterOverlayMaterial;

        public float enterOverlayDuration = 0.7f;

        public GameObject delayedEffectPrefab;

        public GameObject orbEffect;

        public float delayPerHit;

        public GameObject selfOnHitOverlayEffectPrefab;

        private Transform modelTransform;

        private Vector3 dashVector;

        private int originalLayer;

        private int currentHitCount;

        private Vector3 dashVelocity => dashVector * moveSpeedStat * speedCoefficient;

        public override void OnEnter()
        {
            base.OnEnter();

            dashVector = inputBank.moveVector == Vector3.zero ? inputBank.aimDirection : inputBank.moveVector;
            dashVector.y = 0;
            dashVector.Normalize();

            originalLayer = gameObject.layer;
            gameObject.layer = LayerIndex.GetAppropriateFakeLayerForTeam(teamComponent.teamIndex).intVal;
            characterMotor.Motor.RebuildCollidableLayers();

            characterMotor.Motor.ForceUnground();
            characterMotor.velocity = Vector3.zero;

            modelTransform = GetModelTransform();
            /*
            if ((bool)modelTransform)
            {
                TemporaryOverlayInstance temporaryOverlayInstance = TemporaryOverlayManager.AddOverlay(modelTransform.gameObject);
                temporaryOverlayInstance.duration = enterOverlayDuration;
                temporaryOverlayInstance.animateShaderAlpha = true;
                temporaryOverlayInstance.alphaCurve = AnimationCurve.EaseInOut(0f, 1f, 1f, 0f);
                temporaryOverlayInstance.destroyComponentOnEnd = true;
                temporaryOverlayInstance.originalMaterial = enterOverlayMaterial;
                temporaryOverlayInstance.AddToCharacterModel(modelTransform.GetComponent<CharacterModel>());
            }*/


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

            Util.PlaySound(endSoundString, gameObject);

            gameObject.layer = originalLayer;
            characterMotor.Motor.RebuildCollidableLayers();
            base.OnExit();
        }

        public override void PlayAnimation()
        {
            PlayCrossfade(StaticValues.LAYER_FULLBODY, StaticValues.STAB_END, 0.1f);
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

        public override void AuthorityModifyOverlapAttack(OverlapAttack overlapAttack)
        {
            base.AuthorityModifyOverlapAttack(overlapAttack);
            overlapAttack.damage = damageCoefficient * damageStat;
        }

        public override void OnMeleeHitAuthority()
        {
            base.OnMeleeHitAuthority();
            float num = hitPauseDuration / attackSpeedStat;
            if ((bool)selfOnHitOverlayEffectPrefab && num > 1f / 30f)
            {
                EffectData effectData = new EffectData
                {
                    origin = transform.position,
                    genericFloat = hitPauseDuration / attackSpeedStat
                };
                effectData.SetNetworkedObjectReference(gameObject);
                EffectManager.SpawnEffect(selfOnHitOverlayEffectPrefab, effectData, transmit: true);
            }
            foreach (HurtBox hitResult in hitResults)
            {
                currentHitCount++;
                HandleHit(damageValue: characterBody.damage * delayedDamageCoefficient, delay: delay + delayPerHit * currentHitCount, isCrit: RollCrit(), attackerObject: gameObject, victimHurtBox: hitResult, procCoefficient: delayedProcCoefficient, orbEffectPrefab: orbEffect, orbImpactEffectPrefab: delayedEffectPrefab);
            }
        }

        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.PrioritySkill;
        }

        private static void HandleHit(GameObject attackerObject, HurtBox victimHurtBox, float damageValue, float procCoefficient, bool isCrit, float delay, GameObject orbEffectPrefab, GameObject orbImpactEffectPrefab)
        {
            if (!NetworkServer.active)
            {
                NetworkWriter networkWriter = new NetworkWriter();
                networkWriter.StartMessage(77);
                networkWriter.Write(attackerObject);
                networkWriter.Write(HurtBoxReference.FromHurtBox(victimHurtBox));
                networkWriter.Write(damageValue);
                networkWriter.Write(procCoefficient);
                networkWriter.Write(isCrit);
                networkWriter.Write(delay);
                networkWriter.WriteEffectIndex(EffectCatalog.FindEffectIndexFromPrefab(orbEffectPrefab));
                networkWriter.WriteEffectIndex(EffectCatalog.FindEffectIndexFromPrefab(orbImpactEffectPrefab));
                networkWriter.FinishMessage();
                ClientScene.readyConnection?.SendWriter(networkWriter, QosChannelIndex.defaultReliable.intVal);
            }
            else if ((bool)victimHurtBox && (bool)victimHurtBox.healthComponent)
            {
                SetStateOnHurt.SetStunOnObject(victimHurtBox.healthComponent.gameObject, delay);
                OrbManager.instance.AddOrb(new DelayedHitOrb
                {
                    attacker = attackerObject,
                    target = victimHurtBox,
                    damageColorIndex = DamageColorIndex.Default,
                    damageValue = damageValue,
                    damageType = DamageType.ApplyMercExpose,
                    isCrit = isCrit,
                    procChainMask = default,
                    procCoefficient = procCoefficient,
                    delay = delay,
                    orbEffect = orbEffectPrefab,
                    delayedEffectPrefab = orbImpactEffectPrefab
                });
            }
        }
    }
}
