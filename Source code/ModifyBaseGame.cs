global using Il2Cpp;
using System;
using System.Data.SqlTypes;
using System.Reflection;
using HarmonyLib;
using Il2CppDissolveExample;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.Injection;
using Il2CppInterop.Runtime.InteropTypes.Arrays;
using Il2CppRewired.UI.ControlMapper;
using Il2CppSystem.IO;
using MelonLoader;
using MelonLoader.Utils;
using RiddlerMod;
using UnityEngine;

namespace RiddlerMod;
public class ModifyBaseGame
{
    //this one is for Mediums to call out "#x is actually a <disguised outcast>" instead of "#x is a real <disguised outcast>"
    [HarmonyPatch(typeof(Lookout), nameof(Lookout.ConjourInfo))]
    private static class MediumDisguised
    {
        private static void Postfix(Lookout __instance, int id, CharacterData ch, Character charef, ref string __result)
        {
            bool surprised = false;
            if (ch.usuallyDisguised)
                surprised = true;

            string info = "";
            if (surprised)
                info += $"#{id} is actually a\n";
            else
                info += $"#{id} is a real\n";
            info += $"{ch.GetCharacterName()}";
            __result = info;
        }
    }
    // and this one is so that lying mediums stop pointing to evils that register as good because it becomes ambiguous whether the medium is truthful or not
    [HarmonyPatch(typeof(Lookout), nameof(Lookout.GetBluffInfo))]
    private static class FixMediums
    {
        private static void Postfix(Lookout __instance, Character charRef, ref ActedInfo __result)
        {
            Il2CppSystem.Collections.Generic.List<Character> allCharacters = MainMod.GetGameplayCurrentCharacters();
            Il2CppSystem.Collections.Generic.List<Character> filteredAllCharacters = new();

            foreach (Character c in allCharacters)
                if (c.bluff != null && !(c.alignment == EAlignment.Evil && c.GetRegisterAlignment() == EAlignment.Good))
                    if (c != charRef)
                        filteredAllCharacters.Add(c);

            if (filteredAllCharacters.Count == 0)
                foreach (Character c in allCharacters)
                    if (c.bluff != null && !(c.alignment == EAlignment.Evil && c.GetRegisterAlignment() == EAlignment.Good))
                        filteredAllCharacters.Add(c);

            Il2CppSystem.Collections.Generic.List<Character> pickedCh = new();
            pickedCh.Add(filteredAllCharacters[UnityEngine.Random.Range(0, filteredAllCharacters.Count)]);

            string info = __instance.ConjourInfo(pickedCh[0].id, pickedCh[0].bluff, charRef);
            ActedInfo newInfo = new ActedInfo(info, pickedCh);
            __result = newInfo;
        }
    }
    [HarmonyPatch(typeof(Scout), nameof(Scout.GetBluffInfo))]
    private static class ScoutFix
    {
        private static void Postfix(Scout __instance, Character charRef, ref ActedInfo __result)
        {
            float randomId = UnityEngine.Random.Range(0f, 1f);
            Il2CppSystem.Collections.Generic.List<Character> allEvils = MainMod.GetGameplayCurrentCharacters();
            allEvils = Characters.Instance.FilterRealAlignmentCharacters(allEvils, EAlignment.Evil);
            allEvils = Characters.Instance.FilterAlignmentCharacters(allEvils, EAlignment.Evil);

            Character pickedEvil = allEvils[UnityEngine.Random.Range(0, allEvils.Count)];

            while (pickedEvil.dataRef.characterId == "Atheist_scm") pickedEvil = allEvils[UnityEngine.Random.Range(0, allEvils.Count)];

            int id = __instance.GetClosestEvilToEvil(pickedEvil, charRef);
            id = Calculator.RemoveNumberAndGetRandomNumberFromList(id, 0, 3);

            string info = __instance.ConjourInfo(pickedEvil.GetRegisterAs(), id, charRef);
            __result = new ActedInfo(info);
        }
    }
    //super janky fix
    [HarmonyPatch(typeof(NightCycle), nameof(NightCycle.ResetClock))]
    private static class BluffsActivationAtNight
    {
        private static void Postfix()
        {
            foreach (Character c in Gameplay.CurrentCharacters)
            {
                if (c.bluff)
                {
                    if (c.bluff.characterId == "Astronaut_scm" || c.bluff.characterId == "Sharpshooter_scm")
                    {
                        if (!c.statuses.Contains(ECharacterStatus.HealthyBluff))
                        {
                            c.bluffRole.BluffAct(ETriggerPhase.Night, c);
                        }
                        else
                        {
                            c.bluffRole.Act(ETriggerPhase.Night, c);
                        }
                    }
                }

            }
        }
    }
    // Update witness descripton
    public static void UpdateWitness()
    {
        CharacterData[] allDatas = System.Array.Empty<CharacterData>();
        var loadedCharList = Resources.FindObjectsOfTypeAll(Il2CppType.Of<CharacterData>());
        if (loadedCharList != null)
        {
            allDatas = new CharacterData[loadedCharList.Length];
            for (int i = 0; i < loadedCharList.Length; i++)
            {
                allDatas[i] = loadedCharList[i]!.Cast<CharacterData>();
            }
        }
        for (int i = 0; i < allDatas.Count(); i++)
        {
            if (allDatas[i].characterName == "Witness")
            {
                allDatas[i].hints += "\n- Demon protected by Guardian" +
                                     "\n- Character targeted by Accuser, Baffler, Mystifier, or Wizard" +
                                     "\n- Characters summoned by Summoner, Kingmaker, or Rainbow Joker" +
                                     "\n- Outcast added or turned evil by Escapist";
            }
        }
    }
    // ban Shaman from duping a Trickster
    [HarmonyPatch(typeof(Illuzionist), nameof(Illuzionist.Act))]
    private static class ShamanTricksterFix
    {
        private static bool Prefix(ETriggerPhase trigger, Character charRef)
        {
            if (trigger != ETriggerPhase.Start) return false;

            Il2CppSystem.Collections.Generic.List<Character> villagers1 = MainMod.GetGameplayCurrentCharacters();
            villagers1 = Characters.Instance.FilterCharacterType(villagers1, ECharacterType.Villager);

            Il2CppSystem.Collections.Generic.List<Character> villagers = new();
            foreach (Character c in villagers1)
            {
                if (c.dataRef.characterId != "Trickster_scm")
                {
                    villagers.Add(c);
                }
            }

            Character pickedVillager = villagers[UnityEngine.Random.Range(0, villagers.Count)];
            pickedVillager.statuses.AddStatus(ECharacterStatus.MessedUpByEvil, charRef);

            villagers.Remove(pickedVillager);
            Character replacedVillager = villagers[UnityEngine.Random.Range(0, villagers.Count)];

            //replacedVillager.InitWithNoReset(pickedVillager.GetCharacterBluffIfAble());
            replacedVillager.Init(pickedVillager.GetCharacterBluffIfAble());

            if (Characters.Instance.CheckIfCharacterShouldStartAct(pickedVillager.GetCharacterBluffIfAble()))
                replacedVillager.Act(ETriggerPhase.Start);

            replacedVillager.statuses.AddStatus(ECharacterStatus.MessedUpByEvil, charRef);
            return false;
        }
    }
    // messing with the disguise functions to give more bluff variety
    [HarmonyPatch(typeof(Characters), nameof(Characters.PickRoundBluffs))]
    public class MoreBluffVariety
    {
        private static bool Prefix(Characters __instance)
        {
            Il2CppSystem.Collections.Generic.List<CharacterData> allCharacters = Gameplay.Instance.GetAscensionAllStartingCharacters();
            Il2CppSystem.Collections.Generic.List<CharacterData> currentCharacters = Gameplay.Instance.GetScriptCharacters();
            Il2CppSystem.Collections.Generic.List<CharacterData> notInPlayCharacters = new();

            __instance.UniquePool.Clear();

            foreach (CharacterData cd in allCharacters)
            {
                if (!currentCharacters.Contains(cd))
                {
                    notInPlayCharacters.Add(cd);
                }
            }
            notInPlayCharacters = __instance.FilterBluffableCharacters(notInPlayCharacters);
            __instance.UniquePool = notInPlayCharacters;

            // Safeguard
            if (__instance.UniquePool.Count < 1)
            {
                notInPlayCharacters = Gameplay.Instance.GetAllAscensionCharacters();
                notInPlayCharacters = __instance.FilterBluffableCharacters(notInPlayCharacters);
                notInPlayCharacters = __instance.FilterAlignmentCharacters(notInPlayCharacters, EAlignment.Good);
                notInPlayCharacters = __instance.FilterRealCharacterType(notInPlayCharacters, ECharacterType.Villager);

                __instance.UniquePool.Add(notInPlayCharacters[UnityEngine.Random.Range(0, notInPlayCharacters.Count)]);
            }
            return false;
        }
    }
    // Minions (and some outcasts) can now disguise as any not in play character, not just one of 4
    [HarmonyPatch(typeof(Characters), nameof(Characters.GetRandomUniqueBluff))]
    public class MoreBluffVariety2
    {
        private static void Postfix(Characters __instance, ref CharacterData __result)
        {
            Il2CppSystem.Collections.Generic.List<CharacterData> allCharacters = Gameplay.Instance.GetAscensionAllStartingCharacters();
            Il2CppSystem.Collections.Generic.List<Character> currentCharacters = Gameplay.CurrentCharacters;
            Il2CppSystem.Collections.Generic.List<CharacterData> currentCharacterDatas = new();
            foreach (Character c in currentCharacters)
            {
                currentCharacterDatas.Add(c.dataRef);
            }
            Il2CppSystem.Collections.Generic.List<CharacterData> notInPlayCharacters = new();


            foreach (CharacterData cd in allCharacters)
            {
                if (!currentCharacterDatas.Contains(cd))
                {
                    notInPlayCharacters.Add(cd);
                }
            }
            notInPlayCharacters = __instance.FilterBluffableCharacters(notInPlayCharacters);
            __result = notInPlayCharacters[UnityEngine.Random.RandomRangeInt(0, notInPlayCharacters.Count)];
        }
    }
    // Demons (and some other outcasts) can now disguise as any not in play villager, not just one of 4
    [HarmonyPatch(typeof(Characters), nameof(Characters.GetRandomUniqueVillagerBluff))]
    public class MoreBluffVariety3
    {
        private static void Postfix(Characters __instance, ref CharacterData __result)
        {
            Il2CppSystem.Collections.Generic.List<CharacterData> allCharacters = Gameplay.Instance.GetAscensionAllStartingCharacters();
            Il2CppSystem.Collections.Generic.List<Character> currentCharacters = Gameplay.CurrentCharacters;
            Il2CppSystem.Collections.Generic.List<CharacterData> currentCharacterDatas = new();
            foreach (Character c in currentCharacters)
            {
                currentCharacterDatas.Add(c.dataRef);
            }
            Il2CppSystem.Collections.Generic.List<CharacterData> notInPlayCharacters = new();


            foreach (CharacterData cd in allCharacters)
            {
                if (!currentCharacterDatas.Contains(cd) && cd.type == ECharacterType.Villager)
                {
                    notInPlayCharacters.Add(cd);
                }
            }
            notInPlayCharacters = __instance.FilterBluffableCharacters(notInPlayCharacters);

            __result = notInPlayCharacters[UnityEngine.Random.RandomRangeInt(0, notInPlayCharacters.Count)];
        }
    }
}