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
            Il2CppSystem.Collections.Generic.List<Character> adjacent = Characters.Instance.GetAdjacentCharacters(charRef); ;
            foreach (Character character in adjacent) {
                character.statuses.AddStatus(Guarding.guarded, charRef);
            }

        }
        if (trigger == ETriggerPhase.AfterRoundStart)
        {
            foreach (Character c in Gameplay.CurrentCharacters)
            {
                if (c.statuses.Contains(Guarding.guarded) && !c.statuses.Contains(Accused.accused))
                {
                    c.UpdateRegisterAsRole(c.bluff);
                    c.statuses.AddStatus(ECharacterStatus.MessedUpByEvil, charRef);
                    c.statuses.AddStatus(ECharacterStatus.AppearHonest, charRef);
                }
            }
        }
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

[HarmonyPatch(typeof(Character), nameof(Character.RevealAllReal))]
public static class GuardianText
{
    public static void Postfix(Character __instance)
    {
        if (__instance.statuses.Contains(Guarding.guarded))
        {
            __instance.chName.text = __instance.dataRef.name.ToUpper() + "<color=#55FF55><size=18>\n<Guarded></color></size>";
        }
    }
}