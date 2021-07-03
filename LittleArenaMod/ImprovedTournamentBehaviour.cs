using HarmonyLib;
using SandBox.TournamentMissions.Missions;
using TaleWorlds.CampaignSystem;

namespace LittleArenaMod
{
    [HarmonyPatch(typeof(TournamentBehavior), "OnPlayerWinTournament")]
    public class ImprovedTournamentBehaviour
    {
        public static bool Prefix(TournamentBehavior __instance)
        {
            typeof(TournamentBehavior).GetProperty("OverallExpectedDenars").SetValue(__instance, __instance.OverallExpectedDenars + 2000);
            Campaign.Current.PlayerTraitDeveloper.AddTraitXp(DefaultTraits.Valor, 25);
            return true;
        }
    }
}