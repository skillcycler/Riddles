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
using HarmonyLib;
using static MelonLoader.MelonLaunchOptions;

namespace RiddlerMod;

[RegisterTypeInIl2Cpp]
public class Sleeper : Minion
{
    public override string Description
    {
        get
        {
            return "";
        }
    }

    public override void Act(ETriggerPhase trigger, Character charRef)
    {
        if (charRef.state == ECharacterState.Dead)
        {
            return;
        }
        if (trigger == ETriggerPhase.Start) {
            var mod = MainMod.Instance;
            if (mod == null) return;
            mod.shortenNight = true;
        }
        if (trigger == ETriggerPhase.Night)
        {
            var mod = MainMod.Instance;
            if (mod == null) return;
            mod.shortenNight = true;
        }
    }

    public Sleeper() : base(ClassInjector.DerivedConstructorPointer<Sleeper>())
    {
        ClassInjector.DerivedConstructorBody((Il2CppObjectBase)this);
    }

    public Sleeper(System.IntPtr ptr) : base(ptr)
    {

    }
}