using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.SandBox.GameComponents;

namespace LittleArenaMod
{
    public class ImprovedTournamentModel : DefaultTournamentModel
    {
        public override int GetRenownReward(Hero winner, Town town)
        {
            return 10;
        }
    }
}