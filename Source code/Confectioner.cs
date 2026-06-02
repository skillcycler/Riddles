using System;
using System.Diagnostics;
using HarmonyLib;
using Il2Cpp;
using Il2CppInterop.Runtime.Injection;
using Il2CppInterop.Runtime.InteropTypes;
using MelonLoader;
using UnityEngine;
using static Il2CppSystem.Collections.SortedList;
using static MelonLoader.MelonLogger;

namespace RiddlerMod;

[RegisterTypeInIl2Cpp]
public class Confectioner : Role
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
        if (trigger == ETriggerPhase.Start)
        {
            Baker b = new();
            b.CreateNewBaker(charRef);
        }
    }
    public override CharacterData GetBluffIfAble(Character charRef)
    {
        charRef.statuses.AddStatus(ECharacterStatus.HealthyBluff, charRef);
        return ProjectContext.Instance.gameData.GetCharacterDataOfId("Baker_22847064");
    }
    public Confectioner() : base(ClassInjector.DerivedConstructorPointer<Confectioner>())
    {
        ClassInjector.DerivedConstructorBody((Il2CppObjectBase)this);
    }
    public Confectioner(IntPtr ptr) : base(ptr) { }
}