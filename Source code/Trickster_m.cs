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
public class Trickster_m : Role
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
        if (charRef.dataRef.characterId != "Trickster_m_scm")
        {
            return new ActedInfo("Something is Digsuising as a Minion Trickster! This should never happen!");
        }
        Il2CppSystem.Collections.Generic.List<Character> chars = Gameplay.CurrentCharacters;
        Il2CppSystem.Collections.Generic.List<Character> characters = new();
        foreach (Character c in chars)
        {
            if (c.GetRegisterAs().type == ECharacterType.Minion)
            {
                characters.Add(c);
            }
        }
        if (characters.Count > 1)
            characters.Remove(charRef);
        if (characters.Count == 0)
        {
            return new ActedInfo(string.Format("#{0} is my Type", charRef.id));
        }
        Character chosen = characters[UnityEngine.Random.RandomRangeInt(0, characters.Count)];
        string info = string.Format("#{0} is my Type", chosen.id);
        ActedInfo actedInfo = new ActedInfo(info);
        return actedInfo;
    }

    public override ActedInfo GetBluffInfo(Character charRef)
    {
        string info = "I feel sick.";
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
    public override CharacterData GetRegisterAsRole(Character charRef)
    {
        Trickster_m_register register = new Trickster_m_register();
        return register.GetRegisterAsRole(charRef);
    }
    public Trickster_m() : base(ClassInjector.DerivedConstructorPointer<Trickster_m>())
    {
        ClassInjector.DerivedConstructorBody((Il2CppObjectBase)this);
    }

    public Trickster_m(System.IntPtr ptr) : base(ptr)
    {

    }
}
[RegisterTypeInIl2Cpp]
public class Trickster_m_register : Role
{
    public override string Description
    {
        get
        {
            return "";
        }
    }
    public Trickster_m_register() : base(ClassInjector.DerivedConstructorPointer<Trickster_m_register>())
    {
        ClassInjector.DerivedConstructorBody((Il2CppObjectBase)this);
    }

    public Trickster_m_register(System.IntPtr ptr) : base(ptr)
    {

    }
}