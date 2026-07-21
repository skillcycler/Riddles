using Il2Cpp;
using Il2CppInterop.Runtime.Injection;
using Il2CppInterop.Runtime.InteropTypes;
using Il2CppSystem;
using MelonLoader;
using System;
using System.ComponentModel.Design;
using UnityEngine;
using HarmonyLib;

namespace RiddlerMod;

[RegisterTypeInIl2Cpp]

public class Mastermind : Minion
{
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
            Il2CppSystem.Collections.Generic.List<Character> minions = Characters.Instance.FilterRealCharacterType(Gameplay.CurrentCharacters, ECharacterType.Minion);
            minions.Remove(charRef);

            Il2CppSystem.Collections.Generic.List<CharacterData> findThatMastermindData = Gameplay.Instance.GetAscensionAllStartingCharacters();
            CharacterData mastermindData = new();
            foreach (CharacterData character in findThatMastermindData)
            {
                if (character.characterId == "Mastermind_scm")
                {
                    mastermindData = character;
                }
            }
            List<string> doNotTurn = new(); // Certain characters need to still exist for their abilities to work.
            doNotTurn.Add("Snake Charmer_WING");
            doNotTurn.Add("Ritualist_WING");
            doNotTurn.Add("Professional_WING");
            doNotTurn.Add("Undying_WING");
            doNotTurn.Add("Witch_25286521");
            doNotTurn.Add("Sleeper_scm");
            doNotTurn.Add("Squire_scm");
            doNotTurn.Add("Mastermind_scm");
            doNotTurn.Add("Supporter_POW");
            doNotTurn.Add("Ambusher_POW");
            doNotTurn.Add("Grenadier_POW");

            foreach (Character minion in minions)
            {
                if (!doNotTurn.Contains(minion.dataRef.characterId) && minion.dataRef.type == ECharacterType.Minion)
                    minion.Init(mastermindData);
            }
        }
        if (trigger == ETriggerPhase.Night)
        {
            Confused.updateConfusion(charRef); // just in case the Baffler gets turned
        }
    }

    public Mastermind() : base(ClassInjector.DerivedConstructorPointer<Mastermind>())
    {
        ClassInjector.DerivedConstructorBody((Il2CppObjectBase)this);
    }
    public Mastermind(System.IntPtr ptr) : base(ptr) { }

}