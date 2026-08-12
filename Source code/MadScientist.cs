using System.Diagnostics;
using HarmonyLib;
using Il2CppInterop.Runtime.Injection;
using Il2CppInterop.Runtime.InteropTypes;
using MelonLoader;
using UnityEngine;
using static MelonLoader.MelonLogger;
using static UnityEngine.GraphicsBuffer;

namespace RiddlerMod;

[RegisterTypeInIl2Cpp]
public class MadScientist : Role
{
    public CharacterData fakeMinion = GetGenericMinion();
    public CharacterData fakeMinion2 = GetGenericMinion();
    public CharacterData fakeOutcast = GetGenericOutcast();
    public Character chargedActor = new Character();
    public int targetForGhost = 0;
    public int targetForGambler = 0;
    public bool killedLastNight = false; // this is for if Mad Scientist copies Hitman
    public override ActedInfo GetInfo(Character charRef)
    {
        if (fakeMinion.name == "Minion")
        {
            return new ActedInfo("I can't remember what abilities I have.");
        }
        string info = string.Format("I have the {0} and {1} abilities", fakeMinion.name, fakeOutcast.name);
        if (fakeOutcast.name == "Ghost")
        {
            if (targetForGhost == 0)
            {
                info += "\n\nI couldn't haunt anyone";
            }
            else { info += string.Format("\nI haunted #{0}", targetForGhost); }
        }
        else if (fakeOutcast.name == "Gambler")
        {
            if (targetForGambler == 0)
            {
                info += "\n\nI couldn't target anyone. You found a bug.";
            }
            else { info += string.Format("\nI invited #{0} to my casino", targetForGambler); }
        }
        else if (fakeOutcast.name == "Bounty Hunter")
        {
            List<int> corrupted = new();
            List<int> notcorrupted = new();
            foreach (Character c in Gameplay.CurrentCharacters)
            {
                if (c.statuses.Contains(ECharacterStatus.Corrupted)) corrupted.Add(c.id);
                else notcorrupted.Add(c.id);
            }
            List<int> say = new();
            say.Add(corrupted[UnityEngine.Random.RandomRangeInt(0, corrupted.Count)]);
            say.Add(notcorrupted[UnityEngine.Random.RandomRangeInt(0, notcorrupted.Count)]);
            say.Sort();
            if (corrupted.Count > 0)
            info += $"\n\n#{say[0]} or #{say[1]} is corrupted";
        }
        else if (fakeOutcast.name == "Chatterbox")
        {
            List<int> corrupted = new();
            List<int> notcorrupted = new();
            foreach (Character c in Gameplay.CurrentCharacters)
            {
                if (c.statuses.Contains(ECharacterStatus.Corrupted)) corrupted.Add(c.id);
                else notcorrupted.Add(c.id);
            }
            List<int> say = new();
            say.Add(corrupted[UnityEngine.Random.RandomRangeInt(0, corrupted.Count)]);
            int notcorrupted1 = notcorrupted[UnityEngine.Random.RandomRangeInt(0, notcorrupted.Count)];
            notcorrupted.Remove(notcorrupted1);
            say.Add(notcorrupted[UnityEngine.Random.RandomRangeInt(0, notcorrupted.Count)]);
            say.Add(notcorrupted1);
            say.Sort();
            if (corrupted.Count > 0)
                info += $"\n\nOne is corrupted: #{say[0]}, #{say[1]}, #{say[2]}";
        }
        return new ActedInfo(info);
    }
    public override ActedInfo GetBluffInfo(Character charRef)
    {
        return GetInfo(charRef);
    }
    public override string Description
    {
        get
        {
            return "";
        }
    }
    public override void BluffAct(ETriggerPhase trigger, Character charRef)
    { 
        Act(trigger, charRef);
    }
    public override void Act(ETriggerPhase trigger, Character charRef)
    {
        if (trigger == ETriggerPhase.Start)
        {
            Gameplay gameplay = Gameplay.Instance;
            Characters instance = Characters.Instance;
            Il2CppSystem.Collections.Generic.List<CharacterData> chars = gameplay.GetAscensionAllStartingCharacters();
            chars = instance.FilterNotInDeckCharactersUnique(chars);
            Il2CppSystem.Collections.Generic.List<CharacterData> outcasts = instance.FilterRealCharacterType(chars, ECharacterType.Outcast);
            Il2CppSystem.Collections.Generic.List<CharacterData> minions = instance.FilterRealCharacterType(chars, ECharacterType.Minion);
            Il2CppSystem.Collections.Generic.List<CharacterData> listOut = new Il2CppSystem.Collections.Generic.List<CharacterData>();
            Il2CppSystem.Collections.Generic.List<CharacterData> listMin = new Il2CppSystem.Collections.Generic.List<CharacterData>();
            Il2CppSystem.Collections.Generic.List<string> whitelistMinionCharacterIDs = new Il2CppSystem.Collections.Generic.List<string>();
            Il2CppSystem.Collections.Generic.List<string> whitelistOutcastCharacterIDs = new Il2CppSystem.Collections.Generic.List<string>();
            
            // vanilla
            whitelistMinionCharacterIDs.Add("Mezepheles_09511163");
            whitelistMinionCharacterIDs.Add("Poisoner_64796285");
            whitelistMinionCharacterIDs.Add("Witch_25286521");
            whitelistMinionCharacterIDs.Add("Shaman_26945607");
            whitelistMinionCharacterIDs.Add("Werewolf_78350415");
            whitelistOutcastCharacterIDs.Add("Plague Doctor_49312486");
            whitelistOutcastCharacterIDs.Add("Wretch_80988916");

            // This Mod
            whitelistMinionCharacterIDs.Add("Accuser_scm");
            whitelistMinionCharacterIDs.Add("Sleeper_scm");
            whitelistMinionCharacterIDs.Add("Guardian_scm");
            whitelistMinionCharacterIDs.Add("Baffler_scm");
            whitelistMinionCharacterIDs.Add("Mastermind_scm");
            whitelistMinionCharacterIDs.Add("Wizard_scm");
            whitelistMinionCharacterIDs.Add("Enigma_scm");
            whitelistMinionCharacterIDs.Add("PitHag_scm");

            whitelistOutcastCharacterIDs.Add("Ghost_scm");
            whitelistOutcastCharacterIDs.Add("Muddler_scm");
            whitelistOutcastCharacterIDs.Add("Confectioner_scm");
            whitelistOutcastCharacterIDs.Add("Gambler_scm");
            whitelistOutcastCharacterIDs.Add("Prankster_scm");
            whitelistOutcastCharacterIDs.Add("Damsel_scm");

            // Wingidon
            whitelistMinionCharacterIDs.Add("Saboteur_WING");
            whitelistMinionCharacterIDs.Add("Undying_WING");
            whitelistMinionCharacterIDs.Add("Swarm_Good_WING");
            whitelistMinionCharacterIDs.Add("Heretic_WING");

            whitelistOutcastCharacterIDs.Add("Chatterbox_WING");
            whitelistOutcastCharacterIDs.Add("Marionette_WING");
            whitelistOutcastCharacterIDs.Add("Echo_WING");

            // LRZH's circus
            whitelistMinionCharacterIDs.Add("Clown_LRZH");
            whitelistMinionCharacterIDs.Add("Wraith_LRZH");
            whitelistOutcastCharacterIDs.Add("Moonchild_LRZH");

            // Powerplay - Not everything that "works" will be added, for balance.
            whitelistMinionCharacterIDs.Add("Supporter_POW");
            whitelistMinionCharacterIDs.Add("Manipulator_POW");
            whitelistMinionCharacterIDs.Add("Wildling_POW");
            whitelistOutcastCharacterIDs.Add("Industrialist_POW");

            // Dupery Bluff - only adding the ones that aren't clones of existing characters
            whitelistOutcastCharacterIDs.Add("WING_Dupery_Surgeon");
            whitelistOutcastCharacterIDs.Add("WING_Dupery_Bounty Hunter");
            whitelistOutcastCharacterIDs.Add("WING_Dupery_Belfry");
            whitelistMinionCharacterIDs.Add("WING_Dupery_Barkeep");
            whitelistMinionCharacterIDs.Add("WING_Dupery_Serial Killer");
            whitelistMinionCharacterIDs.Add("WING_Dupery_Landlord");
            whitelistMinionCharacterIDs.Add("WING_Dupery_Sniper");


            for (int i = 0; i < minions.Count; i++)
            {
                if (whitelistMinionCharacterIDs.Contains(minions[i].characterId))
                    listMin.Add(minions[i]);
            }
            for (int i = 0; i < outcasts.Count; i++)
            {
                if (whitelistOutcastCharacterIDs.Contains(outcasts[i].characterId))
                    listOut.Add(outcasts[i]);
            }

            int r1 = UnityEngine.Random.RandomRangeInt(0, listMin.Count);
            int r2 = UnityEngine.Random.RandomRangeInt(0, listMin.Count);
            while (r1 == r2 && listMin.Count > 1)
            {
                r2 = UnityEngine.Random.RandomRangeInt(0, listMin.Count);
            }

            fakeMinion = listMin[r1];
            int s1 = UnityEngine.Random.RandomRangeInt(0, listOut.Count);
            fakeOutcast = listOut[s1];
            gameplay.AddScriptCharacter(ECharacterType.Minion, fakeMinion);
            gameplay.AddScriptCharacter(ECharacterType.Outcast, fakeOutcast);

            fakeMinion2 = listMin[r2];
            gameplay.AddScriptCharacter(ECharacterType.Minion, fakeMinion2);
            // time to deal with bad combos
            if (fakeMinion.characterId == "Undying_WING")
            {
                while (fakeOutcast.characterId == "Ghost_scm")
                {
                    fakeOutcast = listOut[UnityEngine.Random.RandomRangeInt(0, listOut.Count)];
                }
            }

            if (fakeMinion.characterId == "Guardian_scm")
            {
                charRef.statuses.AddStatus(SpecialMadScientistTags.hasGuardianAbility, charRef);
                while (fakeOutcast.characterId == "Marionette_WING")
                {
                    fakeOutcast = listOut[UnityEngine.Random.RandomRangeInt(0, listOut.Count)];
                }
            }
            if (charRef.GetCharacterData().characterId == "MadScientist_scm")
            {
                if (fakeMinion.characterId == "Undying_WING")
                {
                    charRef.statuses.AddStatus(SpecialMadScientistTags.hasUndyingAbility, charRef);
                }
                if (fakeMinion.characterId == "Sleeper_scm")
                {
                    charRef.statuses.AddStatus(SpecialMadScientistTags.hasSleeperAbility, charRef);
                }/*
                else if (fakeMinion.characterId == "Guardian_scm")
                {
                    MoveDemonNextToMe(charRef);
                    Il2CppSystem.Collections.Generic.List<Character> demons = Characters.Instance.FilterRealCharacterType(Gameplay.CurrentCharacters, ECharacterType.Demon);
                    if (demons.Count > 0)
                    {
                        foreach (Character demon in demons)
                        {
                            demon.statuses.AddStatus(ECharacterStatus.MessedUpByEvil, charRef);
                            demon.statuses.AddStatus(Guarding.guarded, charRef);
                        }
                    }
                }*/
                else
                {
                    fakeMinion.role.Act(trigger, charRef);
                }
                if (fakeOutcast.characterId == "Marionette_WING")
                {
                    MoveDemonNextToMe(charRef);
                }
                else if (fakeOutcast.characterId == "Gambler_scm")
                {
                    Il2CppSystem.Collections.Generic.List<Character> charss = Gameplay.CurrentCharacters;
                    Character picked = charss[UnityEngine.Random.RandomRangeInt(0, charss.Count)];
                    
                    targetForGambler = picked.id;
                    Gambler.ApplyRandomStatus(picked, charRef);
                }
                else
                {
                    fakeOutcast.role.Act(trigger, charRef);
                }
                MelonLogger.Msg(string.Format("Mad Scientist is copying the {0} and {1} abilities", fakeOutcast.name, fakeMinion.name));
            }
        }
        if (trigger == ETriggerPhase.AfterRoundStart)
        {
            if (fakeOutcast.characterName == "Wretch")
            {
                Il2CppSystem.Collections.Generic.List<CharacterData> allChars = new Il2CppSystem.Collections.Generic.List<CharacterData>();
                foreach (CharacterData charData in Gameplay.Instance.GetScriptCharacters())
                {
                    allChars.Add(charData);
                }
                allChars = Characters.Instance.FilterCharacterType(allChars, ECharacterType.Minion);
                if (allChars.Count == 0)
                    allChars.Add(ProjectContext.Instance.gameData.GetCharacterDataOfId("Puppet_15989619"));
                CharacterData randomMinion = allChars[UnityEngine.Random.Range(0, allChars.Count)];

                charRef.UpdateRegisterAsRole(randomMinion);
            }
            if (fakeOutcast.characterName == "Marionette")
            {
                charRef.UpdateRegisterAsRole(ProjectContext.Instance.gameData.GetCharacterDataOfId("Puppet_15989619"));
            }
            if (fakeOutcast.characterName == "Echo")
            {
                Il2CppSystem.Collections.Generic.List<Character> possibleTargets = new Il2CppSystem.Collections.Generic.List<Character>();
                foreach (Character character in Gameplay.CurrentCharacters)
                {
                    if (character.dataRef.characterId != "MadScientist_scm")
                    {
                        possibleTargets.Add(character);
                    }
                }
                if (possibleTargets.Count != 0)
                {
                    Character chosenTarget = possibleTargets[UnityEngine.Random.RandomRangeInt(0, possibleTargets.Count)];
                    charRef.UpdateRegisterAsRole(chosenTarget.dataRef);
                }
            }
        }
        if (trigger == ETriggerPhase.Day)
        {

            if (fakeOutcast.characterId == "Ghost_scm")
            {
                charRef.statuses.AddStatus(SpecialMadScientistTags.hasGhostAbility, charRef);
                charRef.state = ECharacterState.Dead;
                PlayerController.PlayerInfo.health.Damage(1);
                ActOnDied(charRef);
                Il2CppSystem.Collections.Generic.List<Character> unrevealedCharacters = Characters.Instance.FilterHiddenCharacters(Gameplay.CurrentCharacters);
                unrevealedCharacters = Characters.Instance.FilterAlignmentCharacters(unrevealedCharacters, EAlignment.Good);
                unrevealedCharacters = Characters.Instance.FilterRealAlignmentCharacters(unrevealedCharacters, EAlignment.Good);
                unrevealedCharacters = Characters.Instance.FilterCharacterMissingStatus(unrevealedCharacters, ECharacterStatus.Corrupted);
                charRef.RevealAllReal();
                charRef.RefreshCharacter();
                if (unrevealedCharacters.Count == 0)
                {
                    targetForGhost = 0;
                }
                else
                {
                    Character targetChar = unrevealedCharacters[UnityEngine.Random.RandomRangeInt(0, unrevealedCharacters.Count)];
                    targetChar.statuses.AddStatus(ECharacterStatus.Corrupted, charRef);
                    targetChar.statuses.statuses.Remove(ECharacterStatus.HealthyBluff);
                    targetForGhost = targetChar.id;
                }
            }
            onActed.Invoke(GetInfo(charRef));
        }
        if (charRef.GetCharacterData().characterId == "MadScientist_scm" && trigger != ETriggerPhase.Start)
        {
            if (fakeOutcast.characterId == "Gambler_scm")
            {
                if (trigger == ETriggerPhase.Night)
                {
                    Confused.updateConfusion(charRef);
                }
            } else if (fakeOutcast.characterId != "Ghost_scm" && fakeOutcast.characterId != "Prankster_scm")
                if (!(fakeOutcast.picking && trigger == ETriggerPhase.Day))
                    fakeOutcast.role.Act(trigger, charRef);
            fakeMinion.role.Act(trigger, charRef);
        }
    }
    public override bool CheckIfCanBeKilled(Character charRef)
    {
        if (fakeOutcast.characterName == "Witch") PlayerController.PlayerInfo.blocks.value.Reduce(1);
        return fakeMinion.role.CheckIfCanBeKilled(charRef) && fakeOutcast.role.CheckIfCanBeKilled(charRef);
    }
    public override void ActOnDied(Character charRef)
    {
        if (charRef.GetCharacterData().characterId == "MadScientist_scm")
        {
            fakeMinion.role.ActOnDied(charRef);
            fakeOutcast.role.ActOnDied(charRef);
        }
    }
    public override int GetDamageToYou()
    {
        if (fakeOutcast.name == "Bombardier")
        {
            return 100000;
        }
        if (fakeOutcast.characterId == "Marionette_WING" || fakeOutcast.characterId == "Revolutionary_WING")
        {
            return 3;
        }
        if (fakeOutcast.characterId == "Ghost_scm")
        {
            return 1;
        }
        return 5;
    }
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
    public static CharacterData GetGenericOutcast()
    {
        AscensionsData allCharactersAscension = ProjectContext.Instance.gameData.allCharactersAscension;
        for (int i = 0; i < allCharactersAscension.startingOutsiders.Length; i++)
        {
            if (allCharactersAscension.startingOutsiders[i].name == "Doppelganger")
                return allCharactersAscension.startingOutsiders[i];
        }
        return allCharactersAscension.startingOutsiders[0];
    }
    public MadScientist() : base(ClassInjector.DerivedConstructorPointer<MadScientist>())
    {
        ClassInjector.DerivedConstructorBody((Il2CppObjectBase)this);
    }
    public MadScientist(IntPtr ptr) : base(ptr) { }
    public override CharacterData GetRegisterAsRole(Character charRef)
    {
        if (fakeOutcast.name == "Wretch")
        {
            Il2CppSystem.Collections.Generic.List<CharacterData> allChars = new Il2CppSystem.Collections.Generic.List<CharacterData>();
            foreach (CharacterData charData in Gameplay.Instance.GetScriptCharacters()) {
                allChars.Add(charData);
            }
            allChars = Characters.Instance.FilterCharacterType(allChars, ECharacterType.Minion);
            if (allChars.Count == 0)
                allChars.Add(ProjectContext.Instance.gameData.GetCharacterDataOfId("Puppet_15989619"));
            CharacterData randomMinion = allChars[UnityEngine.Random.Range(0, allChars.Count)];

            return randomMinion;
        }
        if (fakeOutcast.characterId == "Marionette_WING")
        {
            return ProjectContext.Instance.gameData.GetCharacterDataOfId("Puppet_15989619");
        }
        return null;
    }
    private void MoveDemonNextToMe(Character charRef)
    {
        Il2CppSystem.Collections.Generic.List<Character> checkDemons = new Il2CppSystem.Collections.Generic.List<Character>();
        checkDemons = Characters.Instance.FilterRealCharacterType(Gameplay.CurrentCharacters, ECharacterType.Demon);

        Character pickedDemon = checkDemons[UnityEngine.Random.Range(0, checkDemons.Count)];

        Il2CppSystem.Collections.Generic.List<Character> adjacentCharacters = Characters.Instance.GetAdjacentAliveCharacters(charRef);
        Character pickedSwapCharacter = adjacentCharacters[UnityEngine.Random.Range(0, adjacentCharacters.Count)];
        CharacterData pickedData = pickedSwapCharacter.dataRef;
        pickedSwapCharacter.Init(pickedDemon.dataRef);
        pickedDemon.Init(pickedData);
    }
}
public static class SpecialMadScientistTags
{ // These statuses are used to check, for other characters, what mad scientist's abilities are
    public static ECharacterStatus hasGhostAbility = (ECharacterStatus)1201;
    public static ECharacterStatus hasSleeperAbility = (ECharacterStatus)1202;
    public static ECharacterStatus hasUndyingAbility = (ECharacterStatus)1203;
    public static ECharacterStatus hasGuardianAbility = (ECharacterStatus)1204;
}