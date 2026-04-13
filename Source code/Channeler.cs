using Il2Cpp;
using Il2CppInterop.Runtime.Injection;
using Il2CppInterop.Runtime.InteropTypes;
using Il2CppSystem;
using MelonLoader;
using System;
using System.ComponentModel.Design;
using UnityEngine;
using HarmonyLib;

public class Channeler : Minion
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
            Il2CppSystem.Collections.Generic.List<Character> allowedCharacters = new();
            foreach (Character character in characters) { // two characters that will end up causing problems if copied
                if (character.dataRef.characterId != "Undying_WING" && character.dataRef.characterId != "MadScientist_scm")
                    allowedCharacters.Add(character);
            }
            copy = allowedCharacters[UnityEngine.Random.RandomRangeInt(0, allowedCharacters.Count)].dataRef;
            copy.role.Act(trigger, charRef);
        }
        if (trigger != ETriggerPhase.Start)
        {
            copy.role.Act(trigger, charRef);
        }
    }
    public override CharacterData GetBluffIfAble(Character charRef)
    {
        if (copy.characterId == "Illusionist_WING") // no disguise for a Channeler copying Emenverax
            return null;
        int diceRoll = Calculator.RollDice(10);

        if (diceRoll < 5)
        {
            return Characters.Instance.GetRandomDuplicateBluff();
        }
        else
        {
            CharacterData bluff = Characters.Instance.GetRandomUniqueBluff();
            Gameplay.Instance.AddScriptCharacterIfAble(bluff.type, bluff);

            return bluff;
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
    public Channeler() : base(ClassInjector.DerivedConstructorPointer<Channeler>())
    {
        ClassInjector.DerivedConstructorBody((Il2CppObjectBase)this);
    }
    public Channeler(System.IntPtr ptr) : base(ptr) { }
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