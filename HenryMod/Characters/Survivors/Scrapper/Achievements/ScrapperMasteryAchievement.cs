using RoR2;
using ScrapperMod.Modules.Achievements;

namespace ScrapperMod.Survivors.Scrapper.Achievements
{
    //automatically creates language tokens "ACHIEVMENT_{identifier.ToUpper()}_NAME" and "ACHIEVMENT_{identifier.ToUpper()}_DESCRIPTION" 
    [RegisterAchievement(identifier, unlockableIdentifier, null, 10, null)]
    public class ScrapperMasteryAchievement : BaseMasteryAchievement
    {
        public const string identifier = ScrapperSurvivor.SCRAPPER_PREFIX + "masteryAchievement";
        public const string unlockableIdentifier = ScrapperSurvivor.SCRAPPER_PREFIX + "masteryUnlockable";

        public override string RequiredCharacterBody => ScrapperSurvivor.instance.bodyName;

        //difficulty coeff 3 is monsoon. 3.5 is typhoon for grandmastery skins
        public override float RequiredDifficultyCoefficient => 3;
    }
}