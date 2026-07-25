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
using static UnityEngine.GraphicsBuffer;
using static UnityEngine.TouchScreenKeyboard;

namespace RiddlerMod;

[RegisterTypeInIl2Cpp]
public class Kingmaker : Demon
{
    public override Il2CppSystem.Collections.Generic.List<SpecialRule> GetRules()
    {
        Il2CppSystem.Collections.Generic.List<SpecialRule> sr = new Il2CppSystem.Collections.Generic.List<SpecialRule>();
        sr.Add(new NightModeRule(4));
        return sr;
    }
    public override void Act(ETriggerPhase trigger, Character charRef)
    {
        if (trigger == ETriggerPhase.Init)
        {
            Djinn.Jinx("Kingmaker");
        }
        if (trigger == ETriggerPhase.Start)
        {
            Il2CppSystem.Collections.Generic.List<CharacterData> notInPlayMinions = Gameplay.Instance.GetAscensionAllStartingCharacters();
            notInPlayMinions = Characters.Instance.FilterNotInPlayCharactersUnique(notInPlayMinions);
            notInPlayMinions = Characters.Instance.FilterRealCharacterType(notInPlayMinions, ECharacterType.Minion);
            foreach (Character c in Characters.Instance.GetAdjacentCharacters(charRef))
            { // always have 2 extra minions in the deck list
                c.statuses.AddStatus(ECharacterStatus.MessedUpByEvil, charRef);
                CharacterData picked = notInPlayMinions[UnityEngine.Random.Range(0, notInPlayMinions.Count - 1)];
                while (Djinn.GetInvalidCharacterIDs("Kingmaker").Contains(picked.characterId))
                {
                    picked = notInPlayMinions[UnityEngine.Random.Range(0, notInPlayMinions.Count - 1)];
                }
                Gameplay.Instance.AddScriptCharacter(ECharacterType.Minion, picked);
                if (c.dataRef.type != ECharacterType.Minion)
                {
                    c.Init(picked);
                }
                notInPlayMinions.Remove(picked);
            }
            foreach (Character ch in Gameplay.CurrentCharacters)
            {
                if (ch.dataRef.characterId == "Marionette_WING")
                {
                    ch.Init(ProjectContext.Instance.gameData.GetCharacterDataOfId("Drunk_15369527"));
                }
            }
        }
        if (trigger == ETriggerPhase.AfterRoundStart)
        {
            charRef.statuses.statuses.Add(ECharacterStatus.HealthyBluff);
        }
    }
    public Kingmaker() : base(ClassInjector.DerivedConstructorPointer<Kingmaker>())
    {
        ClassInjector.DerivedConstructorBody((Il2CppObjectBase)this);
    }
    public Kingmaker(System.IntPtr ptr) : base(ptr) { }
}
