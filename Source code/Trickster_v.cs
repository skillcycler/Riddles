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
        if (charRef.dataRef.characterId != "Trickster_v_scm")
        {
            return new ActedInfo("Something is Digsuising as a Villager Trickster! This should never happen!");
        }
        // safeguard against getting moved
        bool converted = false;
        foreach (Character ch in Gameplay.CurrentCharacters)
        {
            if (ch.dataRef.characterId == "Trickster_m_scm" || ch.dataRef.characterId == "Trickster_o_scm")
                converted = true;
        }
        if (!converted)
            return new ActedInfo("This village has too few Villagers! I can't perform my tricks here!");
        Il2CppSystem.Collections.Generic.List<Character> characters = Gameplay.CurrentCharacters;
        characters = Characters.Instance.FilterCharacterType(characters, ECharacterType.Villager);
        Il2CppSystem.Collections.Generic.List<Character> characters2 = new();
        foreach (Character ch in characters)
        {
            if (ch.dataRef.characterId != "Trickster_o_scm" && ch.dataRef.characterId != "Trickster_m_scm")
            {
                characters2.Add(ch);
            }
        }
        if (characters2.Count > 1)
            characters2.Remove(charRef);
        Character chosen = characters2[UnityEngine.Random.RandomRangeInt(0, characters2.Count)];
        string info = string.Format("#{0} is my Type", chosen.id);
        Il2CppSystem.Collections.Generic.List<Character> hint = new();
        hint.Add(chosen);
        ActedInfo actedInfo = new ActedInfo(info, hint);
        return actedInfo;
    }

    public override ActedInfo GetBluffInfo(Character charRef)
    {
        return new ActedInfo("I feel sick.");
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