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
using MelonLoader.TinyJSON;
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
        bool disguised1 = false;
        bool disguised2 = false;
        bool disguised3 = false;
        int total = 0;
        if (CharacterPicker.PickedCharacters[0].bluff)
        {
            disguised1 = true;
            total++;
        }
        if (CharacterPicker.PickedCharacters[1].bluff)
        {
            disguised2 = true;
            total++;
        }
        if (CharacterPicker.PickedCharacters[2].bluff)
        {
            disguised3 = true;
            total++;
        }
        int first = 0;
        int second = 0;
        bool bothDisguised = true;
        if (total >= 2)
        {
            if (total == 3)
            {
                int skip = UnityEngine.Random.RandomRangeInt(0, 3);
                if (skip == 2)
                {
                    first = CharacterPicker.PickedCharacters[0].id;
                    second = CharacterPicker.PickedCharacters[1].id;
                }
                if (skip == 1)
                {
                    first = CharacterPicker.PickedCharacters[0].id;
                    second = CharacterPicker.PickedCharacters[2].id;
                }
                if (skip == 0)
                {
                    first = CharacterPicker.PickedCharacters[1].id;
                    second = CharacterPicker.PickedCharacters[2].id;
                }
            }
            else
            {
                if (disguised1 && disguised2)
                {
                    first = CharacterPicker.PickedCharacters[0].id;
                    second = CharacterPicker.PickedCharacters[1].id;
                }
                if (disguised1 && disguised3)
                {
                    first = CharacterPicker.PickedCharacters[0].id;
                    second = CharacterPicker.PickedCharacters[2].id;
                }
                if (disguised3 && disguised2)
                {
                    first = CharacterPicker.PickedCharacters[1].id;
                    second = CharacterPicker.PickedCharacters[2].id;
                }
            }
        } else
        {
            bothDisguised = false;
            if (total == 0)
            {
                int skip = UnityEngine.Random.RandomRangeInt(0, 3);
                if (skip == 2)
                {
                    first = CharacterPicker.PickedCharacters[0].id;
                    second = CharacterPicker.PickedCharacters[1].id;
                }
                if (skip == 1)
                {
                    first = CharacterPicker.PickedCharacters[0].id;
                    second = CharacterPicker.PickedCharacters[2].id;
                }
                if (skip == 0)
                {
                    first = CharacterPicker.PickedCharacters[1].id;
                    second = CharacterPicker.PickedCharacters[2].id;
                }
            }
            else
            {
                if (!disguised1 && !disguised2)
                {
                    first = CharacterPicker.PickedCharacters[0].id;
                    second = CharacterPicker.PickedCharacters[1].id;
                }
                if (!disguised1 && !disguised3)
                {
                    first = CharacterPicker.PickedCharacters[0].id;
                    second = CharacterPicker.PickedCharacters[2].id;
                }
                if (!disguised3 && !disguised2)
                {
                    first = CharacterPicker.PickedCharacters[1].id;
                    second = CharacterPicker.PickedCharacters[2].id;
                }
            }
        }
        string info = ConjureInfo(first, second, bothDisguised);
        onActed?.Invoke(new ActedInfo(info, CharacterPicker.PickedCharacters));
    }
    private void CharacterPickedLiar()
    {
        int choice1 = UnityEngine.Random.RandomRangeInt(0, 3);
        int choice2 = UnityEngine.Random.RandomRangeInt(0, 3);
        while (choice2 == choice1)
        {
            choice2 = UnityEngine.Random.RandomRangeInt(0, 3);
        }
        int first = CharacterPicker.PickedCharacters[choice1].id;
        int second = CharacterPicker.PickedCharacters[choice2].id;
        bool bothDisguised = true;

        if (CharacterPicker.PickedCharacters[choice1].bluff && CharacterPicker.PickedCharacters[choice2].bluff)
        {
            bothDisguised = false;
        }
        else if ((CharacterPicker.PickedCharacters[choice1].bluff && !CharacterPicker.PickedCharacters[choice2].bluff) || (!CharacterPicker.PickedCharacters[choice1].bluff && CharacterPicker.PickedCharacters[choice2].bluff))
        {
            if (UnityEngine.Random.RandomRangeInt(0, 2) == 1)
                bothDisguised = false;
        }

        string info = ConjureInfo(first, second, bothDisguised);
        onActed?.Invoke(new ActedInfo(info, CharacterPicker.PickedCharacters));
    }

    public string ConjureInfo(int id1, int id2, bool both)
    {
        return string.Format("#{0} and #{1} are both {2}Disguised", id1, id2, both?"":"Not ");
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