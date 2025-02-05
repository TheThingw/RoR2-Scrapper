using UnityEngine;

namespace Scrapper.Content
{
    internal static class Assets
    {
        internal static AssetBundle AssetBundle { get; private set; }

        internal static Texture texPrimary, texSecondary1, texSecondary2, texUtility1, texUtility2, texSpecial, texPassive;
        internal static Texture texPortrait, texMastery, texBase;


        public static void Init(AssetBundle assetBundle)
        {
            AssetBundle = assetBundle;

            texPrimary = assetBundle.LoadAsset<Texture>("Scrapper_Primary");
            texSecondary1 = assetBundle.LoadAsset<Texture>("Scrapper_Secondary_1");
            texSecondary2 = assetBundle.LoadAsset<Texture>("Scrapper_Secondary_2");
            texUtility1 = assetBundle.LoadAsset<Texture>("Scrapper_Utility_1");
            texUtility2 = assetBundle.LoadAsset<Texture>("Scrapper_Utility_2");
            texSpecial = assetBundle.LoadAsset<Texture>("Scrapper_Special");
            texPassive = assetBundle.LoadAsset<Texture>("Scrapper_Passive");
            texPortrait = assetBundle.LoadAsset<Texture>("Scrapper_Portrait");
            texMastery = assetBundle.LoadAsset<Texture>("Scrapper_Mastery");
            texBase = assetBundle.LoadAsset<Texture>("Scrapper_Base");
        }
    }
}
