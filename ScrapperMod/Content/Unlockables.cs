using RoR2;
using UnityEngine;
using Scrapper.Achievements;
using Scrapper.Modules;

namespace Scrapper.Content
{
    public static class Unlockables
    {
        public static UnlockableDef characterUnlockableDef = null;
        public static UnlockableDef masterySkinUnlockableDef = null;

        public static void Init()
        {
            /* 
            characterUnlockableDef = Modules.Content.CreateAndAddUnlockbleDef(
                ScrapperMasteryAchievement.unlockableIdentifier,
                Modules.Tokens.GetAchievementNameToken(ScrapperMasteryAchievement.identifier),
                ScrapperSurvivor.instance.assetBundle.LoadAsset<Sprite>("Scrapper_Mastery"));
            */

            masterySkinUnlockableDef = ContentManagement.CreateAndAddUnlockbleDef(
                ScrapperMasteryAchievement.unlockableIdentifier,
                Tokens.GetAchievementNameToken(ScrapperMasteryAchievement.identifier),
                ScrapperSurvivor.instance.assetBundle.LoadAsset<Sprite>("Scrapper_Mastery"));
        }
    }
}
