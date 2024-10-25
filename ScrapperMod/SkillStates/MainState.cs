using BepInEx.Configuration;
using EntityStates;
using RoR2;
using Scrapper.Components;
using Scrapper.Modules;
using UnityEngine;

namespace Scrapper.SkillStates
{
    public class MainState : GenericCharacterMain
    {
        private Animator animator;
        public LocalUser localUser;
        private ScrapCtrl scrapCtrl;

        public override void OnEnter()
        {
            base.OnEnter();
            if (!scrapCtrl) scrapCtrl = GetComponent<ScrapCtrl>();
            animator = modelAnimator;
            FindLocalUser();
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();

            if (animator)
            {
                bool cock = false;
                if (!characterBody.outOfDanger || !characterBody.outOfCombat) cock = true;

                animator.SetBool("inCombat", cock);

                if (isGrounded) animator.SetFloat("airBlend", 0f);
                else animator.SetFloat("airBlend", 1f);
            }
            /* //emotes
             if (isAuthority && characterMotor.isGrounzded)
             {
                 this.CheckEmote<Rest>(Config.restKey);
                 this.CheckEmote<Taunt>(Config.tauntKey);
                 this.CheckEmote<Dance>(Config.danceKey);
             }*/
        }

        private void CheckEmote(KeyCode keybind, EntityState state)
        {
            if (Input.GetKeyDown(keybind))
            {
                if (!localUser.isUIFocused)
                {
                    outer.SetInterruptState(state, InterruptPriority.Any);
                }
            }
        }

        private void CheckEmote<T>(ConfigEntry<KeyboardShortcut> keybind) where T : EntityState, new()
        {
            if (Config.GetKeyPressed(keybind.Value))
            {
                FindLocalUser();

                if (localUser != null && !localUser.isUIFocused)
                {
                    outer.SetInterruptState(new T(), InterruptPriority.Any);
                }
            }
        }

        private void FindLocalUser()
        {
            if (localUser == null)
            {
                if (characterBody)
                {
                    foreach (LocalUser lu in LocalUserManager.readOnlyLocalUsersList)
                    {
                        if (lu.cachedBody == characterBody)
                        {
                            localUser = lu;
                            break;
                        }
                    }
                }
            }
        }

        public override void ProcessJump()
        {
            if (hasCharacterMotor)
            {
                bool hopooFeather = false;
                bool waxQuail = false;

                if (jumpInputReceived && characterBody && characterMotor.jumpCount < characterBody.maxJumpCount)
                {
                    int waxQuailCount = characterBody.inventory.GetItemCount(RoR2Content.Items.JumpBoost);
                    float horizontalBonus = 1f;
                    float verticalBonus = 1f;

                    if (characterMotor.jumpCount >= characterBody.baseJumpCount)
                    {
                        hopooFeather = true;
                        horizontalBonus = 1.5f;
                        verticalBonus = 1.5f;
                    }
                    else if (waxQuailCount > 0 && characterBody.isSprinting)
                    {
                        float v = characterBody.acceleration * characterMotor.airControl;

                        if (characterBody.moveSpeed > 0f && v > 0f)
                        {
                            waxQuail = true;
                            float num2 = Mathf.Sqrt(10f * waxQuailCount / v);
                            float num3 = characterBody.moveSpeed / v;
                            horizontalBonus = (num2 + num3) / num3;
                        }
                    }

                    ApplyJumpVelocity(characterMotor, characterBody, horizontalBonus, verticalBonus, false);

                    if (hasModelAnimator)
                    {
                        int layerIndex = modelAnimator.GetLayerIndex("Body");
                        if (layerIndex >= 0)
                        {
                            if (characterBody.isSprinting)
                            {
                                modelAnimator.CrossFadeInFixedTime("SprintJump", smoothingParameters.intoJumpTransitionTime, layerIndex);
                            }
                            else
                            {
                                if (hopooFeather)
                                {
                                    modelAnimator.CrossFadeInFixedTime("BonusJump", smoothingParameters.intoJumpTransitionTime, layerIndex);
                                }
                                else
                                {
                                    modelAnimator.CrossFadeInFixedTime("Jump", smoothingParameters.intoJumpTransitionTime, layerIndex);
                                }
                            }
                        }
                    }

                    if (hopooFeather)
                    {
                        EffectManager.SpawnEffect(LegacyResourcesAPI.Load<GameObject>("Prefabs/Effects/FeatherEffect"), new EffectData
                        {
                            origin = characterBody.footPosition
                        }, true);
                    }
                    else if (characterMotor.jumpCount > 0)
                    {
                        EffectManager.SpawnEffect(LegacyResourcesAPI.Load<GameObject>("Prefabs/Effects/ImpactEffects/CharacterLandImpact"), new EffectData
                        {
                            origin = characterBody.footPosition,
                            scale = characterBody.radius
                        }, true);
                    }

                    if (waxQuail)
                    {
                        EffectManager.SpawnEffect(LegacyResourcesAPI.Load<GameObject>("Prefabs/Effects/BoostJumpEffect"), new EffectData
                        {
                            origin = characterBody.footPosition,
                            rotation = Util.QuaternionSafeLookRotation(characterMotor.velocity)
                        }, true);
                    }

                    characterMotor.jumpCount++;

                    // set up double jump anim
                    if (animator)
                    {
                        float x = animatorWalkParamCalculator.animatorWalkSpeed.y;
                        float y = animatorWalkParamCalculator.animatorWalkSpeed.x;

                        // neutral jump
                        if (Mathf.Abs(x) <= 0.45f && Mathf.Abs(y) <= 0.45f || inputBank.moveVector == Vector3.zero)
                        {
                            x = 0f;
                            y = 0f;
                        }

                        if (Mathf.Abs(x) > Mathf.Abs(y))
                        {
                            // side flip
                            if (x > 0f) x = 1f;
                            else x = -1f;
                            y = 0f;
                        }
                        else if (Mathf.Abs(x) < Mathf.Abs(y))
                        {
                            // forward/backflips
                            if (y > 0f) y = 1f;
                            else y = -1f;
                            x = 0f;
                        }
                        // eh this feels less dynamic. ignore the slight anim clipping issues ig and just blend them
                        //  actualyl don't because the clipping issues are nightmarish

                        // have to cache it at time of jump otherwise you can fuck up the jump anim in weird ways by turning during it
                        animator.SetFloat("forwardSpeedCached", y);
                        animator.SetFloat("rightSpeedCached", x);
                        // turns out this wasn't even used in the end. the animation didn't break at all in practice, only in theory
                        // Fuck You rob you fucking moron

                        //  update: this was actually used. what the hell are you doing?
                    }
                }
            }
        }
    }
}
