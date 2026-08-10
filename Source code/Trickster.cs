using System;
using System.Linq;
using System.Reflection;
using System.Xml.Linq;
using HarmonyLib;
using Il2Cpp;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.Injection;
using Il2CppInterop.Runtime.InteropTypes;
using Il2CppInterop.Runtime.InteropTypes.Arrays;
using Il2CppSystem.Reflection;
using MelonLoader;
using UnityEngine;
using static Il2CppSystem.Collections.SortedList;
using static MelonLoader.Modules.MelonModule;

namespace RiddlerMod;

[RegisterTypeInIl2Cpp]
public class Trickster : Role
{
    public CharacterData[] allDatas = Il2CppSystem.Array.Empty<CharacterData>();
    public static CharacterData makeTricksterData(string id, ECharacterType type)
    {
        CharacterData character = new CharacterData();
        character.name = "Trickster";
        character.characterName = "Trickster";
        character.picking = false;
        character.startingAlignment = EAlignment.Good;
        character.flavorText = "I register as a Good Outcast/Minion.";
        character.type = type;
        character.bluffable = false;
        character.additionalFlavorTexts = new Il2CppStringArray(1);
        character.additionalFlavorTexts[0] = character.flavorText;
        character.characterId = id + "_scm";
        switch (type)
        {
            case ECharacterType.Outcast:
                character.cardBgColor = new Color(0.102f, 0.0667f, 0.0392f);
                character.cardBorderColor = new Color(0.7843f, 0.6471f, 0f);
                character.color = new Color(0.9659f, 1f, 0.4472f);
                break;
            case ECharacterType.Minion:
                character.cardBgColor = new Color(0.0941f, 0.0431f, 0.0431f);
                character.cardBorderColor = new Color(0.8208f, 0f, 0.0241f);
                character.color = new Color(0.8491f, 0.4555f, 0f);
                break;
        }
        character.bundledCharacters = new Il2CppSystem.Collections.Generic.List<CharacterData>();
        character.additionalPossibleCharacters = new AddedCharacterTypes();
        character.usuallyDisguised = false;
        character.hints = "";
        character.ifLies = "";
        return character;
    }
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
            return new ActedInfo("There is a bug where I am corrupted, despite being unable to be corrupted.");
        }
        if (charRef.dataRef.characterId != "Trickster_scm")
        {
            return new ActedInfo(string.Format("I am actually a {1}", charRef.id, charRef.dataRef.name));
        }
        int converted = 0;
        foreach (Character ch in Gameplay.CurrentCharacters)
        {
            if (ch.bluff) if (ch.bluff.characterId == "Trickster_scm") return new ActedInfo($"One of us has turned into the {ch.dataRef.characterName}");
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
    public void AddResistances(Character c)
    {

        c.statuses.AddResistance(ECharacterStatus.Corrupted, charRef);
        c.statuses.AddResistance(Accused.accused, charRef);
        c.statuses.AddResistance(Guarding.guarded, charRef);
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
            AddResistances(charRef);
            if (converts.Count > 1) {
                if (tricksters == 1)
                {
                    int c1 = UnityEngine.Random.RandomRangeInt(0, converts.Count);
                    converts[c1].statuses.AddStatus(ECharacterStatus.BrokenAbility, charRef);
                    converts[c1].Init(charRef.dataRef);
                    converts[c1].statuses.AddStatus(TricksterRegister.Outcast, charRef);
                    converts[c1].UpdateRegisterAsRole(makeTricksterData("Trickster_o", ECharacterType.Outcast));
                    int c2 = UnityEngine.Random.RandomRangeInt(0, converts.Count);
                    while (c1 == c2)
                    {
                        c2 = UnityEngine.Random.RandomRangeInt(0, converts.Count);
                    }
                    converts[c2].statuses.AddStatus(ECharacterStatus.BrokenAbility, charRef);
                    converts[c2].Init(charRef.dataRef);
                    converts[c2].statuses.AddStatus(TricksterRegister.Minion, charRef);
                    converts[c2].UpdateRegisterAsRole(makeTricksterData("Trickster_m", ECharacterType.Minion));
                    AddResistances(converts[c1]);
                    AddResistances(converts[c2]);
                } else if (tricksters == 2)
                {
                    foreach (Character c in Gameplay.CurrentCharacters)
                    {
                        if (c.dataRef.characterId == "Trickster_scm" && c != charRef)
                        {
                            c.statuses.AddStatus(ECharacterStatus.BrokenAbility, charRef);
                            c.statuses.AddStatus(TricksterRegister.Minion, charRef);
                            c.UpdateRegisterAsRole(makeTricksterData("Trickster_m", ECharacterType.Minion));
                            AddResistances(c);
                        }
                    }
                    int c1 = UnityEngine.Random.RandomRangeInt(0, converts.Count);
                    converts[c1].statuses.AddStatus(ECharacterStatus.BrokenAbility, charRef);
                    converts[c1].Init(charRef.dataRef);
                    converts[c1].statuses.AddStatus(TricksterRegister.Outcast, charRef);
                    converts[c1].UpdateRegisterAsRole(makeTricksterData("Trickster_o", ECharacterType.Outcast));
                    AddResistances(converts[c1]);

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
                                c.UpdateRegisterAsRole(makeTricksterData("Trickster_m", ECharacterType.Minion));
                            } else
                            {
                                c.statuses.AddStatus(TricksterRegister.Outcast, charRef);
                                c.UpdateRegisterAsRole(makeTricksterData("Trickster_o", ECharacterType.Outcast));
                            }
                            AddResistances(c);
                        }
                    }
                }
            }
        }
        // keep updating these on literally any trigger because they keep losing their registration for some reason
        if (trigger == ETriggerPhase.AfterRoundStart || true)
        {
            if (charRef.statuses.Contains(TricksterRegister.NotBugged))
            {
                charRef.statuses.statuses.Remove(TricksterRegister.Outcast);
                charRef.statuses.statuses.Remove(TricksterRegister.Minion);
            }
            foreach (Character c in Gameplay.CurrentCharacters)
            {
                if (c.statuses.Contains(TricksterRegister.Outcast))
                {
                    c.UpdateRegisterAsRole(makeTricksterData("Trickster_o", ECharacterType.Outcast));
                }
                else
                if (c.statuses.Contains(TricksterRegister.Minion))
                {
                    c.UpdateRegisterAsRole(makeTricksterData("Trickster_m", ECharacterType.Minion));
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
    public override CharacterData GetRegisterAsRole(Character charRef)
    {
        if (charRef.statuses.Contains(TricksterRegister.Outcast)) return makeTricksterData("Trickster_o", ECharacterType.Outcast);
        if (charRef.statuses.Contains(TricksterRegister.Minion)) return makeTricksterData("Trickster_m", ECharacterType.Minion);
        return null;
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
                if (__instance.statuses.Contains(Accused.accused))
                    __instance.chName.text = __instance.dataRef.name.ToUpper() + "<color=#4FD659><size=14>\nVillager</color><color=#FF8000>(Accused)</color></size>";
                else __instance.chName.text = __instance.dataRef.name.ToUpper() + "<color=#4FD659><size=18>\nVillager</color></size>";
            }
        }
    }
}