using System;
using System.ComponentModel.Design;
using HarmonyLib;
using Il2Cpp;
using Il2CppInterop.Runtime.Injection;
using Il2CppInterop.Runtime.InteropTypes;
using Il2CppSystem;
using MelonLoader;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class Infestation : Demon
{
    public override Il2CppSystem.Collections.Generic.List<SpecialRule> GetRules()
    {
        Il2CppSystem.Collections.Generic.List<SpecialRule> sr = new Il2CppSystem.Collections.Generic.List<SpecialRule>();
        sr.Add(new NightModeRule(4));
        return sr;
    }
    public override void Act(ETriggerPhase trigger, Character charRef)
    {
        if (trigger == ETriggerPhase.Start)
        {
            // corrupt someone
            Il2CppSystem.Collections.Generic.List<Character> villagers = new();
            foreach (Character c in Gameplay.CurrentCharacters)
            {
                villagers.Add(c);
            }
            villagers = Characters.Instance.FilterCharacterType(villagers, ECharacterType.Villager);
            villagers = Characters.Instance.FilterCharacterMissingStatus(villagers, ECharacterStatus.Corrupted);
            if (villagers.Count <= 0) return;

            Character randomCharacter = villagers[UnityEngine.Random.Range(0, villagers.Count)];
            randomCharacter.statuses.AddStatus(ECharacterStatus.Corrupted, charRef);
            randomCharacter.statuses.AddStatus(ECharacterStatus.MessedUpByEvil, charRef);
        }
        if (trigger == ETriggerPhase.Night)
        {
            if (charRef.state == ECharacterState.Dead) return;
            // corrupt characters adjacent to corruption, then kill previously corrupted characters
            List<int> newCorruptions = new List<int>();
            List<int> oldCorruptions = new List<int>();
            foreach (Character c in Gameplay.CurrentCharacters)
            {
                if (c.statuses.Contains(ECharacterStatus.Corrupted) && c.alignment == EAlignment.Good)
                {
                    oldCorruptions.Add(c.id);
                }
                else
                {
                    bool shouldGetCorrupted = false;
                    foreach (Character adjacent in Characters.Instance.GetAdjacentCharacters(c))
                    {
                        if (adjacent.statuses.Contains(ECharacterStatus.Corrupted) && adjacent.state != ECharacterState.Dead)
                            shouldGetCorrupted = true;
                    }
                    if (shouldGetCorrupted && c.alignment == EAlignment.Good)
                        newCorruptions.Add(c.id);
                }
            }
            foreach (Character c in Gameplay.CurrentCharacters)
            {
                if (oldCorruptions.Contains(c.id) && c.state != ECharacterState.Dead) {
                    c.statuses.AddStatus(ECharacterStatus.KilledByEvil, charRef);
                    c.statuses.AddStatus(ECharacterStatus.MessedUpByEvil, charRef);
                    c.KillByDemon(charRef);
                    PlayerController.PlayerInfo.health.Damage(1);
                }
                else if (newCorruptions.Contains(c.id))
                    c.statuses.AddStatus(ECharacterStatus.Corrupted, charRef);
            }
        }
    }
    public Infestation() : base(ClassInjector.DerivedConstructorPointer<Infestation>())
    {
        ClassInjector.DerivedConstructorBody((Il2CppObjectBase)this);
    }
    public Infestation(System.IntPtr ptr) : base(ptr) { }
}