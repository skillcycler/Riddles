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
public class Channeler : Minion
{
    public CharacterData copy = GetGenericMinion();
    //public int damageTimerForRitualist = 0;
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
            Il2CppSystem.Collections.Generic.List<Character> characters = Gameplay.CurrentCharacters;
            characters = Characters.Instance.FilterRealAlignmentCharacters(characters, EAlignment.Evil);
            characters.Remove(charRef);
            Il2CppSystem.Collections.Generic.List<Character> allowedCharacters = new();
            List<string> blacklistIDs = new();
            blacklistIDs.Add("Undying_WING"); // This is gonna make 2 unkillable evils.
            blacklistIDs.Add("Legion_WING"); // instant death go
            blacklistIDs.Add("Blackmailer_VP");
            blacklistIDs.Add("Summoner_scm"); // just in case
            blacklistIDs.Add("Wizard_scm"); // so this might create infinite wizards which is bad
            blacklistIDs.Add("Kingmaker_scm"); // Once a Channeler replaced the Kingmaker with a minion. This should not happen.
            blacklistIDs.Add("Snake Charmer_WING"); // The way it's coded is not compatible. At least for now. Might get removed later

            //the below characters do nothing when copied, so don't copy them if possible
            blacklistIDs.Add("Puppet_15989619");
            blacklistIDs.Add("Turncoat_WING");
            blacklistIDs.Add("Minion_71804875");
            blacklistIDs.Add("Twin Minion_15695218");
            blacklistIDs.Add("Acolyte_WING");
            blacklistIDs.Add("Fanatic_WING");
            blacklistIDs.Add("Zealot_WING");
            blacklistIDs.Add("Swarm_Evil_WING");
            blacklistIDs.Add("Imp_58992273"); // Channeler+Baa doesn't work for some reason.
            blacklistIDs.Add("Hypnotist_scm");
            blacklistIDs.Add("Professional_WING");
            blacklistIDs.Add("Tenecaligo_WING"); // This won't do anything because of game start act order.
            blacklistIDs.Add("Mendaverte_WING"); // Having 2 of Mendaverte's effects does nothing.
            blacklistIDs.Add("Leviathan_WING"); // Having 2 of Leviathan's effects does nothing.
            blacklistIDs.Add("Viciyon_WING"); // This makes you forced to take damage.
            foreach (Character character in characters) {
                if (!blacklistIDs.Contains(character.dataRef.characterId) && character.GetCharacterType() != ECharacterType.Villager && character.GetCharacterType() != ECharacterType.Outcast)
                    allowedCharacters.Add(character);
            }
            if (allowedCharacters.Count > 0)
            {
                copy = allowedCharacters[UnityEngine.Random.RandomRangeInt(0, allowedCharacters.Count)].dataRef;
                copy.role.Act(trigger, charRef);
            }
        }
        if (trigger != ETriggerPhase.Start)
        {
            /*if (copy.characterId == "Ritualist_WING")
            {
                if (trigger == (ETriggerPhase)1121218522)
                {
                    damageTimerForRitualist++;
                    if (damageTimerForRitualist >= 3)
                    {
                        damageTimerForRitualist -= 3;
                        Health health = PlayerController.PlayerInfo.health;
                        health.Damage(1);
                    }
                }
            }*/
            copy.role.Act(trigger, charRef);
        }
    }
    public override CharacterData GetBluffIfAble(Character charRef)
    {
        if (copy.characterId == "Illusionist_WING") // no disguise for a Channeler copying Emenverax
            return null;
        int diceRoll = Calculator.RollDice(10);

        if (diceRoll < 5)
        {
            return Characters.Instance.GetRandomDuplicateBluff();
        }
        else
        {
            CharacterData bluff = Characters.Instance.GetRandomUniqueBluff();
            Gameplay.Instance.AddScriptCharacterIfAble(bluff.type, bluff);

            return bluff;
        }
    }
    public override void ActOnDied(Character charRef)
    {
        copy.role.ActOnDied(charRef);
        if (copy.characterId == "Veil_scm")
        {
            PlayerController.PlayerInfo.blocks.value.Reduce(1);//since apparently it doesn't work
        }
    }
    public override bool CheckIfCanBeKilled(Character charRef)
    {
        return copy.role.CheckIfCanBeKilled(charRef);
    }
    public Channeler() : base(ClassInjector.DerivedConstructorPointer<Channeler>())
    {
        ClassInjector.DerivedConstructorBody((Il2CppObjectBase)this);
    }
    public Channeler(System.IntPtr ptr) : base(ptr) { }
    public static CharacterData GetGenericMinion()
    {
        AscensionsData allCharactersAscension = ProjectContext.Instance.gameData.allCharactersAscension;
        for (int i = 0; i < allCharactersAscension.startingMinions.Length; i++)
        {
            if (allCharactersAscension.startingMinions[i].name == "Minion")
                return allCharactersAscension.startingMinions[i];
        }
        return allCharactersAscension.startingMinions[0];
    }
}
public static class AvoidingDoubleKills
{
    public static ECharacterStatus killed = (ECharacterStatus)882;
}