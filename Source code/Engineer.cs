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
public class Engineer : Role
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
            if (character.GetRegisterAlignment() == EAlignment.Evil)
            {
                evils.Add(character.id);
            }
        }
        int topEvils = 0;
        int bottomEvils = 0;
        foreach (int evil in evils)
        {
            if (evil <= Gameplay.CurrentCharacters.Count * 0.25f || evil >= Gameplay.CurrentCharacters.Count * 0.75f)
            {
                topEvils++;
            }
            if (evil >= Gameplay.CurrentCharacters.Count * 0.25f && evil <= Gameplay.CurrentCharacters.Count * 0.75f)
            {
                bottomEvils++;
            }
        }

        string info = makeInfo(topEvils, bottomEvils, false);
        ActedInfo actedInfo = new ActedInfo(info);
        return actedInfo;
    }

    public override ActedInfo GetBluffInfo(Character charRef)
    {
        Il2CppSystem.Collections.Generic.List<Character> characters = Gameplay.CurrentCharacters;
        System.Collections.Generic.List<int> evils = new System.Collections.Generic.List<int>();
        foreach (Character character in characters)
        {
            if (character.GetRegisterAlignment() == EAlignment.Evil)
            {
                evils.Add(character.id);
            }
        }
        int topEvils = 0;
        int bottomEvils = 0;
        foreach (int evil in evils)
        {
            if (evil <= Gameplay.CurrentCharacters.Count * 0.25f || evil >= Gameplay.CurrentCharacters.Count * 0.75f)
            {
                topEvils++;
            }
            if (evil >= Gameplay.CurrentCharacters.Count * 0.25f && evil <= Gameplay.CurrentCharacters.Count * 0.75f)
            {
                bottomEvils++;
            }
        }

        string info = makeInfo(topEvils, bottomEvils, true);
        ActedInfo actedInfo = new ActedInfo(info);
        return actedInfo;
    }
    public static string makeInfo (int top, int bottom, bool lying)
    {
        bool topMore = top > bottom;
        bool equal = top == bottom;
        bool bottomMore = top < bottom;
        if (!lying)
        {
            if (topMore) return "Top half is more Evil";
            if (equal) return "Both halves are equally Evil";
            return "Bottom half is more Evil";
        } else
        {
            if (topMore)
            {
                if (UnityEngine.Random.RandomRangeInt(0, 2) == 0) return "Both halves are equally Evil";
                return "Bottom half is more Evil";
            }
            if (equal)
            {
                if (UnityEngine.Random.RandomRangeInt(0, 2) == 0) return "Top half is more Evil";
                return "Bottom half is more Evil";
            }
            if (UnityEngine.Random.RandomRangeInt(0, 2) == 0) return "Top half is more Evil";
            return "Both halves are equally Evil";
        }
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
    public Engineer() : base(ClassInjector.DerivedConstructorPointer<Engineer>())
    {
        ClassInjector.DerivedConstructorBody((Il2CppObjectBase)this);
    }

    public Engineer(System.IntPtr ptr) : base(ptr)
    {

    }
}