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

public class Mystifier : Demon
{
    public override Il2CppSystem.Collections.Generic.List<SpecialRule> GetRules()
    {
        Il2CppSystem.Collections.Generic.List<SpecialRule> sr = new Il2CppSystem.Collections.Generic.List<SpecialRule>();
        sr.Add(new NightModeRule(4));
        return sr;
    }
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
        if (trigger == ETriggerPhase.Start || trigger == ETriggerPhase.Night)
        {
            Il2CppSystem.Collections.Generic.List<Character> characters = Gameplay.CurrentCharacters;
            characters = Characters.Instance.FilterRealCharacterType(characters, ECharacterType.Villager);
            characters = Characters.Instance.FilterCharacterMissingStatus(characters, Confused.confused);

            if (characters.Count > 0)
            {
                Character randomChar = characters[UnityEngine.Random.Range(0, characters.Count)];
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

    public Mystifier() : base(ClassInjector.DerivedConstructorPointer<Mystifier>())
    {
        ClassInjector.DerivedConstructorBody((Il2CppObjectBase)this);
    }
    public Mystifier(System.IntPtr ptr) : base(ptr) { }

}