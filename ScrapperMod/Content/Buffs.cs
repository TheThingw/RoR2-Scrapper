﻿using RoR2;
using UnityEngine;

namespace Scrapper.Content
{
    public static class Buffs
    {
        // armor buff gained during roll
        public static BuffDef armorBuff;
        public static BuffDef opportunistBuff;

        public static void Init(AssetBundle assetBundle)
        {
            armorBuff = Modules.Content.CreateAndAddBuff("HenryArmorBuff",
                LegacyResourcesAPI.Load<BuffDef>("BuffDefs/HiddenInvincibility").iconSprite,
                Color.white,
                false,
                false);

            opportunistBuff = Modules.Content.CreateAndAddBuff("ScrapperOpportunistBuff",
                LegacyResourcesAPI.Load<BuffDef>("BuffDefs/HiddenInvincibility").iconSprite,
                Color.white,
                true,
                false);

            R2API.RecalculateStatsAPI.GetStatCoefficients += RecalculateStatsAPI_GetStatCoefficients;
        }

        private static void RecalculateStatsAPI_GetStatCoefficients(CharacterBody sender, R2API.RecalculateStatsAPI.StatHookEventArgs args)
        {
            var buffCount = sender ? sender.GetBuffCount(opportunistBuff.buffIndex) : 0;
            if (buffCount > 0)
            {
                args.armorAdd += 5f * buffCount;
                args.baseAttackSpeedAdd += 7.5f * buffCount;
            }
        }
    }
}