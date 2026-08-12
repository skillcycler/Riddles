using System;
using System.Linq;
using System.Reflection;
using HarmonyLib;
using Il2Cpp;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.Injection;
using Il2CppInterop.Runtime.InteropTypes;
using Il2CppSystem.Reflection;
using MelonLoader;
using UnityEngine;
using static Il2CppSystem.Collections.SortedList;
using static MelonLoader.MelonLogger;
using static MelonLoader.Modules.MelonModule;

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
        public static void Postfix(Character __instance)
        {
            if (__instance.statuses.Contains(hiddenStatus))
            {
                if (__instance.bluff) __instance.chName.text = __instance.bluff.name.ToUpper();
                else __instance.chName.text = __instance.dataRef.name.ToUpper();
                MelonLogger.Msg($"Muddled: #{__instance.id} is the {__instance.dataRef.characterName}");
            }
        }
    }
    [HarmonyPatch(typeof(Character), nameof(Character.RevealAllReal))]
    public static class pvt2
    {
        public static bool Prefix(Character __instance)
        {
            if (__instance.statuses.Contains(hiddenStatus))
            {
                if (__instance.bluff) __instance.chName.text = __instance.bluff.name.ToUpper();
                else __instance.chName.text = __instance.dataRef.name.ToUpper();
                return false;
            }
            return true;
        }
    }
    [HarmonyPatch(typeof(Character), nameof(Character.ShowDescription))]
    public static class HideTrueDescription
    {
        [HarmonyPriority(Priority.Last)]
        public static void Postfix(Character __instance)
        {
            if (__instance.statuses.Contains(hiddenStatus) && !Characters.Instance.FilterAliveCharacters(Gameplay.CurrentCharacters).Contains(__instance))
            {
                HintInfo info = new HintInfo();
                info.text = "True Role's ability text is hidden.";
                UIEvents.OnShowHint.Invoke(info, __instance.hintPivot);
            }
        }
    }
}