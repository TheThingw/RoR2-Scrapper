using System.Linq;
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

        #region Fields
        private CharacterBody scrapBody;
        private EntityStateMachine weaponStateMachine;

        private float combatStopwatch;
        #endregion

        #region Properties
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
        #endregion

        #region Unity Methods
        private void Awake()
        {
            this.scrapBody = this.GetComponent<CharacterBody>();
            this.weaponStateMachine = this.GetComponents<EntityStateMachine>().First(esm => esm.customName == "Weapon");
        }

        private void Start()
        {

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

        private void OnDestroy()
        {

        }
        #endregion

        public void TriggerImpale()
        {
            weaponStateMachine?.SetNextState(new Impale());
        }

        public void Prepare()
        {

        }
    }
}
