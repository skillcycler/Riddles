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
    public int real;
    public HashSet<int> fakeIDs = new();
    public CharacterData cd;
    public bool hasActivated = false;
    public override string Description
    {
        get
        {
            return "";
        }
    }
    public string makeInfo()
    {
        if (fakeIDs.Count == 0) { return $"#{real} is the {cd.characterName}"; }
        string info = "Among ";
        List<int> ints = new();
        ints.Add(real);

        foreach (int i in fakeIDs)
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
            hasActivated = true;
            Il2CppSystem.Collections.Generic.List<Character> chars = Gameplay.CurrentCharacters;
            Il2CppSystem.Collections.Generic.List<Character> evils = new();
            foreach (Character c in chars) {
                if (c.GetRegisterAlignment() == EAlignment.Evil && c.dataRef.characterId != "Professional_WING" && c.dataRef.characterId != "Iris_WING") { evils.Add(c); }
            }
            Character picked = evils[UnityEngine.Random.RandomRangeInt(0, evils.Count)];
            real = picked.id;
            while (fakeIDs.Count < 4)
            {
                int rand = Calculator.RollDice(chars.Count);
                if (rand != real) fakeIDs.Add(rand);
            }
            cd = picked.GetRegisterAs();
        }
        if (trigger == ETriggerPhase.Night)
        {
            fakeIDs.Remove(fakeIDs.Last());
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
            hasActivated = true;
            Il2CppSystem.Collections.Generic.List<Character> chars = Gameplay.CurrentCharacters;
            if (chars.Count >= 6)
            {
                Il2CppSystem.Collections.Generic.List<Character> evils = new();
                foreach (Character c in chars)
                {
                    if (c.GetRegisterAlignment() == EAlignment.Evil) { evils.Add(c); }
                }
                Character picked = evils[UnityEngine.Random.RandomRangeInt(0, evils.Count)];
                real = Calculator.RemoveNumberAndGetRandomNumberFromList(picked.id, 1, chars.Count);
                while (fakeIDs.Count < 4)
                {
                    int rand = Calculator.RollDice(chars.Count);
                    if (rand != real && rand != picked.id) fakeIDs.Add(rand);
                }
                cd = picked.GetRegisterAs();
            }
        }
        if (trigger == ETriggerPhase.Night)
        {
            if (Gameplay.CurrentCharacters.Count >= 6)
            {
                if (!hasActivated)
                {
                    hasActivated = true;
                    BluffAct(ETriggerPhase.Start, charRef);
                }
                fakeIDs.Remove(fakeIDs.Last());
                if (charRef.revealed)
                {
                    onActed.Invoke(GetInfo(charRef));
                }
            }
        }
        if (trigger == ETriggerPhase.Day)
        {
            if (Gameplay.CurrentCharacters.Count >= 6)
            {
                if (!hasActivated)
                {
                    hasActivated = true;
                    BluffAct(ETriggerPhase.Start, charRef);
                }
                charRef.revealed = true;
                onActed.Invoke(GetInfo(charRef));
            }
            else
            {
                onActed.Invoke(new ActedInfo("I am a dizzy Confessor"));
            }
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