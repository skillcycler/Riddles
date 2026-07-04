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
public class Motivator : Role
{
    public override string Description
    {
        get
        {
            return "";
        }
    }
    public override void Act(ETriggerPhase trigger, Character charRef)
    {
        if (trigger == ETriggerPhase.Night && charRef.revealed)
        {
            Il2CppSystem.Collections.Generic.List<Character> neighbors = Characters.Instance.GetAdjacentCharacters(charRef);
            foreach (Character character in neighbors)
            {
                if (character.dataRef.picking)
                {
                    character.pickableUses = 1;
                    character.pickable.SetActive(true);
                }
                else if (character.bluff)
                {
                    if (character.bluff.picking)
                    {
                        character.pickableUses = 1;
                        character.pickable.SetActive(true);
                    } else {
                        // gonna reuse Stylist's code here
                        character.GiveBluff(character.bluff);
                        character.RevealBluff();
                        character.RefreshCharacter();
                        character.Act(ETriggerPhase.Day);
                    }
                } else
                    character.Act(ETriggerPhase.Day);
            }
        }
        if (trigger == ETriggerPhase.Day)
        {
            charRef.revealed = true;
        }
    }
    public override void BluffAct(ETriggerPhase trigger, Character charRef)
    {
        
    }
    public Motivator() : base(ClassInjector.DerivedConstructorPointer<Motivator>())
    {
        ClassInjector.DerivedConstructorBody((Il2CppObjectBase)this);
    }

    public Motivator(System.IntPtr ptr) : base(ptr)
    {

    }
}