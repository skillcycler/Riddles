using System;
using System.ComponentModel.Design;
using HarmonyLib;
using Il2Cpp;
using Il2CppInterop.Runtime.Injection;
using Il2CppInterop.Runtime.InteropTypes;
using Il2CppSystem;
using MelonLoader;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class Follower : Demon
{
    public override Il2CppSystem.Collections.Generic.List<SpecialRule> GetRules()
    {
        Il2CppSystem.Collections.Generic.List<SpecialRule> sr = new Il2CppSystem.Collections.Generic.List<SpecialRule>();
        sr.Add(new NightModeRule(3));
        return sr;
    }
    public int CheckRolePriority(Character character)
    {
        Il2CppSystem.Collections.Generic.List<string> highPriorityIDs = new Il2CppSystem.Collections.Generic.List<string>();
        Il2CppSystem.Collections.Generic.List<string> midPriorityIDs = new Il2CppSystem.Collections.Generic.List<string>();
        Il2CppSystem.Collections.Generic.List<string> neverAttackIDs = new Il2CppSystem.Collections.Generic.List<string>();

        highPriorityIDs.Add("Bishop_58855542");
        highPriorityIDs.Add("Empress_13782227");

        midPriorityIDs.Add("Lover_91302708");
        midPriorityIDs.Add("Oracle_07039445");
        midPriorityIDs.Add("Archivist_34476114");
        midPriorityIDs.Add("Lookout_41018246");
        midPriorityIDs.Add("Mathematician");
        midPriorityIDs.Add("Gossip_85354100");
        midPriorityIDs.Add("Hunter_93427887");

        neverAttackIDs.Add("Knight_47970624");


        float targetValue = 1f;
        // corrupted characters are 50% less likely to be attacked
        float statusMult = 1f;
        if (character.statuses.Contains(ECharacterStatus.Corrupted)) statusMult *= 0.5f;
        // don't attack dead characters
        if (character.state == ECharacterState.Dead) statusMult = 0f;

        //for unrevealed characters: 6x as likely to attack certain characters
        if (highPriorityIDs.Contains(character.dataRef.characterId) && !character.revealed) targetValue = 6f;

        //for unrevealed characters: 3x as likely to attack certain characters

        if (midPriorityIDs.Contains(character.dataRef.characterId) && !character.revealed) targetValue = 3f;

        // don't attack outcasts unless it's a picking outcast
        // also don't attack minions or demons
        if (character.GetCharacterType() == ECharacterType.Outcast) targetValue = 0f;
        if (character.GetCharacterType() == ECharacterType.Minion) targetValue = 0f;
        if (character.GetCharacterType() == ECharacterType.Demon) targetValue = 0f;

        // de-prioritize revealed characters
        if (character.state == ECharacterState.Alive) targetValue = 0f;

        // 6x as likely to attack refreshing characters / 3x as likely for unused one time abilities (revealed or not)
        // This re-enables attacking revealed characters with unused abilities
        if (character.dataRef.picking && character.dataRef.characterId != "Dreamer_32014895")
        {
            if (character.dataRef.abilityUsage == EAbilityUsage.ResetAfterNight)
            {
                targetValue = 6f;
            } else
            {
                // deprioritize "spent" once per game abilities
                if (character.uses == 0)
                {
                    targetValue = 0f;
                } else
                {
                    targetValue = 3f;
                }
            }
        }
        // don't attack Knights or evils or things that can't die
        if (neverAttackIDs.Contains(character.dataRef.characterId)) targetValue = 0f;
        if (character.alignment == EAlignment.Evil) targetValue = 0f;
        if (character.statuses.Contains(ECharacterStatus.UnkillableByDemon)) targetValue = 0f;
        //MelonLogger.Msg(string.Format("Checking character #{0}: Role is {1}, value is {2}, state: {3}", character.id, character.dataRef.name, Mathf.RoundToInt(targetValue * statusMult), character.state));
        return (Mathf.RoundToInt(targetValue * statusMult));
    }
    public override void Act(ETriggerPhase trigger, Character charRef)
    {
        if (trigger == ETriggerPhase.Start)
        {
            if (Gameplay.CurrentCharacters.Count >= 11)
            {
                Health health = PlayerController.PlayerInfo.health;
                health.AddMaxHp(2);
                health.Heal(100);
            }
        }
        if (trigger == ETriggerPhase.Night)
        {
            if (charRef.state == ECharacterState.Dead) return;
            Il2CppSystem.Collections.Generic.List<Character> targetChars = new Il2CppSystem.Collections.Generic.List<Character>();
            // Kill valuable unrevealed characters and those with unused abilities
            foreach (Character character in Gameplay.CurrentCharacters)
            {
                int chance = CheckRolePriority(character);
                if (chance > 0) {
                    for (int i = 0; i < chance; i++)
                    {
                        targetChars.Add(character);
                    }
                }
            }
            // If there are none, kill unrevealed good characters
            if (targetChars.Count == 0)
            {
                foreach (Character character in Gameplay.CurrentCharacters)
                {
                    if (character.alignment != EAlignment.Evil && character.state == ECharacterState.Hidden && !character.statuses.Contains(ECharacterStatus.UnkillableByDemon))
                    {
                        targetChars.Add(character);
                    }
                }
            }
            // If all options are exhausted, kill revealed villagers
            if (targetChars.Count == 0)
            {
                foreach (Character character in Gameplay.CurrentCharacters)
                {
                    if (character.GetCharacterType() == ECharacterType.Villager && character.state != ECharacterState.Dead && character.alignment == EAlignment.Good && !character.statuses.Contains(ECharacterStatus.UnkillableByDemon))
                    {
                        targetChars.Add(character);
                    }
                }
            }

            // If all options are still exhausted, kill revealed outcasts and good minions
            if (targetChars.Count == 0)
            {
                foreach (Character character in Gameplay.CurrentCharacters)
                {
                    if ((character.GetCharacterType() == ECharacterType.Outcast || character.GetCharacterType() == ECharacterType.Minion) && character.state != ECharacterState.Dead && character.alignment == EAlignment.Good && !character.statuses.Contains(ECharacterStatus.UnkillableByDemon))
                    {
                        targetChars.Add(character);
                    }
                }
            }
            // Sink the kill if no good kills
            if (targetChars.Count == 0)
            {
                PlayerController.PlayerInfo.health.Damage(2);
                return;
            }
            Character nightAttackTarget = targetChars[UnityEngine.Random.RandomRangeInt(0, targetChars.Count)];
            PlayerController.PlayerInfo.health.Damage(2);
            nightAttackTarget.statuses.AddStatus(ECharacterStatus.KilledByEvil, charRef);
            nightAttackTarget.statuses.AddStatus(FollowerKill.followerKill, charRef);
            nightAttackTarget.KillByDemon(charRef);
            if (nightAttackTarget.dataRef.picking)
            {
                nightAttackTarget.uses = 0;
                nightAttackTarget.pickable.SetActive(false);
            }
        }
    }
    public Follower() : base(ClassInjector.DerivedConstructorPointer<Follower>())
    {
        ClassInjector.DerivedConstructorBody((Il2CppObjectBase)this);
    }
    public Follower(System.IntPtr ptr) : base(ptr) { }
}
public static class FollowerKill
{
    public static ECharacterStatus followerKill = (ECharacterStatus)876;
    [HarmonyPatch(typeof(Character), nameof(Character.ShowDescription))]
    public static class ChangeKillByDemonText
    {
        public static void Postfix(Character __instance)
        {
            if (__instance.killedByDemon && __instance.statuses.Contains(followerKill))
            {
                HintInfo info = new HintInfo();
                info.text = "Killed by <color=#33CC33>Follower</color>\nCannot use abilities.\nTrue Role is not revealed.";
                UIEvents.OnShowHint.Invoke(info, __instance.hintPivot);
            }
        }
    }
}