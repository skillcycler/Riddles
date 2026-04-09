global using Il2Cpp;
using System;
using HarmonyLib;
using Il2CppDissolveExample;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.Injection;
using Il2CppInterop.Runtime.InteropTypes.Arrays;
using Il2CppSystem.IO;
using MelonLoader;
using RiddlerMod;
using UnityEngine;
using static Il2Cpp.Interop;
using static Il2CppSystem.Array;
using static UnityEngine.TouchScreenKeyboard;

[assembly: MelonInfo(typeof(MainMod), "Skill Cycler's Riddles", "0.7", "Skill Cycler")]
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
        ClassInjector.RegisterTypeInIl2Cpp<Scanner>();
        ClassInjector.RegisterTypeInIl2Cpp<Trickster_o>();
        ClassInjector.RegisterTypeInIl2Cpp<Trickster_v>();
        ClassInjector.RegisterTypeInIl2Cpp<Trickster_m>();
        ClassInjector.RegisterTypeInIl2Cpp<Obsessor>();
        ClassInjector.RegisterTypeInIl2Cpp<Lawyer>();

        // Outcasts

        ClassInjector.RegisterTypeInIl2Cpp<MadScientist>();
        ClassInjector.RegisterTypeInIl2Cpp<Necromancer>();
        ClassInjector.RegisterTypeInIl2Cpp<Criminal>();
        ClassInjector.RegisterTypeInIl2Cpp<Ghost>();

        // Minions
        ClassInjector.RegisterTypeInIl2Cpp<Accuser>();
        ClassInjector.RegisterTypeInIl2Cpp<Hypnotist>();
        ClassInjector.RegisterTypeInIl2Cpp<Apprentice>();

        // Demons
        ClassInjector.RegisterTypeInIl2Cpp<Follower>();
        ClassInjector.RegisterTypeInIl2Cpp<Veil>();
    }
    public override void OnLateInitializeMelon()
    {
        GameObject content = GameObject.Find("Game/Gameplay/Content");
        NightPhase nightPhase = content.GetComponent<NightPhase>();
        MelonLogger.Msg(typeof(Confessor).ToString(), typeof(Gossip).ToString(), nameof(Confessor.Act), nameof(Gossip.Act));

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

        CharacterData Scanner = new CharacterData();
        Scanner.role = new Scanner();
        Scanner.name = "Scanner";
        Scanner.description = "Learn how many Outcasts are Disguised or being used as a Disguise.";
        Scanner.flavorText = "\"I spy with my two little eyes, two Outcasts in disguise!\"";
        Scanner.hints = "The Outcast Trickster and Accused villager both count towards this.";
        Scanner.ifLies = "";
        Scanner.picking = false;
        Scanner.startingAlignment = EAlignment.Good;
        Scanner.type = ECharacterType.Villager;
        Scanner.bluffable = true;
        Scanner.characterId = "Scanner";
        Scanner.cardBgColor = new Color(0.26f, 0.1519f, 0.3396f);
        Scanner.cardBorderColor = new Color(0.7133f, 0.339f, 0.8679f);
        Scanner.color = new Color(1f, 0.935f, 0.7302f);

        CharacterData Obsessor = new CharacterData();
        Obsessor.role = new Obsessor();
        Obsessor.name = "Obsessor";
        Obsessor.description = "Learn how many Evils are next to a certain role.";
        Obsessor.flavorText = "\"Once snuck into the Lover's house at night. You'll never guess what happened next\"";
        Obsessor.hints = "";
        Obsessor.ifLies = "";
        Obsessor.picking = false;
        Obsessor.startingAlignment = EAlignment.Good;
        Obsessor.type = ECharacterType.Villager;
        Obsessor.bluffable = true;
        Obsessor.characterId = "Obsessor";
        Obsessor.cardBgColor = new Color(0.26f, 0.1519f, 0.3396f);
        Obsessor.cardBorderColor = new Color(0.7133f, 0.339f, 0.8679f);
        Obsessor.color = new Color(1f, 0.935f, 0.7302f);

        CharacterData Lawyer = new CharacterData();
        Lawyer.role = new Lawyer();
        Lawyer.name = "Lawyer";
        Lawyer.description = "My neighbors tell the truth. Learn a truthful character.";
        Lawyer.flavorText = "\"Do you swear to tell the truth, the whole truth, and nothing but the truth?\"";
        Lawyer.hints = "";
        Lawyer.ifLies = "My neighbors will lie.";
        Lawyer.picking = false;
        Lawyer.startingAlignment = EAlignment.Good;
        Lawyer.type = ECharacterType.Villager;
        Lawyer.bluffable = true;
        Lawyer.characterId = "Lawyer";
        Lawyer.cardBgColor = new Color(0.26f, 0.1519f, 0.3396f);
        Lawyer.cardBorderColor = new Color(0.7133f, 0.339f, 0.8679f);
        Lawyer.color = new Color(1f, 0.935f, 0.7302f);

        CharacterData Trickster_v = new CharacterData();
        Trickster_v.role = new Trickster_v();
        Trickster_v.name = "Trickster";
        Trickster_v.description = "Game Start: There are three of us. One is a Villager, one is an Outcast, and one is a Good Minion.\nYou don't know which is which.\nLearn a card that is the same character type as me.";
        Trickster_v.flavorText = "\"If you thought the Minion twins were bad, get ready for the three of us!\"";
        Trickster_v.hints = "";
        Trickster_v.ifLies = "";
        Trickster_v.picking = false;
        Trickster_v.startingAlignment = EAlignment.Good;
        Trickster_v.type = ECharacterType.Villager;
        Trickster_v.bluffable = false;
        Trickster_v.characterId = "Trickster_v";
        Trickster_v.cardBgColor = new Color(0.26f, 0.1519f, 0.3396f);
        Trickster_v.cardBorderColor = new Color(0.7133f, 0.339f, 0.8679f);
        Trickster_v.color = new Color(1f, 0.935f, 0.7302f);

        CharacterData Trickster_o = new CharacterData();
        Trickster_o.role = new Trickster_o();
        Trickster_o.name = "Trickster";
        Trickster_o.description = "Game Start: There are three of us. One is a Villager, one is an Outcast, and one is a Good Minion.\nYou don't know which is which.\nLearn a card that is the same character type as me.";
        Trickster_o.flavorText = "";
        Trickster_o.hints = "";
        Trickster_o.ifLies = "";
        Trickster_o.picking = false;
        Trickster_o.startingAlignment = EAlignment.Good;
        Trickster_o.type = ECharacterType.Outcast;
        Trickster_o.bluffable = false;
        Trickster_o.characterId = "Trickster_o";
        Trickster_o.cardBgColor = new Color(0.26f, 0.1519f, 0.3396f);
        Trickster_o.cardBorderColor = new Color(0.7133f, 0.339f, 0.8679f);
        Trickster_o.color = new Color(1f, 0.935f, 0.7302f);

        CharacterData Trickster_m = new CharacterData();
        Trickster_m.role = new Trickster_m();
        Trickster_m.name = "Trickster";
        Trickster_m.description = "Game Start: There are three of us. One is a Villager, one is an Outcast, and one is a Good Minion.\nYou don't know which is which.\nLearn a card that is the same character type as me.";
        Trickster_m.flavorText = "";
        Trickster_m.hints = "";
        Trickster_m.ifLies = "";
        Trickster_m.picking = false;
        Trickster_m.startingAlignment = EAlignment.Good;
        Trickster_m.type = ECharacterType.Minion;
        Trickster_m.bluffable = false;
        Trickster_m.characterId = "Trickster_m";
        Trickster_m.cardBgColor = new Color(0.26f, 0.1519f, 0.3396f);
        Trickster_m.cardBorderColor = new Color(0.7133f, 0.339f, 0.8679f);
        Trickster_m.color = new Color(1f, 0.935f, 0.7302f);

        CharacterData MadScientist = new CharacterData();
        MadScientist.role = new MadScientist();
        MadScientist.name = "Mad Scientist";
        MadScientist.description = "I have the ability of a not in play Outcast and Minion. I add 1 fake Outcast and 1-2 fake Minions to the Deck.";
        MadScientist.flavorText = "\"Lil bro is ANGRY at the village\"";
        MadScientist.hints = "I cannot be disguised as. No Evil is crazy enough.";
        MadScientist.ifLies = "I will only Lie if I am somehow guaranteed to be Evil.\nIf I have the Doppelganger, Drunk, or Lunatic (from Wingidon's mod) abilities I will disguise accordingly.";
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
        Necromancer.description = "Pick 1 dead card: Revive it and lose 2 Health. I cannot revive Evils.";
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
        Criminal.hints = "I can kill any card, including Knights, Demons, and myself.\nIf there is no night cycle, I'm just a regular Evil Outcast.";
        Criminal.ifLies = "";
        Criminal.picking = false;
        Criminal.startingAlignment = EAlignment.Evil;
        Criminal.type = ECharacterType.Outcast;
        Criminal.bluffable = false;
        Criminal.characterId = "Criminal";
        Criminal.cardBgColor = new Color(0.102f, 0.0667f, 0.0392f);
        Criminal.cardBorderColor = new Color(0.7843f, 0.6471f, 0f);
        Criminal.color = new Color(0.9659f, 1f, 0.4472f);

        CharacterData Ghost = new CharacterData();
        Ghost.role = new Ghost();
        Ghost.name = "Ghost";
        Ghost.description = "On Reveal: Die and Corrupt 1 unrevealed Villager, dealing 1 damage to you.";
        Ghost.flavorText = "\"I would say 'Boo!' but that's not scary anymore.\"";
        Ghost.hints = "I cannot be revived.";
        Ghost.ifLies = "";
        Ghost.picking = false;
        Ghost.startingAlignment = EAlignment.Good;
        Ghost.type = ECharacterType.Outcast;
        Ghost.bluffable = false;
        Ghost.characterId = "Ghost";
        Ghost.cardBgColor = new Color(0.102f, 0.0667f, 0.0392f);
        Ghost.cardBorderColor = new Color(0.7843f, 0.6471f, 0f);
        Ghost.color = new Color(0.9659f, 1f, 0.4472f);

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

        CharacterData Apprentice = new CharacterData();
        Apprentice.role = new Apprentice();
        Apprentice.name = "Channeler";
        Apprentice.description = "I copy the ability of another Evil.";
        Apprentice.flavorText = "\"I will follow in your footsteps.\"";
        Apprentice.hints = "";
        Apprentice.ifLies = "";
        Apprentice.picking = false;
        Apprentice.startingAlignment = EAlignment.Evil;
        Apprentice.type = ECharacterType.Minion;
        Apprentice.abilityUsage = EAbilityUsage.Once;
        Apprentice.bluffable = false;
        Apprentice.characterId = "Apprentice";
        Apprentice.cardBgColor = new Color(0.0941f, 0.0431f, 0.0431f);
        Apprentice.cardBorderColor = new Color(0.8208f, 0f, 0.0241f);
        Apprentice.color = new Color(0.8491f, 0.4555f, 0f);

        CharacterData Follower = new CharacterData();
        Follower.role = new Follower();
        Follower.name = "Follower";
        Follower.description = "You have slightly more HP in larger villages.\nNight falls every 3 ticks.\n<b>At Night:</b>\nKill 1 card, prioritizing more valuable targets.\nDeal 2 damage to you.\n\nI Lie and Disguise.";
        Follower.flavorText = "\"I'm playing chess and you're playing checkers.\"";
        Follower.hints = "Valuable targets are those with unused active abilities and strong information roles.";
        Follower.ifLies = "";
        Follower.picking = false;
        Follower.startingAlignment = EAlignment.Evil;
        Follower.type = ECharacterType.Demon;
        Follower.bluffable = false;
        Follower.characterId = "Follower";
        Follower.artBgColor = new Color(0.111f, 0.0833f, 0.1415f);
        Follower.cardBgColor = new Color(0.0941f, 0.0431f, 0.0431f);
        Follower.cardBorderColor = new Color(0.8196f, 0.0f, 0.0275f);
        Follower.color = new Color(1f, 0.3804f, 0.3804f);


        CharacterData Veil = new CharacterData();
        Veil.role = new Veil();
        Veil.name = "Veil";
        Veil.description = "You can reveal 3 less cards. Villages are much bigger to compensate.\n\nI Lie and Disguise.";
        Veil.flavorText = "\"I cannot see anyone's role through this dense fog!\"";
        Veil.hints = "";
        Veil.ifLies = "";
        Veil.picking = false;
        Veil.startingAlignment = EAlignment.Evil;
        Veil.type = ECharacterType.Demon;
        Veil.bluffable = false;
        Veil.characterId = "Veil";
        Veil.artBgColor = new Color(0.111f, 0.0833f, 0.1415f);
        Veil.cardBgColor = new Color(0.0941f, 0.0431f, 0.0431f);
        Veil.cardBorderColor = new Color(0.8196f, 0.0f, 0.0275f);
        Veil.color = new Color(1f, 0.3804f, 0.3804f);



        nightPhase.nightCharactersOrder.Add(Follower);
        nightPhase.nightCharactersOrder.Add(Criminal);


        // Characters.Instance.startGameActOrder = InsertAfterAct("Baa", Sleeper);
        Characters.Instance.startGameActOrder = InsertAfterAct("Pooka", MadScientist);
        Characters.Instance.startGameActOrder = InsertAfterAct("Shaman", Trickster_v);
        Characters.Instance.startGameActOrder = InsertAfterAct("Alchemist", Accuser);
        Characters.Instance.startGameActOrder = InsertAfterAct("Accuser", Hypnotist);
        Characters.Instance.startGameActOrder = InsertAfterAct("Hypnotist", Follower);
        Characters.Instance.startGameActOrder = InsertAfterAct("Witch", Veil);
        Characters.Instance.startGameActOrder = InsertAfterAct("Puppeteer", Apprentice);


        CustomScriptData followerScriptData = new CustomScriptData();
        followerScriptData.name = "Follower_1";
        ScriptInfo followerScript = new ScriptInfo();
        Il2CppSystem.Collections.Generic.List<CharacterData> followerList = new Il2CppSystem.Collections.Generic.List<CharacterData>();
        followerList.Add(Follower);
        followerScript.mustInclude = followerList;
        followerScript.startingDemons = followerList;
        followerScript.startingTownsfolks = ProjectContext.Instance.gameData.advancedAscension.possibleScriptsData[0].scriptInfo.startingTownsfolks;
        followerScript.startingOutsiders = ProjectContext.Instance.gameData.advancedAscension.possibleScriptsData[0].scriptInfo.startingOutsiders;
        followerScript.startingMinions = ProjectContext.Instance.gameData.advancedAscension.possibleScriptsData[0].scriptInfo.startingMinions;
        CharactersCount follower_8a = setCharacterCount(5, 1, 1, 1);
        CharactersCount follower_8b = setCharacterCount(4, 1, 2, 1);
        CharactersCount follower_8c = setCharacterCount(4, 2, 1, 1);
        CharactersCount follower_9a = setCharacterCount(5, 2, 1, 1);
        CharactersCount follower_9b = setCharacterCount(5, 1, 2, 1);
        CharactersCount follower_9c = setCharacterCount(4, 2, 2, 1);
        CharactersCount follower_9d = setCharacterCount(6, 1, 1, 1);
        CharactersCount follower_10a = setCharacterCount(7, 0, 2, 1);
        CharactersCount follower_10b = setCharacterCount(6, 1, 2, 1);
        CharactersCount follower_10c = setCharacterCount(5, 2, 2, 1);
        CharactersCount follower_11a = setCharacterCount(7, 1, 2, 1);
        CharactersCount follower_11b = setCharacterCount(6, 2, 2, 1);
        CharactersCount follower_11c = setCharacterCount(6, 1, 3, 1);
        CharactersCount follower_11d = setCharacterCount(7, 0, 3, 1);
        CharactersCount follower_12a = setCharacterCount(7, 2, 2, 1);
        CharactersCount follower_12b = setCharacterCount(6, 3, 2, 1);
        CharactersCount follower_12c = setCharacterCount(8, 0, 3, 1);
        CharactersCount follower_12d = setCharacterCount(7, 1, 3, 1);
        CharactersCount follower_13a = setCharacterCount(8, 1, 3, 1);
        CharactersCount follower_13b = setCharacterCount(9, 0, 3, 1);
        Il2CppSystem.Collections.Generic.List<CharactersCount> followerCounterList = new Il2CppSystem.Collections.Generic.List<CharactersCount>();


        followerCounterList.Add(follower_8a);
        followerCounterList.Add(follower_8b);
        followerCounterList.Add(follower_8c);
        followerCounterList.Add(follower_9a);
        followerCounterList.Add(follower_9b);
        followerCounterList.Add(follower_9c);
        followerCounterList.Add(follower_9d);
        followerCounterList.Add(follower_10a);
        followerCounterList.Add(follower_10b);
        followerCounterList.Add(follower_10c);
        followerCounterList.Add(follower_11a);
        followerCounterList.Add(follower_11b);
        followerCounterList.Add(follower_11c);
        followerCounterList.Add(follower_11d);
        followerCounterList.Add(follower_12a);
        followerCounterList.Add(follower_12b);
        followerCounterList.Add(follower_12c);
        followerCounterList.Add(follower_12d);
        followerCounterList.Add(follower_13a);
        followerCounterList.Add(follower_13b);
        followerScript.characterCounts = followerCounterList;
        followerScriptData.scriptInfo = followerScript;

        CustomScriptData veilScriptData = new CustomScriptData();
        veilScriptData.name = "Veil_1";
        ScriptInfo veilScript = new ScriptInfo();
        Il2CppSystem.Collections.Generic.List<CharacterData> veilList = new Il2CppSystem.Collections.Generic.List<CharacterData>();
        veilList.Add(Veil);
        veilScript.mustInclude = veilList;
        veilScript.startingDemons = veilList;
        veilScript.startingTownsfolks = ProjectContext.Instance.gameData.advancedAscension.possibleScriptsData[0].scriptInfo.startingTownsfolks;
        veilScript.startingOutsiders = ProjectContext.Instance.gameData.advancedAscension.possibleScriptsData[0].scriptInfo.startingOutsiders;
        veilScript.startingMinions = ProjectContext.Instance.gameData.advancedAscension.possibleScriptsData[0].scriptInfo.startingMinions;
        CharactersCount veil_13a = setCharacterCount(8, 1, 3, 1);
        CharactersCount veil_13b = setCharacterCount(9, 0, 3, 1);
        CharactersCount veil_13c = setCharacterCount(9, 1, 2, 1);
        CharactersCount veil_13d = setCharacterCount(8, 2, 2, 1);
        CharactersCount veil_14a = setCharacterCount(8, 2, 3, 1);
        CharactersCount veil_14b = setCharacterCount(9, 1, 3, 1);
        CharactersCount veil_14c = setCharacterCount(10, 0, 3, 1);
        CharactersCount veil_15a = setCharacterCount(11, 0, 3, 1);
        CharactersCount veil_15b = setCharacterCount(10, 1, 3, 1);
        CharactersCount veil_15c = setCharacterCount(9, 2, 3, 1);
        Il2CppSystem.Collections.Generic.List<CharactersCount> veilCounterList = new Il2CppSystem.Collections.Generic.List<CharactersCount>();


        veilCounterList.Add(veil_13a);
        veilCounterList.Add(veil_13b);
        veilCounterList.Add(veil_13c);
        veilCounterList.Add(veil_13d);
        veilCounterList.Add(veil_14a);
        veilCounterList.Add(veil_14b);
        veilCounterList.Add(veil_14c);
        veilCounterList.Add(veil_15a);
        veilCounterList.Add(veil_15b);
        veilCounterList.Add(veil_15c);
        veilCounterList.Add(veil_14b);
        veilCounterList.Add(veil_14c);
        veilCounterList.Add(veil_15a);
        veilCounterList.Add(veil_15b);
        veilScript.characterCounts = veilCounterList;
        veilScriptData.scriptInfo = veilScript;

        AscensionsData advancedAscension = ProjectContext.Instance.gameData.advancedAscension;
        addDemonRole(advancedAscension, Follower, "Baa_Difficult", "Follower_1", followerScriptData, 2);
        addDemonRole(advancedAscension, Veil, "Baa_Difficult", "Veil_1", veilScriptData, 2);

        foreach (CustomScriptData scriptData in advancedAscension.possibleScriptsData)
        {
            ScriptInfo script = scriptData.scriptInfo;
            AddRole(script.startingTownsfolks, Riddler);
            AddRole(script.startingTownsfolks, Swapper);
            // AddRole(script.startingTownsfolks, Sleeper);
            AddRole(script.startingTownsfolks, Mathematician);
            AddRole(script.startingTownsfolks, Commander);
            AddRole(script.startingTownsfolks, Director);
            AddRole(script.startingTownsfolks, Scanner);
            AddRole(script.startingTownsfolks, Trickster_v);
            AddRole(script.startingTownsfolks, Obsessor);
            AddRole(script.startingTownsfolks, Lawyer);


            AddRole(script.startingOutsiders, MadScientist);
            AddRole(script.startingOutsiders, Necromancer);
            AddRole(script.startingOutsiders, Criminal);
            AddRole(script.startingOutsiders, Ghost);


            AddRole(script.startingMinions, Accuser);
            AddRole(script.startingMinions, Hypnotist);
            AddRole(script.startingMinions, Apprentice);
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
    public CharactersCount setCharacterCount(int Villagers, int Outcasts, int Minions, int Demons)
    {
        CharactersCount myCharacterCount = new CharactersCount(Villagers + Outcasts + Minions + Demons, Villagers, Demons, Outcasts, Minions);
        myCharacterCount.dOuts = Outcasts + 1;
        return myCharacterCount;
    }
    public void addDemonRole(AscensionsData advancedAscension, CharacterData? data, string oldScriptName, string newScriptName, CustomScriptData NewScript, int weight = 1)
    {
        if (data == null)
        {
            return;
        }
        foreach (CustomScriptData scriptData in advancedAscension.possibleScriptsData)
        {
            if (scriptData.name == oldScriptName)
            {
                CustomScriptData newScriptData = GameObject.Instantiate(scriptData);
                newScriptData.name = newScriptName;
                ScriptInfo newScript = new ScriptInfo();
                ScriptInfo script = NewScript.scriptInfo;
                newScriptData.scriptInfo = newScript;
                newScript.startingTownsfolks = script.startingTownsfolks;
                newScript.startingOutsiders = script.startingOutsiders;
                newScript.startingMinions = script.startingMinions;
                newScript.startingDemons = script.startingDemons;
                newScript.characterCounts = NewScript.scriptInfo.characterCounts;
                var newPSD = advancedAscension.possibleScriptsData.Append(newScriptData);
                for (int i = 0; i < weight - 1; i++)
                {
                    newPSD = newPSD.Append(newScriptData);
                }
                advancedAscension.possibleScriptsData = newPSD.ToArray();
                return;
            }
        }
    }
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
        if (Statics.charactersArray.Length == 0)
        {
            var loadedCharList = Resources.FindObjectsOfTypeAll(Il2CppType.Of<CharacterData>());
            if (loadedCharList != null)
            {
                Statics.charactersArray = new CharacterData[loadedCharList.Length];
                for (int i = 0; i < loadedCharList.Length; i++)
                {
                    CharacterData data = loadedCharList[i]!.Cast<CharacterData>();
                    Statics.CheckAddRole(data);
                    Statics.charactersArray[i] = data;
                }
            }
            if (Statics.charactersArray.Length > 0)
            {
                this.OnFirstUpdate();
            }
        }
    }
    public void OnFirstUpdate()
    {
        Transform chars = GameObject.Find("Game/Gameplay/Content/Canvas/Characters").transform;
        for (int i = 12; i < 16; i++)
        {
            Statics.checkCreateCircle(chars, i);
        }
        for (int j = 2; j < 5; j++)
        {
            Statics.checkCreateCircle(chars, j);
        }
    }
    public static class Statics
    {
        public static Dictionary<string, CharacterData> roles = new Dictionary<string, CharacterData>();
        public static CharacterData[] charactersArray = Il2CppSystem.Array.Empty<CharacterData>();
        static GameObject circChar = GameObject.Find("Game/Gameplay/Content/Canvas/Characters/Circle_6/Character");
        static GameObject circCharLeft = GameObject.Find("Game/Gameplay/Content/Canvas/Characters/Circle_6/Character (1)");
        static GameObject circCharRight = GameObject.Find("Game/Gameplay/Content/Canvas/Characters/Circle_6/Character (4)");
        static GameObject circCharDown = GameObject.Find("Game/Gameplay/Content/Canvas/Characters/Circle_6/Character (3)");
        public static void checkCreateCircle(Transform parent, int size)
        {
            string name = "Circle_" + size;
            Transform t = parent.FindChild(name);
            if (t != null)
            {
                MelonLogger.Msg("Object Already exists!: " + name);
                return;
            }
            createCircle(parent, size, name);
        }
        public static void createCircle(Transform parent, int size, string name)
        {
            GameObject circle = new GameObject();
            circle.name = name;
            circle.transform.SetParent(parent);
            CharactersPool circlePool = circle.AddComponent<CharactersPool>();
            circlePool.characters = new Character[size];
            for (int i = 0; i < size; i++)
            {
                GameObject card;
                int rotation = 360 * i / size;
                if (rotation <= 30)
                {
                    card = GameObject.Instantiate(circChar);
                }
                else if (rotation <= 149)
                {
                    card = GameObject.Instantiate(circCharLeft);
                    rotation += 300;
                }
                else if (rotation <= 210)
                {
                    card = GameObject.Instantiate(circCharDown);
                    rotation += 180;
                }
                else if (rotation <= 329)
                {
                    card = GameObject.Instantiate(circCharRight);
                    rotation += 120;
                }
                else
                {
                    card = GameObject.Instantiate(circChar);
                }
                card.transform.SetParent(circle.transform);
                string cardname = "Character";
                if (i > 0)
                {
                    cardname += " (" + i + ")";
                }
                card.name = cardname;
                Transform icon = card.transform.Find("Icon");
                card.transform.Rotate(0, 0, rotation);
                icon.Rotate(0, 0, 360 - rotation);
                circlePool.characters[i] = card.GetComponent<Character>();
            }
            circle.transform.position = new UnityEngine.Vector3(0f, 1f, 85.9444f);
            circle.transform.localScale = new UnityEngine.Vector3(1f, 1f, 1f);
            circle.SetActive(false);
            addToCharsPool(circlePool);
        }
        public static void addToCharsPool(CharactersPool pool)
        {
            CharactersPool[] pools = Characters.Instance.characterPool;
            CharactersPool[] newPools = new CharactersPool[pools.Length + 1];
            for (int i = 0; i < pools.Length; i++)
            {
                newPools[i] = pools[i];
            }
            newPools[pools.Length] = pool;
            Characters.Instance.characterPool = newPools;
        }

        public static void GetStartingRoles()
        {
            AscensionsData allCharactersAscension = ProjectContext.Instance.gameData.allCharactersAscension;
            foreach (CharacterData data in allCharactersAscension.startingTownsfolks)
            {
                CheckAddRole(data);
            }
            foreach (CharacterData data in allCharactersAscension.startingOutsiders)
            {
                CheckAddRole(data);
            }
            foreach (CharacterData data in allCharactersAscension.startingMinions)
            {
                CheckAddRole(data);
            }
            foreach (CharacterData data in allCharactersAscension.startingDemons)
            {
                CheckAddRole(data);
            }
        }
        public static void CheckAddRole(CharacterData data)
        {
            string name = data.name;
            if (!roles.ContainsKey(name))
            {
                roles.Add(name, data);
            }
        }
    }
    [HarmonyPatch(typeof(Confessor), nameof(Confessor.GetInfo))]
    private static class GetHypnotistConfessorInfo
    {
        private static bool Prefix(Confessor __instance, Character charRef)
        {
            if (charRef.bluff)
            {
                if (charRef.bluff.characterId != "Confessor_18741708")
                {
                    return true;
                }
            }
            else if (charRef.dataRef.characterId != "Confessor_18741708")
            {
                return true;
            }
            ActedInfo myInfo = new ActedInfo("I am Good");
            if (charRef.statuses.Contains(ECharacterStatus.Corrupted) || charRef.GetAlignment() == EAlignment.Evil)
            {
                myInfo = new ActedInfo("I am dizzy");
            }
            if (charRef.bluff && charRef.dataRef.characterId == "Hypnotist")
            {
                myInfo = new ActedInfo("I am Good");
            }
            __instance.onActed?.Invoke(myInfo);
            return false;
        }
    }
}