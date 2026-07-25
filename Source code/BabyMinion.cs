using Il2Cpp;
using Il2CppInterop.Runtime.Injection;
using Il2CppInterop.Runtime.InteropTypes;
using Il2CppSystem;
using MelonLoader;
using System;
using System.ComponentModel.Design;
using UnityEngine;
using HarmonyLib;

namespace RiddlerMod;

[RegisterTypeInIl2Cpp]
public class BabyMinion : Minion
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
    public BabyMinion() : base(ClassInjector.DerivedConstructorPointer<BabyMinion>())
    {
        ClassInjector.DerivedConstructorBody((Il2CppObjectBase)this);
    }
    public BabyMinion(System.IntPtr ptr) : base(ptr) { }

}