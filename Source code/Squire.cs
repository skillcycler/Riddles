using System;
using System.ComponentModel.Design;
using HarmonyLib;
using Il2Cpp;
using Il2CppInterop.Runtime.Injection;
using Il2CppInterop.Runtime.InteropTypes;
using Il2CppSystem;
using MelonLoader;
using UnityEngine;
using static MelonLoader.MelonLogger;


namespace RiddlerMod;

[RegisterTypeInIl2Cpp]
public class Squire : Minion
{
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
    }
    public override CharacterData GetBluffIfAble(Character charRef)
    {
        charRef.statuses.AddStatus(ECharacterStatus.AppearTruthfull, charRef);
        charRef.statuses.AddStatus(ECharacterStatus.AppearHonest, charRef);
        return ProjectContext.Instance.gameData.GetCharacterDataOfId("Knight_47970624");
    }
    public override CharacterData GetRegisterAsRole(Character charRef)
    {
        return ProjectContext.Instance.gameData.GetCharacterDataOfId("Knight_47970624");
    }
    public override bool CheckIfCanBeKilled(Character charRef)
    {
        bool evilsAlive = false;
        foreach (Character character in Gameplay.CurrentCharacters)
        {
            // don't want this interacting weirdly with mad scientist undying
            if (!character.statuses.Contains(SpecialMadScientistTags.hasUndyingAbility))
            {
                if (character.state != ECharacterState.Dead && character.alignment == EAlignment.Evil && !Djinn.GetCharactersThatCannotDie().Contains(character.dataRef.characterId))
                {
                    evilsAlive = true;
                }
            }
        }
        return !evilsAlive;
    }
    public Squire() : base(ClassInjector.DerivedConstructorPointer<Squire>())
    {
        ClassInjector.DerivedConstructorBody((Il2CppObjectBase)this);
    }
    public Squire(System.IntPtr ptr) : base(ptr) { }

}