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
public class Coach : Role
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
        CharacterPicker.Instance.StartPickCharacters(1, charRef);
        CharacterPicker.OnCharactersPicked += action1;
        CharacterPicker.OnStopPick += action2;
    }
    public override void BluffAct(ETriggerPhase trigger, Character charRef)
    {
        if (trigger != ETriggerPhase.Day) return;
        chRef = charRef;
        CharacterPicker.Instance.StartPickCharacters(1, charRef);
        CharacterPicker.OnCharactersPicked += action3;
        CharacterPicker.OnStopPick += action2;
    }
    private void CharacterPicked()
    {
        CharacterPicker.OnCharactersPicked -= action1;
        CharacterPicker.OnStopPick -= action2;
        Character picked = CharacterPicker.PickedCharacters[0];
        Il2CppSystem.Collections.Generic.List<Character> characters = Gameplay.CurrentCharacters;
        int matches = 0;

        foreach (Character character in characters)
        {
            int distanceFromCharacter = character.id - picked.id;
            if (distanceFromCharacter < -2)
                distanceFromCharacter += Gameplay.CurrentCharacters.Count;
            if (character.GetCharacterType() == picked.GetCharacterType() && distanceFromCharacter >= -2 && distanceFromCharacter <= 2 && distanceFromCharacter != 0)
                matches++;
        }
        string info = ConjureInfo(picked, matches);
        onActed?.Invoke(new ActedInfo(info, CharacterPicker.PickedCharacters));
    }
    private void CharacterPickedLiar()
    {

        CharacterPicker.OnCharactersPicked -= action1;
        CharacterPicker.OnStopPick -= action2;
        Character picked = CharacterPicker.PickedCharacters[0];
        Il2CppSystem.Collections.Generic.List<Character> characters = Gameplay.CurrentCharacters;
        int matches = 0;

        foreach (Character character in characters)
        {
            int distanceFromCharacter = character.id - picked.id;
            if (distanceFromCharacter < -2)
                distanceFromCharacter += Gameplay.CurrentCharacters.Count;
            if (character.GetCharacterType() == picked.GetCharacterType() && distanceFromCharacter >= -2 && distanceFromCharacter <= 2 && distanceFromCharacter != 0)
                matches++;
        }
        int lie = UnityEngine.Random.RandomRangeInt(0, 5);
        while (lie == matches)
        {
            lie = UnityEngine.Random.RandomRangeInt(0, 5);
        }
        string info = ConjureInfo(picked, lie);
        onActed?.Invoke(new ActedInfo(info, CharacterPicker.PickedCharacters));
    }

    public string ConjureInfo(Character character, int matches)
    {
        if (matches == 1)
            return string.Format("1 character near #{1} are the same Type as #{1}", matches);
        return string.Format("{0} characters near #{1} are the same Type as #{1}", matches, character.id);
    }
    private void StopPick()
    {
        CharacterPicker.OnCharactersPicked -= action1;
        CharacterPicker.OnStopPick -= action2;
        CharacterPicker.OnCharactersPicked -= action3;
    }
    public Coach() : base(ClassInjector.DerivedConstructorPointer<Coach>())
    {
        ClassInjector.DerivedConstructorBody((Il2CppObjectBase)this);
        action1 = new System.Action(CharacterPicked);
        action2 = new System.Action(StopPick);
        action3 = new System.Action(CharacterPickedLiar);
    }

    public Coach(System.IntPtr ptr) : base(ptr)
    {
        action1 = new System.Action(CharacterPicked);
        action2 = new System.Action(StopPick);
        action3 = new System.Action(CharacterPickedLiar);
    }
}