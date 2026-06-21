using System;
using System.ComponentModel.Design;
using HarmonyLib;
using Il2Cpp;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.Injection;
using Il2CppInterop.Runtime.InteropTypes;
using Il2CppSystem;
using MelonLoader;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;
using static UnityEngine.TouchScreenKeyboard;

namespace RiddlerMod;

[RegisterTypeInIl2Cpp]
public class Escapist : Demon
{
    public override Il2CppSystem.Collections.Generic.List<SpecialRule> GetRules()
    {
        Il2CppSystem.Collections.Generic.List<SpecialRule> sr = new Il2CppSystem.Collections.Generic.List<SpecialRule>();
        sr.Add(new NightModeRule(4));
        return sr;
    }
    public override void Act(ETriggerPhase trigger, Character charRef)
    {
        if (trigger == ETriggerPhase.Start)
        {
            //add an outcast
            Il2CppSystem.Collections.Generic.List<Character> viableCharacters = Gameplay.CurrentCharacters;

            Il2CppSystem.Collections.Generic.List<CharacterData> notInPlayOutsiders = Gameplay.Instance.GetAscensionAllStartingCharacters();
            notInPlayOutsiders = Characters.Instance.FilterNotInDeckCharactersUnique(notInPlayOutsiders);
            notInPlayOutsiders = Characters.Instance.FilterRealCharacterType(notInPlayOutsiders, ECharacterType.Outcast);
            if (notInPlayOutsiders.Count == 0)
            {
                notInPlayOutsiders = Gameplay.Instance.GetAllAscensionCharacters();
                notInPlayOutsiders = Characters.Instance.FilterRealCharacterType(notInPlayOutsiders, ECharacterType.Outcast);
            }
            Il2CppSystem.Collections.Generic.List<CharacterData> validOutcasts = new();
            List<string> invalidOutcastIds = new();
            invalidOutcastIds.Add("Bombardier_79093372"); // Evil bombardier still makes you lose.
            invalidOutcastIds.Add("Doppleganger_52694042"); // Too many bugs.
            invalidOutcastIds.Add("Hitman_scm"); // This outcast is already evil.
            invalidOutcastIds.Add("Ghost_scm"); // This would just die and out itself immediately
            invalidOutcastIds.Add("Renegade_WING"); // This outcast is already evil.
            invalidOutcastIds.Add("Mutant_WING"); // This outcast might already be evil.
            invalidOutcastIds.Add("Revolutionary_WING"); // I know for sure this will cause problems
            invalidOutcastIds.Add("Tergiversator_WING"); // Wow, something that might already be evil!

            foreach (CharacterData data in notInPlayOutsiders)
            {
                if (!invalidOutcastIds.Contains(data.characterId))
                {
                    validOutcasts.Add(data);
                }
            }
            if (validOutcasts.Count != 0)
            {
                CharacterData pickedOutsider = validOutcasts[UnityEngine.Random.Range(0, validOutcasts.Count - 1)];
                Gameplay.Instance.AddScriptCharacter(ECharacterType.Outcast, pickedOutsider);

                viableCharacters = Characters.Instance.FilterAliveCharacters(viableCharacters);
                viableCharacters = Characters.Instance.FilterRealCharacterType(viableCharacters, ECharacterType.Villager);

                Character pickedCharacter = viableCharacters[UnityEngine.Random.Range(0, viableCharacters.Count)];
                pickedCharacter.Init(pickedOutsider);
            }
            // One outcast is evil.
            Il2CppSystem.Collections.Generic.List<Character> outcasts = Gameplay.CurrentCharacters;
            outcasts = Characters.Instance.FilterRealCharacterType(outcasts, ECharacterType.Outcast);
            outcasts = Characters.Instance.FilterAliveCharacters(outcasts);
            outcasts = Characters.Instance.FilterRealAlignmentCharacters(outcasts, EAlignment.Good);
            Il2CppSystem.Collections.Generic.List<Character> filter = new();
            foreach (Character character in outcasts)
            {
                if (!invalidOutcastIds.Contains(character.dataRef.characterId))
                {
                    filter.Add(character);
                }
            }

            if (filter.Count > 0)
            {
                Character pickedOutsider2 = filter[UnityEngine.Random.Range(0, filter.Count - 1)];
                pickedOutsider2.ChangeAlignment(EAlignment.Evil);
                pickedOutsider2.statuses.AddStatus(ECharacterStatus.Corrupted, charRef);
                pickedOutsider2.statuses.statuses.Remove(ECharacterStatus.HealthyBluff);
                pickedOutsider2.statuses.AddStatus(ECharacterStatus.MessedUpByEvil, charRef);
                pickedOutsider2.statuses.AddStatus(Escaped.evilTurned, charRef);
            }
        }
    }
    public override CharacterData GetBluffIfAble(Character charRef)
    {
        Il2CppSystem.Collections.Generic.List<CharacterData> outsiders = Gameplay.Instance.GetAscensionAllStartingCharacters();
        outsiders = Characters.Instance.FilterRealCharacterType(outsiders, ECharacterType.Outcast);
        outsiders = Characters.Instance.FilterBluffableCharacters(outsiders);
        CharacterData pickedOutsider = outsiders[UnityEngine.Random.Range(0, outsiders.Count - 1)];
        Gameplay.Instance.AddScriptCharacterIfAble(ECharacterType.Outcast, pickedOutsider);

        return pickedOutsider;
    }
    public Escapist() : base(ClassInjector.DerivedConstructorPointer<Escapist>())
    {
        ClassInjector.DerivedConstructorBody((Il2CppObjectBase)this);
    }
    public Escapist(System.IntPtr ptr) : base(ptr) { }
}
public static class Escaped // or rather, just literally any evil-turned thing
{
    public static ECharacterStatus evilTurned = (ECharacterStatus)880;

    [HarmonyPatch(typeof(Character), nameof(Character.RevealAllReal))]
    public static class pvt
    {
        public static void Postfix(Character __instance)
        {
            if (__instance.statuses.Contains(evilTurned))
            {
                __instance.chName.text = __instance.dataRef.name.ToUpper() + "<color=#FF3333><size=18>\n<Evil></color></size>";
            }
        }
    }
}