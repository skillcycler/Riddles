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
public class Obsessor : Role
{
    public override string Description
    {
        get
        {
            return "";
        }
    }
    public Character GetValidCharacter(Il2CppSystem.Collections.Generic.List<Character> characters)
    {
        // Don't say any character that has multiple of it or shares the same name with another character
        Il2CppSystem.Collections.Generic.List<Character> nodupe = new();
        foreach (Character c1 in characters)
        {
            int m = 0;
            foreach (Character c2 in characters)
            {
                if (c2.GetRegisterAs().characterName == c1.GetRegisterAs().characterName) m++;
            }
            if (m == 1 && c1.GetRegisterAs().characterId == c1.dataRef.characterId)
            {
                nodupe.Add(c1);
            }
        }
        return nodupe[UnityEngine.Random.RandomRangeInt(0, nodupe.Count)];

    }
    public override ActedInfo GetInfo(Character charRef)
    {
        Il2CppSystem.Collections.Generic.List<Character> characters = Gameplay.CurrentCharacters;
        /*Il2CppSystem.Collections.Generic.List<Character> actualCharacters = new Il2CppSystem.Collections.Generic.List<Character>();
        bool isSpecularusVillage = true;
        foreach (Character character in characters)
        {
            if (!character.bluff)
                isSpecularusVillage = false;
        }
        foreach (Character character in characters) {
            // gonna make it not useless in Specularus villages
            if (!isSpecularusVillage || character.GetRegisterAlignment() == EAlignment.Evil)
                actualCharacters.Add(character);
        }
        Character chosenCharacter = actualCharacters[UnityEngine.Random.RandomRangeInt(0, actualCharacters.Count)];*/
        Character chosenCharacter = GetValidCharacter(characters);
        int evils = 0;
        Il2CppSystem.Collections.Generic.List<Character> adjacentCharacters = Characters.Instance.GetAdjacentCharacters(chosenCharacter);
        foreach (Character character in adjacentCharacters)
        {
            if (character.GetRegisterAlignment() == EAlignment.Evil)
            {
                evils++;
            }
        }
        string info = string.Format("The {0} neighbors {1} Evils", chosenCharacter.GetRegisterAs().name, evils);
        if (evils == 1)
            info = string.Format("The {0} neighbors {1} Evil", chosenCharacter.GetRegisterAs().name, evils);
        ActedInfo actedInfo = new ActedInfo(info);
        return actedInfo;
    }

    public override ActedInfo GetBluffInfo(Character charRef)
    {
        Il2CppSystem.Collections.Generic.List<Character> characters = Gameplay.CurrentCharacters;
        /*Il2CppSystem.Collections.Generic.List<Character> actualCharacters = new Il2CppSystem.Collections.Generic.List<Character>();
        foreach (Character character in characters)
        {
            actualCharacters.Add(character);
        }
        Character chosenCharacter = actualCharacters[UnityEngine.Random.RandomRangeInt(0, actualCharacters.Count)];*/
        Character chosenCharacter = GetValidCharacter(characters);
        int evils = 0;
        Il2CppSystem.Collections.Generic.List<Character> adjacentCharacters = Characters.Instance.GetAdjacentCharacters(chosenCharacter);
        foreach (Character character in adjacentCharacters)
        {
            if (character.GetRegisterAlignment() == EAlignment.Evil)
            {
                evils++;
            }
        }
        int fakeNumber = UnityEngine.Random.RandomRangeInt(0, 3);
        while (fakeNumber == evils)
        {
            fakeNumber = UnityEngine.Random.RandomRangeInt(0, 3);
        }
        string info = string.Format("The {0} neighbors {1} Evils", chosenCharacter.GetRegisterAs().name, fakeNumber);
        if (fakeNumber == 1)
            info = string.Format("The {0} neighbors {1} Evil", chosenCharacter.GetRegisterAs().name, fakeNumber);
        ActedInfo actedInfo = new ActedInfo(info);
        return actedInfo;
    }

    public override void Act(ETriggerPhase trigger, Character charRef)
    {
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
    public Obsessor() : base(ClassInjector.DerivedConstructorPointer<Obsessor>())
    {
        ClassInjector.DerivedConstructorBody((Il2CppObjectBase)this);
    }

    public Obsessor(System.IntPtr ptr) : base(ptr)
    {

    }
}