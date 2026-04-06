global using Il2Cpp;
using RiddlerMod;
using HarmonyLib;
using Il2CppDissolveExample;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.Injection;
using Il2CppInterop.Runtime.InteropTypes.Arrays;
using Il2CppSystem.IO;
using MelonLoader;
using System;
using UnityEngine;
using static Il2Cpp.Interop;
using static Il2CppSystem.Array;

[assembly: MelonInfo(typeof(MainMod), "Skill Cycler's Riddles", "0.4", "Skill Cycler")]
[assembly: MelonGame("UmiArt", "Demon Bluff")]

namespace RiddlerMod;
public class MainMod : MelonMod
{
    public override void OnInitializeMelon()
    {
        ClassInjector.RegisterTypeInIl2Cpp<Riddler>();
        ClassInjector.RegisterTypeInIl2Cpp<Swapper>();
        //ClassInjector.RegisterTypeInIl2Cpp<Sleeper>();
        ClassInjector.RegisterTypeInIl2Cpp<Mathematician>();
        ClassInjector.RegisterTypeInIl2Cpp<Commander>();
        ClassInjector.RegisterTypeInIl2Cpp<Director>();
        //ClassInjector.RegisterTypeInIl2Cpp<Scanner>();

        // Outcasts

        ClassInjector.RegisterTypeInIl2Cpp<MadScientist>();
        ClassInjector.RegisterTypeInIl2Cpp<Necromancer>();
        ClassInjector.RegisterTypeInIl2Cpp<Criminal>();

        // Evils
        ClassInjector.RegisterTypeInIl2Cpp<Accuser>();
        ClassInjector.RegisterTypeInIl2Cpp<Hypnotist>();
    }
    public override void OnLateInitializeMelon()
    {
        GameObject content = GameObject.Find("Game/Gameplay/Content");
        NightPhase nightPhase = content.GetComponent<NightPhase>();


        CharacterData Riddler = new CharacterData();
        Riddler.role = new Riddler();
        Riddler.name = "Riddler";
        Riddler.description = "Learn a true fact about the game.";
        Riddler.flavorText = "\"One day I'll cause a paradox.\"";
        Riddler.hints = "";
        Riddler.ifLies = "Learn a false fact about the game.";
        Riddler.picking = false;
        Riddler.startingAlignment = EAlignment.Good;
        Riddler.type = ECharacterType.Villager;
        Riddler.bluffable = true;
        Riddler.characterId = "Riddler";
        Riddler.artBgColor = new Color(0.111f, 0.0833f, 0.1415f);
        Riddler.cardBgColor = new Color(0.26f, 0.1519f, 0.3396f);
        Riddler.cardBorderColor = new Color(0.7133f, 0.339f, 0.8679f);
        Riddler.color = new Color(1f, 0.935f, 0.7302f);

        CharacterData Swapper = new CharacterData();
        Swapper.role = new Swapper();
        Swapper.name = "Swapper";
        Swapper.description = "Pick 2 cards: They disguise as each other's apparent role. Refresh both of their statements or abilities.";
        Swapper.flavorText = "\"Didn't like the role you got? I'm here to save the day!\"";
        Swapper.hints = "A Swapper cannot swap itself or another Swapper.";
        Swapper.ifLies = "Both targets are Corrupted if they are Villagers.";
        Swapper.picking = true;
        Swapper.startingAlignment = EAlignment.Good;
        Swapper.type = ECharacterType.Villager;
        Swapper.bluffable = true;
        Swapper.characterId = "Swapper";
        Swapper.artBgColor = new Color(0.111f, 0.0833f, 0.1415f);
        Swapper.cardBgColor = new Color(0.26f, 0.1519f, 0.3396f);
        Swapper.cardBorderColor = new Color(0.7133f, 0.339f, 0.8679f);
        Swapper.color = new Color(1f, 0.935f, 0.7302f);
        /*
         * This character has too many bugs that I don't know how to fix.
         * - Sleeper doesn't work when it's an evil disguised as Sleeper
         * - Sleeper doesn't refresh its info each day.
         * - Sleeper in a Lilis village causes 2 night cycles at the same time.
        CharacterData Sleeper = new CharacterData();
        Sleeper.role = new Sleeper();
        Sleeper.name = "Sleeper";
        Sleeper.description = "Learn random information each day. The night cycle is 3 ticks long.";
        Sleeper.flavorText = "\"Ever feel like you get enough sleep? Me neither.\"";
        Sleeper.hints = "";
        Sleeper.ifLies = "The night cycle is still 3 ticks but the information is false.";
        Sleeper.picking = false;
        Sleeper.startingAlignment = EAlignment.Good;
        Sleeper.type = ECharacterType.Villager;
        Sleeper.bluffable = true;
        Sleeper.characterId = "Sleeper";
        Sleeper.artBgColor = new Color(0.111f, 0.0833f, 0.1415f);
        Sleeper.cardBgColor = new Color(0.26f, 0.1519f, 0.3396f);
        Sleeper.cardBorderColor = new Color(0.7133f, 0.339f, 0.8679f);
        Sleeper.color = new Color(1f, 0.935f, 0.7302f);
        */

        CharacterData Mathematician = new CharacterData();
        Mathematician.role = new Mathematician();
        Mathematician.name = "Mathematician";
        Mathematician.description = "Learn a number equal to the sum of the card numbers of 2 Evils.";
        Mathematician.flavorText = "\"21\"";
        Mathematician.hints = "";
        Mathematician.ifLies = "";
        Mathematician.picking = false;
        Mathematician.startingAlignment = EAlignment.Good;
        Mathematician.type = ECharacterType.Villager;
        Mathematician.bluffable = true;
        Mathematician.characterId = "Mathematician";
        Mathematician.cardBgColor = new Color(0.26f, 0.1519f, 0.3396f);
        Mathematician.cardBorderColor = new Color(0.7133f, 0.339f, 0.8679f);
        Mathematician.color = new Color(1f, 0.935f, 0.7302f);


        CharacterData Commander = new CharacterData();
        Commander.role = new Commander();
        Commander.name = "Commander";
        Commander.description = "Pick 2 cards: Learn a card of a different character type from both.";
        Commander.flavorText = "\"Leads the Villagers by day, hunts the Minions at night.\"";
        Commander.hints = "";
        Commander.ifLies = "";
        Commander.picking = true;
        Commander.startingAlignment = EAlignment.Good;
        Commander.type = ECharacterType.Villager;
        Commander.abilityUsage = EAbilityUsage.Once;
        Commander.bluffable = true;
        Commander.characterId = "Commander";
        Commander.cardBgColor = new Color(0.26f, 0.1519f, 0.3396f);
        Commander.cardBorderColor = new Color(0.7133f, 0.339f, 0.8679f);
        Commander.color = new Color(1f, 0.935f, 0.7302f);

        CharacterData Director = new CharacterData();
        Director.role = new Director();
        Director.name = "Director";
        Director.description = "Learn a consecutive group of cards that contain 2 Evils.";
        Director.flavorText = "\"There are no lights. There is no camera. But there's certainly a lot of action.\"";
        Director.hints = "I always go clockwise from the first number to the second number. Both endpoints are included.";
        Director.ifLies = "";
        Director.picking = false;
        Director.startingAlignment = EAlignment.Good;
        Director.type = ECharacterType.Villager;
        Director.bluffable = true;
        Director.characterId = "Director";
        Director.cardBgColor = new Color(0.26f, 0.1519f, 0.3396f);
        Director.cardBorderColor = new Color(0.7133f, 0.339f, 0.8679f);
        Director.color = new Color(1f, 0.935f, 0.7302f);
        /* I don't think this is actually gonna be useful in small villages
        CharacterData Scanner = new CharacterData();
        Scanner.role = new Scanner();
        Scanner.name = "Scanner";
        Scanner.description = "Learn the 2 Outcasts that are closest to each other, bypassing misregistration. Picks randomly if tied.";
        Scanner.flavorText = "\"I spy with my two little eyes, two Outcasts having a party!\"";
        Scanner.hints = "";
        Scanner.ifLies = "";
        Scanner.picking = false;
        Scanner.startingAlignment = EAlignment.Good;
        Scanner.type = ECharacterType.Villager;
        Scanner.bluffable = true;
        Scanner.characterId = "Scanner";
        Scanner.cardBgColor = new Color(0.26f, 0.1519f, 0.3396f);
        Scanner.cardBorderColor = new Color(0.7133f, 0.339f, 0.8679f);
        Scanner.color = new Color(1f, 0.935f, 0.7302f);
        */

        CharacterData MadScientist = new CharacterData();
        MadScientist.role = new MadScientist();
        MadScientist.name = "Mad Scientist";
        MadScientist.description = "I have the ability of a not in play Outcast and Minion. I add 1 fake Outcast and 1-2 fake Minions to the Deck.";
        MadScientist.flavorText = "\"Lil bro is ANGRY at the village\"";
        MadScientist.hints = "I cannot be disguised as. No Evil is crazy enough.";
        MadScientist.ifLies = "";
        MadScientist.picking = false;
        MadScientist.startingAlignment = EAlignment.Good;
        MadScientist.type = ECharacterType.Outcast;
        MadScientist.bluffable = false;
        MadScientist.characterId = "MadScientist";
        MadScientist.cardBgColor = new Color(0.102f, 0.0667f, 0.0392f);
        MadScientist.cardBorderColor = new Color(0.7843f, 0.6471f, 0f);
        MadScientist.color = new Color(0.9659f, 1f, 0.4472f);

        CharacterData Necromancer = new CharacterData();
        Necromancer.role = new Necromancer();
        Necromancer.name = "Necromancer";
        Necromancer.description = "Pick 1 dead card: Revive it and lose 3 Health.";
        Necromancer.flavorText = "\"Second chances are real. Just like Empaths and Mayors.\"";
        Necromancer.hints = "";
        Necromancer.ifLies = "The card will lie with its new info no matter what.";
        Necromancer.picking = true;
        Necromancer.startingAlignment = EAlignment.Good;
        Necromancer.type = ECharacterType.Outcast;
        Necromancer.bluffable = true;
        Necromancer.characterId = "Necromancer";
        Necromancer.cardBgColor = new Color(0.102f, 0.0667f, 0.0392f);
        Necromancer.cardBorderColor = new Color(0.7843f, 0.6471f, 0f);
        Necromancer.color = new Color(0.9659f, 1f, 0.4472f);

        CharacterData Criminal = new CharacterData();
        Criminal.role = new Criminal();
        Criminal.name = "Hitman";
        Criminal.description = "I Lie and Disguise.\n\nAt night: Kill a random card and lose 3 HP.";
        Criminal.flavorText = "\"No one is safe from me, not even myself\"";
        Criminal.hints = "I can kill any card, including Knights, Demons, and myself.";
        Criminal.ifLies = "";
        Criminal.picking = false;
        Criminal.startingAlignment = EAlignment.Evil;
        Criminal.type = ECharacterType.Outcast;
        Criminal.bluffable = false;
        Criminal.characterId = "Criminal";
        Criminal.cardBgColor = new Color(0.102f, 0.0667f, 0.0392f);
        Criminal.cardBorderColor = new Color(0.7843f, 0.6471f, 0f);
        Criminal.color = new Color(0.9659f, 1f, 0.4472f);
        nightPhase.nightCharactersOrder.Add(Criminal);

        CharacterData Accuser = new CharacterData();
        Accuser.role = new Accuser();
        Accuser.name = "Accuser";
        Accuser.description = "Game Start: One adjacent Good Villager registers a random Evil Minion.\n\nI Lie and Disguise.";
        Accuser.flavorText = "\"Uno reverse card!\"";
        Accuser.hints = "I turn the Villager into a Wretch disguised as that Villager. It was too hard to code otherwise.";
        Accuser.ifLies = "";
        Accuser.picking = false;
        Accuser.startingAlignment = EAlignment.Evil;
        Accuser.type = ECharacterType.Minion;
        Accuser.abilityUsage = EAbilityUsage.Once;
        Accuser.bluffable = false;
        Accuser.characterId = "Accuser";
        Accuser.cardBgColor = new Color(0.0941f, 0.0431f, 0.0431f);
        Accuser.cardBorderColor = new Color(0.8208f, 0f, 0.0241f);
        Accuser.color = new Color(0.8491f, 0.4555f, 0f);

        CharacterData Hypnotist = new CharacterData();
        Hypnotist.role = new Hypnotist();
        Hypnotist.name = "Hypnotist";
        Hypnotist.description = "I Disguise as and say something that would normally never be a Lie.";
        Hypnotist.flavorText = "\"You are getting sleepy...\"";
        Hypnotist.hints = "I may tell the truth, but that doesn't mean I have their ability.";
        Hypnotist.ifLies = "";
        Hypnotist.picking = false;
        Hypnotist.startingAlignment = EAlignment.Evil;
        Hypnotist.type = ECharacterType.Minion;
        Hypnotist.abilityUsage = EAbilityUsage.Once;
        Hypnotist.bluffable = false;
        Hypnotist.characterId = "Hypnotist";
        Hypnotist.cardBgColor = new Color(0.0941f, 0.0431f, 0.0431f);
        Hypnotist.cardBorderColor = new Color(0.8208f, 0f, 0.0241f);
        Hypnotist.color = new Color(0.8491f, 0.4555f, 0f);


        // Characters.Instance.startGameActOrder = InsertAfterAct("Baa", Sleeper);
        Characters.Instance.startGameActOrder = InsertAfterAct("Alchemist", Accuser);
        Characters.Instance.startGameActOrder = InsertAfterAct("Pooka", MadScientist);

        AscensionsData advancedAscension = ProjectContext.Instance.gameData.advancedAscension;
        foreach (CustomScriptData scriptData in advancedAscension.possibleScriptsData)
        {
            ScriptInfo script = scriptData.scriptInfo;
            AddRole(script.startingTownsfolks, Riddler);
            AddRole(script.startingTownsfolks, Swapper);
            // AddRole(script.startingTownsfolks, Sleeper);
            AddRole(script.startingTownsfolks, Mathematician);
            AddRole(script.startingTownsfolks, Commander);
            AddRole(script.startingTownsfolks, Director);
            //AddRole(script.startingTownsfolks, Scanner);


            AddRole(script.startingOutsiders, MadScientist);
            AddRole(script.startingOutsiders, Necromancer);
            AddRole(script.startingOutsiders, Criminal);


            AddRole(script.startingMinions, Accuser);
            AddRole(script.startingMinions, Hypnotist);
        }
    }
    public void AddRole(Il2CppSystem.Collections.Generic.List<CharacterData> list, CharacterData data)
    {
        if (list.Contains(data))
        {
            return;
        }
        list.Add(data);
    }
    public CharacterData[] allDatas = Array.Empty<CharacterData>();
    public override void OnUpdate()
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
    }
    public CharacterData[] InsertAfterAct(string previous, CharacterData data)
    {
        CharacterData[] actList = Characters.Instance.startGameActOrder;
        int actSize = actList.Length;
        CharacterData[] newActList = new CharacterData[actSize + 1];
        bool inserted = false;
        for (int i = 0; i < actSize; i++)
        {
            if (inserted)
            {
                newActList[i + 1] = actList[i];
            }
            else
            {
                newActList[i] = actList[i];
                if (actList[i].name == previous)
                {
                    newActList[i + 1] = data;
                    inserted = true;
                }
            }
        }
        if (!inserted)
        {
            LoggerInstance.Msg("");
        }
        return newActList;
    }
    public CharacterData[] insertAtEndOfActOrder(CharacterData data)
    {
        CharacterData[] actList = Characters.Instance.startGameActOrder;
        int actSize = actList.Length;
        CharacterData[] newActList = new CharacterData[actSize + 1];
        for (int i = 0; i < actSize; i++)
        {
            newActList[i] = actList[i];
        }
        newActList[actSize] = data;
        return newActList;
    }
}