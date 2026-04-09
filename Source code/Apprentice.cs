using Il2Cpp;
using Il2CppInterop.Runtime.Injection;
using Il2CppInterop.Runtime.InteropTypes;
using Il2CppSystem;
using MelonLoader;
using System;
using System.ComponentModel.Design;
using UnityEngine;
using HarmonyLib;

public class Apprentice : Minion
{
    public CharacterData copy = GetGenericMinion();
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
            Il2CppSystem.Collections.Generic.List<Character> characters = Gameplay.CurrentCharacters;
            characters = Characters.Instance.FilterRealAlignmentCharacters(characters, EAlignment.Evil);
            characters.Remove(charRef);
            copy = characters[UnityEngine.Random.RandomRangeInt(0, characters.Count)].dataRef;
            copy.role.Act(trigger, charRef);
        }
        if (trigger != ETriggerPhase.Start)
        {
            copy.role.Act(trigger, charRef);
        }
    }
    public override void ActOnDied(Character charRef)
    {
        copy.role.ActOnDied(charRef);
    }
    public override bool CheckIfCanBeKilled(Character charRef)
    {
        return copy.role.CheckIfCanBeKilled(charRef);
    }
    public Apprentice() : base(ClassInjector.DerivedConstructorPointer<Apprentice>())
    {
        ClassInjector.DerivedConstructorBody((Il2CppObjectBase)this);
    }
    public Apprentice(System.IntPtr ptr) : base(ptr) { }
    public static CharacterData GetGenericMinion()
    {
        AscensionsData allCharactersAscension = ProjectContext.Instance.gameData.allCharactersAscension;
        for (int i = 0; i < allCharactersAscension.startingMinions.Length; i++)
        {
            if (allCharactersAscension.startingMinions[i].name == "Minion")
                return allCharactersAscension.startingMinions[i];
        }
        return allCharactersAscension.startingMinions[0];
    }
}