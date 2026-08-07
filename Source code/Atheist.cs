using System;
using System.ComponentModel.Design;
using HarmonyLib;
using Il2Cpp;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.Injection;
using Il2CppInterop.Runtime.InteropTypes;
using Il2CppSystem;
using MelonLoader;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;
using static UnityEngine.TouchScreenKeyboard;

namespace RiddlerMod;

[RegisterTypeInIl2Cpp]
public class Atheist : Demon
{
    public override Il2CppSystem.Collections.Generic.List<SpecialRule> GetRules()
    {
        Il2CppSystem.Collections.Generic.List<SpecialRule> sr = new Il2CppSystem.Collections.Generic.List<SpecialRule>();
        sr.Add(new NightModeRule(4));
        return sr;
    }
    public override ActedInfo GetInfo(Character charRef)
    {
        return new ActedInfo("There are NO Evil characters");
    }

    public override void Act(ETriggerPhase trigger, Character charRef)
    {
        if (trigger == ETriggerPhase.Init)
        {
            Djinn.Jinx("Atheist");
        }
        if (trigger == ETriggerPhase.Start)
        {
            // determine alignment
            if (Calculator.RollDice(10) < 6)
            {
                // good
                charRef.ChangeAlignment(EAlignment.Good);
                if (Calculator.RollDice(2) == 1)
                    charRef.statuses.AddStatus(Accused.accused, charRef);
            }
            else
            {
                // evil
                foreach (Character c in Gameplay.CurrentCharacters)
                {
                    if (c == charRef) continue;
                    if (c.alignment == EAlignment.Evil)
                    {
                        c.ChangeAlignment(EAlignment.Good);
                        c.Init(Characters.Instance.GetRandomUniqueVillagerBluff());
                    }
                }
                int accuse = Calculator.RollDice((int)(Gameplay.CurrentCharacters.Count / 2))+1;
                for (int i = 0; i < accuse; i++)
                {
                    Character ch = Gameplay.CurrentCharacters[UnityEngine.Random.RandomRangeInt(0, Gameplay.CurrentCharacters.Count)];
                    while (ch.statuses.statuses.Contains(Accused.accused) || ch == charRef)
                    {
                        ch = Gameplay.CurrentCharacters[UnityEngine.Random.RandomRangeInt(0, Gameplay.CurrentCharacters.Count)];
                    }
                    ch.statuses.AddStatus(Accused.accused, charRef);
                }
                int corrupt = Calculator.RollDice((int)(Gameplay.CurrentCharacters.Count / 3))+1;
                for (int i = 0; i < corrupt; i++)
                {
                    Character ch = Gameplay.CurrentCharacters[UnityEngine.Random.RandomRangeInt(0, Gameplay.CurrentCharacters.Count)];
                    while (ch.statuses.statuses.Contains(ECharacterStatus.Corrupted) || ch == charRef)
                    {
                        ch = Gameplay.CurrentCharacters[UnityEngine.Random.RandomRangeInt(0, Gameplay.CurrentCharacters.Count)];
                    }
                    ch.statuses.AddStatus(ECharacterStatus.Corrupted, charRef);
                }
                int confuse = Calculator.RollDice((int)(Gameplay.CurrentCharacters.Count / 3))+1;
                for (int i = 0; i < confuse; i++)
                {
                    Character ch = Gameplay.CurrentCharacters[UnityEngine.Random.RandomRangeInt(0, Gameplay.CurrentCharacters.Count)];
                    while (ch.statuses.statuses.Contains(Confused.confused) || ch == charRef)
                    {
                        ch = Gameplay.CurrentCharacters[UnityEngine.Random.RandomRangeInt(0, Gameplay.CurrentCharacters.Count)];
                    }
                    ch.statuses.AddStatus(Confused.confused, charRef);
                }
            }
        }
        foreach (Character c in Gameplay.CurrentCharacters)
        {
            c.statuses.AddStatus(Muddling.hiddenStatus, charRef);
        }
        if (trigger == ETriggerPhase.Day)
        {
            onActed.Invoke(GetInfo(charRef));
        }
        if (trigger == ETriggerPhase.AfterRoundStart)
        {
            Accused.UpdateAccusedRegistration();
        }

    }
    public override CharacterData GetBluffIfAble(Character charRef)
    {
        return null;
    }
    public override void ActOnDied(Character charRef)
    {
        if (charRef.statuses.Contains(ECharacterStatus.KilledByEvil) || charRef.alignment == EAlignment.Evil) return;
        Health health = PlayerController.PlayerInfo.health;
        health.Damage(1000);
    }
    public Atheist() : base(ClassInjector.DerivedConstructorPointer<Atheist>())
    {
        ClassInjector.DerivedConstructorBody((Il2CppObjectBase)this);
    }
    public Atheist(System.IntPtr ptr) : base(ptr) { }
}