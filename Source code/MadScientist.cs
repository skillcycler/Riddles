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
    //public CharacterData fakeOutcast2 = GetGenericOutcast();
    public Character chargedActor = new Character();
    public int targetForGhost = 0;
    public int targetForGambler = 0;
    public bool killedLastNight = false; // this is for if Mad Scientist copies Hitman
    //public int damageTimerForRitualist = 0;
    public override ActedInfo GetInfo(Character charRef)
    {
        if (fakeMinion.name == "Minion")
        {
            return new ActedInfo("I can't remember what abilities I have.");
        }/*
        if (fakeOutcast.name == "Doppelganger")
        {
            return new ActedInfo("Something went wrong and I don't have an Outcast ability");
        }
        if (fakeOutcast.characterId == "Renegade_WING" || fakeOutcast.characterId == "Hitman_scm")
        {
            return new ActedInfo(string.Format("I have the {0} and {1} abilities", fakeMinion2.name, fakeOutcast2.name));
        }*/
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
            //whitelistMinionCharacterIDs.Add("Baron_04539999"); oops this is bugged as well
            whitelistOutcastCharacterIDs.Add("Plague Doctor_49312486");
            whitelistOutcastCharacterIDs.Add("Wretch_80988916");
            // whitelistOutcastCharacterIDs.Add("Bombardier_79093372"); broken also you won't ever stab the mad scientist
            //whitelistOutcastCharacterIDs.Add("Rambler_57930131"); Does not work at all.
            //whitelistOutcastCharacterIDs.Add("Doppleganger_52694042");
            // This Mod
            whitelistMinionCharacterIDs.Add("Accuser_scm");
            //whitelistMinionCharacterIDs.Add("Channeler_scm");
            whitelistMinionCharacterIDs.Add("Sleeper_scm");
            whitelistMinionCharacterIDs.Add("Guardian_scm");
            whitelistMinionCharacterIDs.Add("Baffler_scm");
            whitelistMinionCharacterIDs.Add("Mastermind_scm");
            whitelistMinionCharacterIDs.Add("Wizard_scm");


            whitelistOutcastCharacterIDs.Add("Ghost_scm");
            whitelistOutcastCharacterIDs.Add("Muddler_scm");
            whitelistOutcastCharacterIDs.Add("Hitman_scm");
            whitelistOutcastCharacterIDs.Add("Confectioner_scm");
            whitelistOutcastCharacterIDs.Add("Gambler_scm");
            // Wingidon

            whitelistMinionCharacterIDs.Add("Saboteur_WING");
            whitelistMinionCharacterIDs.Add("Undying_WING");
            whitelistMinionCharacterIDs.Add("Swarm_Good_WING");
            //whitelistMinionCharacterIDs.Add("Snake Charmer_WING"); gonna make this work in some future update
            //whitelistMinionCharacterIDs.Add("Ritualist_WING");
            whitelistMinionCharacterIDs.Add("Heretic_WING");

            whitelistOutcastCharacterIDs.Add("Chatterbox_WING");
            //whitelistOutcastCharacterIDs.Add("Revolutionary_WING");
            whitelistOutcastCharacterIDs.Add("Marionette_WING");
            //whitelistOutcastCharacterIDs.Add("Renegade_WING");
            //whitelistOutcastCharacterIDs.Add("Lunatic_WING");

            // LRZH's circus

            whitelistMinionCharacterIDs.Add("Clown_LRZH");
            whitelistMinionCharacterIDs.Add("Wraith_LRZH");
            whitelistOutcastCharacterIDs.Add("Moonchild_LRZH");

            /* a lot of these are abandoned and will no longer officially be supported
            // Carlz
            //whitelistMinionCharacterIDs.Add("Lycaon_VP"); This has been causing too many bugs
            whitelistMinionCharacterIDs.Add("Blackmailer_VP");
            whitelistOutcastCharacterIDs.Add("Rook_VP");
            //whitelistOutcastCharacterIDs.Add("Mayor_VP");
            // Mass Hysteria
            whitelistMinionCharacterIDs.Add("Siren_MaHy");
            whitelistOutcastCharacterIDs.Add("Magician_MaHy");
            // Reveal Dilemma
            whitelistMinionCharacterIDs.Add("Ambusher_rdm");
            whitelistMinionCharacterIDs.Add("Martyr_rdm");
            whitelistOutcastCharacterIDs.Add("Saboteur_rdm");
            // CSK expansion pack
            whitelistOutcastCharacterIDs.Add("Atheist_EP");
            whitelistMinionCharacterIDs.Add("Cavalier_EP");*/
            // Extra randomized by WWW
            whitelistMinionCharacterIDs.Add("Purifier_ER");



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
            /*int s2 = UnityEngine.Random.RandomRangeInt(0, listOut.Count);
            while ((s1 == s2 && listOut.Count > 1) || listOut[s2].characterId == "Hitman_scm" || listOut[s2].characterId == "Renegade_WING"
                || listOut[s2].name == "Drunk" || listOut[s2].name == "Doppelganger" || listOut[s2].characterId == "Lunatic_WING")
            {
                s2 = UnityEngine.Random.RandomRangeInt(0, listOut.Count);
            }*/
            fakeOutcast = listOut[s1];
            //fakeOutcast2 = listOut[s2]; // Never actually adds this. It's for the rare case when it needs to lie
            gameplay.AddScriptCharacter(ECharacterType.Minion, fakeMinion);
            gameplay.AddScriptCharacter(ECharacterType.Outcast, fakeOutcast);

            fakeMinion2 = listMin[r2];
            // time to deal with bad combos
            if (fakeMinion.characterId == "Undying_WING")
            {
                while (fakeOutcast.characterId == "Ghost_scm")
                {
                    fakeOutcast = listOut[s1 = UnityEngine.Random.RandomRangeInt(0, listOut.Count)];
                }
            }

            if (fakeMinion.characterId == "Guardian_scm")
            {
                while (fakeOutcast.characterId == "Marionette_WING")
                {
                    fakeOutcast = listOut[s1 = UnityEngine.Random.RandomRangeInt(0, listOut.Count)];
                }
            }
            /*
            if (UnityEngine.Random.RandomRangeInt(0, 2) == 0 || fakeOutcast.characterId == "Renegade_WING" || fakeOutcast.characterId == "Hitman_scm")
            {
                gameplay.AddScriptCharacter(ECharacterType.Minion, fakeMinion2);
            }*/
            if (charRef.GetCharacterData().characterId == "MadScientist_scm")
            {
                if (fakeMinion.characterId == "Undying_WING")
                {
                    charRef.statuses.AddStatus(SpecialMadScientistTags.hasUndyingAbility, charRef);
                }
                if (fakeMinion.characterId == "Sleeper_scm")
                {
                    charRef.statuses.AddStatus(SpecialMadScientistTags.hasSleeperAbility, charRef);
                }
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
                }
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
                    if (picked.alignment == EAlignment.Evil)
                    {
                        switch (Calculator.RollDice(3))
                        {
                            case 1:
                                picked.statuses.AddStatus(ECharacterStatus.Corrupted, charRef); break;
                            case 2:
                                picked.statuses.AddStatus(Accused.accused, charRef); break;
                            case 3:
                                picked.statuses.AddStatus(Confused.confused, charRef);
                                Confused.updateConfusion(charRef);
                                break;

                        }
                    }
                    else
                    {
                        switch (Calculator.RollDice(4))
                        {
                            case 1:
                                picked.statuses.AddStatus(ECharacterStatus.Corrupted, charRef); break;
                            case 2:
                                picked.statuses.AddStatus(Escaped.evilTurned, charRef);
                                picked.ChangeAlignment(EAlignment.Evil);
                                break;
                            case 3:
                                picked.statuses.AddStatus(Accused.accused, charRef); break;
                            case 4:
                                picked.statuses.AddStatus(Confused.confused, charRef);
                                Confused.updateConfusion(charRef);
                                break;

                        }
                    }
                }
                else
                {
                    fakeOutcast.role.Act(trigger, charRef);
                }
                MelonLogger.Msg(string.Format("Mad Scientist is copying the {0} and {1} abilities", fakeOutcast.name, fakeMinion.name));
            }
            // check if I should turn evil
            /*if (fakeOutcast.characterId == "Renegade_WING" || fakeOutcast.characterId == "Hitman_scm")
            {
                charRef.ChangeAlignment(EAlignment.Evil);
            }
            if (fakeOutcast.characterId == "Mayor_VP")
            {
                Il2CppSystem.Collections.Generic.List<Character> charList = new Il2CppSystem.Collections.Generic.List<Character>(Gameplay.CurrentCharacters.Pointer);
                charList = CharactersHelper.GetSortedListWithCharacterFirst(charList, charRef);

                charList.RemoveAt(0);
                Il2CppSystem.Collections.Generic.List<Character> adjacentEvils = new Il2CppSystem.Collections.Generic.List<Character>();
                if (charList[0].alignment == EAlignment.Evil)
                {
                    adjacentEvils.Add(charList[0]);
                }
                if (charList[charList.Count - 1].alignment == EAlignment.Evil)
                {
                    adjacentEvils.Add(charList[charList.Count - 1]);
                }

                if (adjacentEvils.Count > 0)
                {
                    charRef.ChangeAlignment(EAlignment.Evil);
                }
            }*/
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
            if (fakeOutcast.characterId == "Hitman_scm")
            {
                if (trigger == ETriggerPhase.Night && charRef.state != ECharacterState.Dead)
                {
                    if (!killedLastNight)
                    {
                        Il2CppSystem.Collections.Generic.List<Character> newList = Gameplay.CurrentCharacters;
                        newList = Characters.Instance.FilterAliveCharacters(newList);
                        Il2CppSystem.Collections.Generic.List<Character> validTargets = new();
                        // not gonna have this guy try to kill the Undying or the Mad Scientist with the Undying ability. It causes too many bugs.
                        foreach (Character target in newList)
                        {
                            if (target.dataRef.characterId != "Undying_WING" && !target.statuses.Contains(SpecialMadScientistTags.hasUndyingAbility))
                            {
                                if (!target.statuses.Contains(AvoidingDoubleKills.killed) && !target.statuses.Contains(ECharacterStatus.KilledByEvil))
                                    validTargets.Add(target);
                            }
                        }
                        if (!(newList.Count == 0))
                        {
                            Character myTarget = validTargets[UnityEngine.Random.Range(0, validTargets.Count)];
                            myTarget.statuses.AddStatus(ECharacterStatus.KilledByEvil, charRef);
                            myTarget.statuses.AddStatus(CriminalKill.criminalKill, charRef);
                            myTarget.statuses.AddStatus(AvoidingDoubleKills.killed, charRef);
                            myTarget.statuses.statuses.Remove(ECharacterStatus.UnkillableByDemon);
                            myTarget.KillByDemon(charRef);
                            myTarget.Reveal();
                            myTarget.onReveal.Invoke();
                            myTarget.RevealReal();
                            if (myTarget.dataRef.picking)
                            {
                                myTarget.pickableUses = 0;
                                myTarget.pickable.SetActive(false);
                            }
                        }
                        killedLastNight = true;
                    }
                    else
                    {
                        Health health = PlayerController.PlayerInfo.health;
                        health.Damage(3);
                        killedLastNight = false;
                    }
                }
            } else if (fakeOutcast.characterId == "Gambler_scm")
            {
                if (trigger == ETriggerPhase.Night)
                {
                    Confused.updateConfusion(charRef);
                }
            } else
                fakeOutcast.role.Act(trigger, charRef);
            /*if (fakeMinion.characterId == "Ritualist_WING")
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
            } else */
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
        }/*
        if (fakeOutcast.characterId == "Drunk_15369527")
        {
            return 2;
        }*/
        if (fakeOutcast.characterId == "Ghost_scm")
        {
            return 1;
        }/*
        if (fakeOutcast.characterId == "Renegade_WING" || fakeOutcast.characterId == "Hitman_scm")
        {
            return 0;
        }*/
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
        /*if (fakeOutcast.name == "Wretch")
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
        }*/
        if (fakeOutcast.characterId == "Marionette_WING")
        {
            return ProjectContext.Instance.gameData.GetCharacterDataOfId("Puppet_15989619");
        }
        return ProjectContext.Instance.gameData.GetCharacterDataOfId("MadScientist_scm");
    }/*
    public override CharacterData GetBluffIfAble(Character charRef)
    {
        if (fakeOutcast.characterId == "Drunk_15369527")
        {
            CharacterData bluff = Characters.Instance.GetRandomUniqueVillagerBluff();
            Gameplay.Instance.AddScriptCharacterIfAble(bluff.type, bluff);
            charRef.statuses.AddStatus(ECharacterStatus.Corrupted, charRef);

            return bluff;
        }
        if (fakeOutcast.characterId == "Doppleganger_52694042")
        {
            charRef.statuses.AddStatus(ECharacterStatus.HealthyBluff, charRef);
            Il2CppSystem.Collections.Generic.List<Character> characters = new Il2CppSystem.Collections.Generic.List<Character>();
            foreach (Character c in Gameplay.CurrentCharacters)
            {
                characters.Add(c);
            }
            characters = Characters.Instance.FilterBluffableCharacters(characters);
            characters = Characters.Instance.FilterCharacterType(characters, ECharacterType.Villager);
            characters = Characters.Instance.FilterAlignmentCharacters(characters, EAlignment.Good);
            CharacterData character = characters[UnityEngine.Random.Range(0, characters.Count)].dataRef;

            return character;
        }
        if (fakeOutcast.characterId == "Lunatic_WING")
        {
            int diceRoll2 = Calculator.RollDice(10);
            if (diceRoll2 < 6 && !charRef.statuses.Contains(ECharacterStatus.Corrupted))
            {
                charRef.statuses.AddStatus(ECharacterStatus.HealthyBluff, charRef);
            }
            else
            {
                charRef.statuses.AddStatus(ECharacterStatus.Corrupted, charRef);
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
        // if not one of those don't disguise
        return null;
    }*/
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
{
    public static ECharacterStatus hasGhostAbility = (ECharacterStatus)1201;
    public static ECharacterStatus hasSleeperAbility = (ECharacterStatus)1202;
    public static ECharacterStatus hasUndyingAbility = (ECharacterStatus)1203;
}