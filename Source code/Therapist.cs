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
public class Therapist : Role
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
        Il2CppSystem.Collections.Generic.List<Character> characters = Gameplay.CurrentCharacters;

        int id1 = 1;
        int id2 = 2;
        int highScore = 0;
        foreach (Character character in characters) { 
            foreach (Character compare in characters)
            {
                // What we can compare: Alignment (very important), Type, Truthfulness, Honesty
                // Corruption (as a tiebreaker, not including Confused), Accused/Confused (2nd tiebreaker), Any other statuses (3rd tiebreaker), Randomly pick (4th tiebreaker)
                int score = 0;
                if (character.GetRegisterAlignment() != compare.GetRegisterAlignment()) score += 5000000;
                if (character.GetType() != compare.GetType()) score += 1000000;
                if (CharacterHelper.CheckLyingAppearance(character) != CharacterHelper.CheckLyingAppearance(compare)) score += 1000000;
                if (CharacterHelper.CheckIfDisguisedAppearance(character) != CharacterHelper.CheckIfDisguisedAppearance(compare)) score += 1000000;
                // 1st tiebreaker
                if ((character.statuses.Contains(ECharacterStatus.Corrupted) && !character.statuses.Contains(Confused.confused)) != 
                    (compare.statuses.Contains(ECharacterStatus.Corrupted) && !compare.statuses.Contains(Confused.confused))) score += 500000;
                // 2nd tiebreaker
                if (character.statuses.Contains(Confused.confused) != compare.statuses.Contains(Confused.confused)) score += 200000;
                if (character.statuses.Contains(Accused.accused) != compare.statuses.Contains(Accused.accused)) score += 200000;
                // 3rd tiebreaker
                foreach (ECharacterStatus status in character.statuses.statuses)
                {
                    if (!compare.statuses.Contains(status)) score++;
                }
                foreach (ECharacterStatus status in compare.statuses.statuses)
                {
                    if (!character.statuses.Contains(status)) score++;
                }
                if (score > highScore)
                {
                    highScore = score;
                    id1 = character.id;
                    id2 = compare.id;
                }
            }
        }
        
        string info = string.Format("#{0} and #{1} are the most different from each other", id1, id2);
        ActedInfo actedInfo = new ActedInfo(info);
        return actedInfo;
    }

    public override ActedInfo GetBluffInfo(Character charRef)
    {
        // 2 of the same alignment
        int id1 = Calculator.RollDice(Gameplay.CurrentCharacters.Count);
        int id2 = Calculator.RemoveNumberAndGetRandomNumberFromList(id1, 1, Gameplay.CurrentCharacters.Count);
        while (Gameplay.CurrentCharacters[id1 - 1].GetRegisterAlignment() != Gameplay.CurrentCharacters[id2 - 1].GetRegisterAlignment())
        {
            id1 = Calculator.RollDice(Gameplay.CurrentCharacters.Count);
            id2 = Calculator.RemoveNumberAndGetRandomNumberFromList(id1, 1, Gameplay.CurrentCharacters.Count);
        }
        string info = string.Format("#{0} and #{1} are the most different from each other", id1, id2);
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
    public Therapist() : base(ClassInjector.DerivedConstructorPointer<Therapist>())
    {
        ClassInjector.DerivedConstructorBody((Il2CppObjectBase)this);
    }

    public Therapist(System.IntPtr ptr) : base(ptr)
    {

    }
}