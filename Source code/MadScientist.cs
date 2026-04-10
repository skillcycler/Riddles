using System.Diagnostics;
using HarmonyLib;
using Il2CppInterop.Runtime.Injection;
using Il2CppInterop.Runtime.InteropTypes;
using MelonLoader;
using UnityEngine;
using static MelonLoader.MelonLogger;

namespace RiddlerMod;

[RegisterTypeInIl2Cpp]
public class MadScientist : Role
{
    public CharacterData fakeMinion = GetGenericMinion();
    public CharacterData fakeMinion2 = GetGenericMinion();
    public CharacterData fakeOutcast = GetGenericOutcast();
    public CharacterData fakeOutcast2 = GetGenericOutcast();
    public Character chargedActor = new Character();
    public override ActedInfo GetInfo(Character charRef)
    {
        if (fakeMinion.name == "Minion")
        {
            return new ActedInfo("Something went wrong and I don't have a Minion ability");
        }
        if (fakeOutcast.name == "Doppelganger")
        {
            return new ActedInfo("Something went wrong and I don't have an Outcast ability");
        }
        if (fakeOutcast.name == "Renegade" || fakeOutcast.name == "Hitman")
        {
            return new ActedInfo(string.Format("I have the {0} and {1} abilities", fakeMinion2.name, fakeOutcast2.name));
        }
        return new ActedInfo(string.Format("I have the {0} and {1} abilities", fakeMinion.name, fakeOutcast.name));
    }
    public override ActedInfo GetBluffInfo(Character charRef)
    {
        return new ActedInfo("This statement should never appear");
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
            whitelistMinionCharacterIDs.Add("Baron_04539999");
            whitelistOutcastCharacterIDs.Add("Plague Doctor_49312486");
            whitelistOutcastCharacterIDs.Add("Wretch_80988916");
            whitelistOutcastCharacterIDs.Add("Bombardier_79093372");
            whitelistOutcastCharacterIDs.Add("Drunk_15369527");
            whitelistOutcastCharacterIDs.Add("Doppleganger_52694042");
            // riddler
            whitelistMinionCharacterIDs.Add("Accuser");
            whitelistMinionCharacterIDs.Add("Apprentice");
            whitelistMinionCharacterIDs.Add("Ghost");
            whitelistMinionCharacterIDs.Add("Hitman");
            // Wingidon
            whitelistMinionCharacterIDs.Add("Saboteur_WING");
            whitelistMinionCharacterIDs.Add("Undying_WING");
            whitelistMinionCharacterIDs.Add("Swarm_Good_WING");
            
            whitelistOutcastCharacterIDs.Add("Chatterbox_WING");
            whitelistOutcastCharacterIDs.Add("Revolutionary_WING");
            whitelistOutcastCharacterIDs.Add("Marionette_WING");
            whitelistOutcastCharacterIDs.Add("Renegade_WING");
            whitelistOutcastCharacterIDs.Add("Lunatic_WING");

            // Carlz
            //whitelistMinionCharacterIDs.Add("Lycaon_VP"); This has been causing too many bugs
            whitelistMinionCharacterIDs.Add("Blackmailer_VP");
            whitelistOutcastCharacterIDs.Add("Rook_VP");
            whitelistOutcastCharacterIDs.Add("Mayor_VP");
            // Mass Hysteria
            whitelistMinionCharacterIDs.Add("Siren_MaHy");
            whitelistOutcastCharacterIDs.Add("Magician_MaHy");


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
            int s2 = UnityEngine.Random.RandomRangeInt(0, listOut.Count);
            while ((s1 == s2 && listOut.Count > 1) || listOut[s2].name == "Hitman" || listOut[s2].name == "Renegade")
            {
                s2 = UnityEngine.Random.RandomRangeInt(0, listOut.Count);
            }
            fakeOutcast = listOut[s1];
            fakeOutcast2 = listOut[s2]; // Never actually adds this. It's for the rare case when it needs to lie
            gameplay.AddScriptCharacter(ECharacterType.Minion, fakeMinion);
            gameplay.AddScriptCharacter(ECharacterType.Outcast, fakeOutcast);

            fakeMinion2 = listMin[r2];
            if (UnityEngine.Random.RandomRangeInt(0, 2) == 0 || fakeOutcast.name == "Renegade" || fakeOutcast.name == "Hitman")
            {
                gameplay.AddScriptCharacter(ECharacterType.Minion, fakeMinion2);
            }
            if (charRef.GetCharacterData().name == "Mad Scientist")
            {
                fakeMinion.role.Act(trigger, charRef);
                fakeOutcast.role.Act(trigger, charRef);
            }
            // check if I should turn evil
            if (fakeOutcast.characterId == "Renegade_WING" || fakeOutcast.characterId == "Criminal")
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
            }
            MelonLogger.Msg(string.Format("I have the {0} and {1} abilities", fakeMinion.name, fakeOutcast.name));
        }
        if (trigger == ETriggerPhase.Day)
        {
            onActed.Invoke(GetInfo(charRef));
        }
        if (charRef.GetCharacterData().name == "Mad Scientist" && trigger != ETriggerPhase.Start)
        {
            fakeMinion.role.Act(trigger, charRef);
            fakeOutcast.role.Act(trigger, charRef);
        }
    }
    public override bool CheckIfCanBeKilled(Character charRef)
    {
        return fakeMinion.role.CheckIfCanBeKilled(charRef) && fakeOutcast.role.CheckIfCanBeKilled(charRef);
    }
    public override void ActOnDied(Character charRef)
    {
        if (charRef.GetCharacterData().name == "Mad Scientist")
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
        if (fakeOutcast.characterId == "Drunk_15369527")
        {
            return 2;
        }
        if (fakeOutcast.characterId == "Ghost")
        {
            return 1;
        }
        if (fakeOutcast.characterId == "Renegade_WING" || fakeOutcast.characterId == "Criminal")
        {
            return 0;
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
            Recluse wretch = new Recluse();
            return wretch.GetRegisterAsRole(charRef);
        }
        if (fakeOutcast.characterId == "Marionette_WING")
        {
            return ProjectContext.Instance.gameData.GetCharacterDataOfId("Puppet_15989619");
        }
        return ProjectContext.Instance.gameData.GetCharacterDataOfId("MadScientist");
    }
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
    }
}