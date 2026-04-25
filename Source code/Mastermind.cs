using Il2Cpp;
using Il2CppInterop.Runtime.Injection;
using Il2CppInterop.Runtime.InteropTypes;
using Il2CppSystem;
using MelonLoader;
using System;
using System.ComponentModel.Design;
using UnityEngine;
using HarmonyLib;

public class Mastermind : Minion
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
            Il2CppSystem.Collections.Generic.List<Character> evils = Characters.Instance.FilterRealAlignmentCharacters(Gameplay.CurrentCharacters, EAlignment.Evil);
            evils.Remove(charRef);
            foreach (Character evil in evils)
            {
                if (evil.dataRef.characterId != "Mastermind_scm" && evil.dataRef.type == ECharacterType.Minion)
                    evil.Init(charRef.GetRegisterAs());
            }
        }
    }

    public Mastermind() : base(ClassInjector.DerivedConstructorPointer<Mastermind>())
    {
        ClassInjector.DerivedConstructorBody((Il2CppObjectBase)this);
    }
    public Mastermind(System.IntPtr ptr) : base(ptr) { }

}