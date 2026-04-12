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
public class Weaver : Role
{
    public override string Description
    {
        get
        {
            return "";
        }
    }
    public int GetPairCount()
    {
        Il2CppSystem.Collections.Generic.List<Character> myList = new();
        foreach (Character c in Gameplay.CurrentCharacters)
        {
            myList.Add(c);
        }
        myList.Add(Gameplay.CurrentCharacters[0]);

        int pairCount = 0;
        bool villagerPrev = false;
        foreach (Character ch in myList)
        {
            if (ch.GetCharacterType() == ECharacterType.Villager)
            {
                if (villagerPrev)
                    pairCount++;
                villagerPrev = true;
            }
            else
                villagerPrev = false;
        }

        return pairCount;
    }
    public string MakeInfo(int pairCount)
    {
        string info = "";
        if (pairCount == 0)
            info = "Villagers are not adjacent to eachother";
        else if (pairCount == 1)
            info = $"There is only 1 pair of Villagers";
        else
            info = $"There are {pairCount} pairs of Villagers";
        return info;
    }
    public override ActedInfo GetInfo(Character charRef)
    {
        int pairCount = GetPairCount();
        string info = MakeInfo(pairCount);
        ActedInfo newInfo = new ActedInfo(info);
        return newInfo;
    }

    public override ActedInfo GetBluffInfo(Character charRef)
    {
        int pairCount = GetPairCount();

        int max = 0;
        foreach (Character c in Gameplay.CurrentCharacters)
        {
            if ((c.GetCharacterType() == ECharacterType.Villager))
                max++;
        }
        if (max < 2)
            max = 2;
        int randomPairCount = Calculator.RemoveNumberAndGetRandomNumberFromList(pairCount, 0, max-1);

        string info = MakeInfo(randomPairCount);
        ActedInfo newInfo = new ActedInfo(info);
        return newInfo;
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
    public Weaver() : base(ClassInjector.DerivedConstructorPointer<Weaver>())
    {
        ClassInjector.DerivedConstructorBody((Il2CppObjectBase)this);
    }

    public Weaver(System.IntPtr ptr) : base(ptr)
    {

    }
}