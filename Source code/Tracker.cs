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
public class Tracker : Role
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
        List<int> outcasts = new();
        foreach (Character character in characters)
        {
            if (character.bluff)
            {
                if (character.bluff.type == ECharacterType.Outcast)
                {
                    outcasts.Add(character.id);
                }
            }
        }
        string info = "There are no characters disguised as Outcasts";
        if (outcasts.Count > 0)
        {
            info = string.Format("#{0} is disguised as an Outcast", outcasts[UnityEngine.Random.RandomRangeInt(0, outcasts.Count)]);
        }
        ActedInfo actedInfo = new ActedInfo(info);
        return actedInfo;
    }

    public override ActedInfo GetBluffInfo(Character charRef)
    {
        Il2CppSystem.Collections.Generic.List<Character> characters = Gameplay.CurrentCharacters;
        List<int> outcasts = new();
        foreach (Character character in characters)
        {
            if (!character.bluff)
            {
                if (character.dataRef.type == ECharacterType.Outcast)
                {
                    outcasts.Add(character.id);
                }
            }
        }
        string info = string.Format("#{0} is disguised as an Outcast", UnityEngine.Random.RandomRangeInt(1, Gameplay.CurrentCharacters.Count+1));
        if (outcasts.Count > 0)
        {
            info = string.Format("#{0} is disguised as an Outcast", outcasts[UnityEngine.Random.RandomRangeInt(0, outcasts.Count)]);
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
    public Tracker() : base(ClassInjector.DerivedConstructorPointer<Tracker>())
    {
        ClassInjector.DerivedConstructorBody((Il2CppObjectBase)this);
    }

    public Tracker(System.IntPtr ptr) : base(ptr)
    {

    }
}