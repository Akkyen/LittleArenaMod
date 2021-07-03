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
            bool isFatal = affectedAgent.Health < 1f;

            float xpGain = isFatal ? 10 : 1;

            PartyBase party = Hero.MainHero.PartyBelongedTo.Party;
            
            
            //If there is no affected or affector or the player character is the affected return true
            if (affectedAgent.Origin == null || affectorAgent == null)
            {
                return true;
            }


            CharacterObject affectedCharacter = (CharacterObject) affectedAgent.Character;
            CharacterObject affectorCharacter = (CharacterObject) affectorAgent.Character;


            if (!Hero.MainHero.CharacterObject.Equals(affectorCharacter))
            {
                return true;
            }


            //If the player killed a NPC he/she will get 1 gold
            if (isFatal)
            {
                GiveGoldAction.ApplyBetweenCharacters(null, Hero.MainHero, 1, false);
            }


            //Adds experience to the combat skills of the heroes of your party
            //and adds experience to the troops in your party
            foreach (TroopRosterElement troop in party.MemberRoster.GetTroopRoster())
            {
                if (!troop.Character.IsHero && !troop.Character.IsPlayerCharacter)
                {
                    party.MemberRoster.AddXpToTroop((int)xpGain, troop.Character);
                }

                if (troop.Character.IsHero && !troop.Character.IsPlayerCharacter)
                {
                    Hero hero = troop.Character.HeroObject;

                    hero.AddSkillXp(DefaultSkills.OneHanded, xpGain);
                    hero.AddSkillXp(DefaultSkills.TwoHanded, xpGain);
                    hero.AddSkillXp(DefaultSkills.Polearm, xpGain);
                    hero.AddSkillXp(DefaultSkills.Bow, xpGain);
                    hero.AddSkillXp(DefaultSkills.Crossbow, xpGain);
                    hero.AddSkillXp(DefaultSkills.Throwing, xpGain);
                    hero.AddSkillXp(DefaultSkills.Athletics, xpGain);
                }
            }


            //Calculates the experience the player hero should get
            SkillLevelingManager.OnCombatHit(affectorCharacter, affectedCharacter, null, null, lastSpeedBonus,
                lastShotDifficulty, attackerWeapon, hitpointRatio, 0,
                affectorAgent.MountAgent != null, affectorAgent.Team == affectedAgent.Team, false,
                damageAmount, isFatal);

            return false;
        }
    }
}