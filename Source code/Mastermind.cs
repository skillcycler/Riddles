using Il2Cpp;
using Il2CppInterop.Runtime.Injection;
using Il2CppInterop.Runtime.InteropTypes;
using Il2CppSystem;
using MelonLoader;
using System;
using System.ComponentModel.Design;
using UnityEngine;
using HarmonyLib;

namespace RiddlerMod;

[RegisterTypeInIl2Cpp]

public class Mastermind : Minion
{
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
        if (trigger == ETriggerPhase.Start)
        {
            Il2CppSystem.Collections.Generic.List<Character> minions = Characters.Instance.FilterRealCharacterType(Gameplay.CurrentCharacters, ECharacterType.Minion);
            foreach (Character minion in minions)
            {
                minion.statuses.AddStatus(BigBrain.minion, charRef);
            }
        }
        if (trigger == ETriggerPhase.AfterRoundStart)
        {
            Il2CppSystem.Collections.Generic.List<CharacterData> findThatMastermindData = Gameplay.Instance.GetAscensionAllStartingCharacters();
            CharacterData mastermindData = new();
            foreach (CharacterData character in findThatMastermindData)
            {
                if (character.characterId == "Mastermind_scm")
                {
                    mastermindData = character;
                }
            }
            foreach (Character c in Gameplay.CurrentCharacters)
            {
                if (!c.statuses.Contains(Guarding.guarded) && c.statuses.Contains(BigBrain.minion))
                {
                    c.UpdateRegisterAsRole(mastermindData);
                }
            }
        }
    }

    public Mastermind() : base(ClassInjector.DerivedConstructorPointer<Mastermind>())
    {
        ClassInjector.DerivedConstructorBody((Il2CppObjectBase)this);
    }
    public Mastermind(System.IntPtr ptr) : base(ptr) { }

}
public static class BigBrain
{
    public static ECharacterStatus minion = (ECharacterStatus)908;

    [HarmonyPatch(typeof(Character), nameof(Character.RevealReal))]
    public static class pvt
    {
        public static void Postfix(Character __instance)
        {
            if (__instance.statuses.Contains(minion) && __instance.bluff != null)
            {
                __instance.chName.text = "MASTERMIND";
            }
        }
    }
    [HarmonyPatch(typeof(Character), nameof(Character.RevealAllReal))]
    public static class pvt2
    {
        public static void Postfix(Character __instance)
        {
            if (__instance.statuses.Contains(minion))
            {
                __instance.chName.text = "MASTERMIND";
            }
        }
    }
}