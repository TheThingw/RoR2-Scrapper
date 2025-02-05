using RoR2;
using Scrapper.Modules;
using System.Collections.Generic;

namespace Scrapper.Content.BaseContent
{
    public abstract class ItemDisplaysBase
    {
        public void SetItemDisplays(ItemDisplayRuleSet itemDisplayRuleSet)
        {
            var itemDisplayRules = new List<ItemDisplayRuleSet.KeyAssetRuleGroup>();

            ItemDisplayManager.LazyInit();

            SetItemDisplayRules(itemDisplayRules);

            itemDisplayRuleSet.keyAssetRuleGroups = itemDisplayRules.ToArray();

            ItemDisplayManager.DisposeWhenDone();
        }

        protected abstract void SetItemDisplayRules(List<ItemDisplayRuleSet.KeyAssetRuleGroup> itemDisplayRules);
    }
}
