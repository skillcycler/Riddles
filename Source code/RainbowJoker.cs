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
public class RainbowJoker : Demon
{
    public override Il2CppSystem.Collections.Generic.List<SpecialRule> GetRules()
    {
        Il2CppSystem.Collections.Generic.List<SpecialRule> sr = new Il2CppSystem.Collections.Generic.List<SpecialRule>();
        sr.Add(new NightModeRule(4));
        return sr;
    }
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
            Il2CppSystem.Collections.Generic.List<Character> viableCharacters = Gameplay.CurrentCharacters;

            Il2CppSystem.Collections.Generic.List<CharacterData> allCharacters = Gameplay.Instance.GetAscensionAllStartingCharacters();
            Il2CppSystem.Collections.Generic.List<CharacterData> outcasts_ = Characters.Instance.FilterRealCharacterType(allCharacters, ECharacterType.Outcast);
            Il2CppSystem.Collections.Generic.List<CharacterData> minions = Characters.Instance.FilterRealCharacterType(allCharacters, ECharacterType.Minion);
            List<string> invalidMinions = new List<string>();
            invalidMinions.Add("Cryptid_WING");
            invalidMinions.Add("Heretic_WING");
            invalidMinions.Add("Enigma_scm"); // no point in having fake characters here
            //Weather is banned
            invalidMinions.Add("Snowy_POW");
            invalidMinions.Add("Sunny_POW");
            invalidMinions.Add("Stormy_POW");
            invalidMinions.Add("Foggy_POW");
            List<string> invalidOutcasts = new List<string>();
            invalidOutcasts.Add("MadScientist_scm"); // too lazy to fix this
            Il2CppSystem.Collections.Generic.List<CharacterData> validMinions = new();
            foreach (CharacterData m in minions)
            {
                if (!invalidMinions.Contains(m.characterId))
                {
                    validMinions.Add(m);
                }
            }
            Il2CppSystem.Collections.Generic.List<CharacterData> outcasts = new();
            foreach (CharacterData m in outcasts_)
            {
                if (!invalidOutcasts.Contains(m.characterId))
                {
                    outcasts.Add(m);
                }
            }
            int minionsToAdd = (int)((Gameplay.CurrentCharacters.Count - 1) / 3);

            int outcastsToAdd = Calculator.RollDice(5) - 1; // add 0-4 outcasts at random
            if (Gameplay.CurrentCharacters.Count < 12)
            {
                outcastsToAdd = Calculator.RollDice(3) - 1; // for small villages only add 0-2
            }
            HashSet<int> numbers = new HashSet<int>(); // for minions

            while (numbers.Count < minionsToAdd)
            {
                int toAdd = UnityEngine.Random.RandomRangeInt(1, Gameplay.CurrentCharacters.Count + 1);
                if (toAdd != charRef.id)
                    numbers.Add(toAdd);
            }
            foreach (Character c in Gameplay.CurrentCharacters)
            {
                if (numbers.Contains(c.id))
                {
                    CharacterData cd = validMinions[UnityEngine.Random.RandomRangeInt(0, validMinions.Count)];
                    validMinions.Remove(cd);
                    c.Init(cd);
                    c.statuses.AddStatus(ECharacterStatus.MessedUpByEvil, charRef);
                    //MelonLogger.Msg($"Turning #{c.id} into a {cd.characterName}");
                }
            }
            if (outcastsToAdd > 0) {
                HashSet<int> numbers2 = new HashSet<int>(); // for outcasts

                while (numbers2.Count < outcastsToAdd)
                {
                    int toAdd = UnityEngine.Random.RandomRangeInt(1, Gameplay.CurrentCharacters.Count + 1);
                    if (toAdd != charRef.id && !numbers.Contains(toAdd)) numbers2.Add(toAdd);
                }
                foreach (Character c2 in Gameplay.CurrentCharacters)
                {
                    if (numbers2.Contains(c2.id))
                    {
                        CharacterData cd = outcasts[UnityEngine.Random.RandomRangeInt(0, outcasts.Count)];
                        outcasts.Remove(cd);
                        c2.Init(cd);
                        c2.statuses.AddStatus(ECharacterStatus.MessedUpByEvil, charRef);
                        //MelonLogger.Msg($"Turning #{c2.id} into a {cd.characterName}");
                    }
                }
            }
        }
    }

    public RainbowJoker() : base(ClassInjector.DerivedConstructorPointer<RainbowJoker>())
    {
        ClassInjector.DerivedConstructorBody((Il2CppObjectBase)this);
    }
    public RainbowJoker(System.IntPtr ptr) : base(ptr) { }

}