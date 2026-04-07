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
public class Commander : Role
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
        CharacterPicker.Instance.StartPickCharacters(2);
        CharacterPicker.OnCharactersPicked += action1;
        CharacterPicker.OnStopPick += action2;
    }
    public override void BluffAct(ETriggerPhase trigger, Character charRef)
    {
        if (trigger != ETriggerPhase.Day) return;
        chRef = charRef;
        CharacterPicker.Instance.StartPickCharacters(2);
        CharacterPicker.OnCharactersPicked += action3;
        CharacterPicker.OnStopPick += action2;
    }
    private void CharacterPicked()
    {
        CharacterPicker.OnCharactersPicked -= action1;
        CharacterPicker.OnStopPick -= action2;
        Il2CppSystem.Collections.Generic.List<Character> characters = Gameplay.CurrentCharacters;

        Il2CppSystem.Collections.Generic.List<Character> chars = new Il2CppSystem.Collections.Generic.List<Character>();
        chars.Add(CharacterPicker.PickedCharacters[0]);
        chars.Add(CharacterPicker.PickedCharacters[1]);
        Il2CppSystem.Collections.Generic.List<Character> possible = new Il2CppSystem.Collections.Generic.List<Character>();

        foreach (Character character in characters)
        {
            if (character.GetCharacterType() != chars[0].GetCharacterType() && character.GetCharacterType() != chars[1].GetCharacterType())
            {
                possible.Add(character);
            }
        }
        if (possible.Count > 0)
        {
            int chosen = possible[UnityEngine.Random.RandomRangeInt(0, possible.Count)].id;
            string info = ConjureInfo(chars, chosen);
            onActed?.Invoke(new ActedInfo(info, chars));
        } else
        {
            string info = "There is no valid truthful info to give.";
            onActed?.Invoke(new ActedInfo(info, chars));
        }
    }
    private void CharacterPickedLiar()
    {

        CharacterPicker.OnCharactersPicked -= action3;
        CharacterPicker.OnStopPick -= action2;
        Il2CppSystem.Collections.Generic.List<Character> characters = Gameplay.CurrentCharacters;

        Il2CppSystem.Collections.Generic.List<Character> chars = new Il2CppSystem.Collections.Generic.List<Character>();
        chars.Add(CharacterPicker.PickedCharacters[0]);
        chars.Add(CharacterPicker.PickedCharacters[1]);
        Il2CppSystem.Collections.Generic.List<Character> possible = new Il2CppSystem.Collections.Generic.List<Character>();

        foreach (Character character in characters)
        {
            if (character.GetCharacterType() == chars[0].GetCharacterType() || character.GetCharacterType() == chars[1].GetCharacterType())
            {
                possible.Add(character);
            }
        }
        int chosen = possible[UnityEngine.Random.RandomRangeInt(0, possible.Count)].id;
        string info = ConjureInfo(chars, chosen);
        onActed?.Invoke(new ActedInfo(info, chars));
    }

    public string ConjureInfo(Il2CppSystem.Collections.Generic.List<Character> characters, int chosen)
    {
        return string.Format("#{2} is a different character type from #{0} and #{1}", characters[0].id, characters[1].id, chosen);
    }
    private void StopPick()
    {
        CharacterPicker.OnCharactersPicked -= action1;
        CharacterPicker.OnStopPick -= action2;
        CharacterPicker.OnCharactersPicked -= action3;
    }
    public Commander() : base(ClassInjector.DerivedConstructorPointer<Commander>())
    {
        ClassInjector.DerivedConstructorBody((Il2CppObjectBase)this);
        action1 = new System.Action(CharacterPicked);
        action2 = new System.Action(StopPick);
        action3 = new System.Action(CharacterPickedLiar);
    }

    public Commander(System.IntPtr ptr) : base(ptr)
    {
        action1 = new System.Action(CharacterPicked);
        action2 = new System.Action(StopPick);
        action3 = new System.Action(CharacterPickedLiar);
    }
}