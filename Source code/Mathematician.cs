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
public class Mathematician : Role
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
        System.Collections.Generic.List<int> possibleSums = new System.Collections.Generic.List<int>();
        foreach (int evil in evils)
        {
            foreach (int evil2 in evils)
            {
                if (evil2 > evil)
                {
                    possibleSums.Add(evil + evil2);
                }
            }
        }

        int chosen = possibleSums[UnityEngine.Random.RandomRangeInt(0, possibleSums.Count)];
        string info = string.Format("2 Evil positions add to {0}", chosen);
        ActedInfo actedInfo = new ActedInfo(info);
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
        System.Collections.Generic.List<int> possibleSums = new System.Collections.Generic.List<int>();
        foreach (int evil in evils)
        {
            foreach (int evil2 in evils)
            {
                if (evil2 > evil && !possibleSums.Contains(evil + evil2))
                {
                    possibleSums.Add(evil + evil2);
                }
            }
        }
        System.Collections.Generic.List<int> impossibleSums = new System.Collections.Generic.List<int>();
        for (int i = 3; i < 2*characters.Count; i++)
        {
            if (!possibleSums.Contains(i))
            {
                impossibleSums.Add(i);
            }
        }
        if (impossibleSums.Count == 0)
        {
            return new ActedInfo("2 Evil positions add to π");
        }

        int chosen = impossibleSums[UnityEngine.Random.RandomRangeInt(0, impossibleSums.Count)];
        string info = string.Format("2 Evil positions add to {0}", chosen);
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
    public Mathematician() : base(ClassInjector.DerivedConstructorPointer<Mathematician>())
    {
        ClassInjector.DerivedConstructorBody((Il2CppObjectBase)this);
    }

    public Mathematician(System.IntPtr ptr) : base(ptr)
    {

    }
}