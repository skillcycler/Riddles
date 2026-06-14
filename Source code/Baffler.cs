using System;
using System.ComponentModel.Design;
using HarmonyLib;
using Il2Cpp;
using Il2CppInterop.Runtime.Injection;
using Il2CppInterop.Runtime.InteropTypes;
using Il2CppSystem;
using MelonLoader;
using UnityEngine;
using static MelonLoader.MelonLogger;

public class Baffler : Minion
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
            Il2CppSystem.Collections.Generic.List<Character> neighbors = Characters.Instance.GetAdjacentCharacters(charRef);
            neighbors = Characters.Instance.FilterRealCharacterType(neighbors, ECharacterType.Villager);
            neighbors = Characters.Instance.FilterCharacterMissingStatus(neighbors, Confused.confused);
            neighbors = Characters.Instance.FilterCharacterMissingStatus(neighbors, ECharacterStatus.Corrupted); // Prefer to Confuse characters that are not corrupted
            if (neighbors.Count == 0)
            {
                neighbors = Characters.Instance.GetAdjacentCharacters(charRef);
                neighbors = Characters.Instance.FilterRealCharacterType(neighbors, ECharacterType.Villager);
                neighbors = Characters.Instance.FilterCharacterMissingStatus(neighbors, Confused.confused);
            }
            if (neighbors.Count > 0)
            {
                Character randomChar = neighbors[UnityEngine.Random.Range(0, neighbors.Count)];
                randomChar.statuses.AddStatus(ECharacterStatus.MessedUpByEvil, charRef);
                randomChar.statuses.AddStatus(Confused.confused, charRef);

                if (Calculator.RollDice(2) == 1)
                {
                    randomChar.statuses.AddStatus(ECharacterStatus.Corrupted, charRef);
                }
            }
        }
        if (trigger == ETriggerPhase.Night)
        {
            foreach (Character c in Gameplay.CurrentCharacters)
            {
                if (c.statuses.Contains(Confused.confused))
                {
                    if (Calculator.RollDice(2) == 1)
                    {
                        c.statuses.AddStatus(ECharacterStatus.Corrupted, charRef);
                    }
                    else if (c.statuses.Contains(ECharacterStatus.Corrupted))
                    {
                        c.statuses.statuses.Remove(ECharacterStatus.Corrupted);
                    }
                }
            }
        }
    }

    public Baffler() : base(ClassInjector.DerivedConstructorPointer<Baffler>())
    {
        ClassInjector.DerivedConstructorBody((Il2CppObjectBase)this);
    }
    public Baffler(System.IntPtr ptr) : base(ptr) { }

}
public static class Confused
{
    public static ECharacterStatus confused = (ECharacterStatus)881;

    [HarmonyPatch(typeof(Character), nameof(Character.RevealAllReal))]
    public static class pvt
    {
        public static void Postfix(Character __instance)
        {
            if (__instance.statuses.Contains(confused))
            {
                __instance.chName.text = __instance.dataRef.name.ToUpper() + "<color=#DDDD00><size=18>\n<Confused></color></size>";
            }
        }
    }
}