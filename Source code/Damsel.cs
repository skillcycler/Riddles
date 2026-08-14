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
public class Damsel : Role
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
        if (trigger == ETriggerPhase.OnPicked)
        {
            if (lastPicker.alignment == EAlignment.Evil || lastPicker.GetRegisterAlignment() == EAlignment.Evil)
            {
                PlayerController.PlayerInfo.health.Damage(5);
            }
        }
    }
    public override CharacterData GetBluffIfAble(Character charRef)
    {
        if (Calculator.RollDice(4) == 1) return Characters.Instance.GetRandomDuplicateBluff();
        return Characters.Instance.GetRandomUniqueBluff();
    }
    public Damsel() : base(ClassInjector.DerivedConstructorPointer<Damsel>())
    {
        ClassInjector.DerivedConstructorBody((Il2CppObjectBase)this);
    }
    public Damsel(System.IntPtr ptr) : base(ptr) { }

    // thanks to Redkiller5451 for this code!
    private static Character lastPicker = null;
    public static void SetLastPicker(Character picker)
    {
        lastPicker = picker;
    }
    [HarmonyPatch(typeof(CharacterPicker), nameof(CharacterPicker.StartPickCharacters))]
    public class CharacterPickerPatch
    {
        static void Prefix(int howMany, Character picker)
        {
            if (picker != null)
            {
                SetLastPicker(picker);
            }
        }
    }
    public override int GetDamageToYou()
    {
        return 4;
    }
}
