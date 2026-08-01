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
using static MelonLoader.MelonLogger;

namespace RiddlerMod;

[RegisterTypeInIl2Cpp]
public class Hypnotist : Spy
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
        Il2CppSystem.Collections.Generic.List<CharacterData> villagers = gameplay.GetAscensionAllStartingCharacters();

        Il2CppSystem.Collections.Generic.List<CharacterData> listV = new Il2CppSystem.Collections.Generic.List<CharacterData>();
        Il2CppSystem.Collections.Generic.List<string> whitelistCharacterIDs = new Il2CppSystem.Collections.Generic.List<string>();

        whitelistCharacterIDs.Add("Confessor_18741708");
        whitelistCharacterIDs.Add("Baker_22847064");
        int corruptions = 0;
        foreach (Character ch in Gameplay.CurrentCharacters)
        {
            if (ch.statuses.Contains(ECharacterStatus.Corrupted)) corruptions++;
        }
        if (corruptions >= 3)
            whitelistCharacterIDs.Add("Alchemist_94446803");
        Il2CppSystem.Collections.Generic.List<CharacterData> inDeckOutcasts = gameplay.GetScriptCharactersOfType(ECharacterType.Outcast);
        foreach (CharacterData outcast in inDeckOutcasts)
        {
            if (outcast.usuallyDisguised)
            {
                whitelistCharacterIDs.Add("Lookout_41018246");
                break;
            }
        }
        whitelistCharacterIDs.Add("Witness_25155076");
        Il2CppSystem.Collections.Generic.List<Character> chs = Gameplay.CurrentCharacters;
        int evils = 0;
        foreach (Character c in chs)
        {
            if (c.GetRegisterAlignment() == EAlignment.Evil)
                evils++;
        }
        /*
        if (evils >= 4)
        {
            whitelistCharacterIDs.Add("Knitter_32352172");
        }*/

        if (Gameplay.CurrentCharacters.Count >= 6 + evils && evils >= 2)
        {
            whitelistCharacterIDs.Add("Scout_88081716");
        }

        whitelistCharacterIDs.Add("Riddler_scm");
        whitelistCharacterIDs.Add("Sentinel_WING");
        foreach (Character c in Gameplay.CurrentCharacters)
        {
            if (c.dataRef.characterId == "Swarm_Good_WING")
            {
                whitelistCharacterIDs.Add("Swarm_Good_WING");
                break;
            }
        }
        whitelistCharacterIDs.Add("Underling_V_WING"); // will always say "I am Good" if disguised as this
        whitelistCharacterIDs.Add("Monarch_POW");
        whitelistCharacterIDs.Add("WING_Dupery_Priest");
        for (int i = 0; i < villagers.Count; i++)
        {
            if (whitelistCharacterIDs.Contains(villagers[i].characterId))
                listV.Add(villagers[i]);
        }
        CharacterData bluff = listV[UnityEngine.Random.RandomRangeInt(0, listV.Count)];
        if (bluff.characterId == "Monarch_POW")
        {
            for (int i = 0; i < villagers.Count; i++)
            {
                if (villagers[i].characterId == "Executive_POW")
                    gameplay.AddScriptCharacterIfAble(ECharacterType.Villager, villagers[i]);
            }
        }
        else gameplay.AddScriptCharacterIfAble(bluff.type, bluff);
        charRef.statuses.AddStatus(ECharacterStatus.HealthyBluff, charRef);
        charRef.statuses.AddStatus(ECharacterStatus.BrokenAbility, charRef);
        return bluff;
    }
    public Hypnotist() : base(ClassInjector.DerivedConstructorPointer<Hypnotist>())
    {
        ClassInjector.DerivedConstructorBody((Il2CppObjectBase)this);
    }
    public Hypnotist(System.IntPtr ptr) : base(ptr) { }
}
