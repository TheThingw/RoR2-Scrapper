using UnityEngine;

namespace Scrapper.Content
{
    internal static class Assets
    {
        internal static AssetBundle AssetBundle { get; private set; }

        internal static Sprite iconPortrait, iconBase, iconMastery;
        internal static Sprite iconPrimary, iconSecondary1, iconSecondary2, iconUtility1, iconUtility2, iconSpecial, iconPassive;

        internal static Material matScrapper, matMastery, matMasterySword;


        public static void Init(AssetBundle assetBundle)
        {
            AssetBundle = assetBundle;

            iconPortrait = assetBundle.LoadAsset<Sprite>("Scrapper_Portrait");
            iconMastery = assetBundle.LoadAsset<Sprite>("Scrapper_Mastery");
            iconBase = assetBundle.LoadAsset<Sprite>("Scrapper_Base");

            iconPrimary = assetBundle.LoadAsset<Sprite>("Scrapper_Primary");
            iconSecondary1 = assetBundle.LoadAsset<Sprite>("Scrapper_Secondary_1");
            iconSecondary2 = assetBundle.LoadAsset<Sprite>("Scrapper_Secondary_2");
            iconUtility1 = assetBundle.LoadAsset<Sprite>("Scrapper_Utility_1");
            iconUtility2 = assetBundle.LoadAsset<Sprite>("Scrapper_Utility_2");
            iconSpecial = assetBundle.LoadAsset<Sprite>("Scrapper_Special");
            iconPassive = assetBundle.LoadAsset<Sprite>("Scrapper_Passive");

            matScrapper = assetBundle.LoadAsset<Material>("matScrapper");
            matMastery = assetBundle.LoadAsset<Material>("matMastery");
            matMasterySword = assetBundle.LoadAsset<Material>("matMasterySword");
        }
    }
}
