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
public class Summoner : Demon
{
    public override Il2CppSystem.Collections.Generic.List<SpecialRule> GetRules()
    {
        Il2CppSystem.Collections.Generic.List<SpecialRule> sr = new Il2CppSystem.Collections.Generic.List<SpecialRule>();
        sr.Add(new NightModeRule(4));
        return sr;
    }
    public CharacterData[] allDatas = Il2CppSystem.Array.Empty<CharacterData>();
    
    public override void Act(ETriggerPhase trigger, Character charRef)
    {
        if (trigger == ETriggerPhase.Init)
        {
            Djinn.JinxVillagers("Summoner");
        }
        if (trigger != ETriggerPhase.Start) return;

        Il2CppSystem.Collections.Generic.List<CharacterData> possibleDemons = new Il2CppSystem.Collections.Generic.List<CharacterData>();
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

        List<string> allowed = new();

        // Vanilla
        allowed.Add("Imp_58992273");
        allowed.Add("Lillith_90453844");
        allowed.Add("Pooka_13445289");

        // This mod
        allowed.Add("Follower_scm");
        allowed.Add("Veil_scm");
        allowed.Add("Infestation_scm");
        allowed.Add("Mystifier_scm");
        allowed.Add("Fracture_scm");

        // Wingidon's mod
        allowed.Add("Caedoccidere_WING");
        allowed.Add("Carnicarius_WING");
        allowed.Add("Iris_WING");
        allowed.Add("Mezepheles_WING");
        allowed.Add("TwinDemon_WING");
        allowed.Add("TwinDemonTwin_WING");
        allowed.Add("TwinDemonTriplet_WING");

        // Dupery Bluff
        allowed.Add("WING_Dupery_Idol");
        allowed.Add("WING_Dupery_Kingpin");
        allowed.Add("WING_Dupery_Hitman");

        // Powerplay
        allowed.Add("Auditor_POW");
        allowed.Add("Starspawn_POW");


        for (int j = 0; j < allDatas.Length; j++)
        {
            CharacterData d = allDatas[j];
            if (d.type == ECharacterType.Demon && allowed.Contains(d.characterId))
            {
                possibleDemons.Add(d);
            }
        }
        Il2CppSystem.Collections.Generic.List<Character> summons = new Il2CppSystem.Collections.Generic.List<Character>();
        foreach (Character c in Gameplay.CurrentCharacters)
        {
            summons.Add(c);
        }
        summons.Remove(charRef);
        int extraDemons = 1;
        if (Gameplay.CurrentCharacters.Count >= 9)
        {
            extraDemons = Calculator.RollDice(2);
        }
        if (Gameplay.CurrentCharacters.Count >= 11)
        {
            extraDemons = Calculator.RollDice(2) + 1;
        }
        if (Gameplay.CurrentCharacters.Count >= 13)
        {
            extraDemons = Calculator.RollDice(2) + 2;
        }
        if (Gameplay.CurrentCharacters.Count == 21) // This village size is specifically for testing characters that I add.
        {
            extraDemons = 0;
            Health health = PlayerController.PlayerInfo.health;
            health.AddMaxHp(100);
            health.Heal(100);
        }
        else
        {
            int extra = 0;
            if (Gameplay.CurrentCharacters.Count >= 9) extra = 1;
            if (Gameplay.CurrentCharacters.Count >= 11) extra = 3;
            if (Gameplay.CurrentCharacters.Count >= 13) extra = 5;
            Health health = PlayerController.PlayerInfo.health;
            health.AddMaxHp(extra);
            health.Heal(100);
        }
        for (int i = 0; i < extraDemons; i++)
        {
            Character currentSummon = summons[UnityEngine.Random.RandomRangeInt(0, summons.Count)];
            int chosen = UnityEngine.Random.RandomRangeInt(0, possibleDemons.Count);
            CharacterData selectedDemon = possibleDemons[chosen];
            possibleDemons.Remove(selectedDemon);
            currentSummon.Init(selectedDemon);
            currentSummon.statuses.AddStatus(ECharacterStatus.MessedUpByEvil, charRef);
            summons.Remove(currentSummon);
        }

    }
    public Summoner() : base(ClassInjector.DerivedConstructorPointer<Summoner>())
    {
        ClassInjector.DerivedConstructorBody((Il2CppObjectBase)this);
    }
    public Summoner(System.IntPtr ptr) : base(ptr) { }
}