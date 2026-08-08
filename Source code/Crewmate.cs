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

namespace RiddlerMod;

[RegisterTypeInIl2Cpp]
public class Crewmate : Role
{
    public override string Description
    {
        get
        {
            return "";
        }
    }
    public string MakeInfo(bool truth)
    {
        // list of who isn't a demon and can kill
        List<string> canAffectOthers = new();
        Il2CppSystem.Collections.Generic.List<Character> sus = new();
        Il2CppSystem.Collections.Generic.List<Character> notSus = new();
        canAffectOthers.Add("Gambler_42592744");

        canAffectOthers.Add("Nurse_scm");
        canAffectOthers.Add("Necromancer_scm");
        canAffectOthers.Add("MadScientist_scm");
        canAffectOthers.Add("Hitman_scm");

        canAffectOthers.Add("Revolutionary_WING");
        canAffectOthers.Add("Switchblade_WING");
        canAffectOthers.Add("Saboteur_WING");
        canAffectOthers.Add("Snake Charmer_WING");
        canAffectOthers.Add("Masquerade_WING");

        canAffectOthers.Add("Vigilante_POW");
        canAffectOthers.Add("Prosecutor_POW");
        canAffectOthers.Add("Veteran_POW");
        canAffectOthers.Add("Jinx_POW");
        canAffectOthers.Add("Slinger_POW");
        canAffectOthers.Add("Grenadier_POW");
        canAffectOthers.Add("Balancer_POW");
        canAffectOthers.Add("Gangster_POW");

        canAffectOthers.Add("WING_Dupery_Vigilante");

        List<ECharacterType> validTypes = new();
        validTypes.Add(ECharacterType.Minion);
        validTypes.Add(ECharacterType.Outcast);
        validTypes.Add(ECharacterType.Villager);
        // Demons & any other custom types are Sus.

        foreach (Character c in Gameplay.CurrentCharacters)
        {
            if (canAffectOthers.Contains(c.dataRef.characterId) || !validTypes.Contains(c.GetCharacterType()))
            {
                sus.Add(c);
            } else
            {
                notSus.Add(c);
            }
        }
        if (truth)
        {
            if (sus.Count == 0)
            {
                return "There are no Impostors.";
            }
            return $"#{sus[UnityEngine.Random.RandomRangeInt(0, sus.Count)].id} is Sus";
        }
        else
        {
            return $"#{notSus[UnityEngine.Random.RandomRangeInt(0, notSus.Count)].id} is Sus";
        }
    }
    public override ActedInfo GetInfo(Character charRef)
    {
        string info = MakeInfo(true);
        ActedInfo actedInfo = new ActedInfo(info);
        return actedInfo;
    }

    public override ActedInfo GetBluffInfo(Character charRef)
    {
        string info = MakeInfo(false);
        ActedInfo actedInfo = new ActedInfo(info);
        return actedInfo;
    }

    public override void Act(ETriggerPhase trigger, Character charRef)
    {
        if (trigger == ETriggerPhase.Day)
        {
            onActed.Invoke(GetInfo(charRef));
        }
    }
    public override void BluffAct(ETriggerPhase trigger, Character charRef)
    {
        if (trigger == ETriggerPhase.Day)
        {
            onActed.Invoke(GetBluffInfo(charRef));
        }
    }
    public Crewmate() : base(ClassInjector.DerivedConstructorPointer<Crewmate>())
    {
        ClassInjector.DerivedConstructorBody((Il2CppObjectBase)this);
    }

    public Crewmate(System.IntPtr ptr) : base(ptr)
    {

    }
}