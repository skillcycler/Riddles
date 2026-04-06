using Il2Cpp;
using Il2CppInterop.Runtime.Injection;
using Il2CppInterop.Runtime.InteropTypes;
using Il2CppSystem;
using MelonLoader;
using System;
using System.ComponentModel.Design;
using UnityEngine;
using HarmonyLib;

public class Accuser : Minion
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
            Il2CppSystem.Collections.Generic.List<Character> neighbors = Characters.Instance.GetAdjacentCharacters(charRef);
            neighbors = Characters.Instance.FilterCharacterType(neighbors, ECharacterType.Villager);
            if (neighbors.Count > 0)
            {
                Character randomChar = neighbors[UnityEngine.Random.Range(0, neighbors.Count)];
                Il2CppSystem.Collections.Generic.List<CharacterData> allChars = Gameplay.Instance.GetScriptCharacters();
                allChars = Characters.Instance.FilterAlignmentCharacters(allChars, EAlignment.Evil);

                CharacterData randomEvil = allChars[UnityEngine.Random.Range(0, allChars.Count)];
                randomChar.statuses.AddStatus(ECharacterStatus.MessedUpByEvil, charRef);
                randomChar.statuses.AddStatus(Accused.accused, charRef);
                randomChar.GiveBluff(randomChar.dataRef);
                randomChar.dataRef = ProjectContext.Instance.gameData.GetCharacterDataOfId("Wretch_80988916");
                randomChar.statuses.AddStatus(ECharacterStatus.HealthyBluff, charRef);
            }
        }
    }
    
    public Accuser() : base(ClassInjector.DerivedConstructorPointer<Accuser>())
    {
        ClassInjector.DerivedConstructorBody((Il2CppObjectBase)this);
    }
    public Accuser(System.IntPtr ptr) : base(ptr) { }
    
}
public static class Accused
{
    public static ECharacterStatus accused = (ECharacterStatus)873;

    [HarmonyPatch(typeof(Character), nameof(Character.RevealAllReal))]
    public static class pvt
    {
        public static void Postfix(Character __instance)
        {
            if (__instance.statuses.Contains(accused))
            {
                __instance.chName.text = __instance.dataRef.name.ToUpper() + "<color=#FF8000><size=18>\n<Accused></color></size>";
            }
        }
    }
}