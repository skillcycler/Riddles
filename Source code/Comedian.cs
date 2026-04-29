using System;
using System.Linq;
using System.Reflection;
using Il2Cpp;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.Injection;
using Il2CppInterop.Runtime.InteropTypes;
using Il2CppSystem;
using Il2CppSystem.Reflection;
using MelonLoader;
using UnityEngine;

namespace RiddlerMod;

[RegisterTypeInIl2Cpp]
public class Comedian : Role
{
    Character chRef;
    private Il2CppSystem.Action action1;
    private Il2CppSystem.Action action2;
    private Il2CppSystem.Action action3;
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
        if (trigger != ETriggerPhase.Day) return;
        chRef = charRef;
        CharacterPicker.Instance.StartPickCharacters(3, charRef);
        CharacterPicker.OnCharactersPicked += action1;
        CharacterPicker.OnStopPick += action2;
    }
    public override void BluffAct(ETriggerPhase trigger, Character charRef)
    {
        if (trigger != ETriggerPhase.Day) return;
        chRef = charRef;
        CharacterPicker.Instance.StartPickCharacters(3, charRef);
        CharacterPicker.OnCharactersPicked += action3;
        CharacterPicker.OnStopPick += action2;
    }
    private void CharacterPicked()
    {
        CharacterPicker.OnCharactersPicked -= action1;
        CharacterPicker.OnStopPick -= action2;
        bool disguised = false;

        foreach (Character character in CharacterPicker.PickedCharacters)
        {
            if (character.bluff)
                disguised = true;
        }
        string d = "No";
        if (disguised)
            d = "Yes";
        string info = ConjureInfo(CharacterPicker.PickedCharacters, d);
        onActed?.Invoke(new ActedInfo(info, CharacterPicker.PickedCharacters));
    }
    private void CharacterPickedLiar()
    {

        CharacterPicker.OnCharactersPicked -= action1;
        CharacterPicker.OnStopPick -= action2;
        bool disguised = false;

        foreach (Character character in CharacterPicker.PickedCharacters)
        {
            if (character.bluff)
                disguised = true;
        }
        string d = "Yes";
        if (disguised)
            d = "No";
        string info = ConjureInfo(CharacterPicker.PickedCharacters, d);
        onActed?.Invoke(new ActedInfo(info, CharacterPicker.PickedCharacters));
    }

    public string ConjureInfo(Il2CppSystem.Collections.Generic.List<Character> characters, string disguised)
    {
        return string.Format("Is #{0}, #{1}, or #{2} Disguised?: {3}", characters[0].id, characters[1].id, characters[2].id, disguised);
    }
    private void StopPick()
    {
        CharacterPicker.OnCharactersPicked -= action1;
        CharacterPicker.OnStopPick -= action2;
        CharacterPicker.OnCharactersPicked -= action3;
    }
    public Comedian() : base(ClassInjector.DerivedConstructorPointer<Comedian>())
    {
        ClassInjector.DerivedConstructorBody((Il2CppObjectBase)this);
        action1 = new System.Action(CharacterPicked);
        action2 = new System.Action(StopPick);
        action3 = new System.Action(CharacterPickedLiar);
    }

    public Comedian(System.IntPtr ptr) : base(ptr)
    {
        action1 = new System.Action(CharacterPicked);
        action2 = new System.Action(StopPick);
        action3 = new System.Action(CharacterPickedLiar);
    }
}