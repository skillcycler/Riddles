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
public class Recruiter : Role
{
    public int removedOutcast = 0;
    public override string Description
    {
        get
        {
            return "";
        }
    }
    public override ActedInfo GetInfo(Character charRef)
    {
        string info = string.Format("I turned #{0} into a villager", removedOutcast);
        if (removedOutcast == 0)
        {
            info = "There are no Outcasts in this village";
            //failsafe for if there are outcasts but they aren't converted
            bool thereIsAnOutcast = false;
            List<int> ids = new();
            foreach (Character c in Gameplay.CurrentCharacters)
            {
                if (c.dataRef.type == ECharacterType.Outcast) { 
                    thereIsAnOutcast = true;
                    ids.Add(c.id);
                }
            }
            if (thereIsAnOutcast)
            {
                info = $"#{ids[UnityEngine.Random.RandomRangeInt(0, ids.Count)]} rejected my offer to join the village";
            }
        }
        ActedInfo actedInfo = new ActedInfo(info);
        return actedInfo;
    }

    public override ActedInfo GetBluffInfo(Character charRef)
    {
        string info = string.Format("I turned #{0} into a villager", Calculator.RemoveNumberAndGetRandomNumberFromList(charRef.id, 1, Gameplay.CurrentCharacters.Count + 1));
        ActedInfo actedInfo = new ActedInfo(info);
        return actedInfo;
    }

    public override void Act(ETriggerPhase trigger, Character charRef)
    {
        if (trigger == ETriggerPhase.Start)
        {
            Il2CppSystem.Collections.Generic.List<Character> characters = Gameplay.CurrentCharacters;
            Il2CppSystem.Collections.Generic.List<Character> outcasts = new Il2CppSystem.Collections.Generic.List<Character>();
            foreach (Character character in characters)
            {
                if (character.dataRef.type == ECharacterType.Outcast)
                {
                    outcasts.Add(character);
                }
            }
            if (outcasts.Count > 0)
            {
                Character chosen = outcasts[UnityEngine.Random.RandomRangeInt(0, outcasts.Count)];
                removedOutcast = chosen.id;
                CharacterData newVillager = Characters.Instance.GetRandomUniqueVillagerBluff();
                Gameplay.Instance.AddScriptCharacterIfAble(ECharacterType.Villager, newVillager);
                chosen.Init(newVillager);
            }
        }
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
    public Recruiter() : base(ClassInjector.DerivedConstructorPointer<Recruiter>())
    {
        ClassInjector.DerivedConstructorBody((Il2CppObjectBase)this);
    }

    public Recruiter(System.IntPtr ptr) : base(ptr)
    {

    }
}