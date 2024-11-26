using EntityStates;
using Scrapper.Components;
using Scrapper.Content;

namespace Scrapper.SkillStates
{
    public class BaseScrapperSkillState : BaseSkillState
    {
        protected ScrapCtrl scrapCtrl;

        public override void OnEnter()
        {
            base.OnEnter();
            RefreshState();
        }

        public void RefreshState()
        {
            if (!scrapCtrl)
                scrapCtrl = GetComponent<ScrapCtrl>();
        }

        // Defaults
        public virtual void PlayGestureDefault() =>
            base.PlayAnimation(StaticValues.GESTURE_OVER, StaticValues.EMPTY);
        public virtual void PlayBodyDefault() =>
            base.PlayAnimation(StaticValues.BODY_OVER, StaticValues.EMPTY);

        // Gesture, Override - Animation
        public virtual void PlayGestureAnimation(string animationStateName) =>
            base.PlayAnimation(StaticValues.GESTURE_OVER, animationStateName);
        public virtual void PlayGestureAnimation(string animationStateName, string playbackRateParam, float duration, float transition = 0f) =>
            base.PlayAnimation(StaticValues.GESTURE_OVER, animationStateName, playbackRateParam, duration, transition);

        // Gesture, Override - Crossfade
        public virtual void PlayGestureCrossfade(string animationStateName, float crossfadeDuration) =>
            base.PlayCrossfade(StaticValues.GESTURE_OVER, animationStateName, crossfadeDuration);
        public virtual void PlayGestureCrossfade(string animationStateName, string playbackRateParam, float duration, float crossfadeDuration) =>
            base.PlayCrossfade(StaticValues.GESTURE_OVER, animationStateName, playbackRateParam, duration, crossfadeDuration);

        // Fullbody, Override - Animation
        public virtual void PlayBodyAnimation(string animationStateName) =>
            base.PlayAnimation(StaticValues.BODY_OVER, animationStateName); 
        public virtual void PlayBodyAnimation(string animationStateName, string playbackRateParam, float duration, float transition = 0f) =>
            base.PlayAnimation(StaticValues.BODY_OVER, animationStateName, playbackRateParam, duration, transition);

        // Fullbody, Override - Crossfade
        public virtual void PlayBodyCrossfade(string animationStateName, float crossfadeDuration) => 
            base.PlayCrossfade(StaticValues.BODY_OVER, animationStateName, crossfadeDuration);
        public virtual void PlayBodyCrossfade(string animationStateName, string playbackRateParam, float duration, float crossfadeDuration) => 
            base.PlayCrossfade(StaticValues.BODY_OVER, animationStateName, playbackRateParam, duration, crossfadeDuration);
    }
}
