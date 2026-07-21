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
public class Sharpshooter : Role
{
    public List<int> characters = new List<int>();
    public CharacterData cd = new();
    public override string Description
    {
        get
        {
            return "";
        }
    }
    public string makeInfo()
    {
        if (characters.Count < 2) return $"#{characters[0]} is the {cd.characterName}";
        string info = "Among ";
        List<int> ints = new();
        foreach (int i in characters)
        {
            ints.Add(i);
        }
        ints.Sort();
        foreach (int i in ints)
        {
            info += $"#{i}, ";
        }
        info += $"there is: {cd.characterName}";
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
            Il2CppSystem.Collections.Generic.List<Character> chars = Gameplay.CurrentCharacters;
            Il2CppSystem.Collections.Generic.List<Character> evils = new();
            foreach (Character c in chars)
            {
                if (c.GetRegisterAlignment() == EAlignment.Evil && c.dataRef.characterId != "Professional_WING" && c.dataRef.characterId != "Iris_WING") { evils.Add(c); }
            }
            Character picked = evils[UnityEngine.Random.RandomRangeInt(0, evils.Count)];
            characters.Add(picked.id);
            while (characters.Count < 5)
            {
                int rand = Calculator.RollDice(chars.Count);
                if (!characters.Contains(rand)) characters.Add(rand);
            }
            cd = picked.GetRegisterAs();
        }
        if (trigger == ETriggerPhase.Night)
        {
            if (characters.Count > 1) {
                int last = characters.Last();
                characters.Remove(last);
                if (charRef.revealed)
                {
                    onActed.Invoke(GetInfo(charRef));
                }
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
            if (characters.Count > 1)
            {
                int last = characters.Last();
                characters.Remove(last);
                if (charRef.revealed)
                {
                    onActed.Invoke(GetInfo(charRef));
                }
            }
        }
        if (trigger == ETriggerPhase.Day)
        {
            
            int addFake = 5 - Gameplay.Instance.currentDay;
            if (addFake < 1) addFake = 1;
            Il2CppSystem.Collections.Generic.List<Character> chars = Gameplay.CurrentCharacters;
            if (chars.Count >= 6)
            {
                Il2CppSystem.Collections.Generic.List<Character> evils = new();
                foreach (Character c in chars)
                {
                    if (c.GetRegisterAlignment() == EAlignment.Evil) { evils.Add(c); }
                }
                Character picked = evils[UnityEngine.Random.RandomRangeInt(0, evils.Count)];
                int real = Calculator.RemoveNumberAndGetRandomNumberFromList(picked.id, 1, chars.Count);
                while (characters.Count < addFake)
                {
                    int rand = Calculator.RollDice(chars.Count);
                    if (rand != real && !characters.Contains(rand)) characters.Add(rand);
                }
                cd = picked.GetRegisterAs();
            }
            else
            {
                onActed.Invoke(new ActedInfo("This role needs 6 cards to function correctly."));
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