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
public class Lawyer : Role
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
        
        Il2CppSystem.Collections.Generic.List<Character> truthfulCharacters = new Il2CppSystem.Collections.Generic.List<Character>();
        foreach (Character character in characters)
        {
            bool lying = false;
            if (character.bluff)
                lying = true;
            if (character.statuses.Contains(ECharacterStatus.Corrupted))
                lying = true;
            if (character.statuses.Contains(ECharacterStatus.HealthyBluff))
                lying = false;
            if (character.dataRef.role is Confessor)
                lying = false;
            if (character.bluff)
                if (character.bluff.role is Confessor)
                    lying = false;
            if (!lying)
            {
                truthfulCharacters.Add(character);
            }
        }
        if (truthfulCharacters.Count > 1)
            truthfulCharacters.Remove(charRef);
        Character chosenCharacter = truthfulCharacters[UnityEngine.Random.RandomRangeInt(0, truthfulCharacters.Count)];
        
        string info = string.Format("#{0} is Truthful", chosenCharacter.id);
        ActedInfo actedInfo = new ActedInfo(info);
        return actedInfo;
    }

    public override ActedInfo GetBluffInfo(Character charRef)
    {
        Il2CppSystem.Collections.Generic.List<Character> characters = Gameplay.CurrentCharacters;
        
        Il2CppSystem.Collections.Generic.List<Character> untruthfulCharacters = new Il2CppSystem.Collections.Generic.List<Character>();
        foreach (Character character in characters)
        {
            bool lying = false;
            if (character.bluff)
                lying = true;
            if (character.statuses.Contains(ECharacterStatus.Corrupted))
                lying = true;
            if (character.statuses.Contains(ECharacterStatus.HealthyBluff))
                lying = false;
            if (character.dataRef.role is Confessor)
                lying = false;
            if (character.bluff)
                if (character.bluff.role is Confessor)
                    lying = false;
            if (lying)
            {
                untruthfulCharacters.Add(character);
            }
        }
        if (untruthfulCharacters.Count > 1)
            untruthfulCharacters.Remove(charRef);
        Character chosenCharacter = untruthfulCharacters[UnityEngine.Random.RandomRangeInt(0, untruthfulCharacters.Count)];

        string info = string.Format("#{0} is Truthful", chosenCharacter.id);
        ActedInfo actedInfo = new ActedInfo(info);
        return actedInfo;
    }
    private async void postSetup(Character charRef)
    {
        await Task.Delay(1200);
        Il2CppSystem.Collections.Generic.List<Character> adjacentCharacters = Characters.Instance.GetAdjacentCharacters(charRef);
        foreach (Character character in adjacentCharacters)
        {
            if (!character.bluff)
                character.statuses.AddStatus(ECharacterStatus.HealthyBluff, charRef);
        }
        return;
    }
    public override void Act(ETriggerPhase trigger, Character charRef)
    {
        if (trigger == ETriggerPhase.Start)
        {
            Il2CppSystem.Collections.Generic.List<Character> adjacentCharacters = Characters.Instance.GetAdjacentCharacters(charRef);
            foreach (Character character in adjacentCharacters)
            {
                character.statuses.AddStatus(ECharacterStatus.HealthyBluff, charRef);
                character.statuses.statuses.Remove(ECharacterStatus.Corrupted);
                
            }
            postSetup(charRef);
        }    
        if (trigger == ETriggerPhase.Day)
        {
            onActed.Invoke(GetInfo(charRef));
        }
    }
    public override void BluffAct(ETriggerPhase trigger, Character charRef)
    {
        if (trigger == ETriggerPhase.Start)
        {
            Il2CppSystem.Collections.Generic.List<Character> adjacentCharacters = Characters.Instance.GetAdjacentCharacters(charRef);
            foreach (Character character in adjacentCharacters)
            {
                character.statuses.AddStatus(ECharacterStatus.Corrupted, charRef);
                character.statuses.statuses.Remove(ECharacterStatus.HealthyBluff);
            }
        }
        if (trigger == ETriggerPhase.Day)
        {
            onActed.Invoke(GetBluffInfo(charRef));
        }
    }
    public Lawyer() : base(ClassInjector.DerivedConstructorPointer<Lawyer>())
    {
        ClassInjector.DerivedConstructorBody((Il2CppObjectBase)this);
    }

    public Lawyer(System.IntPtr ptr) : base(ptr)
    {

    }
}