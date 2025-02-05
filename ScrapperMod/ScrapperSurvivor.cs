using RoR2;
using RoR2.Skills;
using System.Collections.Generic;
using UnityEngine;
using Scrapper.Components;
using EntityStates;
using Scrapper.Content.BaseContent;
using Scrapper.Content;
using Scrapper.Modules;

namespace Scrapper
{
    public class ScrapperSurvivor : SurvivorBase<ScrapperSurvivor>
    {
        public const string SCRAPPER_PREFIX = ScrapperPlugin.DEVELOPER_PREFIX + "_SCRAPPER_";

        public override string assetBundleName => "scrapperassetbundle";
        public override string bodyName => "ScrapperBody";
        public override string masterName => "ScrapperMonsterMaster";
        public override string modelPrefabName => "mdlScrapper";
        public override string displayPrefabName => "ScrapperDisplay";
        public override string survivorTokenPrefix => SCRAPPER_PREFIX;

        public override BodyInfo bodyInfo => new BodyInfo
        {
            bodyName = bodyName,
            bodyNameToken = SCRAPPER_PREFIX + "NAME",
            subtitleNameToken = SCRAPPER_PREFIX + "SUBTITLE",
            bodyNameToClone = "Loader",

            characterPortrait = assetBundle.LoadAsset<Texture>("Scrapper_Portrait"),
            bodyColor = new Color32(180, 115, 75, 255),
            sortPosition = 100,

            crosshair = AssetManager.LoadCrosshair("Standard"),
            podPrefab = LegacyResourcesAPI.Load<GameObject>("Prefabs/NetworkedObjects/SurvivorPod"),

            //main stats
            maxHealth = 160f,
            healthRegen = 2.5f,
            armor = 20f,
            shield = 0f, // base shield is a thing apparently. neat

            jumpCount = 1,

            //conventional base stats, consistent for all survivors
            damage = 12f,
            attackSpeed = 1f,
            crit = 1f,

            //misc stats
            moveSpeed = 7f,
            acceleration = 80f,
            jumpPower = 15f,

            //stat growth
            /// <summary>
            /// Leave this alone, and you don't need to worry about setting any of the stat growth values. They'll be set at the consistent ratio that all vanilla survivors have.
            /// <para>If You do, healthGrowth should be maxHealth * 0.3f, regenGrowth should be healthRegen * 0.2f, damageGrowth should be damage * 0.2f</para>
            /// </summary>
            autoCalculateLevelStats = true,

            healthGrowth = 100f * 0.3f,
            regenGrowth = 1f * 0.2f,
            armorGrowth = 0f,
            shieldGrowth = 0f,

            damageGrowth = 12f * 0.2f,
            attackSpeedGrowth = 0f,
            critGrowth = 0f,

            moveSpeedGrowth = 0f,
            jumpPowerGrowth = 0f,// jump power per level exists for some reason
        };
        
        /*public override CustomRendererInfo[] customRendererInfos => new CustomRendererInfo[]
        {
                new CustomRendererInfo
                {
                    childName = "SwordModel",
                    material = null,
                }
        };*/

        public override UnlockableDef characterUnlockableDef => Unlockables.characterUnlockableDef;

        public override ItemDisplaysBase itemDisplays => new ScrapperItemDisplays();

        public override AssetBundle assetBundle { get; protected set; }

        public override GameObject bodyPrefab { get; protected set; }
        public override CharacterBody prefabCharacterBody { get; protected set; }
        public override GameObject characterModelObject { get; protected set; }
        public override CharacterModel prefabCharacterModel { get; protected set; }
        public override GameObject displayPrefab { get; protected set; }

        public override void Initialize()
        {
            /*
            instance = this as T;
            assetBundle = Asset.LoadAssetBundle(assetBundleName);

            InitializeCharacter();*/
            base.Initialize();

            // static content
            Config.Init();
            Content.Assets.Init(assetBundle);
            Buffs.Init();
            DamageTypes.Init();
            Unlockables.Init();
            States.Init();
            Tokens.Init();
            ScrapAssistManager.Init();

            // base
            InitializeCharacterBodyPrefab();
            InitializeItemDisplays();
            InitializeDisplayPrefab();
            InitializeSurvivor();

            InitializeCharacter();
        }

        public override void InitializeCharacter()
        {
            // skills
            InitializeEntityStateMachines();
            InitializeSkills();

            // skins
            InitializeSkins();

            // misc
            InitializeCharacterMaster();
            AdditionalBodySetup();
        }

        #region Skills
        public override void InitializeEntityStateMachines()
        {
            //clear existing state machines from your cloned body (probably commando)
            //omit all this if you want to just keep theirs
            PrefabManager.ClearEntityStateMachines(bodyPrefab);

            //the main "Body" state machine has some special properties
            PrefabManager.AddMainEntityStateMachine(bodyPrefab, "Body", typeof(GenericCharacterMain), typeof(EntityStates.SpawnTeleporterState));
            //if you set up a custom main characterstate, set it up here
            //don't forget to register custom entitystates in your ScrapperStates.cs

            PrefabManager.AddEntityStateMachine(bodyPrefab, "Weapon");
            PrefabManager.AddEntityStateMachine(bodyPrefab, "Weapon2");
        }

        public override void InitializeSkills()
        {
            //remove the genericskills from the commando body we cloned
            SkillManager.ClearGenericSkills(bodyPrefab);
            //add our own
            AddPassiveSkill();
            AddPrimarySkills();
            AddSecondarySkills();
            AddUtilitySkills();
            AddSpecialSkills();
        }

        //skip if you don't have a passive
        //also skip if this is your first look at skills
        private void AddPassiveSkill()
        {
            //option 1. fake passive icon just to describe functionality we will implement elsewhere
            bodyPrefab.GetComponent<SkillLocator>().passiveSkill = new SkillLocator.PassiveSkill
            {
                enabled = true,
                skillNameToken = SCRAPPER_PREFIX + "PASSIVE_NAME",
                skillDescriptionToken = SCRAPPER_PREFIX + "PASSIVE_DESCRIPTION",
                keywordToken = SCRAPPER_PREFIX + "KEYWORD_IMPALE",
                icon = assetBundle.LoadAsset<Sprite>("Scrapper_Passive"),
            };
            /*
            //option 2. a new SkillFamily for a passive, used if you want multiple selectable passives
            var passiveGenericSkill = Skills.CreateGenericSkillWithSkillFamily(bodyPrefab, "PassiveSkill");
            var passiveSkillDef1 = Skills.CreateSkillDef(new SkillDefInfo
            {
                skillName = "ScrapperPassive",
                skillNameToken = SCRAPPER_PREFIX + "PASSIVE_NAME",
                skillDescriptionToken = SCRAPPER_PREFIX + "PASSIVE_DESCRIPTION",
                keywordTokens = new string[] { "KEYWORD_AGILE" },
                skillIcon = assetBundle.LoadAsset<Sprite>("texPassiveIcon"),

                //unless you're somehow activating your passive like a skill, none of the following is needed.
                //but that's just me saying things. the tools are here at your disposal to do whatever you like with

                //activationState = new EntityStates.SerializableEntityStateType(typeof(SkillStates.Shoot)),
                //activationStateMachineName = "Weapon1",
                //interruptPriority = EntityStates.InterruptPriority.Skill,

                //baseRechargeInterval = 1f,
                //baseMaxStock = 1,

                //rechargeStock = 1,
                //requiredStock = 1,
                //stockToConsume = 1,

                //resetCooldownTimerOnUse = false,
                //fullRestockOnAssign = true,
                //dontAllowPastMaxStocks = false,
                //mustKeyPress = false,
                //beginSkillCooldownOnSkillEnd = false,

                //isCombatSkill = true,
                //canceledFromSprinting = false,
                //cancelSprintingOnActivation = false,
                //forceSprintDuringState = false,

            });
            Skills.AddSkillsToFamily(passiveGenericSkill.skillFamily, passiveSkillDef1);*/
        }

        //if this is your first look at skilldef creation, take a look at Secondary first
        private void AddPrimarySkills()
        {
            SkillManager.CreateGenericSkillWithSkillFamily(bodyPrefab, SkillSlot.Primary);

            //the primary skill is created using a constructor for a typical primary
            //it is also a SteppedSkillDef. Custom Skilldefs are very useful for custom behaviors related to casting a skill. see ror2's different skilldefs for reference
            var primarySkillDef1 = SkillManager.CreateSkillDef<SteppedSkillDef>(new SkillDefInfo
                (
                    "ScrapperThrustCombo",
                    SCRAPPER_PREFIX + "PRIMARY_THRUST_NAME",
                    SCRAPPER_PREFIX + "PRIMARY_THRUST_DESCRIPTION",
                    assetBundle.LoadAsset<Sprite>("Scrapper_Primary"),
                    new EntityStates.SerializableEntityStateType(typeof(SkillStates.Primary.ThrustCombo)),
                    "Weapon",
                    true
                ));
            //custom Skilldefs can have additional fields that you can set manually
            primarySkillDef1.stepCount = 2;
            primarySkillDef1.stepGraceDuration = 0.5f;

            SkillManager.AddPrimarySkills(bodyPrefab, primarySkillDef1);
        }

        private void AddSecondarySkills()
        {
            SkillManager.CreateGenericSkillWithSkillFamily(bodyPrefab, SkillSlot.Secondary);

            //here is a basic skill def with all fields accounted for
            var secondarySkillDef1 = SkillManager.CreateSkillDef(new SkillDefInfo
            {
                skillName = "ScrapperQuickStep",
                skillNameToken = SCRAPPER_PREFIX + "SECONDARY_QUICKSTEP_NAME",
                skillDescriptionToken = SCRAPPER_PREFIX + "SECONDARY_QUICKSTEP_DESCRIPTION",
                keywordTokens = new string[] { SCRAPPER_PREFIX + "KEYWORD_PREPARE" },
                skillIcon = assetBundle.LoadAsset<Sprite>("Scrapper_Secondary_2"),

                activationState = new EntityStates.SerializableEntityStateType(typeof(SkillStates.Secondary.QuickStep)),
                activationStateMachineName = "Weapon2",
                interruptPriority = EntityStates.InterruptPriority.Skill,

                baseRechargeInterval = 1f,
                baseMaxStock = 1,

                rechargeStock = 1,
                requiredStock = 1,
                stockToConsume = 1,

                resetCooldownTimerOnUse = false,
                fullRestockOnAssign = true,
                dontAllowPastMaxStocks = false,
                mustKeyPress = false,
                beginSkillCooldownOnSkillEnd = false,

                isCombatSkill = true,
                canceledFromSprinting = false,
                cancelSprintingOnActivation = false,
                forceSprintDuringState = false,

            });
            var secondarySkillDef2 = SkillManager.CreateSkillDef(new SkillDefInfo
            {
                skillName = "ScrapperThunderStep",
                skillNameToken = SCRAPPER_PREFIX + "SECONDARY_THUNDERSTEP_NAME",
                skillDescriptionToken = SCRAPPER_PREFIX + "SECONDARY_THUNDERSTEP_DESCRIPTION",
                keywordTokens = new string[] { SCRAPPER_PREFIX + "KEYWORD_PREPARE" },
                skillIcon = assetBundle.LoadAsset<Sprite>("Scrapper_Secondary_2"),

                activationState = new EntityStates.SerializableEntityStateType(typeof(SkillStates.Secondary.ThunderStep)),
                activationStateMachineName = "Weapon2",
                interruptPriority = EntityStates.InterruptPriority.Skill,

                baseRechargeInterval = 1f,
                baseMaxStock = 1,

                rechargeStock = 1,
                requiredStock = 1,
                stockToConsume = 1,

                resetCooldownTimerOnUse = false,
                fullRestockOnAssign = true,
                dontAllowPastMaxStocks = false,
                mustKeyPress = false,
                beginSkillCooldownOnSkillEnd = false,

                isCombatSkill = true,
                canceledFromSprinting = false,
                cancelSprintingOnActivation = false,
                forceSprintDuringState = false,

            });

            SkillManager.AddSecondarySkills(bodyPrefab, secondarySkillDef1, secondarySkillDef2);
        }

        private void AddUtilitySkills()
        {
            SkillManager.CreateGenericSkillWithSkillFamily(bodyPrefab, SkillSlot.Utility);

            //here's a skilldef of a typical movement skill.
            var utilitySkillDef1 = SkillManager.CreateSkillDef(new SkillDefInfo
            {
                skillName = "ScrapperSkewer",
                skillNameToken = SCRAPPER_PREFIX + "UTILITY_SKEWER_NAME",
                skillDescriptionToken = SCRAPPER_PREFIX + "UTILITY_SKEWER_DESCRIPTION",
                skillIcon = assetBundle.LoadAsset<Sprite>("Scrapper_Utility_1"),

                activationState = new EntityStates.SerializableEntityStateType(typeof(SkillStates.Utility.ChargeSkewer)),
                activationStateMachineName = "Weapon2",
                interruptPriority = EntityStates.InterruptPriority.PrioritySkill,

                baseRechargeInterval = 4f,
                baseMaxStock = 1,

                rechargeStock = 1,
                requiredStock = 1,
                stockToConsume = 1,

                resetCooldownTimerOnUse = false,
                fullRestockOnAssign = true,
                dontAllowPastMaxStocks = false,
                mustKeyPress = false,
                beginSkillCooldownOnSkillEnd = false,

                isCombatSkill = false,
                canceledFromSprinting = false,
                cancelSprintingOnActivation = false,
                forceSprintDuringState = true,
            });

            var utilitySkillDef2 = SkillManager.CreateSkillDef(new SkillDefInfo
            {
                skillName = "ScrapperReposte",
                skillNameToken = SCRAPPER_PREFIX + "UTILITY_RIPOSTE_NAME",
                skillDescriptionToken = SCRAPPER_PREFIX + "UTILITY_RIPOSTE_DESCRIPTION",
                skillIcon = assetBundle.LoadAsset<Sprite>("Scrapper_Utility_2"),

                activationState = new EntityStates.SerializableEntityStateType(typeof(SkillStates.Utility.ChargeRiposte)),
                activationStateMachineName = "Weapon2",
                interruptPriority = EntityStates.InterruptPriority.PrioritySkill,

                baseRechargeInterval = 4f,
                baseMaxStock = 1,

                rechargeStock = 1,
                requiredStock = 1,
                stockToConsume = 1,

                resetCooldownTimerOnUse = false,
                fullRestockOnAssign = true,
                dontAllowPastMaxStocks = false,
                mustKeyPress = false,
                beginSkillCooldownOnSkillEnd = false,

                isCombatSkill = false,
                canceledFromSprinting = false,
                cancelSprintingOnActivation = false,
                forceSprintDuringState = true,
            });

            SkillManager.AddUtilitySkills(bodyPrefab, utilitySkillDef1, utilitySkillDef2);
        }

        private void AddSpecialSkills()
        {
            SkillManager.CreateGenericSkillWithSkillFamily(bodyPrefab, SkillSlot.Special);

            //a basic skill. some fields are omitted and will just have default values
            var specialSkillDef1 = SkillManager.CreateSkillDef(new SkillDefInfo
            {
                skillName = "ScrapperPylon",
                skillNameToken = SCRAPPER_PREFIX + "SPECIAL_PYLON_NAME",
                skillDescriptionToken = SCRAPPER_PREFIX + "SPECIAL_PYLON_DESCRIPTION",
                skillIcon = assetBundle.LoadAsset<Sprite>("Scrapper_Special"),

                activationState = new EntityStates.SerializableEntityStateType(typeof(SkillStates.Special.ThrowPylon)),
                //setting this to the "weapon2" EntityStateMachine allows us to cast this skill at the same time primary, which is set to the "weapon" EntityStateMachine
                activationStateMachineName = "Weapon2",
                interruptPriority = EntityStates.InterruptPriority.Skill,

                baseMaxStock = 1,
                baseRechargeInterval = 10f,

                isCombatSkill = true,
                mustKeyPress = false,
            });

            SkillManager.AddSpecialSkills(bodyPrefab, specialSkillDef1);
        }
        #endregion

        #region Skins
        public override void InitializeSkins()
        {
            var skinController = prefabCharacterModel.gameObject.AddComponent<ModelSkinController>();
            var childLocator = prefabCharacterModel.GetComponent<ChildLocator>();

            var defaultRendererinfos = prefabCharacterModel.baseRendererInfos;

            var skins = new List<SkinDef>();

            #region DefaultSkin
            //this creates a SkinDef with all default fields
            var defaultSkin = SkinManager.CreateSkinDef(SCRAPPER_PREFIX + "DEFAULT_SKIN_NAME",
                assetBundle.LoadAsset<Sprite>("Scrapper_Base"),
                defaultRendererinfos,
                prefabCharacterModel.gameObject);

            //these are your Mesh Replacements. The order here is based on your CustomRendererInfos from earlier
            //pass in meshes as they are named in your assetbundle
            //currently not needed as with only 1 skin they will simply take the default meshes
            //uncomment this when you have another skin
            /*defaultSkin.meshReplacements = Modules.Skins.getMeshReplacements(assetBundle, defaultRendererinfos,
                "meshScrapperSword",
                "meshScrapperGun",
                "meshScrapper");
            *//*
            defaultSkin.gameObjectActivations = new SkinDef.GameObjectActivation[]
            {
                new SkinDef.GameObjectActivation
                {
                    gameObject = childLocator.FindChildGameObject("Girder"),
                    shouldActivate = true,
                },
                new SkinDef.GameObjectActivation
                {
                    gameObject = childLocator.FindChildGameObject("MasterySword"),
                    shouldActivate = false,
                }
            };*/
            //add new skindef to our list of skindefs. this is what we'll be passing to the SkinController
            skins.Add(defaultSkin);
            #endregion
            /*
            //uncomment this when you have a mastery skin
            #region MasterySkin

            ////creating a new skindef as we did before
            SkinDef masterySkin = SkinManager.CreateSkinDef(SCRAPPER_PREFIX + "MASTERY_SKIN_NAME",
                assetBundle.LoadAsset<Sprite>("Scrapper_Mastery"),
                defaultRendererinfos,
                prefabCharacterModel.gameObject);
                //Unlockables.masterySkinUnlockableDef);

            ////adding the mesh replacements as above. 
            ////if you don't want to replace the mesh (for example, you only want to replace the material), pass in null so the order is preserved
            //masterySkin.meshReplacements = Modules.Skins.getMeshReplacements(assetBundle, defaultRendererinfos,
            //    "meshScrapperSwordAlt",
            //    null,//no gun mesh replacement. use same gun mesh
            //    "meshScrapperAlt");

            ////masterySkin has a new set of RendererInfos (based on default rendererinfos)
            ////you can simply access the RendererInfos' materials and set them to the new materials for your skin.

            var masteryMat = assetBundle.LoadMaterial("matMastery");
            var swordTransform = childLocator.FindChild("MasterySword");
            for (int i = 0; i < masterySkin.rendererInfos.Length; i++)
            {
                if (masterySkin.rendererInfos[i].renderer.transform == swordTransform)
                    masterySkin.rendererInfos[i].defaultMaterial = assetBundle.LoadMaterial("matMasterySword");
                else
                    masterySkin.rendererInfos[i].defaultMaterial = masteryMat;
            }

            ////here's a barebones example of using gameobjectactivations that could probably be streamlined or rewritten entirely, truthfully, but it works
            masterySkin.gameObjectActivations = new SkinDef.GameObjectActivation[]
            {
                new SkinDef.GameObjectActivation
                {
                    gameObject = childLocator.FindChildGameObject("Girder"),
                    shouldActivate = false,
                },
                new SkinDef.GameObjectActivation
                {
                    gameObject = childLocator.FindChildGameObject("MasterySword"),
                    shouldActivate = true,
                }
            };
            ////simply find an object on your child locator you want to activate/deactivate and set if you want to activate/deacitvate it with this skin

            skins.Add(masterySkin);

            #endregion
            */
            skinController.skins = skins.ToArray();
        }
        #endregion

        #region Misc
        //Character Master is what governs the AI of your character when it is not controlled by a player (artifact of vengeance, goobo)
        public override void InitializeCharacterMaster()
        {
            //you must only do one of these. adding duplicate masters breaks the game.

            //if you're lazy or prototyping you can simply copy the AI of a different character to be used
            //Modules.Prefabs.CloneDopplegangerMaster(bodyPrefab, masterName, "Merc");

            //how to set up AI in code
            AI.Init(bodyPrefab, masterName);

            //how to load a master set up in unity, can be an empty gameobject with just AISkillDriver components
            //assetBundle.LoadMaster(bodyPrefab, masterName);
        }

        private void AdditionalBodySetup()
        {
            //example of how to create a HitBoxGroup. see summary for more details
            //PrefabManager.SetupHitBoxGroup(characterModelObject, "StabHitboxGroup", "StabHitbox");

            bodyPrefab.AddComponent<ScrapCtrl>();
        }
        #endregion
    }
}