using UnityEngine;

namespace Scrapper.Content
{
    public static class Assets
    {
        internal static AssetBundle AssetBundle { get; private set; }

        public static void Init(AssetBundle assetBundle)
        {
            AssetBundle = assetBundle;
        }
    }
}
