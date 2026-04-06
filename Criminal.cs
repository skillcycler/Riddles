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
public class Criminal : Role
{
    public override Il2CppSystem.Collections.Generic.List<SpecialRule> GetRules()
    {
        Il2CppSystem.Collections.Generic.List<Character> characters = Gameplay.CurrentCharacters;
        foreach (Character character in characters)
        {
            // make sure no other night cycles exist
            if (character.name == "Lilis" || character.name == "Agmeres" || character.name == "Viciyon" || character.name == "Caedoccidere" || character.name == "Sanguitaurus")
            {
                return new Il2CppSystem.Collections.Generic.List<SpecialRule>();
            }
        }
        Il2CppSystem.Collections.Generic.List<SpecialRule> sr = new Il2CppSystem.Collections.Generic.List<SpecialRule>();
        sr.Add(new NightModeRule(4));
        return sr;
    }
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
            Il2CppSystem.Collections.Generic.List<Character> newList = Gameplay.CurrentCharacters;
            newList = Characters.Instance.FilterAliveCharacters(newList);
            Health health = PlayerController.PlayerInfo.health;
            health.Damage(3);
            if (!(newList.Count == 0))
            {
                Character myTarget = newList[UnityEngine.Random.Range(0, newList.Count)];
                myTarget.Reveal();
                myTarget.onReveal.Invoke();
                myTarget.RevealReal();
                myTarget.statuses.AddStatus(ECharacterStatus.KilledByEvil, charRef);
                myTarget.statuses.AddStatus(CriminalKill.criminalKill, charRef);
                myTarget.KillByDemon(charRef);
                if (myTarget.dataRef.picking)
                {
                    myTarget.uses = 0;
                    myTarget.pickable.SetActive(false);
                }
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
    public Criminal() : base(ClassInjector.DerivedConstructorPointer<Criminal>())
    {
        ClassInjector.DerivedConstructorBody((Il2CppObjectBase)this);
    }
    public Criminal(IntPtr ptr) : base(ptr) { }
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