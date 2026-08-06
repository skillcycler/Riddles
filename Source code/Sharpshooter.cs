using System;
using System.Linq;
using System.Reflection;
using Harmony;
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
public class Sharpshooter : Role
{
    public List<List<int>> characters = new();
    public List<CharacterData> characterDatas = new();
    public override string Description
    {
        get
        {
            return "";
        }
    }
    public string makeInfo()
    {
        if (characters.Count < 1) return "I have no information yet";
        string info = "";
        for (int i = 0; i < characters.Count; i++)
        {
            List<int> li = characters[i];
            CharacterData cd = characterDatas[i];
            li.Sort();
            info += $"#{li[0]}, #{li[1]}, #{li[2]}, #{li[3]}, or #{li[4]} is the {cd.characterName}\n";
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
        if (trigger == ETriggerPhase.Night)
        {
            if (charRef.state == ECharacterState.Dead) return;
            Il2CppSystem.Collections.Generic.List<Character> chars = Gameplay.CurrentCharacters;
            Il2CppSystem.Collections.Generic.List<Character> evils = new();
            foreach (Character c in chars)
            {
                if (c.GetRegisterAlignment() == EAlignment.Evil && c.alignment == EAlignment.Evil) { evils.Add(c); }
            }
            Character picked = evils[UnityEngine.Random.RandomRangeInt(0, evils.Count)];
            characterDatas.Add(picked.GetRegisterAs());
            List<int> possibleLocations = new();
            possibleLocations.Add(picked.id);

            while (possibleLocations.Count < 5)
            {
                int random;
                do { random = UnityEngine.Random.RandomRangeInt(1, Gameplay.CurrentCharacters.Count + 1); }
                while (possibleLocations.Contains(random));
                possibleLocations.Add(random);
            }

            characters.Add(possibleLocations);
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
            Il2CppSystem.Collections.Generic.List<Character> chars = Gameplay.CurrentCharacters;
            Il2CppSystem.Collections.Generic.List<Character> evils = new();
            foreach (Character c in chars)
            {
                if (c.GetRegisterAlignment() == EAlignment.Evil && c.alignment == EAlignment.Evil) { evils.Add(c); }
            }
            Character picked = evils[UnityEngine.Random.RandomRangeInt(0, evils.Count)];
            characterDatas.Add(picked.GetRegisterAs());
            List<int> possibleLocations = new();
            while (possibleLocations.Count < 5)
            {
                int random;
                do { random = UnityEngine.Random.RandomRangeInt(1, Gameplay.CurrentCharacters.Count + 1); }
                while (possibleLocations.Contains(random) || random == picked.id);
                possibleLocations.Add(random);
            }

            characters.Add(possibleLocations);
            if (charRef.revealed)
            {
                onActed.Invoke(GetInfo(charRef));
            }
        }
        if (trigger == ETriggerPhase.Day)
        {
            if (characters == null) characters = new();
            if (characters.Count == 0)
            {
                int add = Gameplay.Instance.currentDay;
                Il2CppSystem.Collections.Generic.List<Character> chars = Gameplay.CurrentCharacters;
                Il2CppSystem.Collections.Generic.List<Character> evils = new();
                foreach (Character c in chars)
                {
                    if (c.GetRegisterAlignment() == EAlignment.Evil && c.alignment == EAlignment.Evil) { evils.Add(c); }
                }
                for (int i = 0; i < add; i++)
                {
                    Character picked = evils[UnityEngine.Random.RandomRangeInt(0, evils.Count)];
                    characterDatas.Add(picked.GetRegisterAs());
                    List<int> possibleLocations = new();
                    while (possibleLocations.Count < 5)
                    {
                        int random;
                        do { random = UnityEngine.Random.RandomRangeInt(1, Gameplay.CurrentCharacters.Count + 1); }
                        while (possibleLocations.Contains(random) || random == picked.id);
                        possibleLocations.Add(random);
                    }

                    characters.Add(possibleLocations);
                }
            }
            charRef.revealed = true;
            onActed.Invoke(GetInfo(charRef));
        }
    }
    public Sharpshooter() : base(ClassInjector.DerivedConstructorPointer<Sharpshooter>())
    {
        ClassInjector.DerivedConstructorBody((Il2CppObjectBase)this);
    }

    public Sharpshooter(System.IntPtr ptr) : base(ptr)
    {

    }
}