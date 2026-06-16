using System;
using System.Linq;
using System.Reflection;
using Il2Cpp;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.Injection;
using Il2CppInterop.Runtime.InteropTypes;
using Il2CppSystem.Reflection;
using MelonLoader;
using UnityEngine;
using static Il2CppSystem.Collections.SortedList;
using static MelonLoader.Modules.MelonModule;

namespace RiddlerMod;

[RegisterTypeInIl2Cpp]
public class Lawyer : Role
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
        Il2CppSystem.Collections.Generic.List<Character> characters = Gameplay.CurrentCharacters;
        
        Il2CppSystem.Collections.Generic.List<Character> truthfulCharacters = new Il2CppSystem.Collections.Generic.List<Character>();
        if (charRef.dataRef.characterId == "Hypnotist_scm") // if Lawyer is a hypnotist, always point to adjacent characters
        {
            Il2CppSystem.Collections.Generic.List<Character> adjacent = Characters.Instance.GetAdjacentCharacters(charRef);
            Character picked = adjacent[UnityEngine.Random.RandomRangeInt(0, adjacent.Count)];
            string info_h = string.Format("#{0} is Truthful", picked.id);
            ActedInfo actedInfo_h = new ActedInfo(info_h);
            return actedInfo_h;
        }
        foreach (Character character in characters)
        {
            bool isAdjacent = false;
            foreach (Character c in Characters.Instance.GetAdjacentCharacters(character))
            {
                if (c.id == charRef.id)
                {
                    isAdjacent = true;
                }
            }
            bool lying = CharacterHelper.CheckLyingAppearance(character);
            bool isEvil = (character.GetRegisterAlignment() == EAlignment.Evil);
            if ((!lying && !isAdjacent) || (isAdjacent && isEvil))
            {
                truthfulCharacters.Add(character);
            }
        }
        if (truthfulCharacters.Count > 1)
            truthfulCharacters.Remove(charRef);
        Character chosenCharacter = truthfulCharacters[UnityEngine.Random.RandomRangeInt(0, truthfulCharacters.Count)];
        
        string info = string.Format("#{0} is Truthful", chosenCharacter.id);
        ActedInfo actedInfo = new ActedInfo(info);
        return actedInfo;
    }

    public override ActedInfo GetBluffInfo(Character charRef)
    {
        Il2CppSystem.Collections.Generic.List<Character> characters = Gameplay.CurrentCharacters;
        
        Il2CppSystem.Collections.Generic.List<Character> untruthfulCharacters = new Il2CppSystem.Collections.Generic.List<Character>();
        foreach (Character character in characters)
        {
            bool isAdjacent = false;
            foreach (Character c in Characters.Instance.GetAdjacentCharacters(character))
            {
                if (c.id == charRef.id)
                {
                    isAdjacent = true;
                }
            }
            bool lying = CharacterHelper.CheckLyingAppearance(character);
            bool isEvil = (character.GetRegisterAlignment() == EAlignment.Evil);
            if ((lying && !isAdjacent) || (lying && isAdjacent && !isEvil))
            {
                untruthfulCharacters.Add(character);
            }
        }
        if (untruthfulCharacters.Count > 1)
            untruthfulCharacters.Remove(charRef);
        Character chosenCharacter = untruthfulCharacters[UnityEngine.Random.RandomRangeInt(0, untruthfulCharacters.Count)];

        string info = string.Format("#{0} is Truthful", chosenCharacter.id);
        ActedInfo actedInfo = new ActedInfo(info);
        return actedInfo;
    }
    public override void Act(ETriggerPhase trigger, Character charRef)
    {
        if (trigger == ETriggerPhase.Start && charRef.dataRef.characterId != "Hypnotist_scm")
        {
            Il2CppSystem.Collections.Generic.List<Character> adjacentCharacters = Characters.Instance.GetAdjacentCharacters(charRef);
            foreach (Character character in adjacentCharacters)
            {
                character.statuses.AddStatus(ECharacterStatus.HealthyBluff, charRef);
                character.statuses.statuses.Remove(ECharacterStatus.Corrupted);
            }
        }    
        if (trigger == ETriggerPhase.Day)
        {
            onActed.Invoke(GetInfo(charRef));
        }
    }
    public override void BluffAct(ETriggerPhase trigger, Character charRef)
    {
        /*
        if (trigger == ETriggerPhase.Start)
        {
            Il2CppSystem.Collections.Generic.List<Character> adjacentCharacters = Characters.Instance.GetAdjacentCharacters(charRef);
            foreach (Character character in adjacentCharacters)
            {
                character.statuses.AddStatus(ECharacterStatus.Corrupted, charRef);
                character.statuses.statuses.Remove(ECharacterStatus.HealthyBluff);
            }
        }
        */
        if (trigger == ETriggerPhase.Day)
        {
            onActed.Invoke(GetBluffInfo(charRef));
        }
    }
    public Lawyer() : base(ClassInjector.DerivedConstructorPointer<Lawyer>())
    {
        ClassInjector.DerivedConstructorBody((Il2CppObjectBase)this);
    }

    public Lawyer(System.IntPtr ptr) : base(ptr)
    {

    }
}