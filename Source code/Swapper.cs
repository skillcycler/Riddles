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
public class Swapper : Role
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

        Il2CppSystem.Collections.Generic.List<Character> chars = new Il2CppSystem.Collections.Generic.List<Character>();
        chars.Add(CharacterPicker.PickedCharacters[0]);
        chars.Add(CharacterPicker.PickedCharacters[1]);
        if (chars[0].bluff)
        {
            if (chars[0].bluff.characterId == "Swapper")
            {
                return;
            }
        }
        if (chars[0].dataRef.characterId == "Swapper")
        {
            return;
        }
        if (chars[1].bluff)
        {
            if (chars[1].bluff.characterId == "Swapper")
            {
                return;
            }
        }
        if (chars[1].dataRef.characterId == "Swapper")
        {
            return;
        }
        Character c1 = CharacterPicker.PickedCharacters[0];
        Character c2 = CharacterPicker.PickedCharacters[1];
        CharacterData bluff1 = c1.bluff;
        CharacterData bluff2 = c2.bluff;
        if (!bluff1)
        {
            bluff1 = c1.dataRef;
        }
        if (!bluff2)
        {
            bluff2 = c2.dataRef;
        }
        bool c1Lying = false;
        bool c2Lying = false;
        if (c1.bluff)
        {
            c1Lying = true;
        }
        if (c2.bluff)
        {
            c2Lying = true;
        }
        if (c1.statuses.Contains(ECharacterStatus.Corrupted))
        {
            c1Lying = true;
        }
        if (c2.statuses.Contains(ECharacterStatus.Corrupted))
        {
            c2Lying = true;
        }
        if (c1.statuses.Contains(ECharacterStatus.HealthyBluff))
        {
            c1Lying = false;
        }
        if (c2.statuses.Contains(ECharacterStatus.HealthyBluff))
        {
            c2Lying = false;
        }
        c1.GiveBluff(bluff2);
        if (!c1Lying)
        {
            c1.statuses.AddStatus(ECharacterStatus.HealthyBluff, chRef);
        }
        c1.RevealBluff();
        c1.RefreshCharacter();
        if (c1.bluff.picking)
        {
            c1.uses = 1;
            c1.pickable.SetActive(true);
        }
        c1.Act(ETriggerPhase.Day);
        if (c1.state == ECharacterState.Dead)
        {
            c1.RevealAllReal();
        }

        c2.GiveBluff(bluff1);
        if (!c2Lying)
        {
            c2.statuses.AddStatus(ECharacterStatus.HealthyBluff, chRef);
        }
        c2.RevealBluff();
        c2.RefreshCharacter();
        if (c2.bluff.picking)
        {
            c2.uses = 1;
            c2.pickable.SetActive(true);
        }
        c2.Act(ETriggerPhase.Day);
        if (c2.state == ECharacterState.Dead)
        {
            c2.RevealAllReal();
        }
        string info = ConjureInfo(chars);
        onActed?.Invoke(new ActedInfo(info, chars));
    }
    private void CharacterPickedLiar()
    {

        CharacterPicker.OnCharactersPicked -= action3;
        CharacterPicker.OnStopPick -= action2;

        Il2CppSystem.Collections.Generic.List<Character> chars = new Il2CppSystem.Collections.Generic.List<Character>();
        chars.Add(CharacterPicker.PickedCharacters[0]);
        chars.Add(CharacterPicker.PickedCharacters[1]);
        if (chars[0].bluff)
        {
            if (chars[0].bluff.characterId == "Swapper")
            {
                return;
            }
        }
        if (chars[0].dataRef.characterId == "Swapper")
        {
            return;
        }
        if (chars[1].bluff)
        {
            if (chars[1].bluff.characterId == "Swapper")
            {
                return;
            }
        }
        if (chars[1].dataRef.characterId == "Swapper")
        {
            return;
        }

        Character c1 = CharacterPicker.PickedCharacters[0];
        Character c2 = CharacterPicker.PickedCharacters[1];
        CharacterData bluff1 = c1.bluff;
        CharacterData bluff2 = c2.bluff;
        if (!bluff1)
        {
            bluff1 = c1.dataRef;
        }
        if (!bluff2)
        {
            bluff2 = c2.dataRef;
        }
        c1.GiveBluff(bluff2);
        if (c1.GetCharacterType() == ECharacterType.Villager)
        {
            c1.statuses.AddStatus(ECharacterStatus.Corrupted, charRef);
        }

        c1.RevealBluff();
        c1.RefreshCharacter();
        if (c1.bluff.picking)
        {
            c1.uses = 1;
            c1.pickable.SetActive(true);
        }
        c1.Act(ETriggerPhase.Day);
        if (c1.state == ECharacterState.Dead)
        {
            c1.RevealAllReal();
        }

        c2.GiveBluff(bluff1);
        if (c2.GetCharacterType() == ECharacterType.Villager)
        {
            c2.statuses.AddStatus(ECharacterStatus.Corrupted, charRef);
        }
        
        c2.RevealBluff();
        c2.RefreshCharacter();
        if (c2.bluff.picking)
        {
            c2.uses = 1;
            c2.pickable.SetActive(true);
        }
        c2.Act(ETriggerPhase.Day);

        if (c2.state == ECharacterState.Dead)
        {
            c2.RevealAllReal();
        }
        string info = ConjureInfo(chars);
        onActed?.Invoke(new ActedInfo(info, chars));
    }

    public string ConjureInfo(Il2CppSystem.Collections.Generic.List<Character> characters)
    {
        return string.Format("I swapped #{0} and #{1}", characters[0].id, characters[1].id);
    }
    private void StopPick()
    {
        CharacterPicker.OnCharactersPicked -= action1;
        CharacterPicker.OnStopPick -= action2;
        CharacterPicker.OnCharactersPicked -= action3;
    }
    public Swapper() : base(ClassInjector.DerivedConstructorPointer<Swapper>())
    {
        ClassInjector.DerivedConstructorBody((Il2CppObjectBase)this);
        action1 = new System.Action(CharacterPicked);
        action2 = new System.Action(StopPick);
        action3 = new System.Action(CharacterPickedLiar);
    }

    public Swapper(System.IntPtr ptr) : base(ptr)
    {
        action1 = new System.Action(CharacterPicked);
        action2 = new System.Action(StopPick);
        action3 = new System.Action(CharacterPickedLiar);
    }
}