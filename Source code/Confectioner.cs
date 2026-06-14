using System;
using System.Diagnostics;
using HarmonyLib;
using Il2Cpp;
using Il2CppInterop.Runtime.Injection;
using Il2CppInterop.Runtime.InteropTypes;
using MelonLoader;
using UnityEngine;
using static Il2CppSystem.Collections.SortedList;
using static MelonLoader.MelonLogger;

namespace RiddlerMod;

[RegisterTypeInIl2Cpp]
public class Confectioner : Role
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
            Il2CppSystem.Collections.Generic.List<Character> characters = Gameplay.CurrentCharacters;
            Il2CppSystem.Collections.Generic.List<Character> goodVillagers = new Il2CppSystem.Collections.Generic.List<Character>();
            foreach (Character c in characters)
            {
                if (c.alignment == EAlignment.Good && c.GetRegisterAlignment() == EAlignment.Good && c.GetCharacterType() == ECharacterType.Villager)
                {
                    goodVillagers.Add(c);
                }
            }
            CharacterData bakerData = ProjectContext.Instance.gameData.GetCharacterDataOfId("Baker_22847064");
            if (goodVillagers.Count > 0)
            {
                Character ch = goodVillagers[UnityEngine.Random.RandomRangeInt(0, goodVillagers.Count)];
                ch.Init(bakerData);
                ch.statuses.AddStatus(ECharacterStatus.Corrupted, charRef);
            }
        }
    }
    public override CharacterData GetBluffIfAble(Character charRef)
    {
        charRef.statuses.AddStatus(ECharacterStatus.HealthyBluff, charRef);
        return ProjectContext.Instance.gameData.GetCharacterDataOfId("Baker_22847064");
    }
    public Confectioner() : base(ClassInjector.DerivedConstructorPointer<Confectioner>())
    {
        ClassInjector.DerivedConstructorBody((Il2CppObjectBase)this);
    }
    public Confectioner(IntPtr ptr) : base(ptr) { }
}