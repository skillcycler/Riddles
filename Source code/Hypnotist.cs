using System;
using System.ComponentModel.Design;
using HarmonyLib;
using Il2Cpp;
using Il2CppFIMSpace.Basics;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.Injection;
using Il2CppInterop.Runtime.InteropTypes;
using Il2CppSystem;
using Il2CppSystem.Collections.Generic;
using MelonLoader;
using UnityEngine;
using static MelonLoader.MelonLogger;

public class Hypnotist : Minion
{
    public CharacterData[] allDatas = Il2CppSystem.Array.Empty<CharacterData>();
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
        
    }
    public override CharacterData GetBluffIfAble(Character charRef)
    {
        if (allDatas.Length == 0)
        {
            var loadedCharList = Resources.FindObjectsOfTypeAll(Il2CppType.Of<CharacterData>());
            if (loadedCharList != null)
            {
                allDatas = new CharacterData[loadedCharList.Length];
                for (int i = 0; i < loadedCharList.Length; i++)
                {
                    allDatas[i] = loadedCharList[i]!.Cast<CharacterData>();
                }
            }
        }
        CharacterData bluff = new CharacterData();
        Il2CppSystem.Collections.Generic.List<CharacterData> possibleBluffs = new Il2CppSystem.Collections.Generic.List<CharacterData>();
        for(int i = 0; i < allDatas.Length; i++)
        {
            if (allDatas[i].characterId == "HypnotistBluff")
                bluff = allDatas[i];
            else if (allDatas[i].characterId == "Alchemist_94446803")
                possibleBluffs.Add(allDatas[i]);
            else if (allDatas[i].characterId == "Confessor_18741708")
                possibleBluffs.Add(allDatas[i]);
            else if (allDatas[i].characterId == "Baker_22847064")
                possibleBluffs.Add(allDatas[i]);
            else if (allDatas[i].characterId == "Witness_25155076")
                possibleBluffs.Add(allDatas[i]);
        }
        CharacterData chosenBluff = new CharacterData();
        chosenBluff = possibleBluffs[UnityEngine.Random.Range(0, possibleBluffs.Count)];
        bluff.name = chosenBluff.name;
        bluff.description = chosenBluff.description;
        bluff.flavorText = chosenBluff.flavorText;
        bluff.hints = chosenBluff.hints;
        bluff.ifLies = chosenBluff.ifLies;
        bluff.art_cute = chosenBluff.art_cute;
        bluff.backgroundArt = chosenBluff.backgroundArt;
        return bluff;
    }

    public Hypnotist() : base(ClassInjector.DerivedConstructorPointer<Hypnotist>())
    {
        ClassInjector.DerivedConstructorBody((Il2CppObjectBase)this);
    }
    public Hypnotist(System.IntPtr ptr) : base(ptr) { }

}