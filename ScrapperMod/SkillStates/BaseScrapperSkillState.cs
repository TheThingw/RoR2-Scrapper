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
    }
}
