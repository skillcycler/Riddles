using HarmonyLib;
using Il2Cpp;
using Il2CppFIMSpace.Basics;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.Injection;
using Il2CppInterop.Runtime.InteropTypes;
using Il2CppSystem;
using Il2CppSystem.Collections.Generic;
using MelonLoader;
using RiddlerMod;
using UnityEngine;
using static Il2CppSystem.Globalization.HebrewNumber;
using static MelonLoader.MelonLogger;

namespace RiddlerMod;

[RegisterTypeInIl2Cpp]
public class Captivator : Role
{
    public CharacterData[] allDatas = Il2CppSystem.Array.Empty<CharacterData>();
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

    }
    public override CharacterData GetBluffIfAble(Character charRef)
    {
        Gameplay gameplay = Gameplay.Instance;
        Characters instance = Characters.Instance;
        Il2CppSystem.Collections.Generic.List<CharacterData> chars = gameplay.GetAscensionAllStartingCharacters();
        Il2CppSystem.Collections.Generic.List<CharacterData> villagers = instance.FilterRealCharacterType(chars, ECharacterType.Villager);

        Il2CppSystem.Collections.Generic.List<CharacterData> listV = new Il2CppSystem.Collections.Generic.List<CharacterData>();
        Il2CppSystem.Collections.Generic.List<string> whitelistCharacterIDs = new Il2CppSystem.Collections.Generic.List<string>();

        int scriptMinions = 0;
        foreach (CharacterData d in gameplay.GetScriptCharactersOfType(ECharacterType.Minion))
        {
            scriptMinions++;
        }
        if (scriptMinions >= 2)
            whitelistCharacterIDs.Add("Oracle_07039445");
        int nonVillagers = 0;
        foreach (Character c in Gameplay.CurrentCharacters)
        {
            if (c.GetRegisterAs().type != ECharacterType.Villager)
                nonVillagers++;
        }
        if (nonVillagers >= 2)
            whitelistCharacterIDs.Add("Bishop_58855542");
        int evils = 0;
        foreach (Character c in Gameplay.CurrentCharacters)
        {
            if (c.GetRegisterAlignment() == EAlignment.Evil)
                evils++;
        }
        if (evils >= 3)
        {
            whitelistCharacterIDs.Add("Empress_13782227");
        }
        if (evils >= 2)
        {
            whitelistCharacterIDs.Add("Chiromancer_WING");
        }
        /*foreach (Character c in Characters.Instance.GetAdjacentCharacters(charRef))
        {
            if (c.alignment == EAlignment.Good)
            {
                whitelistCharacterIDs.Add("Lawyer_scm");
                break;
            }
        }*/
        int bluffs = 0;
        foreach (Character c in Gameplay.CurrentCharacters)
        {
            if (c.bluff)
            {
                bluffs++;
            }
        }
        if (bluffs >= 2)
        {
            whitelistCharacterIDs.Add("Prince_WING");
        }
        whitelistCharacterIDs.Add("Surveyor_scm");
        int corrupted = 0;
        foreach (Character c in Gameplay.CurrentCharacters)
        {
            if (c.statuses.Contains(ECharacterStatus.Corrupted)) corrupted++;
        }
        if (corrupted >= 2)
            whitelistCharacterIDs.Add("Sentinel_WING");
        whitelistCharacterIDs.Add("Knave_WING");
        whitelistCharacterIDs.Add("Politician_WING");
        whitelistCharacterIDs.Add("Puzzlemaster_WING");
        whitelistCharacterIDs.Add("WING_Dupery_Mailman");
        for (int i = 0; i < villagers.Count; i++)
        {
            if (whitelistCharacterIDs.Contains(villagers[i].characterId))
                listV.Add(villagers[i]);
        }
        CharacterData bluff = listV[UnityEngine.Random.RandomRangeInt(0, listV.Count)];
        gameplay.AddScriptCharacterIfAble(ECharacterType.Villager, bluff);
        charRef.statuses.AddStatus(ECharacterStatus.BrokenAbility, charRef);
        return bluff;
    }
    public override int GetDamageToYou()
    {
        return 2;
    }
    public Captivator() : base(ClassInjector.DerivedConstructorPointer<Captivator>())
    {
        ClassInjector.DerivedConstructorBody((Il2CppObjectBase)this);
    }
    public Captivator(System.IntPtr ptr) : base(ptr) { }

    // Captivator stuff. Gotta patch both truth and lying info just in case
    [HarmonyPatch(typeof(Investigator), nameof(Investigator.GetInfo))]
    private static class CaptivatorOracleTruth
    {
        private static void Postfix(Investigator __instance, Character charRef, ref ActedInfo __result)
        {
            if (charRef.dataRef.characterId != "Captivator_scm") return;
            Il2CppSystem.Collections.Generic.List<Character> pickedCharacters = new();

            Il2CppSystem.Collections.Generic.List<Character> evils = MainMod.GetGameplayCurrentCharacters();
            evils = Characters.Instance.FilterAlignmentCharacters(evils, EAlignment.Evil);
            string info = "";

            Il2CppSystem.Collections.Generic.List<Character> other = new();

            Character evil = evils[UnityEngine.Random.Range(0, evils.Count)];

            pickedCharacters.Add(evil);
            foreach (Character character in MainMod.GetGameplayCurrentCharacters())
            {
                if (character.id != evil.id)
                    other.Add(character);
            }
            pickedCharacters.Add(other[UnityEngine.Random.Range(0, other.Count)]);
            Il2CppSystem.Collections.Generic.List<int> pickedIds = new();
            foreach (Character character in pickedCharacters)
            {
                pickedIds.Add(character.id);
            }
            pickedIds.Sort();
            CharacterData cd = new();
            Il2CppSystem.Collections.Generic.List<CharacterData> minions = Gameplay.Instance.GetScriptCharactersOfType(ECharacterType.Minion);
            if (pickedCharacters[0].GetRegisterAlignment() == EAlignment.Evil && pickedCharacters[1].GetRegisterAlignment() == EAlignment.Evil)
            {
                cd = minions[UnityEngine.Random.Range(0, minions.Count)];
            }
            else if (pickedCharacters[0].GetRegisterAlignment() == EAlignment.Evil)
            {
                string evilName = pickedCharacters[0].GetRegisterAs().characterName;
                do
                {
                    cd = minions[UnityEngine.Random.Range(0, minions.Count)];
                } while (cd.characterName == evilName);
            }
            else
            {
                string evilName = pickedCharacters[1].GetRegisterAs().characterName;
                do
                {
                    cd = minions[UnityEngine.Random.Range(0, minions.Count)];
                } while (cd.characterName == evilName);
            }
            info = __instance.ConjourInfo(pickedIds[0], pickedIds[1], cd, charRef);
            __result = new ActedInfo(info, pickedCharacters);
        }
    }
    [HarmonyPatch(typeof(Investigator), nameof(Investigator.GetBluffInfo))]
    private static class CaptivatorOracleLie
    {
        private static void Postfix(Investigator __instance, Character charRef, ref ActedInfo __result)
        {
            if (charRef.dataRef.characterId != "Captivator_scm") return;
            Il2CppSystem.Collections.Generic.List<Character> pickedCharacters = new();

            Il2CppSystem.Collections.Generic.List<Character> evils = MainMod.GetGameplayCurrentCharacters();
            evils = Characters.Instance.FilterAlignmentCharacters(evils, EAlignment.Evil);
            string info = "";

            Il2CppSystem.Collections.Generic.List<Character> other = new();

            Character evil = evils[UnityEngine.Random.Range(0, evils.Count)];

            pickedCharacters.Add(evil);
            foreach (Character character in MainMod.GetGameplayCurrentCharacters())
            {
                if (character.id != evil.id)
                    other.Add(character);
            }
            pickedCharacters.Add(other[UnityEngine.Random.Range(0, other.Count)]);
            Il2CppSystem.Collections.Generic.List<int> pickedIds = new();
            foreach (Character character in pickedCharacters)
            {
                pickedIds.Add(character.id);
            }
            pickedIds.Sort();
            CharacterData cd = new();
            Il2CppSystem.Collections.Generic.List<CharacterData> minions = Gameplay.Instance.GetScriptCharactersOfType(ECharacterType.Minion);
            if (pickedCharacters[0].GetRegisterAlignment() == EAlignment.Evil && pickedCharacters[1].GetRegisterAlignment() == EAlignment.Evil)
            {
                cd = minions[UnityEngine.Random.Range(0, minions.Count)];
            }
            else if (pickedCharacters[0].GetRegisterAlignment() == EAlignment.Evil)
            {
                string evilName = pickedCharacters[0].GetRegisterAs().characterName;
                do
                {
                    cd = minions[UnityEngine.Random.Range(0, minions.Count)];
                } while (cd.characterName == evilName);
            }
            else
            {
                string evilName = pickedCharacters[1].GetRegisterAs().characterName;
                do
                {
                    cd = minions[UnityEngine.Random.Range(0, minions.Count)];
                } while (cd.characterName == evilName);
            }
            info = __instance.ConjourInfo(pickedIds[0], pickedIds[1], cd, charRef);
            __result = new ActedInfo(info, pickedCharacters);
        }
    }
    [HarmonyPatch(typeof(Noble), nameof(Noble.GetInfo))]
    private static class CaptivatorEmpress1
    {
        private static void Postfix(Noble __instance, Character charRef, ref ActedInfo __result)
        {
            if (charRef.dataRef.characterId != "Captivator_scm") return;
            Il2CppSystem.Collections.Generic.List<Character> picked = new();
            Il2CppSystem.Collections.Generic.List<Character> chars = MainMod.GetGameplayCurrentCharacters();
            Il2CppSystem.Collections.Generic.List<Character> evil = new();
            Il2CppSystem.Collections.Generic.List<Character> good = new();
            foreach (Character c in chars)
            {
                if (c.GetRegisterAlignment() == EAlignment.Good)
                {
                    good.Add(c);
                }
                else
                {
                    evil.Add(c);
                }
            }

            Character pick = evil[UnityEngine.Random.Range(0, evil.Count)];
            picked.Add(pick);
            evil.Remove(pick);
            pick = evil[UnityEngine.Random.Range(0, evil.Count)];
            picked.Add(pick);
            evil.Remove(pick);
            if (evil.Count > 0 && Calculator.RollDice(2) == 1)
            {
                pick = evil[UnityEngine.Random.Range(0, evil.Count)];
                picked.Add(pick);
            }
            else if (good.Count > 0)
            {
                pick = good[UnityEngine.Random.Range(0, good.Count)];
                picked.Add(pick);
            }
            else
            {
                pick = evil[UnityEngine.Random.Range(0, evil.Count)];
                picked.Add(pick);
            }
            Il2CppSystem.Collections.Generic.List<int> pickedIds = new();
            foreach (Character character in picked)
            {
                pickedIds.Add(character.id);
            }
            pickedIds.Sort();
            string info = __instance.ConjourInfo(pickedIds[0], pickedIds[1], pickedIds[2], charRef);
            __result = new ActedInfo(info, picked);
        }
    }
    [HarmonyPatch(typeof(Noble), nameof(Noble.GetBluffInfo))]
    private static class CaptivatorEmpress2
    {
        private static void Postfix(Noble __instance, Character charRef, ref ActedInfo __result)
        {
            if (charRef.dataRef.characterId != "Captivator_scm") return;
            Il2CppSystem.Collections.Generic.List<Character> picked = new();
            Il2CppSystem.Collections.Generic.List<Character> chars = MainMod.GetGameplayCurrentCharacters();
            Il2CppSystem.Collections.Generic.List<Character> evil = new();
            Il2CppSystem.Collections.Generic.List<Character> good = new();
            foreach (Character c in chars)
            {
                if (c.GetRegisterAlignment() == EAlignment.Good)
                {
                    good.Add(c);
                }
                else
                {
                    evil.Add(c);
                }
            }

            Character pick = evil[UnityEngine.Random.Range(0, evil.Count)];
            picked.Add(pick);
            evil.Remove(pick);
            pick = evil[UnityEngine.Random.Range(0, evil.Count)];
            picked.Add(pick);
            evil.Remove(pick);
            if (evil.Count > 0 && Calculator.RollDice(2) == 1)
            {
                pick = evil[UnityEngine.Random.Range(0, evil.Count)];
                picked.Add(pick);
            }
            else if (good.Count > 0)
            {
                pick = good[UnityEngine.Random.Range(0, good.Count)];
                picked.Add(pick);
            }
            else
            {
                pick = evil[UnityEngine.Random.Range(0, evil.Count)];
                picked.Add(pick);
            }
            Il2CppSystem.Collections.Generic.List<int> pickedIds = new();
            foreach (Character character in picked)
            {
                pickedIds.Add(character.id);
            }
            pickedIds.Sort();
            string info = __instance.ConjourInfo(pickedIds[0], pickedIds[1], pickedIds[2], charRef);
            __result = new ActedInfo(info, picked);
        }
    }
    public static bool checkCaptivatorBishopInfo(Il2CppSystem.Collections.Generic.List<Character> chars)
    {
        int villagers = 0;
        int outcasts = 0;
        int evils = 0;
        foreach (Character c in chars)
        {
            switch (c.GetCharacterType())
            {
                case ECharacterType.Villager:
                    villagers++;
                    break;
                case ECharacterType.Outcast:
                    outcasts++;
                    break;
                case ECharacterType.Minion:
                    evils++;
                    break;
                case ECharacterType.Demon:
                    evils++;
                    break;
            }
        }
        return (evils >= 2 || outcasts >= 2 || villagers == 2 || villagers == 0);
    }

    [HarmonyPatch(typeof(Bishop), nameof(Bishop.GetInfo))]
    private static class CaptivatorBishop1
    {
        private static void Postfix(Bishop __instance, Character charRef, ref ActedInfo __result)
        {
            if (charRef.dataRef.characterId != "Captivator_scm") return;
            Il2CppSystem.Collections.Generic.List<Character> pickedCharacters = new();
            bool isValid = false;
            Il2CppSystem.Collections.Generic.List<Character> allCharacters = MainMod.GetGameplayCurrentCharacters();
            do
            {
                pickedCharacters = new();
                int a = UnityEngine.Random.Range(0, allCharacters.Count);
                int b = UnityEngine.Random.Range(0, allCharacters.Count);
                while (a == b) b = UnityEngine.Random.Range(0, allCharacters.Count);
                int c = UnityEngine.Random.Range(0, allCharacters.Count);
                while (a == c || b == c) c = UnityEngine.Random.Range(0, allCharacters.Count);
                pickedCharacters.Add(allCharacters[a]);
                pickedCharacters.Add(allCharacters[b]);
                pickedCharacters.Add(allCharacters[c]);

                isValid = checkCaptivatorBishopInfo(pickedCharacters);
            } while (!isValid);

            Il2CppSystem.Collections.Generic.List<int> ids = new Il2CppSystem.Collections.Generic.List<int>();
            foreach (Character c in pickedCharacters)
                ids.Add(c.id);
            ids.Sort();
            Il2CppSystem.Collections.Generic.List<ECharacterType> possiblePicks = new();
            bool minion = false;
            bool outcast = false;
            foreach (Character ch in Gameplay.CurrentCharacters)
            {
                if (ch.GetCharacterType() == ECharacterType.Outcast) outcast = true;
                if (ch.GetCharacterType() == ECharacterType.Minion) minion = true;
            }
            if (minion)
                possiblePicks.Add(ECharacterType.Minion);
            else
                possiblePicks.Add(ECharacterType.Demon);

            if (outcast)
                possiblePicks.Add(ECharacterType.Outcast);
            if (Gameplay.CurrentScript.town > 0)
                possiblePicks.Add(ECharacterType.Villager);

            pickedCharacters = ListHelper.ShuffleList(pickedCharacters);

            Il2CppSystem.Collections.Generic.List<ECharacterType> types = new();
            foreach (ECharacterType ct in possiblePicks)
                types.Add(ct);

            string info = __instance.ConjourInfo(ids, types, charRef);
            __result = new ActedInfo(info, pickedCharacters);
        }
    }
    [HarmonyPatch(typeof(Bishop), nameof(Bishop.GetBluffInfo))]
    private static class CaptivatorBishop2
    {
        private static void Postfix(Bishop __instance, Character charRef, ref ActedInfo __result)
        {
            if (charRef.dataRef.characterId != "Captivator_scm") return;
            Il2CppSystem.Collections.Generic.List<Character> pickedCharacters = new();
            bool isValid = false;
            Il2CppSystem.Collections.Generic.List<Character> allCharacters = MainMod.GetGameplayCurrentCharacters();
            do
            {
                pickedCharacters = new();
                int a = UnityEngine.Random.Range(0, allCharacters.Count);
                int b = UnityEngine.Random.Range(0, allCharacters.Count);
                while (a == b) b = UnityEngine.Random.Range(0, allCharacters.Count);
                int c = UnityEngine.Random.Range(0, allCharacters.Count);
                while (a == c || b == c) c = UnityEngine.Random.Range(0, allCharacters.Count);
                pickedCharacters.Add(allCharacters[a]);
                pickedCharacters.Add(allCharacters[b]);
                pickedCharacters.Add(allCharacters[c]);

                isValid = checkCaptivatorBishopInfo(pickedCharacters);
            } while (!isValid);

            Il2CppSystem.Collections.Generic.List<int> ids = new Il2CppSystem.Collections.Generic.List<int>();
            foreach (Character c in pickedCharacters)
                ids.Add(c.id);
            ids.Sort();
            Il2CppSystem.Collections.Generic.List<ECharacterType> possiblePicks = new();

            bool minion = false;
            bool outcast = false;
            foreach (Character ch in Gameplay.CurrentCharacters)
            {
                if (ch.GetCharacterType() == ECharacterType.Outcast) outcast = true;
                if (ch.GetCharacterType() == ECharacterType.Minion) minion = true;
            }
            if (minion)
                possiblePicks.Add(ECharacterType.Minion);
            else
                possiblePicks.Add(ECharacterType.Demon);

            if (outcast)
                possiblePicks.Add(ECharacterType.Outcast);
            if (Gameplay.CurrentScript.town > 0)
                possiblePicks.Add(ECharacterType.Villager);


            pickedCharacters = ListHelper.ShuffleList(pickedCharacters);

            Il2CppSystem.Collections.Generic.List<ECharacterType> types = new();
            foreach (ECharacterType ct in possiblePicks)
                types.Add(ct);

            string info = __instance.ConjourInfo(ids, types, charRef);
            __result = new ActedInfo(info, pickedCharacters);
        }
    }
}
