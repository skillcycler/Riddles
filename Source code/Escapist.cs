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
    /*public override Il2CppSystem.Collections.Generic.List<SpecialRule> GetRules()
    {
        Il2CppSystem.Collections.Generic.List<SpecialRule> sr = new Il2CppSystem.Collections.Generic.List<SpecialRule>();
        sr.Add(new NightModeRule(4));
        return sr;
    }*/
    public override void Act(ETriggerPhase trigger, Character charRef)
    {
        if (trigger == ETriggerPhase.Init)
        {
            Djinn.Jinx("Escapist");
        }
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
            invalidOutcastIds.Add("Hitman_scm"); // This outcast is already evil.
            invalidOutcastIds.Add("Ghost_scm"); // This would just die and out itself immediately
            invalidOutcastIds.Add("Renegade_WING"); // This outcast is already evil.
            invalidOutcastIds.Add("Mutant_WING"); // This outcast might already be evil.
            invalidOutcastIds.Add("Revolutionary_WING"); // I know for sure this will cause problems
            invalidOutcastIds.Add("Tergiversator_WING"); // Wow, something that might already be evil!
            invalidOutcastIds.Add("Lycanthrope_16077432"); // Guess what? This outcast can turn evil!
            invalidOutcastIds.Add("Mobster_POW"); // Guess what? This outcast can turn evil!

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
                // turn it evil
                pickedCharacter.ChangeAlignment(EAlignment.Evil);
                pickedCharacter.statuses.AddStatus(ECharacterStatus.Corrupted, charRef);
                pickedCharacter.statuses.statuses.Remove(ECharacterStatus.HealthyBluff);
                pickedCharacter.statuses.AddStatus(ECharacterStatus.MessedUpByEvil, charRef);
                pickedCharacter.statuses.AddStatus(Escaped.evilTurned, charRef);
                pickedCharacter.statuses.AddStatus(Escaped.EscapistTarget, charRef);
            } else
            {
                MelonLogger.Msg("Escapist: There were somehow no valid Outcasts to add.");
            }
        }
        if (trigger == ETriggerPhase.AfterRoundStart)
        {
            foreach (Character c in Gameplay.CurrentCharacters)
            {
                if (c.statuses.Contains(Escaped.EscapistTarget)) c.statuses.statuses.Remove(ECharacterStatus.HealthyBluff);
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
    public static ECharacterStatus EscapistTarget = (ECharacterStatus)912;

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