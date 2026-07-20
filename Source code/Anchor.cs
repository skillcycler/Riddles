using System;
using System.Linq;
using System.Reflection;
using Il2Cpp;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.Injection;
using Il2CppInterop.Runtime.InteropTypes;
using Il2CppSystem.Reflection;
using MelonLoader;
using UnityEngine;
using static Il2CppSystem.Collections.SortedList;
using static MelonLoader.Modules.MelonModule;
using HarmonyLib;

namespace RiddlerMod;

[RegisterTypeInIl2Cpp]
public class Anchor : Role
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
        if (trigger == ETriggerPhase.Start)
        {
            Health health = PlayerController.PlayerInfo.health;
            int currentMaxHp = health.value.GetValue();
            health.AddMaxHp(9 - currentMaxHp);
        }
    }
    public override void BluffAct(ETriggerPhase trigger, Character charRef)
    {
    }
    public Anchor() : base(ClassInjector.DerivedConstructorPointer<Anchor>())
    {
        ClassInjector.DerivedConstructorBody((Il2CppObjectBase)this);
    }
    public Anchor(System.IntPtr ptr) : base(ptr)
    {

    }
}