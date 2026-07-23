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
        List<string> canAffectOthers = new();
        Il2CppSystem.Collections.Generic.List<Character> sus = new();
        Il2CppSystem.Collections.Generic.List<Character> notSus = new();
        canAffectOthers.Add("Plague Doctor_49312486");
        canAffectOthers.Add("Rambler_57930131");
        canAffectOthers.Add("Mezepheles_09511163");
        canAffectOthers.Add("Poisoner_64796285");
        canAffectOthers.Add("Shaman_26945607");
        canAffectOthers.Add("Baron_04539999");
        canAffectOthers.Add("Alchemist_94446803");
        canAffectOthers.Add("Baker_22847064");
        canAffectOthers.Add("Gambler_42592744");

        canAffectOthers.Add("Swapper_scm");
        canAffectOthers.Add("Lawyer_scm");
        canAffectOthers.Add("Stylist_scm");
        canAffectOthers.Add("Nurse_scm");
        canAffectOthers.Add("Recruiter_scm");
        canAffectOthers.Add("Necromancer_scm");
        canAffectOthers.Add("Motivator_scm");
        canAffectOthers.Add("MadScientist_scm");
        canAffectOthers.Add("Hitman_scm");
        canAffectOthers.Add("Ghost_scm");
        canAffectOthers.Add("Confectioner_scm");
        canAffectOthers.Add("Gambler_scm");
        canAffectOthers.Add("Accuser_scm");
        canAffectOthers.Add("Channeler_scm");
        canAffectOthers.Add("Mastermind_scm");
        canAffectOthers.Add("Guardian_scm");
        canAffectOthers.Add("Baffler_scm");
        canAffectOthers.Add("Wizard_scm");

        canAffectOthers.Add("Devout_WING");
        canAffectOthers.Add("Chatterbox_WING");
        canAffectOthers.Add("Mutant_WING");
        canAffectOthers.Add("Revolutionary_WING");
        canAffectOthers.Add("Switchblade_WING");
        canAffectOthers.Add("Saboteur_WING");
        canAffectOthers.Add("Snake Charmer_WING");
        canAffectOthers.Add("Swarm_Good_WING"); // lmao good swarm is sus but not evil swarm
        canAffectOthers.Add("Undying_WING");

        canAffectOthers.Add("Guard_POW");
        canAffectOthers.Add("Soldier_POW");
        canAffectOthers.Add("Prosecutor_POW");
        canAffectOthers.Add("Mayor_POW");
        canAffectOthers.Add("Flutist_POW");
        canAffectOthers.Add("Industrialist_POW");
        canAffectOthers.Add("Veteran_POW");
        canAffectOthers.Add("Pirate_POW");
        canAffectOthers.Add("Godfather_POW");
        canAffectOthers.Add("Psychopath_POW");
        canAffectOthers.Add("Supporter_POW");
        canAffectOthers.Add("Jinx_POW");
        canAffectOthers.Add("Slinger_POW");
        canAffectOthers.Add("Grenadier_POW");
        canAffectOthers.Add("Manipulator_POW");
        canAffectOthers.Add("Balancer_POW");
        canAffectOthers.Add("EvilTwin_POW");

        canAffectOthers.Add("Cleric_TST");
        canAffectOthers.Add("Plaguebearer_TST");
        canAffectOthers.Add("Inquisitor_TST");

        foreach (Character c in Gameplay.CurrentCharacters)
        {
            if (canAffectOthers.Contains(c.dataRef.characterId) || c.GetCharacterType() == ECharacterType.Demon)
            {
                sus.Add(c);
            } else
            {
                notSus.Add(c);
            }
        }
        if (truth)
        {
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