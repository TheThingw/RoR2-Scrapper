using RoR2;
using UnityEngine;
using Scrapper.Achievements;

namespace Scrapper.Content
{
    public static class Unlockables
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
