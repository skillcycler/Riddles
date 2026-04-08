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
public class Scanner : Role
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
        int counter = 0;
        foreach (Character character in characters)
        {
            if (character.bluff && character.dataRef.type == ECharacterType.Outcast)
            {
                counter++;
            } else if (character.bluff)
            {
                if (character.bluff.type == ECharacterType.Outcast)
                    counter++;
            }
        }
        string info = string.Format("{0} Outcasts are Disguised or being used as a Disguise", counter);
        if (counter == 0)
            info = "NO Outcasts are Disguised or being used as a Disguise";
        if (counter == 1)
            info = "1 Outcast is Disguised or being used as a Disguise";
        ActedInfo actedInfo = new ActedInfo(info);
        return actedInfo;
    }

    public override ActedInfo GetBluffInfo(Character charRef)
    {
        Il2CppSystem.Collections.Generic.List<Character> characters = Gameplay.CurrentCharacters;
        int counter = 0;
        foreach (Character character in characters)
        {
            if (character.bluff && character.dataRef.type == ECharacterType.Outcast)
            {
                counter++;
            }
            else if (character.bluff)
            {
                if (character.bluff.type == ECharacterType.Outcast)
                    counter++;
            }
        }
        int counterLie = UnityEngine.Random.RandomRangeInt(0, Characters.Instance.FilterCharacterType(Gameplay.CurrentCharacters, ECharacterType.Outcast).Count);
        string info = string.Format("{0} Outcasts are Disguised or being used as a Disguise", counterLie);
        if (counterLie == 0)
            info = "NO Outcasts are Disguised or being used as a Disguise";
        if (counterLie == 1)
            info = "1 Outcast is Disguised or being used as a Disguise";
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
    public Scanner() : base(ClassInjector.DerivedConstructorPointer<Scanner>())
    {
        ClassInjector.DerivedConstructorBody((Il2CppObjectBase)this);
    }

    public Scanner(System.IntPtr ptr) : base(ptr)
    {

    }
}