using System;
using System.Collections.Generic;
using System.Text;
using R2API;
using UnityEngine;

namespace Scrapper.Content
{
    public static class DamageTypes
    {
        public static DamageAPI.ModdedDamageType ImpaleDamageType;
        public static void Init(AssetBundle assetBundle)
        {
            ImpaleDamageType = DamageAPI.ReserveDamageType();
        }
    }
}
