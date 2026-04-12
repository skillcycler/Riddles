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

public class Warlock : Demon
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
        List<string> bannedDemons = new List<string>();
        bannedDemons.Add("Warlock"); // don't turn something into another summoner
        bannedDemons.Add("Pandemonium_WING"); // This just might cause problems.
        bannedDemons.Add("Tenecaligo_WING"); // Adds minions which is not good.
        bannedDemons.Add("Praesect_WING"); // Adds minions which is not good.
        bannedDemons.Add("Hypnotist_ER"); // Adds minions which is not good.
        bannedDemons.Add("Hydra_VP"); // Corrupted hydra telling the truth sucks
        bannedDemons.Add("Mutant_84675843"); // Why is this one even in the list of demons
        for (int j = 0; j < allDatas.Length; j++)
        {
            CharacterData d = allDatas[j];
            if (d.type == ECharacterType.Demon && !bannedDemons.Contains(d.characterId))
            {
                possibleDemons.Add(d);
            }
        }
        Il2CppSystem.Collections.Generic.List<Character> summons = new Il2CppSystem.Collections.Generic.List<Character>();
        foreach (Character c in Gameplay.CurrentCharacters)
            summons.Add(c);
        summons.Remove(charRef);
        int extraDemons = 1;
        if (Gameplay.CurrentCharacters.Count >= 9)
        {
            extraDemons = Calculator.RollDice(2);
        }
        if (Gameplay.CurrentCharacters.Count >= 11)
        {
            extraDemons = 2;
        }
        if (Gameplay.CurrentCharacters.Count >= 13)
        {
            extraDemons = Calculator.RollDice(2)+1;
        }
        if (extraDemons > 1)
        {
            Health health = PlayerController.PlayerInfo.health;
            health.AddMaxHp(5);
            health.Heal(100);
        }
        for (int i = 0; i < extraDemons; i++)
        {
            Character currentSummon = summons[UnityEngine.Random.RandomRangeInt(0, summons.Count)];
            int chosen = UnityEngine.Random.RandomRangeInt(0, possibleDemons.Count);
            CharacterData selectedDemon = possibleDemons[chosen];
            possibleDemons.Remove(selectedDemon);
            currentSummon.Init(selectedDemon);
            summons.Remove(currentSummon);
        }

    }
    public Warlock() : base(ClassInjector.DerivedConstructorPointer<Warlock>())
    {
        ClassInjector.DerivedConstructorBody((Il2CppObjectBase)this);
    }
    public Warlock(System.IntPtr ptr) : base(ptr) { }
}