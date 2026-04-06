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

namespace RiddlerMod;

[RegisterTypeInIl2Cpp]
public class Sleeper : Role
{
    Character chRef;
    public override Il2CppSystem.Collections.Generic.List<SpecialRule> GetRules()
    {
        Il2CppSystem.Collections.Generic.List<SpecialRule> sr = new Il2CppSystem.Collections.Generic.List<SpecialRule>();
        sr.Add(new NightModeRule(3));
        return sr;
    }
    public override string Description
    {
        get
        {
            return "";
        }
    }
    public override ActedInfo GetInfo(Character charRef)
    {
        ActedInfo newInfo = infoRoles[UnityEngine.Random.Range(0, infoRoles.Count)].GetInfo(charRef);
        return newInfo;
    }

    public override void Act(ETriggerPhase trigger, Character charRef)
    {
        if (trigger == ETriggerPhase.Night)
        {
            onActed?.Invoke(GetInfo(charRef));
        }
        if (trigger != ETriggerPhase.Day) return;
        onActed?.Invoke(GetInfo(charRef));
    }
    public override void BluffAct(ETriggerPhase trigger, Character charRef)
    {
        if (trigger == ETriggerPhase.Night)
        {
            onActed?.Invoke(GetBluffInfo(charRef));
        }
        if (trigger != ETriggerPhase.Day) return;
        onActed?.Invoke(GetBluffInfo(charRef));
    }

    public override ActedInfo GetBluffInfo(Character charRef)
    {
        Role role = infoRoles[UnityEngine.Random.Range(0, infoRoles.Count)];
        ActedInfo newInfo = role.GetBluffInfo(charRef);
        return newInfo;
    }

    public List<Role> infoRoles = new List<Role>()
    {
        new Empath(),
        new Scout(),
        new Investigator(),
        new BountyHunter(),
        new Lookout(),
        new Knitter(),
        new Tracker(),
        new Shugenja(),
        new Noble(),
        new Bishop(),
        new Archivist(),
        new Acrobat2(),
    };
    public Sleeper() : base(ClassInjector.DerivedConstructorPointer<Sleeper>())
    {
        ClassInjector.DerivedConstructorBody((Il2CppObjectBase)this);
    }

    public Sleeper(System.IntPtr ptr) : base(ptr)
    {

    }
}