using System;
using System.Diagnostics;
using HarmonyLib;
using Il2Cpp;
using Il2CppInterop.Runtime.Injection;
using Il2CppInterop.Runtime.InteropTypes;
using MelonLoader;
using UnityEngine;
using static Il2CppSystem.Collections.SortedList;
using static MelonLoader.MelonLogger;

namespace RiddlerMod;

[RegisterTypeInIl2Cpp]
public class Hitman : Role
{    
    public bool killedLastNight = false;
    public override string Description
    {
        get
        {
            return "";
        }
    }
    public override ActedInfo GetInfo(Character charRef)
    {
        return new ActedInfo("", null);
    }
    public override ActedInfo GetBluffInfo(Character charRef)
    {
        return new ActedInfo("", null);
    }
    public override void Act(ETriggerPhase trigger, Character charRef)
    {
        if (trigger == ETriggerPhase.Night)
        {
            if (charRef.state == ECharacterState.Dead) return;
            if (!killedLastNight)
            {
                Il2CppSystem.Collections.Generic.List<Character> newList = Gameplay.CurrentCharacters;
                newList = Characters.Instance.FilterAliveCharacters(newList);
                Il2CppSystem.Collections.Generic.List<Character> validTargets = new();
                // not gonna have this guy try to kill the Undying or the Mad Scientist with the Undying ability. It causes too many bugs.
                foreach (Character target in newList) { 
                    if (target.dataRef.characterId != "Undying_WING" && !target.statuses.Contains(SpecialMadScientistTags.hasUndyingAbility))
                    {
                        if (!target.statuses.Contains(AvoidingDoubleKills.killed) && !target.statuses.Contains(ECharacterStatus.KilledByEvil))
                            validTargets.Add(target);
                    }
                }
                if (!(newList.Count == 0))
                {
                    Character myTarget = validTargets[UnityEngine.Random.Range(0, validTargets.Count)];
                    myTarget.statuses.AddStatus(ECharacterStatus.KilledByEvil, charRef);
                    myTarget.statuses.AddStatus(CriminalKill.criminalKill, charRef);
                    myTarget.statuses.AddStatus(AvoidingDoubleKills.killed, charRef);
                    myTarget.statuses.statuses.Remove(ECharacterStatus.UnkillableByDemon);
                    myTarget.KillByDemon(charRef);
                    myTarget.Reveal();
                    myTarget.onReveal.Invoke();
                    myTarget.RevealReal();
                    if (myTarget.dataRef.picking)
                    {
                        myTarget.pickableUses = 0;
                        myTarget.pickable.SetActive(false);
                    }
                }
                killedLastNight = true;
            } else
            {
                Health health = PlayerController.PlayerInfo.health;
                health.Damage(3);
                killedLastNight = false;
            }
        }
    }
    public override CharacterData GetBluffIfAble(Character charRef)
    {
        int diceRoll = Calculator.RollDice(10);

        if (diceRoll < 5)
        {
            // 100% Double Claim
            return Characters.Instance.GetRandomDuplicateBluff();
        }
        else
        {
            // Become a new character
            CharacterData bluff = Characters.Instance.GetRandomUniqueBluff();
            Gameplay.Instance.AddScriptCharacterIfAble(bluff.type, bluff);

            return bluff;
        }
    }
    public Hitman() : base(ClassInjector.DerivedConstructorPointer<Hitman>())
    {
        ClassInjector.DerivedConstructorBody((Il2CppObjectBase)this);
    }
    public Hitman(IntPtr ptr) : base(ptr) { }
}
public static class CriminalKill
{
    public static ECharacterStatus criminalKill = (ECharacterStatus)874;
    [HarmonyPatch(typeof(Character), nameof(Character.ShowDescription))]
    public static class ChangeKillByDemonText
    {
        public static void Postfix(Character __instance)
        {
            if (__instance.killedByDemon && __instance.statuses.Contains(criminalKill))
            {
                HintInfo info = new HintInfo();
                info.text = "Killed by <color=#FFC080>Hitman</color>\nCannot use abilities.";
                UIEvents.OnShowHint.Invoke(info, __instance.hintPivot);
            }
        }
    }
}