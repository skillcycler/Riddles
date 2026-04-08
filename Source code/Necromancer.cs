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
public class Necromancer : Role
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
        CharacterPicker.Instance.StartPickCharacters(1);
        CharacterPicker.OnCharactersPicked += action1;
        CharacterPicker.OnStopPick += action2;
    }
    public override void BluffAct(ETriggerPhase trigger, Character charRef)
    {
        if (trigger != ETriggerPhase.Day) return;
        chRef = charRef;
        CharacterPicker.Instance.StartPickCharacters(1);
        CharacterPicker.OnCharactersPicked += action3;
        CharacterPicker.OnStopPick += action2;
    }
    private void CharacterPicked()
    {
        CharacterPicker.OnCharactersPicked -= action1;
        CharacterPicker.OnStopPick -= action2;
        Character c1 = CharacterPicker.PickedCharacters[0];
        if (c1.GetAlignment() == EAlignment.Evil)
        {
            return;
        }

        if (c1.state != ECharacterState.Dead)
        {
            return;
        }
        if (c1.bluff)
        {
            if (c1.bluff.picking)
            {
                c1.uses = 1;
                c1.pickable.SetActive(true);
            }
        } else if (c1.dataRef.picking)
        {
            c1.uses = 1;
            c1.pickable.SetActive(true);
        }
        c1.state = ECharacterState.Alive;
        c1.InitWithNoReset(c1.GetRegisterAs());
        c1.Act(ETriggerPhase.Day);
        PlayerController.PlayerInfo.health.Damage(2);
        string info = ConjureInfo(c1);
        onActed?.Invoke(new ActedInfo(info));
    }
    private void CharacterPickedLiar()
    {

        CharacterPicker.OnCharactersPicked -= action3;
        CharacterPicker.OnStopPick -= action2;

        Character c1 = CharacterPicker.PickedCharacters[0];
        if (c1.GetAlignment() == EAlignment.Evil)
        {
            return;
        }

        if (c1.state != ECharacterState.Dead)
        {
            return;
        }
        if (c1.bluff)
        {
            if (c1.bluff.picking)
            {
                c1.uses = 1;
                c1.pickable.SetActive(true);
            }
        }
        else if (c1.dataRef.picking)
        {
            c1.uses = 1;
            c1.pickable.SetActive(true);
        }
        c1.state = ECharacterState.Alive;
        c1.statuses.AddStatus(ECharacterStatus.Corrupted, charRef);
        c1.statuses.statuses.Remove(ECharacterStatus.HealthyBluff);
        c1.InitWithNoReset(c1.GetRegisterAs());
        c1.Act(ETriggerPhase.Day);
        PlayerController.PlayerInfo.health.Damage(2);
        string info = ConjureInfo(c1);
        onActed?.Invoke(new ActedInfo(info));
    }

    public string ConjureInfo(Character character)
    {
        return string.Format("I revived #{0}", character.id);
    }
    private void StopPick()
    {
        CharacterPicker.OnCharactersPicked -= action1;
        CharacterPicker.OnStopPick -= action2;
        CharacterPicker.OnCharactersPicked -= action3;
    }
    public Necromancer() : base(ClassInjector.DerivedConstructorPointer<Necromancer>())
    {
        ClassInjector.DerivedConstructorBody((Il2CppObjectBase)this);
        action1 = new System.Action(CharacterPicked);
        action2 = new System.Action(StopPick);
        action3 = new System.Action(CharacterPickedLiar);
    }

    public Necromancer(System.IntPtr ptr) : base(ptr)
    {
        action1 = new System.Action(CharacterPicked);
        action2 = new System.Action(StopPick);
        action3 = new System.Action(CharacterPickedLiar);
    }
}