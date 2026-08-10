using System;
using System.ComponentModel.Design;
using HarmonyLib;
using Il2Cpp;
using Il2CppInterop.Runtime.Injection;
using Il2CppInterop.Runtime.InteropTypes;
using Il2CppInterop.Runtime.InteropTypes.Arrays;
using Il2CppSystem;
using MelonLoader;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

namespace RiddlerMod;

[RegisterTypeInIl2Cpp]
public class Fracture : Demon
{
    public override Il2CppSystem.Collections.Generic.List<SpecialRule> GetRules()
    {
        Il2CppSystem.Collections.Generic.List<SpecialRule> sr = new Il2CppSystem.Collections.Generic.List<SpecialRule>();
        sr.Add(new NightModeRule(4));
        return sr;
    }
    public static CharacterData GetNothing()
    {
        CharacterData character = new CharacterData();
        character.name = "Nothing";
        character.characterName = "Nothing";
        character.picking = false;
        character.startingAlignment = EAlignment.None;
        character.flavorText = "I register as nothing.";
        character.type = ECharacterType.None;
        character.bluffable = false;
        character.additionalFlavorTexts = new Il2CppStringArray(1);
        character.additionalFlavorTexts[0] = character.flavorText;
        character.characterId = "Nothing_scm";
        character.bundledCharacters = new Il2CppSystem.Collections.Generic.List<CharacterData>();
        character.additionalPossibleCharacters = new AddedCharacterTypes();
        character.usuallyDisguised = false;
        character.hints = "";
        character.ifLies = "";
        return character;
    }
    public override void Act(ETriggerPhase trigger, Character charRef)
    {
        if (trigger == ETriggerPhase.Night || trigger == ETriggerPhase.Start)
        {
            if (charRef.state == ECharacterState.Dead) return;
            Character remove = Gameplay.CurrentCharacters[UnityEngine.Random.RandomRangeInt(0, Gameplay.CurrentCharacters.Count)];
            while (remove.id == charRef.id || remove.statuses.Contains(Broken.erased)) remove = Gameplay.CurrentCharacters[UnityEngine.Random.RandomRangeInt(0, Gameplay.CurrentCharacters.Count)];
            remove.statuses.AddStatus(Broken.erased, charRef);
            remove.UpdateRegisterAsRole(GetNothing());
        }
        if (trigger == ETriggerPhase.AfterRoundStart)
        {
            foreach (Character character in Gameplay.CurrentCharacters)
            {
                if (character.statuses.Contains(Broken.erased))
                {
                    character.UpdateRegisterAsRole(GetNothing());
                }
            }
        }
    }
    public Fracture() : base(ClassInjector.DerivedConstructorPointer<Fracture>())
    {
        ClassInjector.DerivedConstructorBody((Il2CppObjectBase)this);
    }
    public Fracture(System.IntPtr ptr) : base(ptr) { }
}
public static class Broken
{
    public static ECharacterStatus erased = (ECharacterStatus)911;

    [HarmonyPatch(typeof(Character), nameof(Character.RevealAllReal))]
    public static class pvt
    {
        public static void Postfix(Character __instance)
        {
            if (__instance.statuses.Contains(erased))
            {
                __instance.chName.text = __instance.dataRef.name.ToUpper() + "<color=#BB6666><size=18>\n<Erased></color></size>";
            }
        }
    }
}