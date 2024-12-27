using BepInEx;
using R2API.Utils;
using Scrapper.Components;
using System.Security;
using System.Security.Permissions;

[assembly: HG.Reflection.SearchableAttribute.OptIn]
[module: UnverifiableCode]
#pragma warning disable CS0618 // Type or member is obsolete
[assembly: SecurityPermission(SecurityAction.RequestMinimum, SkipVerification = true)]
#pragma warning restore CS0618 // Type or member is obsolete

//rename this namespace
namespace Scrapper
{
    [BepInDependency("com.Moffein.AssistManager", BepInDependency.DependencyFlags.HardDependency)]
    [NetworkCompatibility(CompatibilityLevel.EveryoneMustHaveMod, VersionStrictness.EveryoneNeedSameModVersion)]
    [BepInPlugin(MODUID, MODNAME, MODVERSION)]
    public class ScrapperPlugin : BaseUnityPlugin
    {
        public const string MODUID = "com.thingw.ScrapperMod";
        public const string MODNAME = "ScrapperMod";
        public const string MODVERSION = "0.0.1";

        public const string DEVELOPER_PREFIX = "ThingW";

        public static ScrapperPlugin instance;

        private void Awake()
        {
            instance = this;

            Log.Init(Logger);

            Modules.Language.Init();
            new ScrapperSurvivor().Initialize();
            new Modules.ContentPacks().Initialize();
            ScrapAssistManager.Init();
        }
    }
}
