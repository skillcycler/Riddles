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
public class Trickster_o : Role
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
        characters = Characters.Instance.FilterCharacterType(characters, charRef.GetCharacterType());
        if (characters.Count > 1)
            characters.Remove(charRef);
        Character chosen = characters[UnityEngine.Random.RandomRangeInt(0, characters.Count)];
        string info = string.Format("#{0} is my Type", chosen.id);
        ActedInfo actedInfo = new ActedInfo(info);
        return actedInfo;
    }

    public override ActedInfo GetBluffInfo(Character charRef)
    {
        Il2CppSystem.Collections.Generic.List<Character> characters = Gameplay.CurrentCharacters;
        Il2CppSystem.Collections.Generic.List<Character> wrongType = new Il2CppSystem.Collections.Generic.List<Character>();
        foreach (Character character in characters)
        {
            if (character.GetCharacterType() != charRef.GetCharacterType())
            {
                wrongType.Add(character);
            }
        }
        Character chosen = wrongType[UnityEngine.Random.RandomRangeInt(0, characters.Count)];
        string info = string.Format("#{0} is my Type", chosen.id);
        ActedInfo actedInfo = new ActedInfo(info);
        return actedInfo;
    }
    public override CharacterData GetBluffIfAble(Character charRef)
    {
        Gameplay gameplay = Gameplay.Instance;
        Characters instance = Characters.Instance;
        Il2CppSystem.Collections.Generic.List<CharacterData> chars = gameplay.GetAscensionAllStartingCharacters();
        Il2CppSystem.Collections.Generic.List<CharacterData> villagers = instance.FilterRealCharacterType(chars, ECharacterType.Villager);

        Il2CppSystem.Collections.Generic.List<CharacterData> listV = new Il2CppSystem.Collections.Generic.List<CharacterData>();
        Il2CppSystem.Collections.Generic.List<string> whitelistCharacterIDs = new Il2CppSystem.Collections.Generic.List<string>();

        whitelistCharacterIDs.Add("Trickster_v");
        for (int i = 0; i < villagers.Count; i++)
        {
            if (whitelistCharacterIDs.Contains(villagers[i].characterId))
                listV.Add(villagers[i]);
        }
        CharacterData bluff = listV[0];
        return bluff;
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
    public Trickster_o() : base(ClassInjector.DerivedConstructorPointer<Trickster_o>())
    {
        ClassInjector.DerivedConstructorBody((Il2CppObjectBase)this);
    }

    public Trickster_o(System.IntPtr ptr) : base(ptr)
    {

    }
}