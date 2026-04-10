using System.Diagnostics;
using HarmonyLib;
using Il2CppInterop.Runtime.Injection;
using Il2CppInterop.Runtime.InteropTypes;
using MelonLoader;
using UnityEngine;
using static MelonLoader.MelonLogger;

namespace RiddlerMod;

[RegisterTypeInIl2Cpp]
public class Ghost : Role
{
    public int target = 0;
    public override ActedInfo GetInfo(Character charRef)
    {
        if (target == 0)
        {
            return new ActedInfo("I couldn't haunt anyone");
        }
        return new ActedInfo(string.Format("I haunted #{0}", target));
    }
    public override ActedInfo GetBluffInfo(Character charRef)
    {
        return new ActedInfo("This text should literally never appear");
    }
    public override string Description
    {
        get
        {
            return "";
        }
    }
    public override void Act(ETriggerPhase trigger, Character charRef)
    {
        if (trigger == ETriggerPhase.Day)
        {
            charRef.state = ECharacterState.Dead;
            PlayerController.PlayerInfo.health.Damage(1);
            Il2CppSystem.Collections.Generic.List<Character> unrevealedCharacters = Characters.Instance.FilterHiddenCharacters(Gameplay.CurrentCharacters);
            unrevealedCharacters = Characters.Instance.FilterAlignmentCharacters(unrevealedCharacters, EAlignment.Good);
            unrevealedCharacters = Characters.Instance.FilterCharacterMissingStatus(unrevealedCharacters, ECharacterStatus.Corrupted);
            charRef.RevealAllReal();
            charRef.RefreshCharacter();
            if (unrevealedCharacters.Count == 0)
            {
                onActed.Invoke(GetInfo(charRef));
                return;
            }
            Character targetChar = unrevealedCharacters[UnityEngine.Random.RandomRangeInt(0, unrevealedCharacters.Count)];
            targetChar.statuses.AddStatus(ECharacterStatus.Corrupted, charRef);
            targetChar.statuses.statuses.Remove(ECharacterStatus.HealthyBluff);
            target = targetChar.id;
            onActed.Invoke(GetInfo(charRef));
        }
    }
    public Ghost() : base(ClassInjector.DerivedConstructorPointer<Ghost>())
    {
        ClassInjector.DerivedConstructorBody((Il2CppObjectBase)this);
    }
    public Ghost(IntPtr ptr) : base(ptr) { }
}