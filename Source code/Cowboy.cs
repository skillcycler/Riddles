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
        }
        string info = "There are no Evil Villagers or Outcasts";
        if (evilVillagers.Count > 0) {
            info = string.Format("#{0} is an Evil Villager", evilVillagers[UnityEngine.Random.RandomRangeInt(0, evilVillagers.Count)]);
        } else if (evilOutcasts.Count > 0)
        {
            info = string.Format("#{0} is an Evil Outcast", evilOutcasts[UnityEngine.Random.RandomRangeInt(0, evilOutcasts.Count)]);
        }
        else if (evilNeutrals.Count > 0)
        {
            info = string.Format("#{0} is an Evil Neutral", evilNeutrals[UnityEngine.Random.RandomRangeInt(0, evilNeutrals.Count)]);
        }
        ActedInfo actedInfo = new ActedInfo(info);
        return actedInfo;
    }

    public override ActedInfo GetBluffInfo(Character charRef)
    {
        Il2CppSystem.Collections.Generic.List<Character> characters = Gameplay.CurrentCharacters;
        List<int> notevilVillagersOutcasts = new();
        foreach (Character character in characters)
        {
            if (!((character.dataRef.type == ECharacterType.Villager || character.dataRef.type == ECharacterType.Outcast || character.dataRef.type == (ECharacterType)150) && (character.alignment == EAlignment.Evil || character.GetRegisterAlignment() == EAlignment.Evil)))
            {
                notevilVillagersOutcasts.Add(character.id);
            }
        }
        notevilVillagersOutcasts.Remove(charRef.id);
        string info = string.Format("#{0} is an Evil {1}", notevilVillagersOutcasts[UnityEngine.Random.RandomRangeInt(0, notevilVillagersOutcasts.Count)], UnityEngine.Random.RandomRangeInt(0, 2) == 1 ? "Outcast" : "Villager");
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