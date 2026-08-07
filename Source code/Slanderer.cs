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
public class Slanderer : Minion
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
            Il2CppSystem.Collections.Generic.List<Character> furthest = new();
            foreach (Character ch in Gameplay.CurrentCharacters)
            {
                if (Gameplay.CurrentCharacters.Count % 2 == 0)
                {
                    if (System.Math.Abs(ch.id - charRef.id) == Gameplay.CurrentCharacters.Count / 2)
                    {
                        furthest.Add(ch);
                    }
                } else
                {
                    if (System.Math.Abs(ch.id - charRef.id) == (Gameplay.CurrentCharacters.Count - 1) / 2 || System.Math.Abs(ch.id - charRef.id) == (Gameplay.CurrentCharacters.Count + 1) / 2)
                    {
                        furthest.Add(ch);
                    }
                }
            }
            foreach (Character c in furthest)
            {
                if (c.GetRealAlignment() == EAlignment.Good)
                {
                    c.statuses.AddStatus(Accused.accused, charRef);
                    c.statuses.AddStatus(ECharacterStatus.MessedUpByEvil, charRef);
                } else
                {
                    c.statuses.AddStatus(Guarding.guarded, charRef);
                    c.statuses.AddStatus(ECharacterStatus.MessedUpByEvil, charRef);
                }
            }
        }
        //just to make sure it works
        if (trigger == ETriggerPhase.AfterRoundStart)
        {
            foreach (Character c in Gameplay.CurrentCharacters)
            {
                if (c.statuses.Contains(Accused.accused))
                {
                    Accused.UpdateAccusedRegistration();
                }
                else if (c.statuses.Contains(Guarding.guarded))
                {
                    c.UpdateRegisterAsRole(c.bluff);
                }
            }
        }
    }

    public Slanderer() : base(ClassInjector.DerivedConstructorPointer<Slanderer>())
    {
        ClassInjector.DerivedConstructorBody((Il2CppObjectBase)this);
    }
    public Slanderer(System.IntPtr ptr) : base(ptr) { }

}