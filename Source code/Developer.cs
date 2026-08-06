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
public class Developer : Role
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
        Il2CppSystem.Collections.Generic.List<Character> characters = Gameplay.CurrentCharacters;
        // current list of mods: Riddles, Wingidon's Expansion Pack, Dupery Bluff, Powerplay, Circus, The Salem Trials
        // Requirements: Latest update after June 15th, 2026 & At least 1 modded character
        int riddles = 0;
        int wingidon = 0;
        int dupery = 0;
        int powerplay = 0;
        int circus = 0;
        int salem = 0;
        foreach (Character character in characters) {
            if (character.dataRef.characterId.EndsWith("_scm")) riddles++;
            else if (character.dataRef.characterId.EndsWith("_WING")) wingidon++;
            else if (character.dataRef.characterId.EndsWith("_POW")) powerplay++;
            else if (character.dataRef.characterId.EndsWith("_LRZH")) circus++;
            else if (character.dataRef.characterId.EndsWith("_TST")) salem++;
            else if (character.dataRef.characterId.StartsWith("WING_Dupery_")) dupery++;
        }
        List<string> possible = new();
        bool failsafeLie = (riddles == 0);
        if (!truth)
        {
            if (riddles > 0) riddles += (Calculator.RollDice(2) * 2 - 3);
            if (wingidon > 0) wingidon += (Calculator.RollDice(2) * 2 - 3);
            if (dupery > 0) dupery += (Calculator.RollDice(2) * 2 - 3);
            if (powerplay > 0) powerplay += (Calculator.RollDice(2) * 2 - 3);
            if (circus > 0) circus += (Calculator.RollDice(2) * 2 - 3);
            if (salem > 0) salem += (Calculator.RollDice(2) * 2 - 3);
        }
        if (riddles > 0) possible.Add($"{riddles} character{((riddles == 1) ? " is" : "s are")} from Riddles");
        if (wingidon > 0) possible.Add($"{wingidon} character{((wingidon == 1) ? " is":"s are")} from Wingidon's Expansion Pack");
        if (dupery > 0) possible.Add($"{dupery} character{((dupery == 1) ? " is":"s are")} from Dupery Bluff");
        if (powerplay > 0) possible.Add($"{powerplay} character{((powerplay == 1) ? " is":"s are")} from Powerplay");
        if (circus > 0) possible.Add($"{circus} character{((circus == 1) ? " is":"s are")} from Circus");
        if (salem > 0) possible.Add($"{salem} character{((salem == 1) ? " is" : "s are")} from The Salem Trials");
        if (possible.Count == 0)
        {
            if (!truth && failsafeLie) return "1 character is from Riddles";
            return "There are NO modded characters"; //hmm... this would be pretty weird considering Developer is a modded character
        }
        return possible[UnityEngine.Random.RandomRangeInt(0, possible.Count)];
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
    public Developer() : base(ClassInjector.DerivedConstructorPointer<Developer>())
    {
        ClassInjector.DerivedConstructorBody((Il2CppObjectBase)this);
    }

    public Developer(System.IntPtr ptr) : base(ptr)
    {

    }
}