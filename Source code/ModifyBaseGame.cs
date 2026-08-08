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
using Il2CppTMPro;
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
                    if (Djinn.GetNightlyInfoActors().Contains(c.bluff.characterId))
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
                allDatas[i].hints += "\n- Character protected by Guardian" +
                                     "\n- Character targeted by Accuser, Baffler, Mystifier, or Wizard" +
                                     "\n- Characters summoned by Summoner, Kingmaker, or Rainbow Joker" +
                                     "\n- Outcast added or turned evil by Escapist" +
                                     "\n- Character changed by Pit Hag";
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
    // Kingmaker hides evil counter
    // Must modify after all other mods, since Atheist does things
    [HarmonyPatch(typeof(ObjectivesUI), nameof(ObjectivesUI.UpdateObjectives))]
    [HarmonyPriority(Priority.Last)]
    public static class ChangeCounter
    {
        public static void Postfix(ObjectivesUI __instance)
        {
            bool Kingmaker = false;
            bool Atheist = false;
            foreach (Character c in Gameplay.CurrentCharacters)
            {
                if (c.dataRef.characterId == "Kingmaker_scm")
                {
                    Kingmaker = true;
                }
                if (c.dataRef.characterId == "Atheist_scm")
                {
                    Atheist = true;
                }
            }
            if (!Kingmaker && !Atheist) return;
            int minions = Gameplay.CurrentScript.minion;
            int demons = Gameplay.CurrentScript.demon;
            var deadCharacters = Gameplay.DeadCharacters;
            int EvilsKilled = 0;

            foreach (var deadCharacter in deadCharacters)
            {
                if (deadCharacter.alignment == EAlignment.Evil)
                {
                    EvilsKilled++;
                }
            }
            if (Atheist)
            {
                __instance.evilsKilled.text = string.Format("<color=grey>Evils killed:</color> <color=red>?");
            }
            else
            {
                __instance.evilsKilled.text = string.Format("<color=grey>Evils killed:</color> <color=red>{0}", EvilsKilled);
            }


            string minionCountText = "Minions";
            if (minions == 1)
            {
                minionCountText = "Minion";
            }
            string demonCountText = "Demons";
            if (demons == 1)
            {
                demonCountText = "Demon";
            }
            __instance.objective.text = string.Format("Find and Execute all Evil Characters<br><color=grey><size=18>(<color=orange>{0}+ {2}</color> and <color=red>{1}+ {3} </color>)", minions, demons, minionCountText, demonCountText);
            if (Atheist)
            {
                __instance.objective.text = "Find and Execute all Evil Characters.";
                var texts = __instance.GetComponentsInChildren<TMP_Text>(true);

                foreach (var text in texts)
                {
                    if (text == null)
                        continue;

                    if (text.text != null && text.text.Contains("Score:"))
                    {
                        text.text = "<size=20><color=grey>Score: <color=green><size=24>?";
                    }
                }
            }
        }
    }
    public static void MakeTwelve()
    {
        Transform chars = GameObject.Find("Game/Gameplay/Content/Canvas/Panel/Characters").transform;
        for (int i = 12; i < 15; i++)
        {
            checkCreateCircle(chars, i);
        }
        checkCreateCircle(chars, 21);
    }
    public static void checkCreateCircle(Transform parent, int size)
    {
        string name = "Circle_" + size;
        Transform t = parent.FindChild(name);
        if (t != null)
        {
            MelonLogger.Msg("Object Already exists!: " + name);
            return;
        }
        CreateCircle(size);
    }
    public static GameObject CreateCircle(int size)
    {
        GameObject circle = new GameObject();
        circle.name = "Circle_" + size;
        circle.transform.SetParent(Characters.Instance.gameObject.transform);
        RectTransform rt = circle.AddComponent<RectTransform>();
        CharactersPool cp = circle.AddComponent<CharactersPool>();
        GameObject gameObject = Characters.Instance.gameObject.transform.Find("Circle_6").gameObject;
        CharactersPool component = gameObject.GetComponent<CharactersPool>();
        cp.characterPrefab = component.characterPrefab;
        cp.characters = Array.Empty<Character>();
        cp.cardPlaceHolders = new CardPlaceholder[size];
        for (int i = 0; i < size; i++)
        {
            GameObject card = new GameObject();
            card.transform.SetParent(circle.transform);
            string text = "CardPlaceholder";
            if (i > 0)
            {
                text = text + " (" + i + ")";
            }
            card.name = text;
            RectTransform card_rt = card.AddComponent<RectTransform>();
            card_rt.anchoredPosition3D = new Vector3(0f, 0f, 0f);
            CardPlaceholder cardPlaceholder = card.AddComponent<CardPlaceholder>();
            int num = i * 360 / size;
            if (num <= 30)
            {
                cardPlaceholder.actedSide = EActedSide.Down;
            }
            else if (num <= 149)
            {
                cardPlaceholder.actedSide = EActedSide.Left;
            }
            else if (num <= 210)
            {
                cardPlaceholder.actedSide = EActedSide.Up;
            }
            else if (num <= 329)
            {
                cardPlaceholder.actedSide = EActedSide.Right;
            }
            else
            {
                cardPlaceholder.actedSide = EActedSide.Down;
            }
            cp.cardPlaceHolders[i] = cardPlaceholder;
        }
        circle.transform.position = new Vector3(0f, 1f, 85.9444f);
        circle.transform.localScale = new Vector3(1f, 1f, 1f);
        circle.SetActive(false);
        addToCharsPool(cp);
        return circle;
    }
    public static void addToCharsPool(CharactersPool pool)
    {
        CharactersPool[] oldpool = Characters.Instance.characterPool;
        CharactersPool[] newPool = new CharactersPool[oldpool.Length + 1];
        for (int i = 0; i < oldpool.Length; i++)
        {
            newPool[i] = oldpool[i];
        }
        newPool[oldpool.Length] = pool;
        Characters.Instance.characterPool = newPool;
    }
    [HarmonyPatch(typeof(TextTooltipRecognizer), "GetTooltipInfo")]
    public static class TooltipPatch
    {
        static void Postfix(string linkID, ref TooltipInfo __result)
        {
            if (linkID == "Accused")
            {
                __result = new TooltipInfo(
                    "Accused characters register as a random Evil character.",
                    "Accused",
                    new Color32(255, 128, 0, 255)
                );
            }
            if (linkID == "Erased")
            {
                __result = new TooltipInfo(
                    "Erased characters register as no Alignment and no Type.",
                    "Erased",
                    new Color32(187, 102, 102, 255)
                );
            }
            if (linkID == "Confused")
            {
                __result = new TooltipInfo(
                    "Confused characters have a 50% chance of Lying. Lying Confused characters register as Corrupted.",
                    "Confused",
                    new Color32(187, 102, 102, 255)
                );
            }
        }
    }
    public static string PatchTooltip(string value)
    {
        if (value != null)
        {
            if (value.Contains("Accused"))
            {
                value = value.Replace(
                    "Accused",
                    "<link=\"Accused\"><color=#FF8000>Accused</color></link>"
                );
            }
            if (value.Contains("Erased"))
            {
                value = value.Replace(
                    "Erased",
                    "<link=\"Erased\"><color=#BB6666>Erased</color></link>"
                );
            }
            if (value.Contains("Confused"))
            {
                value = value.Replace(
                    "Confused",
                    "<link=\"Confused\"><color=#DDDD00>Confused</color></link>"
                );
            }
        }
        return value;
    }
    public static void DisableRedText()
    {
        GameObject[] objects = Resources.FindObjectsOfTypeAll<GameObject>();

        foreach (GameObject obj in objects)
        {
            if (obj != null && obj.name == "FloatingScore")
            {
                obj.SetActive(false);
            }
        }
    }
    [HarmonyPatch(typeof(DisguiseIcon), nameof(DisguiseIcon.OnEnable))]
    public static class HideDisguiseIconPatch
    {
        public static void Postfix(DisguiseIcon __instance)
        {
            if (__instance != null)
            {
                __instance.gameObject.SetActive(false);
            }
        }
    }
}