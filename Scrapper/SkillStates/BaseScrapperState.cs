﻿using EntityStates;
using Scrapper.Components;

namespace Scrapper.SkillStates
{
    public class BaseScrapperState : BaseState
    {
        protected ScrapCtrl scrapCtrl;

        public override void OnEnter()
        {
            base.OnEnter();
            RefreshState();
        }

        public void RefreshState()
        {
            if (!scrapCtrl) scrapCtrl = GetComponent<ScrapCtrl>();
        }

        public override void OnExit()
        {
            base.OnExit();
        }
    }
}