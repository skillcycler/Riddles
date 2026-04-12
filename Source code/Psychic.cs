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
public class Psychic : Role
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
        Il2CppSystem.Collections.Generic.List<CharacterData> deck = Gameplay.Instance.GetScriptCharacters();
        Il2CppSystem.Collections.Generic.List<CharacterData> outOfPlay = Characters.Instance.FilterNotInPlayCharacters(deck);
        Il2CppSystem.Collections.Generic.List<CharacterData> inPlayChars = new();
        foreach (CharacterData c in deck)
        {
            bool inPlay = false;
            foreach (Character character in characters)
            {
                if (character.dataRef.characterId == c.characterId)
                    inPlay = true;
            }
            if (!inPlay)
            {
                outOfPlay.Add(c);
            }
            else
            {
                inPlayChars.Add(c);
            }
        }
        int randomInPlay = UnityEngine.Random.RandomRangeInt(0, inPlayChars.Count);
        int randomOutOfPlay = UnityEngine.Random.RandomRange(0, outOfPlay.Count);
        string info = string.Format("Either the {0} or the {1} is in play", inPlayChars[randomInPlay].name, outOfPlay[randomOutOfPlay].name);
        if (Calculator.RollDice(2) == 1)
        {
            info = string.Format("Either the {1} or the {0} is in play", inPlayChars[randomInPlay].name, outOfPlay[randomOutOfPlay].name);
        }
        ActedInfo actedInfo = new ActedInfo(info);
        return actedInfo;
    }

    public override ActedInfo GetBluffInfo(Character charRef)
    {
        Il2CppSystem.Collections.Generic.List<Character> characters = Gameplay.CurrentCharacters;
        Il2CppSystem.Collections.Generic.List<CharacterData> deck = Gameplay.Instance.GetScriptCharacters();
        Il2CppSystem.Collections.Generic.List<CharacterData> outOfPlay = Characters.Instance.FilterNotInPlayCharacters(deck);
        Il2CppSystem.Collections.Generic.List<CharacterData> inPlayChars = new();
        foreach (CharacterData c in deck)
        {
            bool inPlay = false;
            foreach (Character character in characters)
            {
                if (character.dataRef.characterId == c.characterId)
                    inPlay = true;
            }
            if (!inPlay)
            {
                outOfPlay.Add(c);
            }
            else
            {
                inPlayChars.Add(c);
            }
        }
        bool bothTrue = (Calculator.RollDice(2) == 1);
        
        if (bothTrue)
        {
            int randomInPlay1 = UnityEngine.Random.RandomRangeInt(0, inPlayChars.Count);
            int randomInPlay2 = UnityEngine.Random.RandomRangeInt(0, inPlayChars.Count);
            while (randomInPlay1 == randomInPlay2)
            {
                randomInPlay2 = UnityEngine.Random.RandomRangeInt(0, inPlayChars.Count);
            }
            string inf = string.Format("Either the {0} or the {1} is in play", inPlayChars[randomInPlay1].name, inPlayChars[randomInPlay2].name);
            if (Calculator.RollDice(2) == 1)
            {
                inf = string.Format("Either the {1} or the {0} is in play", inPlayChars[randomInPlay1].name, inPlayChars[randomInPlay2].name);
            }
            ActedInfo actedInf = new ActedInfo(inf);
            return actedInf;
        }
        int randomOutOfPlay1 = UnityEngine.Random.RandomRangeInt(0, outOfPlay.Count);
        int randomOutOfPlay2 = UnityEngine.Random.RandomRangeInt(0, outOfPlay.Count);
        while (randomOutOfPlay1 == randomOutOfPlay2)
        {
            randomOutOfPlay2 = UnityEngine.Random.RandomRangeInt(0, outOfPlay.Count);
        }
        string info = string.Format("Either the {0} or the {1} is in play", outOfPlay[randomOutOfPlay1].name, outOfPlay[randomOutOfPlay2].name);
        if (Calculator.RollDice(2) == 1)
        {
            info = string.Format("Either the {1} or the {0} is in play", outOfPlay[randomOutOfPlay1].name, outOfPlay[randomOutOfPlay2].name);
        }
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
    public Psychic() : base(ClassInjector.DerivedConstructorPointer<Psychic>())
    {
        ClassInjector.DerivedConstructorBody((Il2CppObjectBase)this);
    }

    public Psychic(System.IntPtr ptr) : base(ptr)
    {

    }
}