using System;
using System.ComponentModel.Design;
using HarmonyLib;
using Il2Cpp;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.Injection;
using Il2CppInterop.Runtime.InteropTypes;
using Il2CppSystem;
using MelonLoader;
using UnityEngine;
using static MelonLoader.MelonLogger;


namespace RiddlerMod;

[RegisterTypeInIl2Cpp]
public class Enigma : Minion
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
            Il2CppSystem.Collections.Generic.List<CharacterData> allDatas = Gameplay.Instance.GetAscensionAllStartingCharacters();
            Il2CppSystem.Collections.Generic.List<CharacterData> inPlayCharacters = new Il2CppSystem.Collections.Generic.List<CharacterData>();
            foreach (Character character in Gameplay.CurrentCharacters)
            {
                inPlayCharacters.Add(character.dataRef);
            }
            foreach (CharacterData character in Gameplay.Instance.GetScriptCharacters())
            {
                inPlayCharacters.Add(character);
            }
            Il2CppSystem.Collections.Generic.List<CharacterData> fake = new();
            

            foreach (CharacterData d in allDatas)
            {
                if (!inPlayCharacters.Contains(d) && d.type != ECharacterType.Villager && !(d.type == ECharacterType.Outcast && d.usuallyDisguised == false))
                {
                    fake.Add(d);
                }
            }
            for (int i = 0; i < charRef.id % 10; i++)
            {
                CharacterData add = fake[UnityEngine.Random.RandomRangeInt(0, fake.Count)];
                fake.Remove(add);
                Gameplay.Instance.AddScriptCharacter(add.type, add);
            }
        }
    }

    public Enigma() : base(ClassInjector.DerivedConstructorPointer<Enigma>())
    {
        ClassInjector.DerivedConstructorBody((Il2CppObjectBase)this);
    }
    public Enigma(System.IntPtr ptr) : base(ptr) { }

}