using System;
using System.IO;
using HarmonyLib;
using TaleWorlds.CampaignSystem;
using TaleWorlds.Core;
using TaleWorlds.MountAndBlade;

namespace LittleArenaMod
{
    public class SubModule : MBSubModuleBase
    {
        protected override void OnSubModuleLoad()
        {
            base.OnSubModuleLoad();
            new Harmony("LittleArenaMod").PatchAll();
        }

        protected override void OnGameStart(Game game, IGameStarter gameStarterObject)
        {
            base.OnGameStart(game, gameStarterObject);
            this.AddModels(gameStarterObject as CampaignGameStarter);
        }

        private void AddModels(CampaignGameStarter gameStarter)
        {
            bool flag = gameStarter != null;
            if (flag)
            {
                gameStarter.AddModel(new ImprovedTournamentModel());
            }
        }

        public static void LogDebug(string message)
        {
            using (StreamWriter sw = new StreamWriter("MyLittleArenaModDebugLog", true))
            {
                sw.WriteLine(DateTime.Now.ToString("o") + " > " + message);
            }
        }
    }
}