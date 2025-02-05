using System.Linq;
using R2API;
using RoR2;
using Scrapper.Content;
using Scrapper.SkillStates;
using UnityEngine;

namespace Scrapper.Components
{
    public class ScrapCtrl : MonoBehaviour
    {
        public const float MAX_COMBAT_TIMER = 3f;
        public const int MAX_OPPORTUNIST_BUFFS = 5;

        private CharacterBody scrapBody;
        private SkillLocator skillLoc;
        private ChildLocator childLoc;
        private CharacterModel characterModel;
        private EntityStateMachine weaponStateMachine;

        private float combatStopwatch;
        public int OpportunistStacks
        { 
            get => this.scrapBody ? this.scrapBody.GetBuffCount(Buffs.opportunistBuff.buffIndex) : 0;
            set
            {
                if (this.scrapBody)
                {
                    this.scrapBody.SetBuffCount(Buffs.opportunistBuff.buffIndex, HGMath.Clamp(value, 0, MAX_OPPORTUNIST_BUFFS));
                    this.combatStopwatch = MAX_COMBAT_TIMER;
                }
            }
        }

        #region Unity Methods
        private void Awake()
        {
            this.scrapBody = this.GetComponent<CharacterBody>();
            this.skillLoc = this.GetComponent<SkillLocator>();
            this.characterModel = this.GetComponent<ModelLocator>().modelTransform.GetComponent<CharacterModel>();
            this.childLoc = characterModel.GetComponent<ChildLocator>();

            this.weaponStateMachine = this.GetComponents<EntityStateMachine>().First(esm => esm.customName == "Weapon");
        }

        private void FixedUpdate()
        {
            if (this.scrapBody)
            {
                // buff decay
                if (!this.scrapBody.outOfCombat)
                {
                    this.combatStopwatch = MAX_COMBAT_TIMER;
                }
                else if (this.OpportunistStacks > 0)
                {
                    this.combatStopwatch -= Time.fixedDeltaTime;

                    if (this.combatStopwatch <= 0f)
                        this.OpportunistStacks--;
                }
            }
        }
        #endregion

        public void TriggerImpale()
        {
            weaponStateMachine?.SetNextState(new Impale());
        }

        public void Prepare()
        {
            var genericSkill = this.skillLoc ? this.skillLoc.specialBonusStockSkill : null;
            if (genericSkill && genericSkill.stock < genericSkill.maxStock)
            {
                genericSkill.rechargeStopwatch++;
            }
        }
    }
}
