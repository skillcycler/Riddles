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
using static UnityEngine.GraphicsBuffer;

namespace RiddlerMod;

[RegisterTypeInIl2Cpp]
public class Necromancer : Role
{    
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
        CharacterPicker.Instance.StartPickCharacters(2, charRef);
        CharacterPicker.OnCharactersPicked += action1;
        CharacterPicker.OnStopPick += action2;
    }
    public override void BluffAct(ETriggerPhase trigger, Character charRef)
    {
        if (trigger != ETriggerPhase.Day) return;
        CharacterPicker.Instance.StartPickCharacters(2, charRef);
        CharacterPicker.OnCharactersPicked += action3;
        CharacterPicker.OnStopPick += action2;
    }
    private void CharacterPicked()
    {
        CharacterPicker.OnCharactersPicked -= action1;
        CharacterPicker.OnStopPick -= action2;
        Character c1 = CharacterPicker.PickedCharacters[0];
        Character c2 = CharacterPicker.PickedCharacters[1];
        if (c1.state == ECharacterState.Dead && c2.state == ECharacterState.Dead) return;
        if (c1.state != ECharacterState.Dead && c2.state != ECharacterState.Dead) return;
        Character dead = c1;
        if (c1.state != ECharacterState.Dead) dead = c2;
        Character alive = c2;
        if (c1.state != ECharacterState.Dead) alive = c1;
        if (dead.GetRegisterAlignment() == EAlignment.Evil || dead.dataRef.characterId == "Ghost_scm" || dead.alignment == EAlignment.Evil)
        {
            return;
        }

        PlayerController.PlayerInfo.health.Damage(Calculator.RollDice(3) - 1);
        alive.KillByDemon(charRef);
        alive.Reveal();
        alive.onReveal.Invoke();
        alive.RevealReal();
        

        if (dead.bluff)
        {
            if (dead.bluff.picking)
            {
                dead.pickableUses = 1;
                dead.pickable.SetActive(true);
            }
        }
        else if (dead.dataRef.picking)
        {
            dead.pickableUses = 1;
            dead.pickable.SetActive(true);
        }
        dead.state = ECharacterState.Alive;
        dead.InitWithNoReset(dead.dataRef);
        dead.Act(ETriggerPhase.Day);
        string info = ConjureInfo(alive, dead);
        onActed?.Invoke(new ActedInfo(info));
        
    }
    private void CharacterPickedLiar()
    {

        CharacterPicker.OnCharactersPicked -= action3;
        CharacterPicker.OnStopPick -= action2;
        Character c1 = CharacterPicker.PickedCharacters[0];
        Character c2 = CharacterPicker.PickedCharacters[1];
        if (c1.state == ECharacterState.Dead && c2.state == ECharacterState.Dead) return;
        if (c1.state != ECharacterState.Dead && c2.state != ECharacterState.Dead) return;
        Character dead = c1;
        if (c1.state != ECharacterState.Dead) dead = c2;
        Character alive = c2;
        if (c1.state != ECharacterState.Dead) alive = c1;
        if (dead.GetRegisterAlignment() == EAlignment.Evil || dead.dataRef.characterId == "Ghost_scm" || dead.alignment == EAlignment.Evil)
        {
            return;
        }

        PlayerController.PlayerInfo.health.Damage(Calculator.RollDice(3)-1);
        alive.KillByDemon(charRef);
        alive.Reveal();
        alive.onReveal.Invoke();
        alive.RevealReal();


        if (dead.bluff)
        {
            if (dead.bluff.picking)
            {
                dead.pickableUses = 1;
                dead.pickable.SetActive(true);
            }
        }
        else if (dead.dataRef.picking)
        {
            dead.pickableUses = 1;
            dead.pickable.SetActive(true);
        }
        dead.statuses.AddStatus(ECharacterStatus.Corrupted, charRef);
        dead.statuses.statuses.Remove(ECharacterStatus.HealthyBluff);
        dead.state = ECharacterState.Alive;
        dead.InitWithNoReset(dead.dataRef);
        dead.Act(ETriggerPhase.Day);
        string info = ConjureInfo(alive, dead);
        onActed?.Invoke(new ActedInfo(info));
    }

    public string ConjureInfo(Character alive, Character dead)
    {
        return string.Format("I killed #{0} and revived #{1}", alive.id, dead.id);
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