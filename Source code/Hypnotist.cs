using System;
using System.ComponentModel.Design;
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
using static MelonLoader.MelonLogger;

namespace RiddlerMod;

[RegisterTypeInIl2Cpp]
public class Hypnotist : Minion
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
        Il2CppSystem.Collections.Generic.List<CharacterData> villagers = gameplay.GetAscensionAllStartingCharacters();

        Il2CppSystem.Collections.Generic.List<CharacterData> listV = new Il2CppSystem.Collections.Generic.List<CharacterData>();
        Il2CppSystem.Collections.Generic.List<string> whitelistCharacterIDs = new Il2CppSystem.Collections.Generic.List<string>();

        whitelistCharacterIDs.Add("Confessor_18741708");
        whitelistCharacterIDs.Add("Baker_22847064");
        int corruptions = 0;
        foreach (Character ch in Gameplay.CurrentCharacters)
        {
            if (ch.statuses.Contains(ECharacterStatus.Corrupted)) corruptions++;
        }
        if (corruptions >= 3)
            whitelistCharacterIDs.Add("Alchemist_94446803");
        Il2CppSystem.Collections.Generic.List<CharacterData> inDeckOutcasts = gameplay.GetScriptCharactersOfType(ECharacterType.Outcast);
        foreach (CharacterData outcast in inDeckOutcasts)
        {
            if (outcast.usuallyDisguised)
            {
                whitelistCharacterIDs.Add("Lookout_41018246");
                break;
            }
        }
        whitelistCharacterIDs.Add("Witness_25155076");
        Il2CppSystem.Collections.Generic.List<Character> chs = Gameplay.CurrentCharacters;
        int evils = 0;
        foreach (Character c in chs)
        {
            if (c.GetRegisterAlignment() == EAlignment.Evil)
                evils++;
        }
        /*
        if (evils >= 4)
        {
            whitelistCharacterIDs.Add("Knitter_32352172");
        }*/

        if (Gameplay.CurrentCharacters.Count >= 6 + evils && evils >= 2)
        {
            whitelistCharacterIDs.Add("Scout_88081716");
        }

        whitelistCharacterIDs.Add("Riddler_scm");
        whitelistCharacterIDs.Add("Sentinel_WING");
        foreach (Character c in Gameplay.CurrentCharacters)
        {
            if (c.dataRef.characterId == "Swarm_Good_WING")
            {
                whitelistCharacterIDs.Add("Swarm_Good_WING");
                break;
            }
        }
        whitelistCharacterIDs.Add("Underling_V_WING"); // will always say "I am Good" if disguised as this
        whitelistCharacterIDs.Add("Monarch_POW");
        whitelistCharacterIDs.Add("WING_Dupery_Priest");
        foreach (Character c in Gameplay.CurrentCharacters)
        {
            if (c.dataRef.characterId == "WING_Dupery_Casanova")
            {
                whitelistCharacterIDs.Add("WING_Dupery_Romantic");
                break;
            }
        }
        for (int i = 0; i < villagers.Count; i++)
        {
            if (whitelistCharacterIDs.Contains(villagers[i].characterId))
                listV.Add(villagers[i]);
        }
        CharacterData bluff = listV[UnityEngine.Random.RandomRangeInt(0, listV.Count)];
        if (bluff.characterId == "Monarch_POW")
        {
            for (int i = 0; i < villagers.Count; i++)
            {
                if (villagers[i].characterId == "Executive_POW")
                    gameplay.AddScriptCharacterIfAble(ECharacterType.Villager, villagers[i]);
            }
        }
        else gameplay.AddScriptCharacterIfAble(bluff.type, bluff);
        charRef.statuses.AddStatus(ECharacterStatus.HealthyBluff, charRef);
        charRef.statuses.AddStatus(ECharacterStatus.BrokenAbility, charRef);
        return bluff;
    }
    public Hypnotist() : base(ClassInjector.DerivedConstructorPointer<Hypnotist>())
    {
        ClassInjector.DerivedConstructorBody((Il2CppObjectBase)this);
    }
    public Hypnotist(System.IntPtr ptr) : base(ptr) { }

    // Vanilla character modification for Hypnotist support
    [HarmonyPatch(typeof(Confessor), nameof(Confessor.GetInfo))]
    private static class HypnotistConfessor
    {
        private static void Postfix(Confessor __instance, Character charRef, ref ActedInfo __result)
        {
            if (charRef.statuses.Contains(Preacher.fakePreacher))
            {
                if (charRef.GetRegisterAlignment() == EAlignment.Evil || charRef.statuses.Contains(ECharacterStatus.Corrupted)) __result = new ActedInfo("I am Good");
                else __result = new ActedInfo("I am dizzy");
            }
            if (charRef.dataRef.characterId != "Hypnotist_scm") return;
            string info = "I am Good";
            __result = new ActedInfo(info);
        }
    }
    [HarmonyPatch(typeof(Alchemist), nameof(Alchemist.GetInfo))]
    private static class HypnotistAlchemist
    {
        private static void Postfix(Alchemist __instance, Character charRef, ref ActedInfo __result)
        {
            if (charRef.dataRef.characterId != "Hypnotist_scm") return;
            string info = __instance.ConjourInfo(3, charRef);

            __result = new ActedInfo(info);
        }
    }
    [HarmonyPatch(typeof(Witness), nameof(Witness.GetInfo))]
    private static class HypnotistWitness
    {
        private static void Postfix(Witness __instance, Character charRef, ref ActedInfo __result)
        {
            if (charRef.dataRef.characterId != "Hypnotist_scm") return;
            string info = __instance.ConjourInfo(null, charRef);

            __result = new ActedInfo(info);
        }
    }
    [HarmonyPatch(typeof(Lookout), nameof(Lookout.GetInfo))]
    private static class HypnotistMedium
    {
        private static void Postfix(Lookout __instance, Character charRef, ref ActedInfo __result)
        {
            if (charRef.dataRef.characterId != "Hypnotist_scm") return;

            Il2CppSystem.Collections.Generic.List<CharacterData> inDeckOutcasts = Gameplay.Instance.GetScriptCharactersOfType(ECharacterType.Outcast);
            Il2CppSystem.Collections.Generic.List<string> disguisingOutcasts = new Il2CppSystem.Collections.Generic.List<string>();
            foreach (CharacterData outcast in inDeckOutcasts)
            {
                if (outcast.usuallyDisguised)
                {
                    disguisingOutcasts.Add(outcast.characterName);
                }
            }

            string fakeDisguisingOutcastName = disguisingOutcasts[UnityEngine.Random.RandomRangeInt(0, disguisingOutcasts.Count)];
            int card = UnityEngine.Random.RandomRangeInt(1, Gameplay.CurrentCharacters.Count + 1);
            string info = string.Format("#{0} is actually a {1}", card, fakeDisguisingOutcastName);
            Il2CppSystem.Collections.Generic.List<Character> hintArrows = new();
            foreach (Character c in Gameplay.CurrentCharacters)
            {
                if (card == c.id)
                {
                    hintArrows.Add(c);
                }
            }
            __result = new ActedInfo(info, hintArrows);
        }
    }
    // Hypnotist, plus also remove the ability for Scout to mention good-registering evils as an evil
    [HarmonyPatch(typeof(Scout), nameof(Scout.GetInfo))]
    private static class HypnotistScout
    {
        private static void Postfix(Scout __instance, Character charRef, ref ActedInfo __result)
        {
            if (charRef.dataRef.characterId != "Hypnotist_scm")
            {
                Il2CppSystem.Collections.Generic.List<Character> allEvils = MainMod.GetGameplayCurrentCharacters();
                allEvils = Characters.Instance.FilterRealAlignmentCharacters(allEvils, EAlignment.Evil);
                allEvils = Characters.Instance.FilterAlignmentCharacters(allEvils, EAlignment.Evil);
                Il2CppSystem.Collections.Generic.List<Character> say = new();
                foreach (Character c in allEvils)
                {
                    if (c.dataRef.startingAlignment == EAlignment.Evil)
                    {
                        say.Add(c);
                    }
                }

                Character pickedEvil = say[UnityEngine.Random.Range(0, say.Count)];

                while (pickedEvil.dataRef.characterId == "Atheist_scm") pickedEvil = say[UnityEngine.Random.Range(0, say.Count)];

                int closestEvil = __instance.GetClosestEvilToEvil(pickedEvil, charRef);

                string info = __instance.ConjourInfo(pickedEvil.GetRegisterAs(), closestEvil, charRef);
                __result = new ActedInfo(info);
            }
            else
            {
                Il2CppSystem.Collections.Generic.List<Character> allEvils = MainMod.GetGameplayCurrentCharacters();
                allEvils = Characters.Instance.FilterRealAlignmentCharacters(allEvils, EAlignment.Evil);
                allEvils = Characters.Instance.FilterAlignmentCharacters(allEvils, EAlignment.Evil);

                Character pickedEvil = allEvils[UnityEngine.Random.Range(0, allEvils.Count)];

                string info = __instance.ConjourInfo(pickedEvil.dataRef, 3, charRef);

                __result = new ActedInfo(info);
            }
        }
    }
}
