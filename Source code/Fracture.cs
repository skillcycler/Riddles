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

namespace RiddlerMod;

[RegisterTypeInIl2Cpp]
public class Fracture : Demon
{
    public override Il2CppSystem.Collections.Generic.List<SpecialRule> GetRules()
    {
        Il2CppSystem.Collections.Generic.List<SpecialRule> sr = new Il2CppSystem.Collections.Generic.List<SpecialRule>();
        sr.Add(new NightModeRule(4));
        return sr;
    }
    public override void Act(ETriggerPhase trigger, Character charRef)
    {
        if (trigger == ETriggerPhase.Start)
        {
            Character remove = Gameplay.CurrentCharacters[UnityEngine.Random.RandomRangeInt(0, Gameplay.CurrentCharacters.Count)];
            while (remove.id == charRef.id) remove = Gameplay.CurrentCharacters[UnityEngine.Random.RandomRangeInt(0, Gameplay.CurrentCharacters.Count)];
            remove.statuses.AddStatus(Broken.erased, charRef);
            Gameplay.CurrentCharacters.Remove(remove);
        }
        if (trigger == ETriggerPhase.Night)
        {
            if (charRef.state == ECharacterState.Dead) return;
            Character remove = Gameplay.CurrentCharacters[UnityEngine.Random.RandomRangeInt(0, Gameplay.CurrentCharacters.Count)];
            while (remove.id == charRef.id) remove = Gameplay.CurrentCharacters[UnityEngine.Random.RandomRangeInt(0, Gameplay.CurrentCharacters.Count)];
            remove.statuses.AddStatus(Broken.erased, charRef);
            Gameplay.CurrentCharacters.Remove(remove);
        }
    }
    public Fracture() : base(ClassInjector.DerivedConstructorPointer<Fracture>())
    {
        ClassInjector.DerivedConstructorBody((Il2CppObjectBase)this);
    }
    public Fracture(System.IntPtr ptr) : base(ptr) { }
}
public static class Broken
{
    public static ECharacterStatus erased = (ECharacterStatus)911;

    [HarmonyPatch(typeof(Character), nameof(Character.RevealAllReal))]
    public static class pvt
    {
        public static void Postfix(Character __instance)
        {
            if (__instance.statuses.Contains(erased))
            {
                __instance.chName.text = __instance.dataRef.name.ToUpper() + "<color=#FF8000><size=18>\n<Erased></color></size>";
            }
        }
    }
}