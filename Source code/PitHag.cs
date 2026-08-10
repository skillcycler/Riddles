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

namespace RiddlerMod;

[RegisterTypeInIl2Cpp]
public class PitHag : Minion
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
        if (trigger == ETriggerPhase.Start)
        {
            Il2CppSystem.Collections.Generic.List<CharacterData> notInPlayOutsiders = Gameplay.Instance.GetAscensionAllStartingCharacters();
            notInPlayOutsiders = Characters.Instance.FilterNotInDeckCharactersUnique(notInPlayOutsiders);
            notInPlayOutsiders = Characters.Instance.FilterRealCharacterType(notInPlayOutsiders, ECharacterType.Outcast);
            if (notInPlayOutsiders.Count >= 3)
            {
                for (int i = 0; i < 3; i++)
                {
                    CharacterData pickedOutsider = notInPlayOutsiders[UnityEngine.Random.Range(0, notInPlayOutsiders.Count - 1)];
                    Gameplay.Instance.AddScriptCharacter(ECharacterType.Outcast, pickedOutsider);
                    notInPlayOutsiders.Remove(pickedOutsider);
                }
            }
            else // we're out of outcasts, can't imagine when this will ever happen
            {
                foreach (CharacterData d in notInPlayOutsiders)
                {
                    Gameplay.Instance.AddScriptCharacter(ECharacterType.Outcast, d);
                }
            }
        }
        if (trigger == ETriggerPhase.Night)
        {
            Il2CppSystem.Collections.Generic.List<Character> villagers = MainMod.GetGameplayCurrentCharacters();
            villagers = Characters.Instance.FilterRealCharacterType(villagers, ECharacterType.Villager);
            villagers = Characters.Instance.FilterHiddenCharacters(villagers);
            if (villagers.Count == 0) return;
            Il2CppSystem.Collections.Generic.List<CharacterData> notInPlayOutsiders = Gameplay.Instance.GetScriptCharactersOfType(ECharacterType.Outcast);
            foreach (Character c in Gameplay.CurrentCharacters)
            {
                if (c.dataRef.type == ECharacterType.Outcast)
                {
                    notInPlayOutsiders.Remove(c.dataRef);
                }
            }
            if (notInPlayOutsiders.Count == 0) // somehow all the fake outcasts were used up already
            {
                notInPlayOutsiders = Gameplay.Instance.GetAscensionAllStartingCharacters();
                notInPlayOutsiders = Characters.Instance.FilterRealCharacterType(notInPlayOutsiders, ECharacterType.Outcast);
                foreach (Character c in Gameplay.CurrentCharacters)
                {
                    if (c.dataRef.type == ECharacterType.Outcast)
                    {
                        notInPlayOutsiders.Remove(c.dataRef);
                    }
                }
            }
            CharacterData newOutcast = notInPlayOutsiders[UnityEngine.Random.RandomRangeInt(0, villagers.Count)];
            Character v = villagers[UnityEngine.Random.RandomRangeInt(0, villagers.Count)];
            v.Init(newOutcast);
            v.Act(ETriggerPhase.Start);
            v.statuses.AddStatus(ECharacterStatus.AlteredCharacter, charRef);
            v.statuses.AddStatus(ECharacterStatus.MessedUpByEvil, charRef);
        }
    }

    public PitHag() : base(ClassInjector.DerivedConstructorPointer<PitHag>())
    {
        ClassInjector.DerivedConstructorBody((Il2CppObjectBase)this);
    }
    public PitHag(System.IntPtr ptr) : base(ptr) { }

}