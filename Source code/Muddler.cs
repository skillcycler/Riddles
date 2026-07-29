using System;
using System.Linq;
using System.Reflection;
using Il2Cpp;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.Injection;
using Il2CppInterop.Runtime.InteropTypes;
using Il2CppSystem.Reflection;
using MelonLoader;
using UnityEngine;
using static Il2CppSystem.Collections.SortedList;
using static MelonLoader.Modules.MelonModule;
using HarmonyLib;

namespace RiddlerMod;

[RegisterTypeInIl2Cpp]
public class Muddler : Role
{
    public override string Description
    {
        get
        {
            return "";
        }
    }
    public override void Act(ETriggerPhase trigger, Character charRef)
    {
        if (trigger == ETriggerPhase.Start)
        {
            Il2CppSystem.Collections.Generic.List<Character> allCharacters = Gameplay.CurrentCharacters;
            foreach (Character character in allCharacters) {
                character.statuses.AddStatus(Muddling.hiddenStatus, charRef);
            }
        }
    }
    public override void BluffAct(ETriggerPhase trigger, Character charRef)
    {
    }
    public Muddler() : base(ClassInjector.DerivedConstructorPointer<Muddler>())
    {
        ClassInjector.DerivedConstructorBody((Il2CppObjectBase)this);
    }
    public Muddler(System.IntPtr ptr) : base(ptr)
    {

    }
}
public static class Muddling
{
    public static ECharacterStatus hiddenStatus = (ECharacterStatus)879;
    [HarmonyPatch(typeof(Character), nameof(Character.RevealAllReal))]
    public static class pvt
    {
        [HarmonyPostfix]
        [HarmonyPriority(Priority.Last)]
        public static void Postfix(Character __instance)
        {
            if (__instance.statuses.Contains(hiddenStatus) && Gameplay.GameplayState != EGameplayState.Summary)
            {
                __instance.chName.text = __instance.dataRef.name.ToUpper();
            }
        }
    }
}