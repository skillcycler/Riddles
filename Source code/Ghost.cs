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
    public Character target;
    public override ActedInfo GetInfo(Character charRef)
    {
        if (!target)
        {
            return new ActedInfo("I couldn't haunt anyone");
        }
        return new ActedInfo(string.Format("I haunted #{0}", target.id));
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
            unrevealedCharacters = Characters.Instance.FilterCharacterType(unrevealedCharacters, ECharacterType.Villager);
            unrevealedCharacters = Characters.Instance.FilterAlignmentCharacters(unrevealedCharacters, EAlignment.Good);
            unrevealedCharacters = Characters.Instance.FilterCharacterMissingStatus(unrevealedCharacters, ECharacterStatus.Corrupted);
            charRef.RevealAllReal();
            charRef.RefreshCharacter();
            onActed.Invoke(GetInfo(charRef));
            if (unrevealedCharacters.Count == 0) return;
            target = unrevealedCharacters[UnityEngine.Random.RandomRangeInt(0, unrevealedCharacters.Count)];
            target.statuses.AddStatus(ECharacterStatus.Corrupted, charRef);
        }
    }
    public Ghost() : base(ClassInjector.DerivedConstructorPointer<Ghost>())
    {
        ClassInjector.DerivedConstructorBody((Il2CppObjectBase)this);
    }
    public Ghost(IntPtr ptr) : base(ptr) { }
}