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
public class Trickster_v : Role
{
    public bool converted = false;
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
        if (charRef.dataRef.characterId != "Trickster_v_scm" && charRef.dataRef.characterId != "Trickster_o_scm" && charRef.dataRef.characterId != "Trickster_m_scm")
        {
            if (!charRef.statuses.Contains(Accused.accused))
                return new ActedInfo("I feel sick.");
        }
        if (!converted)
            return new ActedInfo("This village has too few Villagers! I can't perform my tricks here!");
        Il2CppSystem.Collections.Generic.List<Character> characters = Gameplay.CurrentCharacters;
        characters = Characters.Instance.FilterCharacterType(characters, charRef.GetCharacterType());
        if (characters.Count > 1)
            characters.Remove(charRef);
        Character chosen = characters[UnityEngine.Random.RandomRangeInt(0, characters.Count)];
        string info = string.Format("#{0} is my Type", chosen.id);
        Il2CppSystem.Collections.Generic.List<Character> hint = new();
        hint.Add(chosen);
        ActedInfo actedInfo = new ActedInfo(info, hint);
        return actedInfo;
    }

    public override ActedInfo GetBluffInfo(Character charRef)
    {
        if (charRef.dataRef.characterId != "Trickster_v_scm" && charRef.dataRef.characterId != "Trickster_o_scm" && charRef.dataRef.characterId != "Trickster_m_scm")
        {
            if (!charRef.statuses.Contains(Accused.accused))
                return new ActedInfo("I feel sick.");
        }
        if (charRef.statuses.Contains(ECharacterStatus.Corrupted))
        {
            return new ActedInfo("I feel sick.");
        }
        if (charRef.GetCharacterType() == ECharacterType.Outcast || charRef.GetCharacterType() == ECharacterType.Minion)
        {
            return GetInfo(charRef);
        }
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
        Il2CppSystem.Collections.Generic.List<Character> hint = new();
        hint.Add(chosen);
        ActedInfo actedInfo = new ActedInfo(info, hint);
        return actedInfo;
    }

    public override void Act(ETriggerPhase trigger, Character charRef)
    {
        if (trigger == ETriggerPhase.Day)
        {
            onActed.Invoke(GetInfo(charRef));
        }
        if (charRef.statuses.Contains(ECharacterStatus.BrokenAbility))
            return;
        if (trigger == ETriggerPhase.Start)
        {
            if (allDatas.Length == 0)
            {
                var loadedCharList = Resources.FindObjectsOfTypeAll(Il2CppType.Of<CharacterData>());
                if (loadedCharList != null)
                {
                    allDatas = new CharacterData[loadedCharList.Length];
                    for (int j = 0; j < loadedCharList.Length; j++)
                    {
                        allDatas[j] = loadedCharList[j]!.Cast<CharacterData>();
                    }
                }
            }
            Il2CppSystem.Collections.Generic.List<Character> converts = Gameplay.CurrentCharacters;
            converts = Characters.Instance.FilterRealCharacterType(converts, ECharacterType.Villager);
            converts.Remove(charRef);
            if (converts.Count > 1) {
                converted = true;
                int c1 = UnityEngine.Random.RandomRangeInt(0, converts.Count);
                int c2 = UnityEngine.Random.RandomRangeInt(0, converts.Count);
                while (c1 == c2)
                {
                    c2 = UnityEngine.Random.RandomRangeInt(0, converts.Count);
                }
                for (int j = 0; j < allDatas.Length; j++)
                {
                    if (allDatas[j].characterId == "Trickster_o_scm")
                    {
                        converts[c1].Init(allDatas[j]);
                    }
                    if (allDatas[j].characterId == "Trickster_m_scm")
                    {
                        converts[c2].Init(allDatas[j]);
                    }
                }
            }
        }
    }
    public override void BluffAct(ETriggerPhase trigger, Character charRef)
    {
        if (trigger == ETriggerPhase.Day)
        {
            onActed.Invoke(GetInfo(charRef));
        }
        if (charRef.statuses.Contains(ECharacterStatus.BrokenAbility))
            return;
        if (trigger == ETriggerPhase.Start && charRef.GetCharacterType() == ECharacterType.Villager)
        {
            if (allDatas.Length == 0)
            {
                var loadedCharList = Resources.FindObjectsOfTypeAll(Il2CppType.Of<CharacterData>());
                if (loadedCharList != null)
                {
                    allDatas = new CharacterData[loadedCharList.Length];
                    for (int j = 0; j < loadedCharList.Length; j++)
                    {
                        allDatas[j] = loadedCharList[j]!.Cast<CharacterData>();
                    }
                }
            }
            Il2CppSystem.Collections.Generic.List<Character> converts = Gameplay.CurrentCharacters;
            converts = Characters.Instance.FilterRealCharacterType(converts, ECharacterType.Villager);
            converts.Remove(charRef);
            if (converts.Count > 1)
            {
                converted = true;
                int c1 = UnityEngine.Random.RandomRangeInt(0, converts.Count);
                int c2 = UnityEngine.Random.RandomRangeInt(0, converts.Count);
                while (c1 == c2)
                {
                    c2 = UnityEngine.Random.RandomRangeInt(0, converts.Count);
                }
                for (int j = 0; j < allDatas.Length; j++)
                {
                    if (allDatas[j].characterId == "Trickster_o_scm")
                    {
                        converts[c1].Init(allDatas[j]);
                    }
                    if (allDatas[j].characterId == "Trickster_m_scm")
                    {
                        converts[c2].Init(allDatas[j]);
                    }
                }
            }
        }
    }
    public Trickster_v() : base(ClassInjector.DerivedConstructorPointer<Trickster_v>())
    {
        ClassInjector.DerivedConstructorBody((Il2CppObjectBase)this);
    }

    public Trickster_v(System.IntPtr ptr) : base(ptr)
    {

    }
}