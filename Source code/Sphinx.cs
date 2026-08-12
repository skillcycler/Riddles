using System;
using System.Linq;
using System.Reflection;
using Harmony;
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
public class Sphinx : Role
{
    public override string Description
    {
        get
        {
            return "";
        }
    }
    public string MakeInfo(bool truth)
    {
        int bestKill = FindBestKill().id;
        if (!truth) bestKill = FindWorstKill().id;
        if (truth) MelonLogger.Msg($"Best Kill: {bestKill}");
        if (!truth) MelonLogger.Msg($"Worst Kill: {bestKill}");
        List<string> validQuestions = GetQuestions(bestKill);
        return validQuestions[UnityEngine.Random.RandomRangeInt(0, validQuestions.Count)];
    }
    public override ActedInfo GetInfo(Character charRef)
    {
        string info = MakeInfo(true);
        ActedInfo actedInfo = new ActedInfo(info);
        return actedInfo;
    }

    public override ActedInfo GetBluffInfo(Character charRef)
    {
        string info = MakeInfo(false);
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
    public Sphinx() : base(ClassInjector.DerivedConstructorPointer<Sphinx>())
    {
        ClassInjector.DerivedConstructorBody((Il2CppObjectBase)this);
    }

    public Sphinx(System.IntPtr ptr) : base(ptr)
    {

    }
    public Character FindBestKill()
    {
        Il2CppSystem.Collections.Generic.List<Character> possible = MainMod.GetGameplayCurrentCharacters();
        List<string> important = new(); // from most to least important in the super important category
        important.Add("Scapegoat_POW");
        important.Add("Leviathan_WING");
        important.Add("WING_Dupery_Critic");
        important.Add("Balancer_POW");
        important.Add("Veil_scm");
        important.Add("Starspawn_POW");
        important.Add("WING_Dupery_Travel Agent");
        important.Add("Witch_25286521");
        important.Add("Supporter_POW");
        foreach (string id in important)
        {
            foreach (Character character in possible)
            {
                if (character.state != ECharacterState.Dead)
                    if (character.dataRef.characterId == id && character.alignment == EAlignment.Evil) return character;
            }
        }
        // round 2: non-v/o/m/d evils
        foreach (Character character in possible)
        {
            if (character.state != ECharacterState.Dead)
                if (character.dataRef.type != ECharacterType.Demon && character.dataRef.type != ECharacterType.Minion && character.dataRef.type != ECharacterType.Outcast && character.dataRef.type != ECharacterType.Villager && character.alignment == EAlignment.Evil)
                return character;
        }
        // round 3: demons
        foreach (Character character in possible)
        {
            if (character.state != ECharacterState.Dead && character.dataRef.characterId != "Praesect_WING")
                if (character.dataRef.type == ECharacterType.Demon && character.alignment == EAlignment.Evil) return character;
        }
        // round 4: disguised truthful evils
        foreach (Character character in possible)
        {
            if (character.state != ECharacterState.Dead)
                if (character.bluff != null && character.statuses.Contains(ECharacterStatus.HealthyBluff) && character.alignment == EAlignment.Evil) return character;
        }
        // round 5: Anything that isn't part of the can't die list
        foreach (Character character in possible)
        {
            if (character.state != ECharacterState.Dead)
                if (!Djinn.GetCharactersThatCannotDie().Contains(character.dataRef.characterId) && character.alignment == EAlignment.Evil) return character;
        }
        // round 6: Any other alive evil
        foreach (Character character in possible)
        {
            if (character.state != ECharacterState.Dead)
                if (character.alignment == EAlignment.Evil) return character;
        }
        //failsafe
        return possible[0];
    }
    public Character FindWorstKill()
    {
        Il2CppSystem.Collections.Generic.List<Character> possible = MainMod.GetGameplayCurrentCharacters();
        List<string> important = new(); // from most to least important in the super important category
        important.Add("Bombardier_79093372");
        important.Add("Atheist_scm");
        important.Add("WING_Dupery_Youngster");

        foreach (string id in important)
        {
            foreach (Character character in possible)
            {
                if (character.state != ECharacterState.Dead)
                    if (character.dataRef.characterId == id && character.alignment == EAlignment.Good) return character;
            }
        }
        //round 1.5: Leviathan from Wingidon's Expansion Pack
        bool isLeviathan = false;
        foreach (Character c in Gameplay.CurrentCharacters) if (c.dataRef.characterId == "Leviathan_WING") isLeviathan = true;
        if (isLeviathan)
        {
            //villagers
            foreach (Character c in Gameplay.CurrentCharacters) if (c.dataRef.type == ECharacterType.Villager) return c;
        }

        // round 2: Undying, if it can't die

        foreach (Character character in possible)
        {
            if (character.state != ECharacterState.Dead)
                if (character.dataRef.characterId == "Undying_WING")
                {
                    Il2CppSystem.Collections.Generic.List<Character> characters = Gameplay.CurrentCharacters;
                    bool evilLives = false;
                    Il2CppSystem.Collections.Generic.List<Character> charactersNotMe = new Il2CppSystem.Collections.Generic.List<Character>();
                    foreach (Character character2 in characters)
                    {
                        if (character2.id != charRef.id && !Djinn.GetCharactersThatCannotDie().Contains(character2.dataRef.characterId))
                        {
                            charactersNotMe.Add(character2);
                        }
                    }
                    charactersNotMe = Characters.Instance.FilterAliveCharacters(charactersNotMe);
                    foreach (Character character2 in charactersNotMe)
                    {
                        if (character2.alignment == EAlignment.Evil)
                        {
                            evilLives = true;
                        }
                    }
                    if (evilLives) return character;
                }
        }
        // round 3: Things that deal extra damage
        foreach (Character character in possible)
        {
            if (character.state != ECharacterState.Dead)
            {
                if (character.dataRef.characterId == "Knight_47970624" && character.statuses.Contains(ECharacterStatus.Corrupted)) return character;
                if (character.dataRef.characterId == "Revolutionary_WING") return character;
            }
        }
        foreach (Character character in possible)
        {
            if (character.state != ECharacterState.Dead)
                if (character.statuses.Contains((ECharacterStatus)270)) return character;
        }
        // any good character
        foreach (Character character in possible)
        {
            if (character.state != ECharacterState.Dead)
                if (character.alignment == EAlignment.Good) return character;
        }
        //failsafe
        return possible[0];
    }
    public List<string> GetQuestions(int id)
    {
        List<string> questions = new();
        int evils = 0;
        int goods = 0;
        int villagers = 0;
        int outcasts = 0;
        int minions = 0;
        int demons = 0;
        int liars = 0;
        int truthers = 0;
        int disguised = 0;
        int honest = 0;
        int corrupted = 0;
        foreach (Character c in Gameplay.CurrentCharacters)
        {
            if (c.GetRegisterAlignment() == EAlignment.Good) goods++;
            else evils++;

            switch (c.GetRegisterAs().type)
            {
                case ECharacterType.Villager:
                    villagers++;
                    break;
                case ECharacterType.Outcast:
                    outcasts++;
                    break;
                case ECharacterType.Minion:
                    minions++;
                    break;
                case ECharacterType.Demon:
                    demons++;
                    break;
            }
            if (CharacterHelper.CheckLyingAppearance(c)) liars++;
            else truthers++;
            if (CharacterHelper.CheckIfDisguisedAppearance(c)) disguised++;
            else honest++;
            if (c.statuses.Contains(ECharacterStatus.Corrupted)) corrupted++;
        }
        if (id == evils) questions.Add("How many Evil characters are there?");
        if (id == goods) questions.Add("How many Good characters are there?");
        if (id == villagers) questions.Add("How many Villagers are there?");
        if (id == outcasts) questions.Add("How many Outcasts are there?");
        if (id == minions) questions.Add("How many Minions are there?");
        if (id == demons) questions.Add("How many Demons are there?");
        if (id == honest) questions.Add("How many Non-Disguised characters are there?");
        if (id == disguised) questions.Add("How many Disguised characters are there?");
        if (id == liars) questions.Add("How many Lying characters are there?");
        if (id == truthers) questions.Add("How many Truthful characters are there?");
        if (id == corrupted) questions.Add("How many Corrupted characters are there?");

        // time for pairs
        int evilPairs = 0;
        int goodPairs = 0;
        int villagerPairs = 0;
        int honestPairs = 0;
        int disguisedPairs = 0;
        int liarsPairs = 0;
        int truthersPairs = 0;
        Il2CppSystem.Collections.Generic.List<Character> pairChecker = MainMod.GetGameplayCurrentCharacters();
        pairChecker.Add(pairChecker[0]);

        bool evilPrev = false;
        bool goodPrev = false;
        bool villagerPrev = false;
        bool honestPrev = false;
        bool disguisedPrev = false;
        bool liarsPrev = false;
        bool truthersPrev = false;
        foreach (Character character in pairChecker)
        {
            if (character.GetRegisterAlignment() == EAlignment.Evil)
            {
                if (evilPrev)
                    evilPairs++;
                evilPrev = true;
                goodPrev = false;
            }
            else {
                if (goodPrev)
                    goodPairs++;
                goodPrev = true;
                evilPrev = false;
            }
            if (character.GetRegisterAs().type == ECharacterType.Villager)
            {
                if (villagerPrev)
                    villagerPairs++;
                villagerPrev = true;
            }
            else villagerPrev = false;
            if (CharacterHelper.CheckLyingAppearance(character))
            {
                if (liarsPrev)
                    liarsPairs++;
                liarsPrev = true;
                truthersPrev = false;
            }
            else
            {
                if (truthersPrev)
                    truthersPairs++;
                truthersPrev = true;
                liarsPrev = false;
            }
            if (CharacterHelper.CheckIfDisguisedAppearance(character))
            {
                if (disguisedPrev)
                    disguisedPairs++;
                disguisedPrev = true;
                honestPrev = false;
            }
            else
            {
                if (honestPrev)
                    honestPairs++;
                honestPrev = true;
                disguisedPrev = false;
            }
        }
        if (id == evilPairs) questions.Add("How many pairs of Evil characters are there?");
        if (id == goodPairs) questions.Add("How many pairs of Good characters are there?");
        if (id == villagerPairs) questions.Add("How many pairs of Villagers are there?");
        if (id == honestPairs) questions.Add("How many pairs of Non-Disguised characters are there?");
        if (id == disguisedPairs) questions.Add("How many pairs of Disguised characters are there?");
        if (id == liarsPairs) questions.Add("How many pairs of Lying characters are there?");
        if (id == truthersPairs) questions.Add("How many pairs of Truthful characters are there?");

        // Miscellaneous questions
        Il2CppSystem.Collections.Generic.List<Character> disguisedCharacters = new();
        foreach (Character c in Gameplay.CurrentCharacters)
        {
            if (CharacterHelper.CheckIfDisguisedAppearance(c) && c.bluff != null) disguisedCharacters.Add(c);
        }
        // don't ask about characters that have 2 of them somewhere
        Il2CppSystem.Collections.Generic.List<Character> nodupe = new();
        foreach (Character c1 in disguisedCharacters)
        {
            int m = 0;
            foreach (Character c2 in Gameplay.CurrentCharacters)
            {
                if (c2.GetRegisterAs().characterName == c1.GetRegisterAs().characterName) m++;
            }
            if (m == 1 && c1.GetRegisterAs().characterId == c1.dataRef.characterId)
            {
                nodupe.Add(c1);
            }
        }
        if (id < Gameplay.CurrentCharacters.Count / 2)
        {
            foreach (Character c in nodupe)
            {
                foreach (Character c2 in nodupe)
                {
                    int distance = Math.Max(c.id - c2.id, c2.id - c.id);
                    if (distance > Gameplay.CurrentCharacters.Count / 2)
                    {
                        distance = Gameplay.CurrentCharacters.Count - distance;
                    }
                    if (distance == id)
                    {
                        questions.Add($"How far is the {c.GetRegisterAs().characterName} from the {c2.GetRegisterAs().characterName}?");
                    }
                }
            }
        }
        if (questions.Count == 0)
        {
            // if none of the above, add 2 of these together
            Dictionary<int, string> sumthing = new();
            sumthing.Add(goods, "Good characters");
            sumthing.Add(evils + 1000, "Evils");
            sumthing.Add(villagers + 2000, "Villagers");
            sumthing.Add(outcasts + 3000, "Outcasts");
            sumthing.Add(minions + 4000, "Minions");
            sumthing.Add(demons + 5000, "Demons");
            sumthing.Add(liars + 6000, "Liars");
            sumthing.Add(truthers + 7000, "Truthful characters");
            sumthing.Add(disguised + 8000, "Disguised characters");
            sumthing.Add(honest + 9000, "Non-Disguised characters");
            sumthing.Add(corrupted + 10000, "Corrupted characters");
            sumthing.Add(goodPairs + 11000, "pairs of Good characters");
            sumthing.Add(evilPairs + 12000, "pairs of Evils");
            sumthing.Add(villagerPairs + 13000, "pairs of Villagers");
            sumthing.Add(honestPairs + 14000, "pairs of Non-Disguised characters");
            sumthing.Add(disguisedPairs + 15000, "pairs of Disguised characters");
            sumthing.Add(liarsPairs + 16000, "pairs of Liars");
            sumthing.Add(truthersPairs + 17000, "pairs of Truthful characters");
            foreach (var pair in sumthing)
            {
                foreach (var pair2 in sumthing)
                {
                    if ((pair.Key + pair2.Key) % 1000 == id)
                    {
                        if (pair.Value == pair2.Value) questions.Add($"What is 2x the number of {pair.Value}?");
                        else questions.Add($"What is the number of {pair.Value} + {pair2.Value}?");
                    }
                }
            }
        }
        //if, somehow, no sum works:
        if (questions.Count == 0)
        {
            if (id > goods) questions.Add($"What is {id - goods} + the number of Good characters?");
            if (id > evils) questions.Add($"What is {id - evils} + the number of Evil characters?");
            if (id > villagers) questions.Add($"What is {id - villagers} + the number of Villagers?");
            if (id > outcasts) questions.Add($"What is {id - outcasts} + the number of Outcasts?");
            if (id > minions) questions.Add($"What is {id - minions} + the number of Minions?");
            if (id > demons) questions.Add($"What is {id - demons} + the number of Demons?");
            if (id > liars) questions.Add($"What is {id - liars} + the number of Lying characters?");
            if (id > truthers) questions.Add($"What is {id - truthers} + the number of Truthful characters?");
            if (id > disguised) questions.Add($"What is {id - disguised} + the number of Disguised characters?");
            if (id > honest) questions.Add($"What is {id - honest} + the number of Non-Disguised characters?");
            if (id > corrupted) questions.Add($"What is {id - corrupted} + the number of Corrupted characters?");

            if (id > goodPairs) questions.Add($"What is {id - goodPairs} + the number of pairs of Good characters?");
            if (id > evilPairs) questions.Add($"What is {id - evilPairs} + the number of pairs of Evil characters?");
            if (id > villagerPairs) questions.Add($"What is {id - villagerPairs} + the number of pairs of Villagers?");
            if (id > honestPairs) questions.Add($"What is {id - honestPairs} + the number of pairs of Non-Disguised characters?");
            if (id > disguisedPairs) questions.Add($"What is {id - disguisedPairs} + the number of pairs of Disguised characters?");
            if (id > liarsPairs) questions.Add($"What is {id - liarsPairs} + the number of pairs of Lying characters?");
            if (id > truthersPairs) questions.Add($"What is {id - truthersPairs} + the number of pairs of Truthful characters?");

        }
        return questions;
    }
}