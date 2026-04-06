using System;
using System.ComponentModel.Design;
using HarmonyLib;
using Il2Cpp;
using Il2CppInterop.Runtime.Injection;
using Il2CppInterop.Runtime.InteropTypes;
using Il2CppSystem;
using MelonLoader;
using UnityEngine;
using static MelonLoader.MelonLogger;

public class Hypnotist : Minion
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

        whitelistCharacterIDs.Add("Confessor_18741708");
        whitelistCharacterIDs.Add("Baker_22847064");
        whitelistCharacterIDs.Add("Alchemist_94446803");
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

    public Hypnotist() : base(ClassInjector.DerivedConstructorPointer<Hypnotist>())
    {
        ClassInjector.DerivedConstructorBody((Il2CppObjectBase)this);
    }
    public Hypnotist(System.IntPtr ptr) : base(ptr) { }

}