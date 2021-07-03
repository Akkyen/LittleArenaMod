using System;
using HarmonyLib;
using SandBox;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.Actions;
using TaleWorlds.CampaignSystem.CharacterDevelopment.Managers;
using TaleWorlds.Core;
using TaleWorlds.MountAndBlade;

namespace LittleArenaMod
{
    [HarmonyPatch(typeof(ArenaPracticeFightMissionController), "EnemyHitReward")]
    public class ImprovedPracticeController
    {
        public static bool Prefix(Agent affectedAgent, Agent affectorAgent, float lastSpeedBonus,
            float lastShotDifficulty, WeaponComponentData attackerWeapon, float hitpointRatio, float damageAmount)
        {
            bool result;


            bool isFatal = affectedAgent.Health < 1f;

            float xpGain = isFatal ? 10 : 1;

            PartyBase party = Hero.MainHero.PartyBelongedTo.Party;


            CharacterObject affectedCharacter = (CharacterObject) affectedAgent.Character;
            CharacterObject affectorCharacter = (CharacterObject) affectorAgent.Character;


            if (!affectedCharacter.IsPlayerCharacter || affectedAgent.Origin == null || affectorAgent == null)
            {
                return true;
            }

            

            SkillLevelingManager.OnCombatHit(affectorCharacter, affectedCharacter, null, null, lastSpeedBonus,
                lastShotDifficulty, attackerWeapon, hitpointRatio, 0,
                affectorAgent.MountAgent != null, affectorAgent.Team == affectedAgent.Team, false,
                damageAmount, isFatal);

            foreach (Hero hero in Hero.MainHero.CompanionsInParty)
            {
                hero.AddSkillXp(DefaultSkills.OneHanded, (float) xpGain);
                hero.AddSkillXp(DefaultSkills.TwoHanded, (float) xpGain);
                hero.AddSkillXp(DefaultSkills.Polearm, (float) xpGain);
                hero.AddSkillXp(DefaultSkills.Bow, (float) xpGain);
                hero.AddSkillXp(DefaultSkills.Crossbow, (float) xpGain);
                hero.AddSkillXp(DefaultSkills.Throwing, (float) xpGain);
                hero.AddSkillXp(DefaultSkills.Athletics, (float) xpGain);
            }


            

            for (int i = 0; i < party.MemberRoster.Count; i++)
            {
                party.MemberRoster.AddXpToTroopAtIndex((int) xpGain, i);
            }

            if (isFatal)
            {
                GiveGoldAction.ApplyBetweenCharacters(null, Hero.MainHero, 1, false);
            }

            return false;
        }
    }
}