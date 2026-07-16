using System;
using System.Linq;
using System.Reflection;
using Il2Cpp;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.Injection;
using Il2CppInterop.Runtime.InteropTypes;
using Il2CppSystem.Reflection;
using MelonLoader;
using UnityEngine;
using static Il2CppSystem.Collections.SortedList;
using static MelonLoader.Modules.MelonModule;
using HarmonyLib;

namespace RiddlerMod;

[RegisterTypeInIl2Cpp]
public class Trickster : Role
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
        if (charRef.statuses.Contains(ECharacterStatus.Corrupted))
        {
            return new ActedInfo("I feel sick.");
        }
        if (charRef.dataRef.characterId != "Trickster_scm")
        {
            return new ActedInfo(string.Format("I am actually a {1}", charRef.id, charRef.dataRef.name));
        }
        int converted = 0;
        foreach (Character ch in Gameplay.CurrentCharacters)
        {
            if (ch.dataRef.characterId == "Trickster_scm")
                converted++;
        }
        if (converted < 3)
            return new ActedInfo("This village has too few Villagers! I can't perform my tricks here!");
        Il2CppSystem.Collections.Generic.List<Character> characters = Gameplay.CurrentCharacters;
        // time for a more robust way of dealing with this
        ECharacterType ctype = ECharacterType.Villager;
        if (charRef.statuses.Contains(TricksterRegister.Minion)) ctype = ECharacterType.Minion;
        if (charRef.statuses.Contains(TricksterRegister.Outcast)) ctype = ECharacterType.Outcast;
        characters = Characters.Instance.FilterCharacterType(characters, ctype);
        if (characters.Count > 1)
        {
            characters.Remove(charRef);
        }
        Character chosen = characters[UnityEngine.Random.RandomRangeInt(0, characters.Count)];
        string info = string.Format("#{0} is my Type", chosen.id);
        Il2CppSystem.Collections.Generic.List<Character> hint = new();
        hint.Add(chosen);
        ActedInfo actedInfo = new ActedInfo(info, hint);
        return actedInfo;
    }

    public override ActedInfo GetBluffInfo(Character charRef)
    {
        return new ActedInfo("I am dizzy");
    }

    public override void Act(ETriggerPhase trigger, Character charRef)
    {
        if (trigger == ETriggerPhase.Day)
        {
            onActed.Invoke(GetInfo(charRef));
        }
        if (charRef.statuses.Contains(ECharacterStatus.BrokenAbility) || charRef.statuses.Contains(TricksterRegister.Outcast) || charRef.statuses.Contains(TricksterRegister.Minion))
            return;
        if (trigger == ETriggerPhase.Start && !charRef.statuses.Contains(TricksterRegister.NotBugged))
        {
            // first check to make sure there aren't extra tricksters
            int tricksters = 0;
            foreach (Character c in Gameplay.CurrentCharacters)
            {
                if (c.dataRef.characterId == "Trickster_scm") tricksters++;
            }
            MelonLogger.Msg($"I am the original Trickster #{charRef.id}");
            charRef.statuses.AddStatus(TricksterRegister.NotBugged, charRef);
            Il2CppSystem.Collections.Generic.List<Character> converts = Gameplay.CurrentCharacters;
            converts = Characters.Instance.FilterRealCharacterType(converts, ECharacterType.Villager);
            converts.Remove(charRef);
            charRef.statuses.AddStatus(TricksterRegister.Villager, charRef);
            if (converts.Count > 1) {
                if (tricksters == 1)
                {
                    int c1 = UnityEngine.Random.RandomRangeInt(0, converts.Count);
                    converts[c1].statuses.AddStatus(ECharacterStatus.BrokenAbility, charRef);
                    converts[c1].Init(charRef.dataRef);
                    converts[c1].statuses.AddStatus(TricksterRegister.Outcast, charRef);
                    int c2 = UnityEngine.Random.RandomRangeInt(0, converts.Count);
                    while (c1 == c2)
                    {
                        c2 = UnityEngine.Random.RandomRangeInt(0, converts.Count);
                    }
                    converts[c2].statuses.AddStatus(ECharacterStatus.BrokenAbility, charRef);
                    converts[c2].Init(charRef.dataRef);
                    converts[c2].statuses.AddStatus(TricksterRegister.Minion, charRef);
                } else if (tricksters == 2)
                {
                    foreach (Character c in Gameplay.CurrentCharacters)
                    {
                        if (c.dataRef.characterId == "Trickster_scm" && c != charRef)
                        {
                            c.statuses.AddStatus(ECharacterStatus.BrokenAbility, charRef);
                            c.statuses.AddStatus(TricksterRegister.Minion, charRef);
                        }
                    }
                    int c1 = UnityEngine.Random.RandomRangeInt(0, converts.Count);
                    converts[c1].statuses.AddStatus(ECharacterStatus.BrokenAbility, charRef);
                    converts[c1].Init(charRef.dataRef);
                    converts[c1].statuses.AddStatus(TricksterRegister.Outcast, charRef);

                } else if (tricksters == 3)
                {
                    bool seen = false;
                    foreach (Character c in Gameplay.CurrentCharacters)
                    {
                        if (c.dataRef.characterId == "Trickster_scm" && c != charRef)
                        {
                            c.statuses.AddStatus(ECharacterStatus.BrokenAbility, charRef);
                            if (!seen)
                            {
                                c.statuses.AddStatus(TricksterRegister.Minion, charRef);
                                seen = true;
                            } else
                            {
                                c.statuses.AddStatus(TricksterRegister.Outcast, charRef);
                            }
                        }
                    }
                }
            }
        }
        if (trigger == ETriggerPhase.AfterRoundStart)
        {
            if (charRef.statuses.Contains(TricksterRegister.NotBugged))
            {
                charRef.statuses.statuses.Remove(TricksterRegister.Outcast);
                charRef.statuses.statuses.Remove(TricksterRegister.Minion);
            }
            CharacterData Trickster_Outcast = MainMod.Instance.makeNewCharacter("Trickster_o", EAlignment.Good, ECharacterType.Outcast, false, false, "");
            Trickster_Outcast.role = new Trickster();
            Trickster_Outcast.characterName = "Trickster";
            CharacterData Trickster_Minion = MainMod.Instance.makeNewCharacter("Trickster_m", EAlignment.Good, ECharacterType.Minion, false, false, "");
            Trickster_Minion.role = new Trickster();
            Trickster_Minion.characterName = "Trickster";
            foreach (Character c in Gameplay.CurrentCharacters)
            {
                if (c.statuses.Contains(TricksterRegister.Outcast))
                {
                    c.UpdateRegisterAsRole(Trickster_Outcast);
                }
                else
                if (c.statuses.Contains(TricksterRegister.Minion))
                {
                    c.UpdateRegisterAsRole(Trickster_Minion);
                }
            }
        }
    }
    public override void BluffAct(ETriggerPhase trigger, Character charRef)
    {
        if (trigger == ETriggerPhase.Day)
        {
            onActed.Invoke(GetInfo(charRef));
        }
        if (charRef.statuses.Contains(ECharacterStatus.BrokenAbility) || charRef.statuses.Contains(TricksterRegister.Outcast) || charRef.statuses.Contains(TricksterRegister.Minion))
            return;
        if (trigger == ETriggerPhase.Start)
        {
            Act(trigger, charRef);
        }
    }
    public Trickster() : base(ClassInjector.DerivedConstructorPointer<Trickster>())
    {
        ClassInjector.DerivedConstructorBody((Il2CppObjectBase)this);
    }

    public Trickster(System.IntPtr ptr) : base(ptr)
    {

    }
}
public class TricksterRegister
{
    public static ECharacterStatus Villager = (ECharacterStatus)901;
    public static ECharacterStatus Outcast = (ECharacterStatus)902;
    public static ECharacterStatus Minion = (ECharacterStatus)903;
    public static ECharacterStatus NotBugged = (ECharacterStatus)904;
    [HarmonyPatch(typeof(Character), nameof(Character.RevealAllReal))]
    public static class pvt
    {
        public static void Postfix(Character __instance)
        {
            if (__instance.statuses.Contains(Minion))
            {
                __instance.chName.text = __instance.dataRef.name.ToUpper() + "<color=#FA4444><size=18>\nMinion</color></size>";
            }
            else if (__instance.statuses.Contains(Outcast))
            {
                __instance.chName.text = __instance.dataRef.name.ToUpper() + "<color=#FFBB33><size=18>\nOutcast</color></size>";
            }
            else if (__instance.statuses.Contains(Villager))
            {
                __instance.chName.text = __instance.dataRef.name.ToUpper() + "<color=#4FD659><size=18>\nVillager</color></size>";
            }
        }
    }
}