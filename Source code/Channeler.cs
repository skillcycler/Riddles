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

            List<string> whitelistIDs = new();

            // Vanilla
            whitelistIDs.Add("Mezepheles_09511163");
            whitelistIDs.Add("Poisoner_64796285");
            whitelistIDs.Add("Witch_25286521");
            whitelistIDs.Add("Shaman_26945607");
            whitelistIDs.Add("Baron_04539999");

            whitelistIDs.Add("Pooka_13445289");
            whitelistIDs.Add("Lillith_90453844");

            // This Mod
            whitelistIDs.Add("Accuser_scm");
            whitelistIDs.Add("Sleeper_scm");
            whitelistIDs.Add("Baffler_scm");
            whitelistIDs.Add("Accuser_scm");
            whitelistIDs.Add("Enigma_scm");

            whitelistIDs.Add("Follower_scm");
            whitelistIDs.Add("Veil_scm");
            whitelistIDs.Add("Infestation_scm");
            whitelistIDs.Add("Escapist_scm");
            whitelistIDs.Add("Mystifier_scm");

            // Wingidon's Mod
            whitelistIDs.Add("Heretic_WING");
            whitelistIDs.Add("Professional_WING");
            whitelistIDs.Add("Saboteur_WING");
            whitelistIDs.Add("Swarm_Good_WING");

            whitelistIDs.Add("Caedoccidere_WING");
            whitelistIDs.Add("Carnicarius_WING");
            whitelistIDs.Add("Iris_WING");
            whitelistIDs.Add("Praesect_WING");
            whitelistIDs.Add("Sanguitaurus_WING");
            whitelistIDs.Add("Mezepheles_WING");
            whitelistIDs.Add("TwinDemon_WING");
            whitelistIDs.Add("TwinDemonTwin_WING");
            whitelistIDs.Add("TwinDemonTriplet_WING");

            // Misc
            whitelistIDs.Add("Wraith_LRZH");
            // can't guarantee anything else works correctly or is balanced, if they do anything at all
            whitelistIDs.Add("Slinger_POW");
            whitelistIDs.Add("Manipulator_POW");
            whitelistIDs.Add("Ambusher_POW");

            // Dupery Bluff
            whitelistIDs.Add("WING_Dupery_Barkeep");
            whitelistIDs.Add("WING_Dupery_Poisoner");
            whitelistIDs.Add("WING_Dupery_Serial Killer");
            whitelistIDs.Add("WING_Dupery_Travel Agent");


            foreach (Character character in characters) {
                if (whitelistIDs.Contains(character.dataRef.characterId))
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
            if (copy.characterId == "Professional_WING")
            {
                if (charRef.bluff == true)
                {
                    charRef.UpdateRegisterAsRole(charRef.bluff);
                }

                Il2CppSystem.Collections.Generic.List<CharacterData> notInPlayCh = Gameplay.Instance.GetScriptCharacters();
                notInPlayCh = Characters.Instance.FilterCharacterType(notInPlayCh, ECharacterType.Villager);
                notInPlayCh = Characters.Instance.FilterBluffableCharacters(notInPlayCh);

                charRef.UpdateRegisterAsRole(notInPlayCh[UnityEngine.Random.Range(0, notInPlayCh.Count - 1)]);
            }
            copy.role.Act(trigger, charRef);

        }
    }
    public override CharacterData GetBluffIfAble(Character charRef)
    {
        if (copy.characterId == "Illusionist_WING") // no disguise for a Channeler copying Emenverax
            return null;
        if (copy.characterId == "Escapist_scm")
        {
            Il2CppSystem.Collections.Generic.List<CharacterData> outsiders = Gameplay.Instance.GetAscensionAllStartingCharacters();
            outsiders = Characters.Instance.FilterRealCharacterType(outsiders, ECharacterType.Outcast);
            outsiders = Characters.Instance.FilterBluffableCharacters(outsiders);
            CharacterData pickedOutsider = outsiders[UnityEngine.Random.Range(0, outsiders.Count - 1)];
            Gameplay.Instance.AddScriptCharacterIfAble(ECharacterType.Outcast, pickedOutsider);

            return pickedOutsider;
        }
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