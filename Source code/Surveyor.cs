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
public class Surveyor : Role
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
        int outcasts = 0;
        int minions = 0;
        foreach (Character character in characters)
        {
            if (character.GetCharacterType() == ECharacterType.Outcast)
            {
                outcasts++;
            }
            if (character.GetCharacterType() == ECharacterType.Minion)
            {
                minions++;
            }
        }
        string info = string.Format("There {2} {0} Outcast{3} and {1} Minion{4}", outcasts, minions, outcasts == 1 ? "is" : "are", outcasts == 1 ? "" : "s", minions == 1 ? "" : "s");
        ActedInfo actedInfo = new ActedInfo(info);
        return actedInfo;
    }

    public override ActedInfo GetBluffInfo(Character charRef)
    {
        Il2CppSystem.Collections.Generic.List<Character> characters = Gameplay.CurrentCharacters;
        int outcasts = 0;
        int minions = 0;
        foreach (Character character in characters)
        {
            if (character.GetCharacterType() == ECharacterType.Outcast)
            {
                outcasts++;
            }
            if (character.GetCharacterType() == ECharacterType.Minion)
            {
                minions++;
            }
        }
        outcasts += UnityEngine.Random.RandomRangeInt(0, 2) * 2 - 1;
        if (outcasts == -1) outcasts = 1;
        minions += UnityEngine.Random.RandomRangeInt(0, 2) * 2 - 1;
        if (minions == -1) minions = 1;
        string info = string.Format("There {2} {0} Outcast{3} and {1} Minion{4}", outcasts, minions, outcasts == 1 ? "is" : "are", outcasts == 1 ? "" : "s", minions == 1 ? "" : "s");
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
    public Surveyor() : base(ClassInjector.DerivedConstructorPointer<Surveyor>())
    {
        ClassInjector.DerivedConstructorBody((Il2CppObjectBase)this);
    }

    public Surveyor(System.IntPtr ptr) : base(ptr)
    {

    }
}