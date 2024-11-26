using RoR2;
using Scrapper.Content;
using UnityEngine;

namespace Scrapper.Components
{
    public class ScrapCtrl : MonoBehaviour
    {
        // amount of time till stacks decay after Out of Combat is true (5s)
        public const float MAX_COMBAT_TIMER = 3f;
        public const int MAX_OPPORTUNIST_BUFFS = 5;

        public CharacterBody scrapBody;
        public SkillLocator skillLoc;

        public float combatStopwatch;

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

        public bool impaleReady;

        public static int GetScrapperCount() => RoR2.InstanceTracker.GetInstancesList<ScrapCtrl>().Count;
        public static bool AnyScrappersGaming() => RoR2.InstanceTracker.Any<ScrapCtrl>();

        public void Awake()
        {
            this.scrapBody = this.GetComponent<CharacterBody>();
            this.skillLoc = this.GetComponent<SkillLocator>();
        }

        public void OnEnable()
        {
            RoR2.InstanceTracker.Add(this);

            this.combatStopwatch = MAX_COMBAT_TIMER;
        }
        public void OnDisable()
        {
            RoR2.InstanceTracker.Remove(this);
        }

        public void FixedUpdate()
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

        public void TriggerImpale()
        {

        }

        public void Prepare()
        {

        }
    }
}
