using System;
using System.ComponentModel.Design;
using HarmonyLib;
using Il2Cpp;
using Il2CppInterop.Runtime.Injection;
using Il2CppInterop.Runtime.InteropTypes;
using Il2CppSystem;
using MelonLoader;
using UnityEngine;
using static MelonLoader.MelonLogger;


namespace RiddlerMod;

[RegisterTypeInIl2Cpp]
public class Accuser : Minion
{
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
            Il2CppSystem.Collections.Generic.List<Character> neighbors = Characters.Instance.GetAdjacentCharacters(charRef);
            // I only want actual Good characters that register as Good to be Accused.
            // No shenanigans involving Guardian or Evils registering as Good some other way
            neighbors = Characters.Instance.FilterAlignmentCharacters(neighbors, EAlignment.Good);
            neighbors = Characters.Instance.FilterRealAlignmentCharacters(neighbors, EAlignment.Good);
            if (neighbors.Count > 0)
            {
                Character randomChar = neighbors[UnityEngine.Random.Range(0, neighbors.Count)];
                randomChar.statuses.AddStatus(ECharacterStatus.MessedUpByEvil, charRef);
                randomChar.statuses.AddStatus(Accused.accused, charRef);
                Il2CppSystem.Collections.Generic.List<CharacterData> allChars = new Il2CppSystem.Collections.Generic.List<CharacterData>();
                foreach (CharacterData charData in Gameplay.Instance.GetScriptCharacters())
                {
                    allChars.Add(charData);
                }
                allChars = Characters.Instance.FilterCharacterType(allChars, ECharacterType.Minion);
                if (allChars.Count == 0)
                    allChars.Add(ProjectContext.Instance.gameData.GetCharacterDataOfId("Puppet_15989619"));
                CharacterData randomMinion = allChars[UnityEngine.Random.Range(0, allChars.Count)];
                randomChar.UpdateRegisterAsRole(randomMinion);
            }
        }
        //just to make sure accused things register as evil
        if (trigger == ETriggerPhase.AfterRoundStart)
        {
            Accused.UpdateAccusedRegistration();
        }
    }
    
    public Accuser() : base(ClassInjector.DerivedConstructorPointer<Accuser>())
    {
        ClassInjector.DerivedConstructorBody((Il2CppObjectBase)this);
    }
    public Accuser(System.IntPtr ptr) : base(ptr) { }
    
}
public static class Accused
{
    public static ECharacterStatus accused = (ECharacterStatus)873;
    public static void UpdateAccusedRegistration()
    {
        List<ECharacterType> validTypes = new();
        validTypes.Add(ECharacterType.Minion);
        validTypes.Add(ECharacterType.Demon);
        validTypes.Add((ECharacterType)155);
        validTypes.Add((ECharacterType)160);
        validTypes.Add((ECharacterType)165);
        validTypes.Add((ECharacterType)170);

        foreach (Character c in Gameplay.CurrentCharacters)
        {
            if (c.statuses.Contains(Accused.accused))
            {
                Il2CppSystem.Collections.Generic.List<CharacterData> allChars = new Il2CppSystem.Collections.Generic.List<CharacterData>();
                foreach (CharacterData charData in Gameplay.Instance.GetScriptCharacters())
                {
                    if (validTypes.Contains(charData.type) && charData.startingAlignment == EAlignment.Evil)
                    allChars.Add(charData);
                }
                if (allChars.Count == 0)
                    allChars.Add(ProjectContext.Instance.gameData.GetCharacterDataOfId("Puppet_15989619"));
                CharacterData randomEvil = allChars[UnityEngine.Random.Range(0, allChars.Count)];
                c.UpdateRegisterAsRole(randomEvil);
            }
        }
    }

    [HarmonyPatch(typeof(Character), nameof(Character.RevealAllReal))]
    public static class pvt
    {
        public static void Postfix(Character __instance)
        {
            if (__instance.statuses.Contains(accused))
            {
                if (__instance.statuses.Contains(Confused.confused))
                {
                    __instance.chName.text = __instance.dataRef.name.ToUpper() + "<color=#DDDD00><size=12>\n<Confused+</color></size><color=#FF8000><size=12>Accused></color></size>";
                } else if (__instance.statuses.Contains(ECharacterStatus.Corrupted))
                {
                    __instance.chName.text = __instance.dataRef.name.ToUpper() + "<color=#999999><size=12>\nCorrupted+</color></size><color=#FF8000><size=12>Accused</color></size>";
                }
                else if (__instance.statuses.Contains(TricksterRegister.Villager))
                    __instance.chName.text = __instance.dataRef.name.ToUpper() + "<color=#4FD659><size=14>\nVillager</color><color=#FF8000>(Accused)</color></size>";
                else
                {
                    __instance.chName.text = __instance.dataRef.name.ToUpper() + "<color=#FF8000><size=18>\n<Accused></color></size>";
                }
            }
        }
    }
}
[HarmonyPatch(typeof(Character), nameof(Character.Reveal))]
public static class Accusing
{
    public static void Postfix(Character __instance)
    {
        if (__instance.statuses.Contains(Accused.accused))
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
            __instance.UpdateRegisterAsRole(randomMinion);
        }
    }
}