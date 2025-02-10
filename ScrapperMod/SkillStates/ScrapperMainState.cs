using EntityStates;
using RoR2;
using Scrapper.Components;
using UnityEngine;

namespace Scrapper.SkillStates
{
    public class ScrapperMainState : GenericCharacterMain
    {
        private Animator animator;
        //private LocalUser localUser;
        private ScrapCtrl scrapCtrl;

        public override void OnEnter()
        {
            base.OnEnter();
            
            this.animator = base.modelAnimator;
            this.scrapCtrl = base.GetComponent<ScrapCtrl>();
            
            //this.FindLocalUser();
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();

            if (this.animator)
            {
                bool inCombat = false;
                if (!base.characterBody.outOfDanger || !base.characterBody.outOfCombat)
                {
                    inCombat = true;
                }
                this.animator.SetBool("inCombat", inCombat);
                this.animator.SetBool("isGrounded", base.isGrounded);
                /*
                if (base.isGrounded)
                {
                    this.animator.SetFloat("airBlend", 0f);
                }
                else
                {
                    this.animator.SetFloat("airBlend", 1f);
                }

                this.animator.SetFloat("aimDir", base.inputBank.aimDirection.y);*/
            }
        }
        /*
        public override void Update()
        {
            base.Update();
            if (base.isAuthority && base.characterMotor.isGrounded)
            {
                this.CheckEmote<Rest>(Config.restKey);
                this.CheckEmote<Taunt>(Config.tauntKey);
                this.CheckEmote<Dance>(Config.danceKey);
            }
        }

        private void CheckEmote(KeyCode keybind, EntityState state)
        {
            if (Input.GetKeyDown(keybind) && !this.localUser.isUIFocused)
            {
                base.outer.SetInterruptState(state, InterruptPriority.Any);
            }
        }

        private void CheckEmote<T>(ConfigEntry<KeyboardShortcut> keybind) where T : EntityState, new()
        {
            if (Config.GetKeyPressed(keybind.Value))
            {
                this.FindLocalUser();
                if (this.localUser != null && !this.localUser.isUIFocused)
                {
                    base.outer.SetInterruptState(new T(), InterruptPriority.Any);
                }
            }
        }

        private void FindLocalUser()
        {
            if (this.localUser != null || !base.characterBody)
            {
                return;
            }
            foreach (LocalUser readOnlyLocalUsers in LocalUserManager.readOnlyLocalUsersList)
            {
                if (readOnlyLocalUsers.cachedBody == base.characterBody)
                {
                    this.localUser = readOnlyLocalUsers;
                    break;
                }
            }
        }*/

        public override void ProcessJump()
        {
            if (!hasCharacterMotor)
            {
                return;
            }

            bool flag = false;
            bool flag2 = false;
            if (!jumpInputReceived || !base.characterBody || base.characterMotor.jumpCount >= base.characterBody.maxJumpCount)
            {
                return;
            }

            int itemCount = base.characterBody.inventory.GetItemCount(RoR2Content.Items.JumpBoost);
            float horizontalBonus = 1f;
            float verticalBonus = 1f;
            if (base.characterMotor.jumpCount >= base.characterBody.baseJumpCount)
            {
                flag = true;
                horizontalBonus = 1.5f;
                verticalBonus = 1.5f;
            }
            else if ((float)itemCount > 0f && base.characterBody.isSprinting)
            {
                float num = base.characterBody.acceleration * base.characterMotor.airControl;
                if (base.characterBody.moveSpeed > 0f && num > 0f)
                {
                    flag2 = true;
                    float num2 = Mathf.Sqrt(10f * (float)itemCount / num);
                    float num3 = base.characterBody.moveSpeed / num;
                    horizontalBonus = (num2 + num3) / num3;
                }
            }

            ApplyJumpVelocity(base.characterMotor, base.characterBody, horizontalBonus, verticalBonus);
            if (hasModelAnimator)
            {
                int layerIndex = base.modelAnimator.GetLayerIndex("Body");
                if (layerIndex >= 0)
                {
                    base.modelAnimator.CrossFadeInFixedTime("Jump", smoothingParameters.intoJumpTransitionTime, layerIndex);
                    /*
                    if (base.characterMotor.jumpCount == 0 || base.characterBody.baseJumpCount == 1)
                    {
                        base.modelAnimator.CrossFadeInFixedTime("Jump", smoothingParameters.intoJumpTransitionTime, layerIndex);
                    }
                    else
                    {
                        base.modelAnimator.CrossFadeInFixedTime("BonusJump", smoothingParameters.intoJumpTransitionTime, layerIndex);
                    }*/
                }
            }

            if (flag)
            {
                EffectManager.SpawnEffect(LegacyResourcesAPI.Load<GameObject>("Prefabs/Effects/FeatherEffect"), new EffectData
                {
                    origin = base.characterBody.footPosition
                }, transmit: true);
            }
            else if (base.characterMotor.jumpCount > 0)
            {
                EffectManager.SpawnEffect(LegacyResourcesAPI.Load<GameObject>("Prefabs/Effects/ImpactEffects/CharacterLandImpact"), new EffectData
                {
                    origin = base.characterBody.footPosition,
                    scale = base.characterBody.radius
                }, transmit: true);
            }

            if (flag2)
            {
                EffectManager.SpawnEffect(LegacyResourcesAPI.Load<GameObject>("Prefabs/Effects/BoostJumpEffect"), new EffectData
                {
                    origin = base.characterBody.footPosition,
                    rotation = Util.QuaternionSafeLookRotation(base.characterMotor.velocity)
                }, transmit: true);
            }

            base.characterMotor.jumpCount++;
            base.characterBody.onJump?.Invoke();
        }
    }
}
