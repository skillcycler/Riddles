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
public class Guide : Role
{
    public List<int> characters = new List<int>();
    public override string Description
    {
        get
        {
            return "";
        }
    }
    public string makeInfo()
    {
        if (characters.Count < 2) return "I have no information yet";
        string info = "";
        for (int i = 0; i < characters.Count - 1; i++)
        {
            info += $"#{characters[i]} and #{characters[i + 1]} have different Types\n";
        }
        return info;
    }
    public override ActedInfo GetInfo(Character charRef)
    {
        ActedInfo actedInfo = new ActedInfo(makeInfo());
        return actedInfo;
    }

    public override ActedInfo GetBluffInfo(Character charRef)
    {
        ActedInfo actedInfo = new ActedInfo(makeInfo());
        return actedInfo;
    }

    public override void Act(ETriggerPhase trigger, Character charRef)
    {
        if (trigger == ETriggerPhase.Start)
        {
            characters.Add(UnityEngine.Random.RandomRangeInt(1, Gameplay.CurrentCharacters.Count + 1));
        }
        if (trigger == ETriggerPhase.Night)
        {
            if (charRef.state == ECharacterState.Dead) return;
            Il2CppSystem.Collections.Generic.List<Character> ch = Gameplay.CurrentCharacters;
            if (characters.Count == 0) characters.Add(UnityEngine.Random.RandomRangeInt(1, Gameplay.CurrentCharacters.Count + 1));
            int last = characters.Last();
            Il2CppSystem.Collections.Generic.List<Character> valid = new();
            ECharacterType lastType = ECharacterType.None;
            foreach (Character c in ch)
            {
                if (c.id == last) lastType = c.GetCharacterType();
            }
            foreach (Character c in ch)
            {
                if (c.GetCharacterType() != lastType)
                {
                    valid.Add(c);
                }
            }
            characters.Add(valid[UnityEngine.Random.RandomRangeInt(0, valid.Count)].id);
            if (charRef.revealed)
            {
                onActed.Invoke(GetInfo(charRef));
            }
        }
        if (trigger == ETriggerPhase.Day)
        {
            charRef.revealed = true;
            onActed.Invoke(GetInfo(charRef));
        }
    }
    public override void BluffAct(ETriggerPhase trigger, Character charRef)
    {
        if (trigger == ETriggerPhase.Night)
        {
            if (charRef.state == ECharacterState.Dead) return;
            Il2CppSystem.Collections.Generic.List<Character> ch = Gameplay.CurrentCharacters;
            if (characters == null) characters = new List<int>();
            if (characters.Count == 0) characters.Add(UnityEngine.Random.RandomRangeInt(1, Gameplay.CurrentCharacters.Count + 1));
            int last = characters.Last();
            Il2CppSystem.Collections.Generic.List<Character> valid = new();
            ECharacterType lastType = ECharacterType.None;
            foreach (Character c in ch)
            {
                if (c.id == last) lastType = c.GetCharacterType();
            }
            foreach (Character c in ch)
            {
                if (c.GetCharacterType() != lastType)
                {
                    valid.Add(c);
                }
            }
            Character chr = valid[UnityEngine.Random.RandomRangeInt(0, valid.Count)];
            if (valid.Count > 1)
            {
                while (chr.id == last) chr = valid[UnityEngine.Random.RandomRangeInt(0, valid.Count)];
            } else
            {
                while (chr.id == last) chr = Gameplay.CurrentCharacters[UnityEngine.Random.RandomRangeInt(0, Gameplay.CurrentCharacters.Count)];
            }
                characters.Add(chr.id);

            if (charRef.revealed)
            {
                var info = GetInfo(charRef);
                onActed?.Invoke(info);
            }
        }
        if (trigger == ETriggerPhase.Day)
        {
            if (characters == null) characters = new List<int>();
            if (characters.Count == 0)
            { // Completely random info if lying
                int add = Gameplay.Instance.currentDay;
                int previous = UnityEngine.Random.RandomRangeInt(1, Gameplay.CurrentCharacters.Count + 1);
                for (int i = 0; i < add; i++)
                {
                    int next = UnityEngine.Random.RandomRangeInt(1, Gameplay.CurrentCharacters.Count + 1);
                    while (next == previous) next = UnityEngine.Random.RandomRangeInt(1, Gameplay.CurrentCharacters.Count + 1);
                    characters.Add(next);
                    previous = next;
                }
            }
            charRef.revealed = true;
            onActed?.Invoke(GetInfo(charRef));
        }
    }
    public Guide() : base(ClassInjector.DerivedConstructorPointer<Guide>())
    {
        ClassInjector.DerivedConstructorBody((Il2CppObjectBase)this);
    }

    public Guide(System.IntPtr ptr) : base(ptr)
    {

    }
}