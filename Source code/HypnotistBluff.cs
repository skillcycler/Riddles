using RiddlerMod;
using HarmonyLib;
using Il2Cpp;
using Il2CppFIMSpace.Basics;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.Injection;
using Il2CppInterop.Runtime.InteropTypes;
using Il2CppSystem;
using Il2CppSystem.Collections.Generic;
using Il2CppTMPro;
using MelonLoader;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Xml.Linq;
using UnityEngine;
using UnityEngine.Diagnostics;
using static Il2CppSystem.Globalization.CultureInfo;
using static MelonLoader.MelonLaunchOptions;

namespace RiddlerMod;
[RegisterTypeInIl2Cpp]
public class HypnotistBluff : Role
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
        int rng = UnityEngine.Random.Range(0, 100);
        if (charRef.bluff.name == "Confessor" && rng < 50)
        {
            ActedInfo newInfo = new ActedInfo("I am Good");
            return newInfo;
        }
        else if (charRef.bluff.name == "Confessor")
        {
            ActedInfo newInfo = new ActedInfo("I am dizzy");
            return newInfo;
        }
        else if (charRef.bluff.name == "Alchemist" && rng == 23)
        {
            ActedInfo newInfo = new ActedInfo("I cured 3 Corruptions");
            return newInfo;
        }
        else if (charRef.bluff.name == "Alchemist")
        {
            ActedInfo newInfo = new ActedInfo("I cured 0 Corruptions");
            return newInfo;
        }
        else if (charRef.bluff.name == "Baker")
        {
            ActedInfo newInfo = new ActedInfo("I am the original Baker");
            return newInfo;
        }
        else if (charRef.bluff.name == "Witness")
        {
            ActedInfo newInfo = new ActedInfo("NO character was affected by an Evil");
            return newInfo;
        }
        else
        {
            ActedInfo newInfo = new ActedInfo("I am literally the Drunk fr");
            return newInfo;
        }
    }

    public override ActedInfo GetBluffInfo(Character charRef)
    {
        return new ActedInfo("", null);
    }

    public override void Act(ETriggerPhase trigger, Character charRef)
    {
        if (trigger != ETriggerPhase.Day) return;
        onActed?.Invoke(GetInfo(charRef));
    }

    public override CharacterData GetBluffIfAble(Character charRef)
    {
        return null;
    }

    public HypnotistBluff() : base(ClassInjector.DerivedConstructorPointer<HypnotistBluff>())
    {
        ClassInjector.DerivedConstructorBody((Il2CppObjectBase)this);

    }

    public HypnotistBluff(System.IntPtr ptr) : base(ptr)
    {

    }
}