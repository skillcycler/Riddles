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
public class Prankster : Role
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
        Il2CppSystem.Collections.Generic.List<Character> c = Gameplay.CurrentCharacters;
        Il2CppSystem.Collections.Generic.List<Character> goodTurned = new();
        Il2CppSystem.Collections.Generic.List<Character> evilTurned = new();
        foreach(Character ch in c)
        {
            if (ch.statuses.Contains(SwappedAlignment.turnedEvil)) evilTurned.Add(ch);
            if (ch.statuses.Contains(SwappedAlignment.turnedGood)) goodTurned.Add(ch);
        }
        if (goodTurned.Count == 0 || evilTurned.Count == 0)
        {
            return new ActedInfo("There were no valid swaps");
        }
        Il2CppSystem.Collections.Generic.List<Character> hints = new();
        hints.Add(goodTurned[UnityEngine.Random.RandomRangeInt(0, goodTurned.Count)]);
        hints.Add(evilTurned[UnityEngine.Random.RandomRangeInt(0, evilTurned.Count)]);

        return new ActedInfo($"#{Math.Min(hints[0].id, hints[1].id)} and #{Math.Max(hints[0].id, hints[1].id)}'s alignments have been swapped", hints);
    }
    public override ActedInfo GetBluffInfo(Character charRef)
    {
        Il2CppSystem.Collections.Generic.List<Character> c = Gameplay.CurrentCharacters;
        Il2CppSystem.Collections.Generic.List<Character> goodTurned = new();
        Il2CppSystem.Collections.Generic.List<Character> evilTurned = new();
        foreach (Character ch in c)
        {
            if (ch.id == charRef.id) continue;
            if (!ch.statuses.Contains(SwappedAlignment.turnedEvil)) evilTurned.Add(ch);
            if (!ch.statuses.Contains(SwappedAlignment.turnedGood)) goodTurned.Add(ch);
        }
        Il2CppSystem.Collections.Generic.List<Character> hints = new();
        hints.Add(goodTurned[UnityEngine.Random.RandomRangeInt(0, goodTurned.Count)]);
        evilTurned.Remove(hints[0]);
        hints.Add(evilTurned[UnityEngine.Random.RandomRangeInt(0, evilTurned.Count)]);

        return new ActedInfo($"#{Math.Min(hints[0].id, hints[1].id)} and #{Math.Max(hints[0].id, hints[1].id)}'s alignments have been swapped", hints);
    }
    public override void Act(ETriggerPhase trigger, Character charRef)
    {
        if (trigger == ETriggerPhase.Start)
        {
            Il2CppSystem.Collections.Generic.List<Character> c = Gameplay.CurrentCharacters;
            Il2CppSystem.Collections.Generic.List<Character> valid = new();
            foreach (Character ch in c)
            {
                if (ch.alignment == EAlignment.Evil && ch.bluff == null) continue; // don't pick outed evils
                if (ch.alignment == EAlignment.Good && !ch.dataRef.bluffable && ch.bluff == null) continue; // don't pick good characters that can't be disguised as
                valid.Add(ch);
            }
            Il2CppSystem.Collections.Generic.List<Character> goods = Characters.Instance.FilterRealAlignmentCharacters(valid, EAlignment.Good);
            Il2CppSystem.Collections.Generic.List<Character> evils = Characters.Instance.FilterRealAlignmentCharacters(valid, EAlignment.Evil);
            goods.Remove(charRef);
            if (goods.Count == 0 || evils.Count == 0) return;

            Character good = goods[UnityEngine.Random.RandomRangeInt(0, goods.Count)];
            Character evil = evils[UnityEngine.Random.RandomRangeInt(0, evils.Count)];
            good.statuses.AddStatus(SwappedAlignment.turnedEvil, charRef);
            evil.statuses.AddStatus(SwappedAlignment.turnedGood, charRef);
            good.ChangeAlignment(EAlignment.Evil);
            evil.ChangeAlignment(EAlignment.Good);
        }
        if (trigger == ETriggerPhase.Day)
        {
            onActed.Invoke(GetInfo(charRef));
        }
    }
    public override void BluffAct(ETriggerPhase trigger, Character charRef)
    {
        if (trigger == ETriggerPhase.Day)
        {
            onActed.Invoke(GetBluffInfo(charRef));
        }
    }
    public Prankster() : base(ClassInjector.DerivedConstructorPointer<Prankster>())
    {
        ClassInjector.DerivedConstructorBody((Il2CppObjectBase)this);
    }
    public Prankster(IntPtr ptr) : base(ptr) { }
}
public static class SwappedAlignment
{
    public static ECharacterStatus turnedGood = (ECharacterStatus)906;
    public static ECharacterStatus turnedEvil = (ECharacterStatus)907;
    [HarmonyPatch(typeof(Character), nameof(Character.RevealAllReal))]
    public static class pvt
    {
        public static void Postfix(Character __instance)
        {
            if (__instance.statuses.Contains(turnedGood))
            {
                __instance.chName.text = __instance.dataRef.name.ToUpper() + "<color=#00FF00><size=18>\n<Good></color></size>";
            } else if (__instance.statuses.Contains(turnedEvil))
            {
                __instance.chName.text = __instance.dataRef.name.ToUpper() + "<color=#FF0000><size=18>\n<Evil></color></size>";
            }
        }
    }
}