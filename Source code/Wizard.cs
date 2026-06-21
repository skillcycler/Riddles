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

namespace RiddlerMod;

[RegisterTypeInIl2Cpp]

public class Wizard : Minion
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
        if (trigger != ETriggerPhase.Start) return;

        Il2CppSystem.Collections.Generic.List<Character> characters = Gameplay.CurrentCharacters;
        Il2CppSystem.Collections.Generic.List<Character> availableToDupe = new();
        foreach (Character character in characters)
        {
            if (character.dataRef.type == ECharacterType.Outcast || character.dataRef.type == ECharacterType.Minion && character.dataRef.characterId != "Undying_WING")
            {
                availableToDupe.Add(character);
            }
        }
        availableToDupe.Remove(charRef);
        if (availableToDupe.Count < 1) return;
        Character picked = availableToDupe[UnityEngine.Random.Range(0, availableToDupe.Count)];
        picked.statuses.AddStatus(ECharacterStatus.MessedUpByEvil, charRef);

        Il2CppSystem.Collections.Generic.List<Character> availableToBeDuped = new();
        foreach (Character character in characters)
        {
            if (character.dataRef.type != ECharacterType.Demon && picked.id != character.id)
            {
                availableToBeDuped.Add(character);
            }
        }
        availableToBeDuped.Remove(charRef);

        Character replaced = availableToBeDuped[UnityEngine.Random.Range(0, availableToBeDuped.Count)];
        MelonLogger.Msg(string.Format("I am duplicating #{0}, the {1}, onto #{2} which was originally a {3}.", picked.id, picked.dataRef.characterName, replaced.id, replaced.dataRef.characterName));

        replaced.Init(picked.dataRef);

        if (Characters.Instance.CheckIfCharacterShouldStartAct(picked.dataRef))
            replaced.Act(ETriggerPhase.Start);

        replaced.statuses.AddStatus(ECharacterStatus.MessedUpByEvil, charRef);
    }

    public Wizard() : base(ClassInjector.DerivedConstructorPointer<Wizard>())
    {
        ClassInjector.DerivedConstructorBody((Il2CppObjectBase)this);
    }
    public Wizard(System.IntPtr ptr) : base(ptr) { }

}