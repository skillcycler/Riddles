using Il2Cpp;
using Il2CppInterop.Runtime.Injection;
using Il2CppInterop.Runtime.InteropTypes;
using Il2CppSystem;
using MelonLoader;
using System;
using System.ComponentModel.Design;
using UnityEngine;
using HarmonyLib;

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
                Character randomChar = demons[UnityEngine.Random.Range(0, demons.Count)];
                randomChar.statuses.AddStatus(ECharacterStatus.MessedUpByEvil, charRef);
                randomChar.statuses.AddStatus(Guarding.guarded, charRef);
            }
        }
    }
    private void SitNextToDemon(Character charRef)
    {
        Il2CppSystem.Collections.Generic.List<Character> checkDemons = new Il2CppSystem.Collections.Generic.List<Character>();
        checkDemons = Characters.Instance.FilterRealCharacterType(Gameplay.CurrentCharacters, ECharacterType.Demon);

        Character pickedDemon = checkDemons[UnityEngine.Random.Range(0, checkDemons.Count)];

        Il2CppSystem.Collections.Generic.List<Character> adjacentCharacters = Characters.Instance.GetAdjacentAliveCharacters(pickedDemon);
        Character pickedSwapCharacter = adjacentCharacters[UnityEngine.Random.Range(0, adjacentCharacters.Count)];
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