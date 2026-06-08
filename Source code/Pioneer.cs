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
public class Pioneer : Role
{
    public override string Description
    {
        get
        {
            return "";
        }
    }
    public int GetDistanceBetween(int c1, int c2, int cards)
    {
        return Math.Min(Math.Abs(c1 - c2), Math.Min(Math.Abs(c1 + cards - c2), Math.Abs(c2 + cards - c1)));
    }
    public override ActedInfo GetInfo(Character charRef)
    {
        Il2CppSystem.Collections.Generic.List<Character> characters = Gameplay.CurrentCharacters;
        Il2CppSystem.Collections.Generic.List<Character> evils = new Il2CppSystem.Collections.Generic.List<Character>();
        foreach (Character c in characters)
        {
            if (c.alignment == EAlignment.Evil)
            {
                evils.Add(c);
            }
        }
        Character chosen = evils[UnityEngine.Random.RandomRangeInt(0, evils.Count)];
        int closestid = 1000;
        bool found = false;
        for (int i = 1; i <= Gameplay.CurrentCharacters.Count / 2 && !found; i++) {
            Il2CppSystem.Collections.Generic.List<Character> check = Characters.Instance.GetCharactersAtRange(i, charRef); 
            foreach (Character c in check)
            {
                if (c.alignment == EAlignment.Evil)
                {
                    closestid = c.id;
                    found = true;
                    break;
                }
            }
        }
        if (closestid == 1000) {
            return new ActedInfo("There are no Evils");
        }
        int distance = GetDistanceBetween(closestid, chosen.id, Gameplay.CurrentCharacters.Count);

        string info = string.Format("{0} is {1} card{2} away from my closest Evil", chosen.dataRef.name, distance, distance==1?"":"s");

        if (distance == 0)
        {
            info = string.Format("{0} is my closest Evil", chosen.dataRef.name);
        }
        ActedInfo actedInfo = new ActedInfo(info);
        return actedInfo;
    }

    public override ActedInfo GetBluffInfo(Character charRef)
    {
        Il2CppSystem.Collections.Generic.List<Character> characters = Gameplay.CurrentCharacters;
        Il2CppSystem.Collections.Generic.List<Character> evils = new Il2CppSystem.Collections.Generic.List<Character>();
        foreach (Character c in characters)
        {
            if (c.alignment == EAlignment.Evil)
            {
                evils.Add(c);
            }
        }
        Character chosen = evils[UnityEngine.Random.RandomRangeInt(0, evils.Count)];
        int closestid = 1000;
        bool found = false;
        for (int i = 1; i <= Gameplay.CurrentCharacters.Count / 2 && !found; i++)
        {
            Il2CppSystem.Collections.Generic.List<Character> check = Characters.Instance.GetCharactersAtRange(i, charRef);
            foreach (Character c in check)
            {
                if (c.alignment == EAlignment.Evil)
                {
                    closestid = c.id;
                    found = true;
                    break;
                }
            }
        }
        if (closestid == 1000)
        {
            return new ActedInfo("There are no Evils");
        }
        int distance = GetDistanceBetween(closestid, chosen.id, Gameplay.CurrentCharacters.Count);
        int fakeDistance = Calculator.RemoveNumberAndGetRandomNumberFromList(distance, 1, (int)(Gameplay.CurrentCharacters.Count/2)+1);
        string info = string.Format("{0} is {1} card{2} away from my closest Evil", chosen.dataRef.name, fakeDistance, fakeDistance == 1 ? "" : "s");

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
    public Pioneer() : base(ClassInjector.DerivedConstructorPointer<Pioneer>())
    {
        ClassInjector.DerivedConstructorBody((Il2CppObjectBase)this);
    }

    public Pioneer(System.IntPtr ptr) : base(ptr)
    {

    }
}