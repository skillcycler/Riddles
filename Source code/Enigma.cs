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
            CharacterData[] allDatas = Il2CppSystem.Array.Empty<CharacterData>();
            var loadedCharList = Resources.FindObjectsOfTypeAll(Il2CppType.Of<CharacterData>());
            if (loadedCharList != null)
            {
                allDatas = new CharacterData[loadedCharList.Length];
                for (int j = 0; j < loadedCharList.Length; j++)
                {
                    allDatas[j] = loadedCharList[j]!.Cast<CharacterData>();
                }
            }
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
            // Same list as Wingidon's Heretic
            List<string> blacklistMinionIDs = new();
            blacklistMinionIDs.Add("Puppet_15989619"); // Puppet is never in the Deck to begin with.
            blacklistMinionIDs.Add("Swarm_Good_WING"); // Swarm adding its counterpart to the Deck makes it far too obvious
            blacklistMinionIDs.Add("Swarm_Evil_WING"); // Swarm adding its counterpart to the Deck makes it far too obvious.
            blacklistMinionIDs.Add("Trickster_m_scm"); // Just in case.
            blacklistMinionIDs.Add("Trickster_m_register_scm"); // Just in case.
            blacklistMinionIDs.Add("Undying_WING"); // Undying is face-up. Don't add him as a fake Minion.
            blacklistMinionIDs.Add("Marionette_11628408"); // That's the wrong Marionette.
            blacklistMinionIDs.Add("Werewolf_78350415"); // Werewolf is never in the Deck to begin with.
            blacklistMinionIDs.Add("Wretch_Evil_91222191"); // That's the wrong Wretch.
            // now for demons
            blacklistMinionIDs.Add("Mutant_84675843");
            blacklistMinionIDs.Add("Delusion_10561407");

            foreach (CharacterData d in allDatas)
            {
                if (!inPlayCharacters.Contains(d) && d.type != ECharacterType.Villager && !(d.type == ECharacterType.Outcast && d.usuallyDisguised == false) && !blacklistMinionIDs.Contains(d.characterId) && d.name.ToLower() != "relic_copyvillager")
                {
                    fake.Add(d);
                }
            }
            for (int i = 0; i < charRef.id; i++)
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