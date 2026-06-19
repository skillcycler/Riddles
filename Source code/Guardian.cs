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
public class Guardian : Minion
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
            SitNextToDemon(charRef);
            Il2CppSystem.Collections.Generic.List<Character> demons = Characters.Instance.FilterRealCharacterType(Gameplay.CurrentCharacters, ECharacterType.Demon);
            if (demons.Count > 0)
            {
                foreach (Character demon in demons)
                {
                    demon.statuses.AddStatus(ECharacterStatus.MessedUpByEvil, charRef);
                    demon.statuses.AddStatus(Guarding.guarded, charRef);
                }
            }

        }
        if (trigger == ETriggerPhase.AfterRoundStart)
        {
            foreach (Character c in Gameplay.CurrentCharacters)
            {
                if (c.statuses.Contains(Guarding.guarded))
                {
                    c.UpdateRegisterAsRole(c.bluff);
                }
                if (c.dataRef.characterId == "Mendaverte_WING")
                {
                    foreach (Character ch in Gameplay.CurrentCharacters)
                    {
                        if (c.alignment == EAlignment.Evil)
                        {
                            // I noticed that the problem only happens when Guardian is in play. So to fix it, Guardian will gain Mendaverte's ability if both are in play.
                            charRef.statuses.AddStatus(ECharacterStatus.HealthyBluff, charRef); // if Wingidon can't fix the bug, maybe I can
                        }
                    }
                }
            }
        }
    }
    private void SitNextToDemon(Character charRef)
    {
        Il2CppSystem.Collections.Generic.List<Character> checkDemons = new Il2CppSystem.Collections.Generic.List<Character>();
        checkDemons = Characters.Instance.FilterRealCharacterType(Gameplay.CurrentCharacters, ECharacterType.Demon);

        Character pickedDemon = checkDemons[UnityEngine.Random.Range(0, checkDemons.Count)];

        Il2CppSystem.Collections.Generic.List<Character> adjacentCharacters = Characters.Instance.GetAdjacentAliveCharacters(pickedDemon);
        Il2CppSystem.Collections.Generic.List<Character> filteredCharacters = new();
        foreach (Character c in adjacentCharacters) { 
            if (c.dataRef.characterId != "MadScientist_scm")
            {
                filteredCharacters.Add(c);
            }
        }
        Character pickedSwapCharacter = filteredCharacters[UnityEngine.Random.Range(0, filteredCharacters.Count)];
        CharacterData pickedData = pickedSwapCharacter.dataRef;
        pickedSwapCharacter.Init(charRef.dataRef);
        charRef.Init(pickedData);
    }

    public Guardian() : base(ClassInjector.DerivedConstructorPointer<Guardian>())
    {
        ClassInjector.DerivedConstructorBody((Il2CppObjectBase)this);
    }
    public Guardian(System.IntPtr ptr) : base(ptr) { }

}
[HarmonyPatch(typeof(Character), nameof(Character.Reveal))]
public static class Guarding
{
    public static ECharacterStatus guarded = (ECharacterStatus)878;
    public static void Postfix(Character __instance)
    {
        if (__instance.statuses.Contains(Guarding.guarded))
        {
            __instance.UpdateRegisterAsRole(__instance.bluff);
        }
    }
}