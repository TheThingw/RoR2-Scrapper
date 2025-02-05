using RoR2;
using UnityEngine.Networking;
using UnityEngine;
using AM = AssistManager;
using R2API;
using Scrapper.Components;

namespace Scrapper.Content
{
    public static class ScrapAssistManager
    {
        public static void Init()
        {
            On.RoR2.GlobalEventManager.ProcessHitEnemy += GlobalEventManager_ProcessHitEnemy;
            AM.AssistManager.HandleDirectAssistActions += HandleScrapperAssists;
        }

        private static void GlobalEventManager_ProcessHitEnemy(On.RoR2.GlobalEventManager.orig_ProcessHitEnemy orig, GlobalEventManager self, DamageInfo damageInfo, GameObject victim)
        {
            if (NetworkServer.active && AM.AssistManager.instance && victim && damageInfo.attacker && !damageInfo.rejected && damageInfo.HasModdedDamageType(DamageTypes.ImpaleDamageType))
            {
                var attackerBody = damageInfo.attacker.GetComponent<CharacterBody>();
                var victimBody = victim.GetComponent<CharacterBody>();

                var assist = new AM.AssistManager.Assist(attackerBody, victimBody, AM.AssistManager.GetDirectAssistDurationForAttacker(damageInfo.attacker));
                assist.moddedDamageTypes.Add(DamageTypes.ImpaleDamageType);

                AM.AssistManager.instance.AddDirectAssist(assist);
            }

            orig(self, damageInfo, victim);
        }

        private static void HandleScrapperAssists(AM.AssistManager.Assist assist, CharacterBody killerBody, DamageInfo damageInfo)
        {
            if (assist.moddedDamageTypes.Contains(DamageTypes.ImpaleDamageType) && assist.attackerBody && assist.attackerBody.TryGetComponent<ScrapCtrl>(out var scrapCtrl))
                scrapCtrl.TriggerImpale();
        }
    }
}
