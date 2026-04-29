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

public class Veil : Demon
{
    public override void Act(ETriggerPhase trigger, Character charRef)
    {
        if (trigger != ETriggerPhase.Start) return;
        if (charRef.dataRef.characterId != "Veil_scm")
        {
            PlayerController.PlayerInfo.blocks.value.Add(1);
        }
        else if (Gameplay.CurrentCharacters.Count > 10)
        {
            PlayerController.PlayerInfo.blocks.value.Add(3);
        }
        else
        {
            PlayerController.PlayerInfo.blocks.value.Add(2);
        }
    }
    public override void ActOnDied(Character charRef)
    {
        if (charRef.dataRef.characterId != "Veil_scm")
        {
            PlayerController.PlayerInfo.blocks.value.Reduce(1);
        }
        else if (Gameplay.CurrentCharacters.Count > 10)
        {
            PlayerController.PlayerInfo.blocks.value.Reduce(3);
        }
        else
        {
            PlayerController.PlayerInfo.blocks.value.Reduce(2);
        }
    }
    public Veil() : base(ClassInjector.DerivedConstructorPointer<Veil>())
    {
        ClassInjector.DerivedConstructorBody((Il2CppObjectBase)this);
    }
    public Veil(System.IntPtr ptr) : base(ptr) { }
}