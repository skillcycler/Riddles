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
using UnityEngine.Playables;

namespace RiddlerMod;

[RegisterTypeInIl2Cpp]
public class Preacher : Role
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

        Il2CppSystem.Collections.Generic.List<Character> chars = new Il2CppSystem.Collections.Generic.List<Character>();
        chars.Add(CharacterPicker.PickedCharacters[0]);
        Character c = chars[0];
        if (c.state == ECharacterState.Dead)
        {
            return;
        }
        c.GiveBluff(ProjectContext.Instance.gameData.GetCharacterDataOfId("Confessor_18741708"));
        c.RevealBluff();
        c.RefreshCharacter();
        c.Act(ETriggerPhase.Day);
        string inf = string.Format("#{0} is now a Confessor", c.id);
        onActed?.Invoke(new ActedInfo(inf, chars));
    }

    public static ECharacterStatus fakePreacher = (ECharacterStatus)910;
    private void CharacterPickedLiar()
    {

        CharacterPicker.OnCharactersPicked -= action3;
        CharacterPicker.OnStopPick -= action2;
        Il2CppSystem.Collections.Generic.List<Character> chars = new Il2CppSystem.Collections.Generic.List<Character>();
        chars.Add(CharacterPicker.PickedCharacters[0]);
        Character c = chars[0];
        if (c.state == ECharacterState.Dead)
        {
            return;
        }
        c.statuses.AddStatus(fakePreacher, charRef);
        c.GiveBluff(ProjectContext.Instance.gameData.GetCharacterDataOfId("Confessor_18741708"));
        c.RevealBluff();
        c.RefreshCharacter();
        c.Act(ETriggerPhase.Day);
        string inf = string.Format("#{0} is now a Confessor", c.id);
        onActed?.Invoke(new ActedInfo(inf, chars));
    }
    private void StopPick()
    {
        CharacterPicker.OnCharactersPicked -= action1;
        CharacterPicker.OnStopPick -= action2;
        CharacterPicker.OnCharactersPicked -= action3;
    }
    public Preacher() : base(ClassInjector.DerivedConstructorPointer<Preacher>())
    {
        ClassInjector.DerivedConstructorBody((Il2CppObjectBase)this);
        action1 = new System.Action(CharacterPicked);
        action2 = new System.Action(StopPick);
        action3 = new System.Action(CharacterPickedLiar);
    }

    public Preacher(System.IntPtr ptr) : base(ptr)
    {
        action1 = new System.Action(CharacterPicked);
        action2 = new System.Action(StopPick);
        action3 = new System.Action(CharacterPickedLiar);
    }
}