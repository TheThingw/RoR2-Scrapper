using ScrapperMod.Survivors.Scrapper.Achievements;
using RoR2;
using UnityEngine;

namespace ScrapperMod.Survivors.Scrapper
{
    public static class ScrapperUnlockables
    {
        public static UnlockableDef characterUnlockableDef = null;
        public static UnlockableDef masterySkinUnlockableDef = null;

        public static void Init()
        {
            masterySkinUnlockableDef = Modules.Content.CreateAndAddUnlockbleDef(
                ScrapperMasteryAchievement.unlockableIdentifier,
                Modules.Tokens.GetAchievementNameToken(ScrapperMasteryAchievement.identifier),
                ScrapperSurvivor.instance.assetBundle.LoadAsset<Sprite>("texMasteryAchievement"));
        }
    }
}
