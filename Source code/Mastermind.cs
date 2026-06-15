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

            Il2CppSystem.Collections.Generic.List<CharacterData> findThatMastermindData = Gameplay.Instance.GetAscensionAllStartingCharacters();
            CharacterData mastermindData = new();
            foreach (CharacterData character in findThatMastermindData)
            {
                if (character.characterId == "Mastermind_scm")
                {
                    mastermindData = character;
                }
            }
            foreach (Character evil in evils)
            {
                if (evil.dataRef.name == "Witch")
                {
                    PlayerController.PlayerInfo.blocks.value.Reduce(1); // You can probably guess why
                }
                if (evil.dataRef.characterId != "Mastermind_scm" && evil.dataRef.type == ECharacterType.Minion)
                    evil.Init(mastermindData);
            }
        }
    }

    public Mastermind() : base(ClassInjector.DerivedConstructorPointer<Mastermind>())
    {
        ClassInjector.DerivedConstructorBody((Il2CppObjectBase)this);
    }
    public Mastermind(System.IntPtr ptr) : base(ptr) { }

}