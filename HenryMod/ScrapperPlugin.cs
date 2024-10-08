using BepInEx;
using ScrapperMod.Survivors.Scrapper;
using R2API.Utils;
using RoR2;
using System.Collections.Generic;
using System.Security;
using System.Security.Permissions;

[module: UnverifiableCode]
#pragma warning disable CS0618 // Type or member is obsolete
[assembly: SecurityPermission(SecurityAction.RequestMinimum, SkipVerification = true)]
#pragma warning restore CS0618 // Type or member is obsolete

//rename this namespace
namespace ScrapperMod
{
    //[BepInDependency("com.rune580.riskofoptions", BepInDependency.DependencyFlags.SoftDependency)]
    [NetworkCompatibility(CompatibilityLevel.EveryoneMustHaveMod, VersionStrictness.EveryoneNeedSameModVersion)]
    [BepInPlugin(MODUID, MODNAME, MODVERSION)]
    public class ScrapperPlugin : BaseUnityPlugin
    {
        public const string MODUID = "com.thingw.ScrapperMod";
        public const string MODNAME = "ScrapperMod";
        public const string MODVERSION = "1.0.0";

        public const string DEVELOPER_PREFIX = "THINGW";

        public static ScrapperPlugin instance;

        void Awake()
        {
            instance = this;

            Log.Init(Logger);

            Modules.Language.Init();
            new ScrapperSurvivor().Initialize();
            new Modules.ContentPacks().Initialize();
        }
    }
}
