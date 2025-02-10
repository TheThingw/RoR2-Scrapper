using EntityStates;
using RoR2;
using Scrapper.Components;
using Scrapper.Content;

namespace Scrapper.SkillStates
{
    public class BasicScrapperMeleeAttack : BasicMeleeAttack
    {
        protected static string LAYER_FULLBODY => StaticValues.LAYER_FULLBODY;
        protected static string LAYER_GESTURE => StaticValues.LAYER_GESTURE;

        protected ScrapCtrl scrapCtrl;

        #region OnEnter
        public override void OnEnter()
        {
            /*
            Standard melee attack flow

            ** -> empty method/does nothing without override

            OnEnter
                CalcDuration
                GetHitboxGroupName **
                new OverlapAttack
                PlayAnimation **
             */

            this.RefreshState();

            this.baseDuration = 1f;

            base.OnEnter();
        }

        public void RefreshState() => scrapCtrl ??= GetComponent<ScrapCtrl>();

        public override float CalcDuration() => base.CalcDuration();

        public override string GetHitBoxGroupName() => ChildLocatorEntry.StabHitbox.GetName() + "Group";

        // Overlap attack created in OnEnter, if the hitboxgroup exists

        public override void PlayAnimation() => base.PlayAnimation();
        #endregion

        #region FixedUpdate
        public override void FixedUpdate()
        {
            /*
            FixedUpdate
                BeginMeleeAttackEffect
                AuthorityFixedUpdate
                    ~AuthFireAttack
                        AuthModifyOverlapAttack **
                        ~AuthTriggerHitPause
                        OnMeleeHitAuth **
                    AuthExitHitPause
                    AuthOnFinish **
            */
            base.FixedUpdate();
        }

        public override void BeginMeleeAttackEffect() => base.BeginMeleeAttackEffect();

        public override void AuthorityFixedUpdate() => base.AuthorityFixedUpdate();

        // attack is fired in AuthorityFireAttack

        public override void AuthorityModifyOverlapAttack(OverlapAttack overlapAttack) => base.AuthorityModifyOverlapAttack(overlapAttack);

        // Hitpause is handled in AuthorityTriggerHitPause

        public override void OnMeleeHitAuthority() => base.OnMeleeHitAuthority();

        public override void AuthorityExitHitPause() => base.AuthorityExitHitPause();

        public override void AuthorityOnFinish() => base.AuthorityOnFinish();
        #endregion

        #region OnExit
        public override void OnExit()
        {
            /*
            OnExit - if the attack wasn't fired
                BeginMeleeAttackEffect
                AuthFireAttack
                
                - if in hitpause
                AuthExitHitPause
           */
            base.OnExit();
        }
        #endregion

        public override InterruptPriority GetMinimumInterruptPriority() => InterruptPriority.PrioritySkill;
    }
}
