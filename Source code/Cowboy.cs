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
public class Cowboy : Role
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
        List<int> evilVillagers = new();
        List<int> evilOutcasts = new();
        List<int> evilNeutrals = new(); // from Powerplay
        List<int> evilMinions = new();
        List<int> evilDemons = new();
        foreach (Character character in characters)
        {
            if (character.dataRef.type == ECharacterType.Villager && (character.alignment == EAlignment.Evil || character.GetRegisterAlignment() == EAlignment.Evil))
            {
                evilVillagers.Add(character.id);
            }
            if (character.dataRef.type == ECharacterType.Outcast && (character.alignment == EAlignment.Evil || character.GetRegisterAlignment() == EAlignment.Evil))
            {
                evilOutcasts.Add(character.id);
            }
            if (character.dataRef.type == (ECharacterType)150 && (character.alignment == EAlignment.Evil || character.GetRegisterAlignment() == EAlignment.Evil))
            {
                evilNeutrals.Add(character.id);
            }
            if (character.dataRef.type == ECharacterType.Minion && (character.alignment == EAlignment.Evil || character.GetRegisterAlignment() == EAlignment.Evil))
            {
                evilMinions.Add(character.id);
            }
            if (character.dataRef.type == ECharacterType.Demon && (character.alignment == EAlignment.Evil || character.GetRegisterAlignment() == EAlignment.Evil))
            {
                evilDemons.Add(character.id);
            }
        }
        string info = "There are NO Evil characters";
        if (evilVillagers.Count > 0) {
            info = string.Format("#{0} is the most sneaky", evilVillagers[UnityEngine.Random.RandomRangeInt(0, evilVillagers.Count)]);
        } else if (evilOutcasts.Count > 0)
        {
            info = string.Format("#{0} is the most sneaky", evilOutcasts[UnityEngine.Random.RandomRangeInt(0, evilOutcasts.Count)]);
        }
        else if (evilNeutrals.Count > 0)
        {
            info = string.Format("#{0} is the most sneaky", evilNeutrals[UnityEngine.Random.RandomRangeInt(0, evilNeutrals.Count)]);
        }
        else if (evilMinions.Count > 0)
        {
            info = string.Format("#{0} is the most sneaky", evilMinions[UnityEngine.Random.RandomRangeInt(0, evilMinions.Count)]);
        }
        else if (evilDemons.Count > 0)
        {
            info = string.Format("#{0} is the most sneaky", evilDemons[UnityEngine.Random.RandomRangeInt(0, evilDemons.Count)]);
        }
        ActedInfo actedInfo = new ActedInfo(info);
        return actedInfo;
    }

    public override ActedInfo GetBluffInfo(Character charRef)
    {
        Il2CppSystem.Collections.Generic.List<Character> characters = Gameplay.CurrentCharacters;
        List<int> notevilVillagers = new();
        List<int> notevilOutcasts = new();
        List<int> notevilNeutrals = new(); // from Powerplay
        List<int> notevilMinions = new();
        List<int> notevilDemons = new();
        foreach (Character character in characters)
        {
            if (!(character.dataRef.type == ECharacterType.Villager && (character.alignment == EAlignment.Evil || character.GetRegisterAlignment() == EAlignment.Evil)))
            {
                notevilVillagers.Add(character.id);
            }
            if (!(character.dataRef.type == ECharacterType.Outcast && (character.alignment == EAlignment.Evil || character.GetRegisterAlignment() == EAlignment.Evil)))
            {
                notevilOutcasts.Add(character.id);
            }
            if (!(character.dataRef.type == (ECharacterType)150 && (character.alignment == EAlignment.Evil || character.GetRegisterAlignment() == EAlignment.Evil)))
            {
                notevilNeutrals.Add(character.id);
            }
            if (!(character.dataRef.type == ECharacterType.Minion && (character.alignment == EAlignment.Evil || character.GetRegisterAlignment() == EAlignment.Evil)))
            {
                notevilMinions.Add(character.id);
            }
            if (!(character.dataRef.type == ECharacterType.Demon && (character.alignment == EAlignment.Evil || character.GetRegisterAlignment() == EAlignment.Evil)))
            {
                notevilDemons.Add(character.id);
            }
        }
        string info = "There are NO Evil characters";
        if (notevilVillagers.Count < Gameplay.CurrentCharacters.Count)
        {
            info = string.Format("#{0} is the most sneaky", notevilVillagers[UnityEngine.Random.RandomRangeInt(0, notevilVillagers.Count)]);
        }
        else if (notevilOutcasts.Count < Gameplay.CurrentCharacters.Count)
        {
            info = string.Format("#{0} is the most sneaky", notevilOutcasts[UnityEngine.Random.RandomRangeInt(0, notevilOutcasts.Count)]);
        }
        else if (notevilNeutrals.Count < Gameplay.CurrentCharacters.Count)
        {
            info = string.Format("#{0} is the most sneaky", notevilNeutrals[UnityEngine.Random.RandomRangeInt(0, notevilNeutrals.Count)]);
        }
        else if (notevilMinions.Count < Gameplay.CurrentCharacters.Count)
        {
            info = string.Format("#{0} is the most sneaky", notevilMinions[UnityEngine.Random.RandomRangeInt(0, notevilMinions.Count)]);
        }
        else if (notevilDemons.Count < Gameplay.CurrentCharacters.Count)
        {
            info = string.Format("#{0} is the most sneaky", notevilDemons[UnityEngine.Random.RandomRangeInt(0, notevilDemons.Count)]);
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
    public Cowboy() : base(ClassInjector.DerivedConstructorPointer<Cowboy>())
    {
        ClassInjector.DerivedConstructorBody((Il2CppObjectBase)this);
    }

    public Cowboy(System.IntPtr ptr) : base(ptr)
    {

    }
}