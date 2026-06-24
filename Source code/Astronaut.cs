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
public class Astronaut : Role
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
            info += $"#{characters[i]} is enemies with #{characters[i + 1]}\n";
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
            characters.Add(UnityEngine.Random.RandomRangeInt(1, Gameplay.CurrentCharacters.Count+1));
        }
        if (trigger == ETriggerPhase.Night)
        {
            Il2CppSystem.Collections.Generic.List<Character> ch = Gameplay.CurrentCharacters;
            if (characters.Count == 0) characters.Add(UnityEngine.Random.RandomRangeInt(1, Gameplay.CurrentCharacters.Count + 1));
            int last = characters.Last();
            Il2CppSystem.Collections.Generic.List<Character> valid = new();
            bool evil = false;
            foreach (Character c in ch)
            {
                if (c.id == last && c.GetRegisterAlignment() == EAlignment.Evil) evil = true;
            }
            foreach (Character c in ch)
            {
                if (!evil && c.GetRegisterAlignment() == EAlignment.Evil)
                {
                    valid.Add(c);
                } else if (evil && c.GetRegisterAlignment() == EAlignment.Good)
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
        if (trigger == ETriggerPhase.Start)
        {
            characters.Add(UnityEngine.Random.RandomRangeInt(1, Gameplay.CurrentCharacters.Count + 1));
        }
        if (trigger == ETriggerPhase.Night)
        {
            Il2CppSystem.Collections.Generic.List<Character> ch = Gameplay.CurrentCharacters;
            if (characters.Count == 0) characters.Add(UnityEngine.Random.RandomRangeInt(1, Gameplay.CurrentCharacters.Count + 1));
            int last = characters.Last();
            Il2CppSystem.Collections.Generic.List<Character> valid = new();
            bool evil = false;
            foreach (Character c in ch)
            {
                if (c.id == last && c.GetRegisterAlignment() == EAlignment.Evil) evil = true;
            }
            foreach (Character c in ch)
            {
                if (evil && c.GetRegisterAlignment() == EAlignment.Evil)
                {
                    valid.Add(c);
                }
                else if (!evil && c.GetRegisterAlignment() == EAlignment.Good)
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
    public Astronaut() : base(ClassInjector.DerivedConstructorPointer<Astronaut>())
    {
        ClassInjector.DerivedConstructorBody((Il2CppObjectBase)this);
    }

    public Astronaut(System.IntPtr ptr) : base(ptr)
    {

    }
}