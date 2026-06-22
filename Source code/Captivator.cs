using System;
using System.ComponentModel.Design;
using HarmonyLib;
using Il2Cpp;
using Il2CppFIMSpace.Basics;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.Injection;
using Il2CppInterop.Runtime.InteropTypes;
using Il2CppSystem;
using Il2CppSystem.Collections.Generic;
using MelonLoader;
using RiddlerMod;
using UnityEngine;
using static Il2CppSystem.Globalization.HebrewNumber;
using static MelonLoader.MelonLogger;

namespace RiddlerMod;

[RegisterTypeInIl2Cpp]
public class Captivator : Role
{
    public CharacterData[] allDatas = Il2CppSystem.Array.Empty<CharacterData>();
    public override string Description
    {
        get
        {
            return "";
        }
    }
    public override ActedInfo GetInfo(Character charRef)
    {
        return new ActedInfo("", null);
    }
    public override ActedInfo GetBluffInfo(Character charRef)
    {
        return new ActedInfo("", null);
    }
    public override void Act(ETriggerPhase trigger, Character charRef)
    {

    }
    public override CharacterData GetBluffIfAble(Character charRef)
    {
        Gameplay gameplay = Gameplay.Instance;
        Characters instance = Characters.Instance;
        Il2CppSystem.Collections.Generic.List<CharacterData> chars = gameplay.GetAscensionAllStartingCharacters();
        Il2CppSystem.Collections.Generic.List<CharacterData> villagers = instance.FilterRealCharacterType(chars, ECharacterType.Villager);

        Il2CppSystem.Collections.Generic.List<CharacterData> listV = new Il2CppSystem.Collections.Generic.List<CharacterData>();
        Il2CppSystem.Collections.Generic.List<string> whitelistCharacterIDs = new Il2CppSystem.Collections.Generic.List<string>();

        //whitelistCharacterIDs.Add("Oracle_07039445");
        int nonVillagers = 0;
        foreach (Character c in Gameplay.CurrentCharacters)
        {
            if (c.GetRegisterAs().type != ECharacterType.Villager)
                nonVillagers++;
        }
        if (nonVillagers >= 2)
            whitelistCharacterIDs.Add("Bishop_58855542");
        int evils = 0;
        foreach (Character c in Gameplay.CurrentCharacters)
        {
            if (c.GetRegisterAlignment() == EAlignment.Evil)
                evils++;
        }
        if (evils >= 3)
        {
            whitelistCharacterIDs.Add("Empress_13782227");
        }
        if (evils >= 2)
        {
            whitelistCharacterIDs.Add("Chiromancer_WING");
        }
        /*foreach (Character c in Characters.Instance.GetAdjacentCharacters(charRef))
        {
            if (c.alignment == EAlignment.Good)
            {
                whitelistCharacterIDs.Add("Lawyer_scm");
                break;
            }
        }*/
        int bluffs = 0;
        foreach (Character c in Gameplay.CurrentCharacters)
        {
            if (c.bluff)
            {
                bluffs++;
            }
        }
        if (bluffs >= 2)
        {
            whitelistCharacterIDs.Add("Prince_WING");
        }
        whitelistCharacterIDs.Add("Surveyor_scm");
        int corrupted = 0;
        foreach (Character c in Gameplay.CurrentCharacters)
        {
            if (c.statuses.Contains(ECharacterStatus.Corrupted)) corrupted++;
        }
        if (corrupted >= 2)
            whitelistCharacterIDs.Add("Sentinel_WING");
        for (int i = 0; i < villagers.Count; i++)
        {
            if (whitelistCharacterIDs.Contains(villagers[i].characterId))
                listV.Add(villagers[i]);
        }
        CharacterData bluff = listV[UnityEngine.Random.RandomRangeInt(0, listV.Count)];
        gameplay.AddScriptCharacterIfAble(ECharacterType.Villager, bluff);
        charRef.statuses.AddStatus(ECharacterStatus.HealthyBluff, charRef);
        charRef.statuses.AddStatus(ECharacterStatus.BrokenAbility, charRef);
        return bluff;
    }
    public override int GetDamageToYou()
    {
        return 2;
    }
    public Captivator() : base(ClassInjector.DerivedConstructorPointer<Captivator>())
    {
        ClassInjector.DerivedConstructorBody((Il2CppObjectBase)this);
    }
    public Captivator(System.IntPtr ptr) : base(ptr) { }
}
