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
public class Officer : Role
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
        int evils = 0;
        foreach (Character character in characters)
        {
            if (character.GetRegisterAlignment() == EAlignment.Evil)
            {
                evils++;
            }
        }
        string info = string.Format("{0} characters register as Evil", evils);
        if (evils == 1)
        {
            info = "1 character registers as Evil";
        }
        ActedInfo actedInfo = new ActedInfo(info);
        return actedInfo;
    }

    public override ActedInfo GetBluffInfo(Character charRef)
    {
        Il2CppSystem.Collections.Generic.List<Character> characters = Gameplay.CurrentCharacters;
        int evils = 0;
        foreach (Character character in characters)
        {
            if (character.GetRegisterAlignment() == EAlignment.Evil)
            {
                evils++;
            }
        }
        evils += UnityEngine.Random.RandomRangeInt(0, 2) * 2 - 1;
        string info = string.Format("{0} characters register as Evil", evils);

        if (evils == 1)
        {
            info = "1 character registers as Evil";
        }
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
    public Officer() : base(ClassInjector.DerivedConstructorPointer<Officer>())
    {
        ClassInjector.DerivedConstructorBody((Il2CppObjectBase)this);
    }

    public Officer(System.IntPtr ptr) : base(ptr)
    {

    }
}