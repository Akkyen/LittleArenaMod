using System;
using System.Reflection;
using HarmonyLib;
using SandBox.ViewModelCollection.Tournament;
using TaleWorlds.CampaignSystem;

namespace LittleArenaMod
{
    [HarmonyPatch(typeof(TournamentVM), "RefreshBetProperties")]
    public class ImprovedMaxBet
    {
        public static void Postfix(TournamentVM __instance)
        {
            int bettedAmount = (int)typeof(TournamentVM).GetField("_thisRoundBettedAmount", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(__instance);
            int maxVal = Math.Min(10000 - bettedAmount, Hero.MainHero.Gold);
            __instance.MaximumBetValue = maxVal;
        }
    }
}