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
public class Director : Role
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
        System.Collections.Generic.List<int> evils = new System.Collections.Generic.List<int>();
        foreach (Character character in characters)
        {
            if (character.GetAlignment() == EAlignment.Evil)
            {
                evils.Add(character.id);
            }
        }
        if (evils.Count < 2)
        {
            return new ActedInfo("There is only 1 Evil");
        }
        System.Collections.Generic.List<int> possibleStart = new System.Collections.Generic.List<int>();
        System.Collections.Generic.List<int> possibleEnd = new System.Collections.Generic.List<int>();
        // inefficient ahh triple for loop
        for (int i = 1; i < characters.Count; i++)
        {
            for (int j = characters.Count; j > i; j--)
            {
                int evilsFound = 0;
                for (int k = i; k <= j; k++)
                {
                    if (evils.Contains(k))
                    {
                        evilsFound++;
                    }
                }
                if (evilsFound == 2)
                {
                    possibleStart.Add(i);
                    possibleEnd.Add(j);
                }
            }
        }
        int chosen = UnityEngine.Random.RandomRangeInt(0, possibleStart.Count);
        int chosenStart = possibleStart[chosen];
        int chosenEnd = possibleEnd[chosen];
        string info = string.Format("There are 2 evils from #{0} to #{1}", chosenStart, chosenEnd);
        Il2CppSystem.Collections.Generic.List<Character> hint = new();
        for (int i = chosenStart; i <= chosenEnd; i++)
        {
            foreach (Character character in Gameplay.CurrentCharacters)
            {
                if (character.id == i)
                    hint.Add(character);
            }
        }
        ActedInfo actedInfo = new ActedInfo(info, hint);
        return actedInfo;
    }

    public override ActedInfo GetBluffInfo(Character charRef)
    {
        Il2CppSystem.Collections.Generic.List<Character> characters = Gameplay.CurrentCharacters;
        System.Collections.Generic.List<int> evils = new System.Collections.Generic.List<int>();
        foreach (Character character in characters)
        {
            if (character.GetAlignment() == EAlignment.Evil)
            {
                evils.Add(character.id);
            }
        }
        System.Collections.Generic.List<int> possibleStart = new System.Collections.Generic.List<int>();
        System.Collections.Generic.List<int> possibleEnd = new System.Collections.Generic.List<int>();
        // inefficient ahh triple for loop
        for (int i = 1; i < characters.Count; i++)
        {
            for (int j = characters.Count; j > i; j--)
            {
                int evilsFound = 0;
                for (int k = i; k <= j; k++)
                {
                    if (evils.Contains(k))
                    {
                        evilsFound++;
                    }
                }
                if (evilsFound != 2)
                {
                    possibleStart.Add(i);
                    possibleEnd.Add(j);
                }
            }
        }
        int chosen = UnityEngine.Random.RandomRangeInt(0, possibleStart.Count);
        int chosenStart = possibleStart[chosen];
        int chosenEnd = possibleEnd[chosen];
        string info = string.Format("There are 2 evils from #{0} to #{1}", chosenStart, chosenEnd);
        Il2CppSystem.Collections.Generic.List<Character> hint = new();
        for (int i = chosenStart; i <= chosenEnd; i++)
        {
            foreach (Character character in Gameplay.CurrentCharacters)
            {
                if (character.id == i)
                    hint.Add(character);
            }
        }
        ActedInfo actedInfo = new ActedInfo(info, hint);
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
    public Director() : base(ClassInjector.DerivedConstructorPointer<Director>())
    {
        ClassInjector.DerivedConstructorBody((Il2CppObjectBase)this);
    }

    public Director(System.IntPtr ptr) : base(ptr)
    {

    }
}