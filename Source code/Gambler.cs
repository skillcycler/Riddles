using System;
using System.ComponentModel.Design;
using HarmonyLib;
using Il2Cpp;
using Il2CppFIMSpace.Basics;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.Injection;
using Il2CppInterop.Runtime.InteropTypes;
using Il2CppSystem;
using Il2CppSystem.Collections.Generic;
using MelonLoader;
using RiddlerMod;
using UnityEngine;
using static Il2CppSystem.Globalization.HebrewNumber;
using static MelonLoader.MelonLogger;

namespace RiddlerMod;

[RegisterTypeInIl2Cpp]
public class Gambler : Role
{
    public int affected = 0;
    public override string Description
    {
        get
        {
            return "";
        }
    }
    public override ActedInfo GetInfo(Character charRef)
    {
        if (affected == 0 && charRef.alignment == EAlignment.Evil) { return new ActedInfo("I am a Truthful Evil disguised as the Gambler."); }
        if (affected == 0) { return new ActedInfo($"I forgot who I gambled with."); }
        return new ActedInfo($"I invited #{affected} to my casino.");
    }
    public override ActedInfo GetBluffInfo(Character charRef)
    {
        int fake = Calculator.RemoveNumberAndGetRandomNumberFromList(charRef.id, 1, Gameplay.CurrentCharacters.Count+1);
        return new ActedInfo($"I invited #{fake} to my casino.");
    }
    public static void ApplyRandomStatus(Character picked, Character charRef)
    {

        bool isAtheist = false;
        foreach (Character c in Gameplay.CurrentCharacters)
        {
            if (c.dataRef.characterId == "Atheist_scm" && c.alignment == EAlignment.Evil) isAtheist = true;
        }
        if (picked.alignment == EAlignment.Evil || isAtheist)
        {
            switch (Calculator.RollDice(3))
            {
                case 1:
                    picked.statuses.AddStatus(ECharacterStatus.Corrupted, charRef); break;
                case 2:
                    picked.statuses.AddStatus(Confused.confused, charRef);
                    Confused.updateConfusion(charRef);
                    break;
                case 3:
                    picked.statuses.AddStatus(Broken.erased, charRef);
                    picked.UpdateRegisterAsRole(Fracture.GetNothing());
                    break;

            }
        }
        else
        {
            switch (Calculator.RollDice(4))
            {
                case 1:
                    picked.statuses.AddStatus(ECharacterStatus.Corrupted, charRef); break;
                case 2:
                    picked.statuses.AddStatus(Escaped.evilTurned, charRef);
                    picked.ChangeAlignment(EAlignment.Evil);
                    break;
                case 3:
                    picked.statuses.AddStatus(Accused.accused, charRef); break;
                case 4:
                    picked.statuses.AddStatus(Confused.confused, charRef);
                    Confused.updateConfusion(charRef);
                    break;
                case 5:
                    picked.statuses.AddStatus(Broken.erased, charRef);
                    picked.UpdateRegisterAsRole(Fracture.GetNothing());
                    break;

            }
        }
    }
    public override void Act(ETriggerPhase trigger, Character charRef)
    {
        if (trigger == ETriggerPhase.Start)
        {
            Il2CppSystem.Collections.Generic.List<Character> chars = Gameplay.CurrentCharacters;
            Character picked = chars[Calculator.RemoveNumberAndGetRandomNumberFromList(charRef.id, 0, chars.Count)];
            affected = picked.id;
            ApplyRandomStatus(picked, charRef);
        }
        if (trigger == ETriggerPhase.AfterRoundStart)
        {
            Accused.UpdateAccusedRegistration();
        }
        if (trigger == ETriggerPhase.Night)
        {
            Confused.updateConfusion(charRef);
        }
        if (trigger == ETriggerPhase.Day)
        {
            onActed.Invoke(GetInfo(charRef));
        }
    }
    public override void BluffAct(ETriggerPhase trigger, Character charRef)
    {
        if (trigger == ETriggerPhase.Night)
        {
            Confused.updateConfusion(charRef);
        }
        if (trigger == ETriggerPhase.Day)
        {
            onActed.Invoke(GetBluffInfo(charRef));
        }
    }
    public Gambler() : base(ClassInjector.DerivedConstructorPointer<Gambler>())
    {
        ClassInjector.DerivedConstructorBody((Il2CppObjectBase)this);
    }
    public Gambler(System.IntPtr ptr) : base(ptr) { }
}
