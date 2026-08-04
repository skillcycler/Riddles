global using Il2Cpp;
using System;
using System.Data.SqlTypes;
using System.Reflection;
using HarmonyLib;
using Il2CppDissolveExample;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.Injection;
using Il2CppInterop.Runtime.InteropTypes.Arrays;
using Il2CppRewired.UI.ControlMapper;
using Il2CppSystem.IO;
using MelonLoader;
using MelonLoader.Utils;
using RiddlerMod;
using UnityEngine;
using static Il2Cpp.Interop;
using static Il2CppSystem.Array;
using static Il2CppSystem.Runtime.Remoting.RemotingServices;
using static MelonLoader.MelonLaunchOptions;
using static MelonLoader.MelonLogger;
using static UnityEngine.TouchScreenKeyboard;

[assembly: MelonInfo(typeof(MainMod), "Skill Cycler's Riddles", "1.10", "Skill Cycler")]
[assembly: MelonGame("UmiArt", "Demon Bluff")]

namespace RiddlerMod;
public class MainMod : MelonMod
{
    public override void OnInitializeMelon()
    {
        ClassInjector.RegisterTypeInIl2Cpp<Riddler>();
        ClassInjector.RegisterTypeInIl2Cpp<Swapper>();
        ClassInjector.RegisterTypeInIl2Cpp<Mathematician>();
        ClassInjector.RegisterTypeInIl2Cpp<Commander>();
        ClassInjector.RegisterTypeInIl2Cpp<Director>();
        ClassInjector.RegisterTypeInIl2Cpp<Scanner>();
        ClassInjector.RegisterTypeInIl2Cpp<Trickster>();
        ClassInjector.RegisterTypeInIl2Cpp<Obsessor>();
        ClassInjector.RegisterTypeInIl2Cpp<Lawyer>();
        ClassInjector.RegisterTypeInIl2Cpp<Psychic>();
        ClassInjector.RegisterTypeInIl2Cpp<Weaver>();
        ClassInjector.RegisterTypeInIl2Cpp<Nurse>();
        ClassInjector.RegisterTypeInIl2Cpp<Stylist>();
        ClassInjector.RegisterTypeInIl2Cpp<Coach>();
        ClassInjector.RegisterTypeInIl2Cpp<Comedian>();
        ClassInjector.RegisterTypeInIl2Cpp<Innkeeper>();
        ClassInjector.RegisterTypeInIl2Cpp<Recruiter>();
        ClassInjector.RegisterTypeInIl2Cpp<Engineer>();
        ClassInjector.RegisterTypeInIl2Cpp<Governor>();
        ClassInjector.RegisterTypeInIl2Cpp<Officer>();
        ClassInjector.RegisterTypeInIl2Cpp<Cowboy>();
        ClassInjector.RegisterTypeInIl2Cpp<Surveyor>();
        ClassInjector.RegisterTypeInIl2Cpp<Tracker>();
        ClassInjector.RegisterTypeInIl2Cpp<Pioneer>();
        ClassInjector.RegisterTypeInIl2Cpp<Necromancer>();
        ClassInjector.RegisterTypeInIl2Cpp<Astronaut>();
        ClassInjector.RegisterTypeInIl2Cpp<Motivator>();
        ClassInjector.RegisterTypeInIl2Cpp<Therapist>();
        ClassInjector.RegisterTypeInIl2Cpp<Crewmate>();
        ClassInjector.RegisterTypeInIl2Cpp<Sharpshooter>();
        ClassInjector.RegisterTypeInIl2Cpp<Guide>();
        ClassInjector.RegisterTypeInIl2Cpp<Preacher>();

        // Outcasts

        ClassInjector.RegisterTypeInIl2Cpp<MadScientist>();
        ClassInjector.RegisterTypeInIl2Cpp<Hitman>();
        ClassInjector.RegisterTypeInIl2Cpp<Ghost>();
        ClassInjector.RegisterTypeInIl2Cpp<Muddler>();
        ClassInjector.RegisterTypeInIl2Cpp<Confectioner>();
        ClassInjector.RegisterTypeInIl2Cpp<Captivator>();
        ClassInjector.RegisterTypeInIl2Cpp<Reflector>();
        ClassInjector.RegisterTypeInIl2Cpp<Gambler>();
        ClassInjector.RegisterTypeInIl2Cpp<Anchor>();
        ClassInjector.RegisterTypeInIl2Cpp<Prankster>();

        // Minions
        ClassInjector.RegisterTypeInIl2Cpp<Accuser>();
        ClassInjector.RegisterTypeInIl2Cpp<Hypnotist>();
        ClassInjector.RegisterTypeInIl2Cpp<Channeler>();
        ClassInjector.RegisterTypeInIl2Cpp<Sleeper>();
        ClassInjector.RegisterTypeInIl2Cpp<Guardian>();
        ClassInjector.RegisterTypeInIl2Cpp<Mastermind>();
        ClassInjector.RegisterTypeInIl2Cpp<Baffler>();
        ClassInjector.RegisterTypeInIl2Cpp<Wizard>();
        ClassInjector.RegisterTypeInIl2Cpp<Slanderer>();
        ClassInjector.RegisterTypeInIl2Cpp<Enigma>();
        ClassInjector.RegisterTypeInIl2Cpp<Squire>();

        // Demons
        ClassInjector.RegisterTypeInIl2Cpp<Follower>();
        ClassInjector.RegisterTypeInIl2Cpp<Veil>();
        ClassInjector.RegisterTypeInIl2Cpp<Summoner>();
        ClassInjector.RegisterTypeInIl2Cpp<Infestation>();
        ClassInjector.RegisterTypeInIl2Cpp<Escapist>();
        ClassInjector.RegisterTypeInIl2Cpp<Kingmaker>();
        ClassInjector.RegisterTypeInIl2Cpp<Mystifier>();
        ClassInjector.RegisterTypeInIl2Cpp<RainbowJoker>();
        ClassInjector.RegisterTypeInIl2Cpp<Atheist>();
        ClassInjector.RegisterTypeInIl2Cpp<Fracture>();
        Instance = this;
    }
    public CharacterData makeNewCharacter(string name, EAlignment startingAlignment, ECharacterType type, bool bluffable, bool usuallyDisguised, string flavorText, bool picking = false)
    {
        CharacterData character = new CharacterData();
        character.name = name;
        character.characterName = name;
        character.picking = picking;
        character.startingAlignment = startingAlignment;
        character.flavorText = flavorText;
        character.type = type;
        character.bluffable = bluffable;
        character.additionalFlavorTexts = new Il2CppStringArray(1);
        character.additionalFlavorTexts[0] = flavorText;
        character.characterId = name + "_scm";
        switch (type)
        {
            case ECharacterType.Villager:
                character.artBgColor = new Color(0.111f, 0.0833f, 0.1415f);
                character.cardBgColor = new Color(0.26f, 0.1519f, 0.3396f);
                character.cardBorderColor = new Color(0.7133f, 0.339f, 0.8679f);
                character.color = new Color(1f, 0.935f, 0.7302f);
                break;
            case ECharacterType.Outcast:
                character.cardBgColor = new Color(0.102f, 0.0667f, 0.0392f);
                character.cardBorderColor = new Color(0.7843f, 0.6471f, 0f);
                character.color = new Color(0.9659f, 1f, 0.4472f);
                break;
            case ECharacterType.Minion:
                character.cardBgColor = new Color(0.0941f, 0.0431f, 0.0431f);
                character.cardBorderColor = new Color(0.8208f, 0f, 0.0241f);
                character.color = new Color(0.8491f, 0.4555f, 0f);
                break;
            case ECharacterType.Demon:
                character.artBgColor = new Color(0.111f, 0.0833f, 0.1415f);
                character.cardBgColor = new Color(0.0941f, 0.0431f, 0.0431f);
                character.cardBorderColor = new Color(0.8196f, 0.0f, 0.0275f);
                character.color = new Color(1f, 0.3804f, 0.3804f);
                break;
        }
        character.bundledCharacters = new Il2CppSystem.Collections.Generic.List<CharacterData>();
        character.additionalPossibleCharacters = new AddedCharacterTypes();
        character.usuallyDisguised = usuallyDisguised;
        character.hints = "";
        character.ifLies = "";
        return character;
    }
    public AddedCharacterTypes MakeAddedCharacters(int v, int o, int m, int d)
    {
        AddedCharacterTypes a = new AddedCharacterTypes();
        CharacterCount cv = new CharacterCount();
        cv.count = v;
        cv.type = ECharacterType.Villager;
        CharacterCount co = new CharacterCount();
        co.count = o;
        co.type = ECharacterType.Outcast;
        CharacterCount cm = new CharacterCount();
        cm.count = m;
        cm.type = ECharacterType.Minion;
        CharacterCount cd = new CharacterCount();
        cd.count = d;
        cd.type = ECharacterType.Demon;
        a.count.Add(cv);
        a.count.Add(co);
        a.count.Add(cm);
        a.count.Add(cd);
        return a;
    }
    public static void MakeTwelve()
    {
        GameObject circle12 = CreateCircle(12);
        GameObject circle13 = CreateCircle(13);
        GameObject circle14 = CreateCircle(14);
        GameObject circle15 = CreateCircle(15);
        GameObject circleForTesting = CreateCircle(21);
    }
    public static void AddConfigs()
    {
        MelonPreferences_Category configCategory = MelonPreferences.CreateCategory("RiddlesConfig");
        configCategory.CreateEntry("Follower", true, "Follower", "Whether Follower can show up");
        configCategory.CreateEntry("Summoner", true, "Summoner", "Whether Summoner can show up");
        configCategory.CreateEntry("Escapist", true, "Escapist", "Whether Escapist can show up");
        configCategory.CreateEntry("Rainbow Joker", true, "Rainbow Joker", "Whether Rainbow Joker can show up");
        configCategory.CreateEntry("Atheist", true, "Atheist", "Whether Atheist can show up");
        configCategory.CreateEntry("Veil", true, "Veil", "Whether Veil can show up");
        configCategory.CreateEntry("Kingmaker", true, "Kingmaker", "Whether Kingmaker can show up");
        configCategory.CreateEntry("Infestation", true, "Infestation", "Whether Infestation can show up");
        configCategory.CreateEntry("Mystifier", true, "Mystifier", "Whether Mystifier can show up");
        configCategory.CreateEntry("Fracture", true, "Fracture", "Whether Fracture can show up");
        configCategory.SetFilePath(System.IO.Path.Combine(MelonEnvironment.UserDataDirectory, "RiddlesConfig.cfg"));
        configCategory.SaveToFile();
    }
    public override void OnLateInitializeMelon()
    {
        GameObject content = GameObject.Find("Game/Gameplay/Content");
        NightPhase nightPhase = content.GetComponent<NightPhase>();
        MakeTwelve();
        ModifyBaseGame.UpdateWitness();
        PatchNights.Patch();
        AddConfigs();

        CharacterData Riddler = makeNewCharacter("Riddler", EAlignment.Good, ECharacterType.Villager, true, false, "\"One day I'll cause a paradox.\"");
        Riddler.role = new Riddler();
        Riddler.description = "Learn a true fact about the game.";
        Riddler.hints = "Statements are accurate as of July 15, 2026, or version 0.762a of the game. If you are playing in a later version, statements may not be accurate.";
        Riddler.ifLies = "Learn a false fact about the game.";

        CharacterData Swapper = makeNewCharacter("Swapper", EAlignment.Good, ECharacterType.Villager, true, false, "\"Didn't like the role you got? I'm here to save the day!\"", true);
        Swapper.role = new Swapper();
        Swapper.description = "Pick 2 cards: They disguise as each other's apparent role. Refresh both of their statements or abilities.";
        Swapper.hints = "A Swapper cannot swap itself or another Swapper.\n\nIf you have Wingidon's Expansion Pack installed, Swapper also cannot swap Devout claims.";
        Swapper.ifLies = "Both targets are Corrupted if they are Villagers.";


        CharacterData Mathematician = makeNewCharacter("Mathematician", EAlignment.Good, ECharacterType.Villager, true, false, "\"21\"");
        Mathematician.role = new Mathematician();
        Mathematician.description = "Learn a number equal to the sum of the card numbers of 2 Evils.";

        CharacterData Commander = makeNewCharacter("Commander", EAlignment.Good, ECharacterType.Villager, true, false, "\"Leads the Villagers by day, hunts the Minions at night.\"", true);
        Commander.role = new Commander();
        Commander.description = "Pick 2 cards: Learn a card of a different character type from both.";

        CharacterData Director = makeNewCharacter("Director", EAlignment.Good, ECharacterType.Villager, true, false, "\"There are no lights. There is no camera. But there's certainly a lot of action.\"");
        Director.role = new Director();
        Director.description = "Learn a consecutive group of cards that contain 2 Evils.";
        Director.hints = "I always go clockwise from the first number to the second number. Both endpoints are included.";

        CharacterData Scanner = makeNewCharacter("Scanner", EAlignment.Good, ECharacterType.Villager, true, false, "\"I spy with my two little eyes, two Outcasts in disguise!\"");
        Scanner.role = new Scanner();
        Scanner.description = "Learn how many cards are either Disguised as Outcasts or Outcasts that are Disguised. I ignore outcasts' misregistration abilities.";

        CharacterData Obsessor = makeNewCharacter("Obsessor", EAlignment.Good, ECharacterType.Villager, true, false, "\"Once snuck into the Lover's house at night. You'll never guess what happened next\"");
        Obsessor.role = new Obsessor();
        Obsessor.description = "Learn how many Evils are next to a certain role.";

        CharacterData Lawyer = makeNewCharacter("Lawyer", EAlignment.Good, ECharacterType.Villager, true, false, "\"Do you swear to tell the truth, the whole truth, and nothing but the truth?\"");
        Lawyer.role = new Lawyer();
        Lawyer.description = "My neighbors tell the truth. Learn a truthful character.";
        Lawyer.hints = "If I am not Lying:\nI will only point to my neighbors if they are evil.";

        CharacterData Psychic = makeNewCharacter("Psychic", EAlignment.Good, ECharacterType.Villager, true, false, "\"I may be able to read your mind.\"");
        Psychic.role = new Psychic();
        Psychic.description = "Learn 2 characters. Exactly 1 is in play.";
        Psychic.hints = "I can see through misregistration.";
        Psychic.ifLies = "Neither or both are in play.";

        CharacterData Weaver = makeNewCharacter("Weaver", EAlignment.Good, ECharacterType.Villager, true, false, "\"The Knitter's younger sister. Still recovering from that incident with the Evil Villagers.\"");
        Weaver.role = new Weaver();
        Weaver.description = "Learn how many pairs of Villagers there are.";

        CharacterData Nurse = makeNewCharacter("Nurse", EAlignment.Good, ECharacterType.Villager, true, false, "\"I can cure the Drunk, I promise!\"", true);
        Nurse.role = new Nurse();
        Nurse.description = "Pick 1 alive card: I cure most of their negative status effects.\nIf I cure an Evil character, I also kill them.\n\nIf I am not Lying and there are no curable characters, I will say so.";
        Nurse.hints = "My ability refreshes every night.\n\nThe statuses I can cure include, but are not limited to: Corrupted, Confused, Accused.";
        Nurse.ifLies = "\"I couldn't cure #x\"";
        Nurse.abilityUsage = EAbilityUsage.ResetAfterNight;

        CharacterData Stylist = makeNewCharacter("Stylist", EAlignment.Good, ECharacterType.Villager, true, false, "\"Taking clients from the Swapper since 2025\"", true);
        Stylist.role = new Stylist();
        Stylist.description = "Pick an alive Disguised character. Change their Disguise.";
        Stylist.ifLies = "\"I couldn't change #x's Disguise\"";

        CharacterData Coach = makeNewCharacter("Coach", EAlignment.Good, ECharacterType.Villager, true, false, "\"Demon Bluff is a team building game.\"", true);
        Coach.role = new Coach();
        Coach.description = "Pick 1 card: Learn how many characters near them [Range 2] are the same Type as them.";

        CharacterData Comedian = makeNewCharacter("Comedian", EAlignment.Good, ECharacterType.Villager, true, false, "\"You will be blown away by his performance when he teams up with the Jester!\"", true);
        Comedian.role = new Comedian();
        Comedian.description = "Pick 3 cards: Learn 2 that are both disguised or both not disguised.";

        CharacterData Innkeeper = makeNewCharacter("Innkeeper", EAlignment.Good, ECharacterType.Villager, true, false, "\"Need a place to stay? I got you!\"", true);
        Innkeeper.role = new Innkeeper();
        Innkeeper.description = "On activate: Heal 1 HP. Refreshes at night.";
        Innkeeper.ifLies = "Lose 1 HP.";
        Innkeeper.abilityUsage = EAbilityUsage.ResetAfterNight;

        CharacterData Recruiter = makeNewCharacter("Recruiter", EAlignment.Good, ECharacterType.Villager, true, false, "\"Hello and Welcome to the greatest village of all time!\"");
        Recruiter.role = new Recruiter();
        Recruiter.description = "Game Start: 1 random Outcast is turned into a Villager.";
        Recruiter.additionalPossibleCharacters = MakeAddedCharacters(0, -1, 0, 0);
        Recruiter.hints = "My ability runs before any Corruption-causing characters, so it still works if I am Corrupted.\n\nIf I am Truthful and my ability somehow fails when there are Outcasts in play:\n\"#x rejected my offer to join the village\"";
        Recruiter.ifLies = "I point to someone that isn't a Villager";

        CharacterData Engineer = makeNewCharacter("Engineer", EAlignment.Good, ECharacterType.Villager, true, false, "\"The long lost brother of the Architect.\"");
        Engineer.role = new Engineer();
        Engineer.description = "Learn whether the top or bottom half of the circle has more Evils.";
        Engineer.hints = "The top half is considered as all cards within [Range (Cards/4)] of the highest numbered card.\n\nThe bottom half is all cards outside that range, except in the case where the number of cards is a multiple of 4, in which the leftmost and rightmost cards are also counted.";

        CharacterData Governor = makeNewCharacter("Governor", EAlignment.Good, ECharacterType.Villager, true, false, "\"I know everyone around here.\"");
        Governor.role = new Governor();
        Governor.description = "Learn how many Villagers are actually in the village.";
        Governor.ifLies = "The number will be off by 1.";

        CharacterData Officer = makeNewCharacter("Officer", EAlignment.Good, ECharacterType.Villager, true, false, "\"Worried about not knowing if that Undying is safe to stab? Fear not, I'm here to save the day.\"");
        Officer.role = new Officer();
        Officer.description = "Learn how many characters register as Evil.";
        Officer.ifLies = "The number will be off by 1.";

        CharacterData Cowboy = makeNewCharacter("Cowboy", EAlignment.Good, ECharacterType.Villager, true, false, "\"Never approach a bull from the front, a horse from the rear or a fool from any direction.\"");
        Cowboy.role = new Cowboy();
        Cowboy.description = "Learn an Evil or Evil-registering Villager or Outcast.";
        Cowboy.hints = "For example, I see the Wretch as an Evil-registering Outcast.";

        CharacterData Surveyor = makeNewCharacter("Surveyor", EAlignment.Good, ECharacterType.Villager, true, false, "This land belongs to the Outcasts, not the Minions. Wretch, you're not welcome here.");
        Surveyor.role = new Surveyor();
        Surveyor.description = "Learn how many Outcasts and Minions there actually are";
        Surveyor.ifLies = "Both the Outcast and Minion count will be wrong";

        CharacterData Tracker = makeNewCharacter("Tracker", EAlignment.Good, ECharacterType.Villager, true, false, "\"You're not hiding from me that easily!\"");
        Tracker.role = new Tracker();
        Tracker.description = "Learn who is Disguised as an Outcast.";

        CharacterData Pioneer = makeNewCharacter("Pioneer", EAlignment.Good, ECharacterType.Villager, true, false, "\"Why does everyone keep mistaking me for the Scout?\"");
        Pioneer.role = new Pioneer();
        Pioneer.description = "Learn how many cards a particular Evil is from my closest Evil.";
        Pioneer.hints = "If my closest 2 Evils are equidistant, which one I refer to is arbitrary.";

        CharacterData Necromancer = makeNewCharacter("Necromancer", EAlignment.Good, ECharacterType.Villager, true, false, "\"Second chances are real. Just like Empaths and Mayors.\"", true);
        Necromancer.role = new Necromancer();
        Necromancer.description = "Pick 2 cards (not myself), one alive and one dead. Kill the alive and revive the dead. I cannot revive Evils or the Ghost.";
        Necromancer.ifLies = "The revived card will lie with its new info.";

        CharacterData Astronaut = makeNewCharacter("Astronaut", EAlignment.Good, ECharacterType.Villager, true, false, "\"Always has been.\"");
        Astronaut.role = new Astronaut();
        Astronaut.description = "At Night: Learn a character of a different Alignment than the previous night.";
        Astronaut.ifLies = "I provide arbitrary info for nights before I was flipped and false info after being flipped.";

        CharacterData Sharpshooter = makeNewCharacter("Sharpshooter", EAlignment.Good, ECharacterType.Villager, true, false, "\"Fastest gunslinger in the West\"");
        Sharpshooter.role = new Sharpshooter();
        Sharpshooter.description = "At Night: Learn that a particular Evil is among 5 cards.";

        CharacterData Motivator = makeNewCharacter("Motivator", EAlignment.Good, ECharacterType.Villager, true, false, "\"Go Go Go! You can do it!\"");
        Motivator.role = new Motivator();
        Motivator.description = "If revealed: My alive neighbors refresh at night.";

        CharacterData Trickster = makeNewCharacter("Trickster", EAlignment.Good, ECharacterType.Villager, false, false, "\"If you thought the Minion twins were bad, get ready for the three of us!\"");
        Trickster.description = "Game Start: There are three of us. One is a Villager, one is an Outcast, and one is a Good Minion.\nWhile alive, you don't know which is which.\nLearn a card that is the same character type as me.\nI am immune to Corruption and getting Accused.";
        Trickster.role = new Trickster();

        CharacterData Therapist = makeNewCharacter("Therapist", EAlignment.Good, ECharacterType.Villager, true, false, "\"Let's all get along, shall we?\"");
        Therapist.description = "Learn the 2 characters that I think have the least in common.";
        Therapist.role = new Therapist();
        Therapist.hints = "Any two characters of different alignments will always have less in common than any two characters of the same alignment.";
        Therapist.ifLies = "I point to two cards of the same alignment.";

        CharacterData Crewmate = makeNewCharacter("Crewmate", EAlignment.Good, ECharacterType.Villager, true, false, "\"Red.\"");
        Crewmate.description = "Learn someone who is Sus. (In other words, a Demon or someone that can affect someone else)";
        Crewmate.role = new Crewmate();

        CharacterData Guide = makeNewCharacter("Guide", EAlignment.Good, ECharacterType.Villager, true, false, "\"You seem lost. Want a hand?\"");
        Guide.description = "At night: Learn who is a different character type from the previous character.";
        Guide.role = new Guide();
        Guide.ifLies = "I will never say a character is a different type from itself.";

        CharacterData Preacher = makeNewCharacter("Preacher", EAlignment.Good, ECharacterType.Villager, true, false, "\"Confess all your sins to me.\"", true);
        Preacher.description = "Pick 1 alive character: They disguise as a Confessor. My ability refreshes at night.";
        Preacher.role = new Preacher();
        Preacher.ifLies = "The Confessor will be randomly good or dizzy";
        Preacher.abilityUsage = EAbilityUsage.ResetAfterNight;

        CharacterData MadScientist = makeNewCharacter("MadScientist", EAlignment.Good, ECharacterType.Outcast, false, false, "\"Lil bro is ANGRY at the village\"");
        MadScientist.role = new MadScientist();
        MadScientist.name = "Mad Scientist";
        MadScientist.characterName = "Mad Scientist";
        MadScientist.description = "I have the ability of a not in play Outcast and Minion. I add 1 fake Outcast and 2 fake Minions to the Deck.";
        MadScientist.hints = "I cannot be disguised as.\nI will not Disguise or turn Evil if part of my Outcast's ability includes those.";
        
        CharacterData Hitman = makeNewCharacter("Hitman", EAlignment.Evil, ECharacterType.Outcast, false, true, "\"No one is safe from me, not even myself\"");
        Hitman.role = new Hitman();
        Hitman.name = "Hitman";
        Hitman.characterName = "Hitman";
        Hitman.description = "I Lie and Disguise.\n\nOn odd numbered nights: Kill a random card.\nOn even numbered nights: lose 3 HP.";
        Hitman.hints = "I can kill any card, including Knights, Demons, and myself.\nIf there is no night cycle, I'm just a regular Evil Outcast.";
        
        CharacterData Ghost = makeNewCharacter("Ghost", EAlignment.Good, ECharacterType.Outcast, false, false, "\"I would say 'Boo!' but that's not scary anymore.\"");
        Ghost.role = new Ghost();
        Ghost.description = "On Reveal: Die, dealing 1 damage to you. One unrevealed Good character is Corrupted. The night counter does not tick.";
        Ghost.hints = "I cannot be revived by the Necromancer.";
        Ghost.ifLies = "\"I am a real Medium.\"\nI still die, dealing 1 damage, but I don't Corrupt anyone.";

        CharacterData Muddler = makeNewCharacter("Muddler", EAlignment.Good, ECharacterType.Outcast, true, false, "\"I don't know, was it?\"");
        Muddler.role = new Muddler();
        Muddler.description = "Status effects (like Corrupted) are not displayed.";
        
        CharacterData Confectioner = makeNewCharacter("Confectioner", EAlignment.Good, ECharacterType.Outcast, false, true, "\"She got jealous of the Baker. So she took revenge.\"");
        Confectioner.role = new Confectioner();
        Confectioner.description = "Game Start: One random Villager is turned into a Corrupted Baker.\n\nI disguise as the Original Baker.";

        CharacterData Captivator = makeNewCharacter("Captivator", EAlignment.Good, ECharacterType.Outcast, false, true, "\"My information makes sense, I swear!\"");
        Captivator.role = new Captivator();
        Captivator.description = "I disguise as and say something that neither a truth teller nor liar could say in my position.\n\nI am normally seen as Lying.\n\nYou only take 2 damage if I am Executed.";
        Captivator.hints = "Some examples of what I can say are: Empress pointing to 2 Evils, Bishop pointing to 3 Outcasts";

        CharacterData Reflector = makeNewCharacter("Reflector", EAlignment.Good, ECharacterType.Outcast, false, true, "\"Look at you, you're so confused!\"");
        Reflector.role = new Reflector();
        Reflector.description = "I disguise as a Villager and am Confused.\nConfused characters have a 50% chance of Lying.\n\nYou only take 3 damage if I am Executed.";

        CharacterData Gambler = makeNewCharacter("Gambler", EAlignment.Good, ECharacterType.Outcast, false, false, "\"Aw dang it!\"");
        Gambler.role = new Gambler();
        Gambler.description = "Game Start: 1 random character (not myself) is afflicted with a random status effect: Accused, Corrupted, Confused, Evil-turned. Learn who I affected.";

        CharacterData Anchor = makeNewCharacter("Anchor", EAlignment.Good, ECharacterType.Outcast, true, false, "\"Where do you think you're going?\"");
        Anchor.role = new Anchor();
        Anchor.description = "You have 9 max HP, even if other cards add or subtract from max HP.";

        CharacterData Prankster = makeNewCharacter("Prankster", EAlignment.Good, ECharacterType.Outcast, true, false, "\"Oh no, here we go again...\"");
        Prankster.role = new Prankster();
        Prankster.description = "Game Start: 2 cards of different alignments swap alignments. Learn who.";

        CharacterData BabyMinion = makeNewCharacter("BabyMinion", EAlignment.Evil, ECharacterType.Minion, false, true, "\"The youngest member of the Minion family.\"");
        BabyMinion.role = new BabyMinion();
        BabyMinion.description = "I am a Minion created from a problematic interaction between a Demon from this mod and a Minion from another mod.\nThere may be multiple of me in play.\n\nI Lie and Disguise.";
        BabyMinion.name = "Baby Minion";
        BabyMinion.characterName = "Baby Minion";

        CharacterData Accuser = makeNewCharacter("Accuser", EAlignment.Evil, ECharacterType.Minion, false, true, "\"Uno reverse card!\"");
        Accuser.role = new Accuser();
        Accuser.description = "Game Start: One adjacent Good character registers a random Evil Minion.\n\nI Lie and Disguise.";

        CharacterData Hypnotist = makeNewCharacter("Hypnotist", EAlignment.Evil, ECharacterType.Minion, false, true, "\"You are getting sleepy...\"");
        Hypnotist.role = new Hypnotist();
        Hypnotist.description = "I disguise as and say something that would otherwise always be true.";
        Hypnotist.hints = "I may tell the truth, but that doesn't mean I have their ability.\n\nI always register as Truthful.";

        CharacterData Channeler = makeNewCharacter("Channeler", EAlignment.Evil, ECharacterType.Minion, false, true, "\"I will follow in your footsteps.\"");
        Channeler.role = new Channeler();
        Channeler.description = "I copy the ability of another Evil Minion or Demon. Some Evil abilities cannot be copied.\n\nI Lie and Disguise.";
        
        CharacterData Sleeper = makeNewCharacter("Sleeper", EAlignment.Evil, ECharacterType.Minion, false, true, "\"Ever feel like you get enough sleep? Not anymore!\"");
        Sleeper.role = new Sleeper();
        Sleeper.description = "The night cycle is 1 tick shorter if there is one.\n\nI Lie and Disguise.";

        CharacterData Guardian = makeNewCharacter("Guardian", EAlignment.Evil, ECharacterType.Minion, false, true, "\"You're gonna have to get through me first.\"");
        Guardian.role = new Guardian();
        Guardian.description = "Adjacent non-Accused characters register as Good, as their Disguise, and as Honest.";

        CharacterData Mastermind = makeNewCharacter("Mastermind", EAlignment.Evil, ECharacterType.Minion, false, true, "\"It all comes back to me.\"");
        Mastermind.role = new Mastermind();
        Mastermind.description = "Game Start: All Minions register as the Mastermind and appear as a Mastermind on death.\n\nI Lie and Disguise.";

        CharacterData Baffler = makeNewCharacter("Baffler", EAlignment.Evil, ECharacterType.Minion, false, true, "\"Want to reliably know whether someone's lying? Well too bad. You're not getting it this time.\"");
        Baffler.role = new Baffler();
        Baffler.description = "Game Start: One adjacent Villager is Confused.\nConfused characters have a 50% chance of Lying.\n\nI Lie and Disguise.";

        CharacterData Wizard = makeNewCharacter("Wizard", EAlignment.Evil, ECharacterType.Minion, false, true, "\"It's black magic.\"");
        Wizard.role = new Wizard();
        Wizard.description = "Game Start: One random Outcast or Minion (not myself), if there is one, is duplicated.\n\nI Lie and Disguise.";
        Wizard.additionalPossibleCharacters = MakeAddedCharacters(0, 1, 1, 0);
        Wizard.hints = "The duplicated character can replace any other Villager, Outcast, or Minion.";

        CharacterData Slanderer = makeNewCharacter("Slanderer", EAlignment.Evil, ECharacterType.Minion, false, true, "\"They're not Corrupted! They're just Evil!\"");
        Slanderer.role = new Slanderer();
        Slanderer.description = "Game Start: The card(s) furthest away from me register as the wrong alignment.\n\nI Lie and Disguise.";

        CharacterData Enigma = makeNewCharacter("Enigma", EAlignment.Evil, ECharacterType.Minion, false, true, "\"lbh jvyy arire thrff jung guvf fnlf\"");
        Enigma.role = new Enigma();
        Enigma.description = "There are extra fake characters in the deck equal to the last digit of my card number.\n\nI Lie and Disguise.";
        Enigma.hints = "I can add fake disguised Outcasts, fake Minions, and fake Demons.";

        CharacterData Squire = makeNewCharacter("Squire", EAlignment.Evil, ECharacterType.Minion, false, true, "\"Come on, show me how much you can take!\"");
        Squire.role = new Squire();
        Squire.description = "I disguise as a Knight and can't be killed unless all other Evils are dead.\nI register as a Truthful and Honest Knight.";

        CharacterData Follower = makeNewCharacter("Follower", EAlignment.Evil, ECharacterType.Demon, false, true, "\"I'm playing chess and you're playing checkers.\"");
        Follower.role = new Follower();
        Follower.description = "You have slightly more HP in larger villages.\nNight falls every 3 ticks.\n<b>At Night:</b>\nKill 1 card, prioritizing more valuable targets.\nDeal 2 damage to you.\n\nI Lie and Disguise.";
        Follower.hints = "Valuable targets are those with unused active abilities and strong information roles.";

        CharacterData Veil = makeNewCharacter("Veil", EAlignment.Evil, ECharacterType.Demon, false, true, "\"You cannot see anyone's role through this dense fog!\"");
        Veil.role = new Veil();
        Veil.description = "2-3 cards cannot be revealed. Villages are much bigger to compensate.\n\nI Lie and Disguise.";
        Veil.hints = "If someone else copies my effect, they only hide 1 additional card.";
        
        CharacterData Summoner = makeNewCharacter("Summoner", EAlignment.Evil, ECharacterType.Demon, false, true, "\"Let's see... What does this spell do? Summon a demon? That sounds useful.\"");
        Summoner.role = new Summoner();
        Summoner.description = "Game Start: There are no Outcasts or Minions in play. One or more other cards become Demons, which are not added to the Deck.\n\nI Lie and Disguise.\n\nYou start with 0-5 extra HP.";
        Summoner.hints = "The night cycle is always active if I am in play.";
        Summoner.additionalPossibleCharacters = MakeAddedCharacters(0, 0, 0, 4);

        CharacterData Infestation = makeNewCharacter("Infestation", EAlignment.Evil, ECharacterType.Demon, false, true, "\"The one zombie apocalypse you'll stand no chance in\"");
        Infestation.role = new Infestation();
        Infestation.description = "Game Start: 1 random character is Corrupted.\n\nAt Night: Kill all Good Corrupted characters, dealing 1 damage each. Good Characters adjacent to alive Corrupted characters are Corrupted.\n\nI Lie and Disguise.";
        Infestation.hints = "Certain characters that remove Corruptions will stop my ability from working.";

        CharacterData Escapist = makeNewCharacter("Escapist", EAlignment.Evil, ECharacterType.Demon, false, true, "\"Catch me if you can!\"");
        Escapist.role = new Escapist();
        Escapist.description = "Game Start: There is an extra Outcast. One non-Bombardier Outcast is Evil and Corrupted.\n\nI Lie and Disguise as an Outcast.";
        Escapist.additionalPossibleCharacters = MakeAddedCharacters(0, 1, 0, 0);

        CharacterData Kingmaker = makeNewCharacter("Kingmaker", EAlignment.Evil, ECharacterType.Demon, false, true, "\"'Puppet Master' is more like it.\"");
        Kingmaker.role = new Kingmaker();
        Kingmaker.description = "Game Start: Both my neighbors become Minions. I act before any Minions do.\n\nYou don't know how many Evils there are.\n\nI tell the truth and Disguise.";
        Kingmaker.hints = "There may be fewer Outcasts in play than expected.";
        Kingmaker.additionalPossibleCharacters = MakeAddedCharacters(0, -2, 2, 0);

        CharacterData Mystifier = makeNewCharacter("Mystifier", EAlignment.Evil, ECharacterType.Demon, false, true, "\"Puzzled, confounded, or astonished yet?\"");
        Mystifier.role = new Mystifier();
        Mystifier.description = "At night: One random Villager is Confused.\nConfused characters have a 50% chance of Lying.\n\nI Lie and Disguise.";
        Mystifier.hints = "Confused characters that are currently Lying also register as Corrupted.";

        CharacterData RainbowJoker = makeNewCharacter("RainbowJoker", EAlignment.Evil, ECharacterType.Demon, false, true, "\"A total wild card.\"");
        RainbowJoker.role = new RainbowJoker();
        RainbowJoker.description = "1/3 (rounded down) of all non-Demons become random Minions. Then, 0-4 characters become random Outcasts. None of these characters are added to the Deck.\n\nI Lie and Disguise.";
        RainbowJoker.name = "Rainbow Joker";
        RainbowJoker.characterName = "Rainbow Joker";
        RainbowJoker.additionalPossibleCharacters = MakeAddedCharacters(0, 4, 4, 0);

        CharacterData Atheist = makeNewCharacter("Atheist", EAlignment.Evil, ECharacterType.Demon, false, true, "\"I can break whatever rules I feel like.\"");
        Atheist.role = new Atheist();
        Atheist.description = "Game Start: I have a 50% chance to turn Good. All statuses are hidden.\n\nIf Good:\nLose if you execute me. I might register as Evil.\nIf Evil:\nThere are no Evil characters. Some characters may lie or register as Evil.";

        CharacterData Fracture = makeNewCharacter("Fracture", EAlignment.Evil, ECharacterType.Demon, false, true, "\"And into my pocket dimension you go!\"");
        Fracture.role = new Fracture();
        Fracture.description = "Game Start & At night: 1 random character is Erased. Erased characters are unable to be mentioned by passive abilities and don't count towards adjacencies or distances.";

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
        CharactersCount veil_9a = setCharacterCount(6, 0, 2, 1);
        CharactersCount veil_9b = setCharacterCount(6, 1, 1, 1);
        CharactersCount veil_10a = setCharacterCount(7, 0, 2, 1);
        CharactersCount veil_10b = setCharacterCount(7, 1, 1, 1);
        CharactersCount veil_10c = setCharacterCount(6, 1, 2, 1);
        CharactersCount veil_11a = setCharacterCount(7, 1, 2, 1);
        CharactersCount veil_11b = setCharacterCount(7, 0, 3, 1);
        CharactersCount veil_11c = setCharacterCount(8, 0, 2, 1);
        CharactersCount veil_11d = setCharacterCount(8, 1, 1, 1);
        CharactersCount veil_12a = setCharacterCount(9, 0, 2, 1);
        CharactersCount veil_12b = setCharacterCount(8, 0, 3, 1);
        CharactersCount veil_12c = setCharacterCount(8, 1, 2, 1);
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

        for (int i = 0; i < 5; i++)
        {
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
        }
        veilCounterList.Add(veil_9a);
        veilCounterList.Add(veil_9b);
        veilCounterList.Add(veil_10a);
        veilCounterList.Add(veil_10b);
        veilCounterList.Add(veil_10c);
        veilCounterList.Add(veil_11a);
        veilCounterList.Add(veil_11b);
        veilCounterList.Add(veil_11c);
        veilCounterList.Add(veil_11d);
        veilCounterList.Add(veil_11a);
        veilCounterList.Add(veil_11b);
        veilCounterList.Add(veil_11c);
        veilCounterList.Add(veil_11d);
        veilCounterList.Add(veil_12a);
        veilCounterList.Add(veil_12b);
        veilCounterList.Add(veil_12c);

        veilScript.characterCounts = veilCounterList;
        veilScriptData.scriptInfo = veilScript;
        
        CustomScriptData summonerScriptData = new CustomScriptData();
        summonerScriptData.name = "Summoner_1";
        ScriptInfo summonerScript = new ScriptInfo();
        Il2CppSystem.Collections.Generic.List<CharacterData> summonerList = new Il2CppSystem.Collections.Generic.List<CharacterData>();
        summonerList.Add(Summoner);
        summonerScript.mustInclude = summonerList;
        summonerScript.startingDemons = summonerList;
        summonerScript.startingTownsfolks = ProjectContext.Instance.gameData.advancedAscension.possibleScriptsData[0].scriptInfo.startingTownsfolks;
        summonerScript.startingOutsiders = ProjectContext.Instance.gameData.advancedAscension.possibleScriptsData[0].scriptInfo.startingOutsiders;
        summonerScript.startingMinions = ProjectContext.Instance.gameData.advancedAscension.possibleScriptsData[0].scriptInfo.startingMinions;
        // 7-8 cards: 1 summon
        CharactersCount summoner_7 = setCharacterCount(6, 0, 0, 1);
        CharactersCount summoner_8 = setCharacterCount(7, 0, 0, 1);
        // 9-10 cards: 1-2 summons
        CharactersCount summoner_9 = setCharacterCount(8, 0, 0, 1);
        CharactersCount summoner_10 = setCharacterCount(9, 0, 0, 1);
        // 11-12 cards: 2-3 summons
        CharactersCount summoner_11 = setCharacterCount(10, 0, 0, 1);
        CharactersCount summoner_12 = setCharacterCount(11, 0, 0, 1);
        // 13+ cards: 3-4 summons
        CharactersCount summoner_13 = setCharacterCount(12, 0, 0, 1);
        CharactersCount summoner_14 = setCharacterCount(13, 0, 0, 1);
        CharactersCount summoner_15 = setCharacterCount(14, 0, 0, 1);

        Il2CppSystem.Collections.Generic.List<CharactersCount> summonerCounterList = new Il2CppSystem.Collections.Generic.List<CharactersCount>();
        
        CustomScriptData infestationScriptData = new CustomScriptData();
        infestationScriptData.name = "Infestation_1";
        ScriptInfo infestationScript = new ScriptInfo();
        Il2CppSystem.Collections.Generic.List<CharacterData> infestationList = new Il2CppSystem.Collections.Generic.List<CharacterData>();
        infestationList.Add(Infestation);
        infestationScript.mustInclude = infestationList;
        infestationScript.startingDemons = infestationList;
        infestationScript.startingTownsfolks = ProjectContext.Instance.gameData.advancedAscension.possibleScriptsData[0].scriptInfo.startingTownsfolks;
        infestationScript.startingOutsiders = ProjectContext.Instance.gameData.advancedAscension.possibleScriptsData[0].scriptInfo.startingOutsiders;
        infestationScript.startingMinions = ProjectContext.Instance.gameData.advancedAscension.possibleScriptsData[0].scriptInfo.startingMinions;
        CharactersCount clocktower_8 = setCharacterCount(5, 1, 1, 1);
        CharactersCount clocktower_9 = setCharacterCount(5, 2, 1, 1);
        CharactersCount infestation_9b = setCharacterCount(5, 1, 2, 1);
        CharactersCount infestation_9c = setCharacterCount(6, 1, 1, 1);
        CharactersCount clocktower_10 = setCharacterCount(7, 0, 2, 1);
        CharactersCount infestation_10b = setCharacterCount(7, 1, 1, 1);
        CharactersCount clocktower_11 = setCharacterCount(7, 1, 2, 1);
        CharactersCount clocktower_12 = setCharacterCount(7, 2, 2, 1);
        CharactersCount clocktower_13 = setCharacterCount(9, 0, 3, 1);
        CharactersCount clocktower_14 = setCharacterCount(9, 1, 3, 1);
        CharactersCount clocktower_15 = setCharacterCount(9, 2, 3, 1);
        Il2CppSystem.Collections.Generic.List<CharactersCount> infestationCounterList = new Il2CppSystem.Collections.Generic.List<CharactersCount>();


        infestationCounterList.Add(clocktower_8);
        infestationCounterList.Add(clocktower_9);
        infestationCounterList.Add(infestation_9b);
        infestationCounterList.Add(infestation_9c);
        infestationCounterList.Add(clocktower_10);
        infestationCounterList.Add(infestation_10b);
        infestationCounterList.Add(clocktower_10);
        infestationCounterList.Add(infestation_10b);
        infestationCounterList.Add(clocktower_11);
        infestationCounterList.Add(clocktower_11);
        infestationCounterList.Add(clocktower_12);
        infestationCounterList.Add(clocktower_12);
        infestationCounterList.Add(clocktower_13);
        infestationCounterList.Add(clocktower_13);
        infestationCounterList.Add(clocktower_14);
        infestationCounterList.Add(clocktower_14);
        infestationCounterList.Add(clocktower_15);
        infestationCounterList.Add(clocktower_15);

        infestationScript.characterCounts = infestationCounterList;
        infestationScriptData.scriptInfo = infestationScript;

        CustomScriptData escapistScriptData = new CustomScriptData();
        escapistScriptData.name = "Escapist_1";
        ScriptInfo escapistScript = new ScriptInfo();
        Il2CppSystem.Collections.Generic.List<CharacterData> escapistList = new Il2CppSystem.Collections.Generic.List<CharacterData>();
        escapistList.Add(Escapist);
        escapistScript.mustInclude = escapistList;
        escapistScript.startingDemons = escapistList;
        escapistScript.startingTownsfolks = ProjectContext.Instance.gameData.advancedAscension.possibleScriptsData[0].scriptInfo.startingTownsfolks;
        escapistScript.startingOutsiders = ProjectContext.Instance.gameData.advancedAscension.possibleScriptsData[0].scriptInfo.startingOutsiders;
        escapistScript.startingMinions = ProjectContext.Instance.gameData.advancedAscension.possibleScriptsData[0].scriptInfo.startingMinions;
        CharactersCount escapist_8a = setCharacterCount(5, 2, 0, 1);
        CharactersCount escapist_8b = setCharacterCount(4, 2, 1, 1);
        CharactersCount escapist_9 = setCharacterCount(5, 1, 2, 1);
        CharactersCount escapist_10a = setCharacterCount(6, 1, 2, 1);
        CharactersCount escapist_10b = setCharacterCount(6, 2, 1, 1);
        CharactersCount escapist_11 = setCharacterCount(7, 2, 1, 1);
        Il2CppSystem.Collections.Generic.List<CharactersCount> escapistCounterList = new Il2CppSystem.Collections.Generic.List<CharactersCount>();


        escapistCounterList.Add(escapist_8a);
        escapistCounterList.Add(escapist_8b);
        escapistCounterList.Add(clocktower_8);
        escapistCounterList.Add(escapist_9);
        escapistCounterList.Add(clocktower_9);
        escapistCounterList.Add(escapist_10a);
        escapistCounterList.Add(escapist_10b);
        escapistCounterList.Add(escapist_11);
        escapistCounterList.Add(clocktower_11);

        escapistScript.characterCounts = escapistCounterList;
        escapistScriptData.scriptInfo = escapistScript;

        CustomScriptData kingmakerScriptData = new CustomScriptData();
        kingmakerScriptData.name = "Kingmaker_1";
        ScriptInfo kingmakerScript = new ScriptInfo();
        Il2CppSystem.Collections.Generic.List<CharacterData> kingmakerList = new Il2CppSystem.Collections.Generic.List<CharacterData>();
        kingmakerList.Add(Kingmaker);
        kingmakerScript.mustInclude = kingmakerList;
        kingmakerScript.startingDemons = kingmakerList;
        kingmakerScript.startingTownsfolks = ProjectContext.Instance.gameData.advancedAscension.possibleScriptsData[0].scriptInfo.startingTownsfolks;
        kingmakerScript.startingOutsiders = ProjectContext.Instance.gameData.advancedAscension.possibleScriptsData[0].scriptInfo.startingOutsiders;
        kingmakerScript.startingMinions = ProjectContext.Instance.gameData.advancedAscension.possibleScriptsData[0].scriptInfo.startingMinions;
        CharactersCount kingmaker_7a = setCharacterCount(4, 1, 1, 1);
        CharactersCount kingmaker_7b = setCharacterCount(4, 0, 2, 1);
        CharactersCount kingmaker_8 = setCharacterCount(4, 2, 1, 1);
        CharactersCount kingmaker_9a = setCharacterCount(5, 1, 2, 1);
        CharactersCount kingmaker_9b = setCharacterCount(6, 1, 1, 1);
        CharactersCount kingmaker_10a = setCharacterCount(6, 1, 2, 1);
        CharactersCount kingmaker_10b = setCharacterCount(6, 2, 1, 1);
        CharactersCount kingmaker_10c = setCharacterCount(5, 2, 2, 1);
        CharactersCount kingmaker_10d = setCharacterCount(7, 1, 1, 1);
        CharactersCount kingmaker_11a = setCharacterCount(8, 0, 2, 1);
        CharactersCount kingmaker_11b = setCharacterCount(7, 0, 3, 1);
        CharactersCount kingmaker_11c = setCharacterCount(6, 0, 4, 1);
        CharactersCount kingmaker_11d = setCharacterCount(6, 1, 3, 1);
        CharactersCount kingmaker_12a = setCharacterCount(7, 1, 3, 1);
        CharactersCount kingmaker_12b = setCharacterCount(7, 0, 4, 1);
        CharactersCount kingmaker_12c = setCharacterCount(8, 0, 3, 1);
        CharactersCount kingmaker_12d = setCharacterCount(6, 2, 3, 1);
        Il2CppSystem.Collections.Generic.List<CharactersCount> kingmakerCounterList = new Il2CppSystem.Collections.Generic.List<CharactersCount>();


        kingmakerCounterList.Add(kingmaker_7a);
        kingmakerCounterList.Add(kingmaker_7b);
        kingmakerCounterList.Add(kingmaker_8);
        kingmakerCounterList.Add(clocktower_8);
        kingmakerCounterList.Add(kingmaker_9a);
        kingmakerCounterList.Add(kingmaker_9b);
        kingmakerCounterList.Add(clocktower_9);
        kingmakerCounterList.Add(kingmaker_10a);
        kingmakerCounterList.Add(kingmaker_10b);
        kingmakerCounterList.Add(kingmaker_10c);
        kingmakerCounterList.Add(kingmaker_10d);
        kingmakerCounterList.Add(clocktower_10);
        kingmakerCounterList.Add(kingmaker_11a);
        kingmakerCounterList.Add(kingmaker_11b);
        kingmakerCounterList.Add(kingmaker_11c);
        kingmakerCounterList.Add(kingmaker_11d);
        kingmakerCounterList.Add(clocktower_11);
        kingmakerCounterList.Add(kingmaker_12a);
        kingmakerCounterList.Add(kingmaker_12b);
        kingmakerCounterList.Add(kingmaker_12c);
        kingmakerCounterList.Add(kingmaker_12d);

        kingmakerScript.characterCounts = kingmakerCounterList;
        kingmakerScriptData.scriptInfo = kingmakerScript;

        CustomScriptData MystifierScriptData = new CustomScriptData();
        MystifierScriptData.name = "Mystifier_1";
        ScriptInfo MystifierScript = new ScriptInfo();
        Il2CppSystem.Collections.Generic.List<CharacterData> MystifierList = new Il2CppSystem.Collections.Generic.List<CharacterData>();
        MystifierList.Add(Mystifier);
        MystifierScript.mustInclude = MystifierList;
        MystifierScript.startingDemons = MystifierList;
        MystifierScript.startingTownsfolks = ProjectContext.Instance.gameData.advancedAscension.possibleScriptsData[0].scriptInfo.startingTownsfolks;
        MystifierScript.startingOutsiders = ProjectContext.Instance.gameData.advancedAscension.possibleScriptsData[0].scriptInfo.startingOutsiders;
        MystifierScript.startingMinions = ProjectContext.Instance.gameData.advancedAscension.possibleScriptsData[0].scriptInfo.startingMinions;
        CharactersCount Mystifier_8 = setCharacterCount(4, 1, 2, 1);
        CharactersCount Mystifier_9a = setCharacterCount(6, 0, 2, 1);
        CharactersCount Mystifier_9b = setCharacterCount(5, 1, 2, 1);
        CharactersCount Mystifier_9c = setCharacterCount(6, 1, 1, 1);
        CharactersCount Mystifier_10a = setCharacterCount(6, 1, 2, 1);
        CharactersCount Mystifier_10b = setCharacterCount(6, 2, 1, 1);
        CharactersCount Mystifier_11a = setCharacterCount(7, 0, 3, 1);
        CharactersCount Mystifier_11b = setCharacterCount(6, 1, 3, 1);
        CharactersCount Mystifier_11c = setCharacterCount(8, 0, 2, 1);
        Il2CppSystem.Collections.Generic.List<CharactersCount> MystifierCounterList = new Il2CppSystem.Collections.Generic.List<CharactersCount>();

        MystifierCounterList.Add(clocktower_8);
        MystifierCounterList.Add(Mystifier_8);
        MystifierCounterList.Add(clocktower_8);
        MystifierCounterList.Add(Mystifier_8);
        MystifierCounterList.Add(Mystifier_9a);
        MystifierCounterList.Add(Mystifier_9b);
        MystifierCounterList.Add(Mystifier_10a);
        MystifierCounterList.Add(Mystifier_10b);
        MystifierCounterList.Add(clocktower_10);
        MystifierCounterList.Add(Mystifier_9a);
        MystifierCounterList.Add(Mystifier_9b);
        MystifierCounterList.Add(Mystifier_10a);
        MystifierCounterList.Add(Mystifier_10b);
        MystifierCounterList.Add(clocktower_10);
        MystifierCounterList.Add(Mystifier_11a);
        MystifierCounterList.Add(Mystifier_11b);
        MystifierCounterList.Add(Mystifier_11c);
        MystifierCounterList.Add(clocktower_11);
        MystifierCounterList.Add(clocktower_12);
        MystifierCounterList.Add(clocktower_13);
        MystifierCounterList.Add(clocktower_14);
        MystifierCounterList.Add(clocktower_15);

        MystifierScript.characterCounts = MystifierCounterList;
        MystifierScriptData.scriptInfo = MystifierScript;

        CustomScriptData RainbowJokerScriptData = new CustomScriptData();
        RainbowJokerScriptData.name = "RainbowJoker_1";
        ScriptInfo RainbowJokerScript = new ScriptInfo();
        Il2CppSystem.Collections.Generic.List<CharacterData> RainbowJokerList = new Il2CppSystem.Collections.Generic.List<CharacterData>();
        RainbowJokerList.Add(RainbowJoker);
        RainbowJokerScript.mustInclude = RainbowJokerList;
        RainbowJokerScript.startingDemons = RainbowJokerList;
        RainbowJokerScript.startingTownsfolks = ProjectContext.Instance.gameData.advancedAscension.possibleScriptsData[0].scriptInfo.startingTownsfolks;
        RainbowJokerScript.startingOutsiders = ProjectContext.Instance.gameData.advancedAscension.possibleScriptsData[0].scriptInfo.startingOutsiders;
        RainbowJokerScript.startingMinions = ProjectContext.Instance.gameData.advancedAscension.possibleScriptsData[0].scriptInfo.startingMinions;
        Il2CppSystem.Collections.Generic.List<CharactersCount> RainbowJokerCounterList = new Il2CppSystem.Collections.Generic.List<CharactersCount>();

        // same compositions as Summoner
        RainbowJokerCounterList.Add(summoner_7);
        RainbowJokerCounterList.Add(summoner_8);
        RainbowJokerCounterList.Add(summoner_9);
        RainbowJokerCounterList.Add(summoner_10);
        RainbowJokerCounterList.Add(summoner_11);
        RainbowJokerCounterList.Add(summoner_12);
        RainbowJokerCounterList.Add(summoner_13);
        RainbowJokerCounterList.Add(summoner_14);
        RainbowJokerCounterList.Add(summoner_15);

        RainbowJokerScript.characterCounts = RainbowJokerCounterList;
        RainbowJokerScriptData.scriptInfo = RainbowJokerScript;

        CustomScriptData AtheistScriptData = new CustomScriptData();
        AtheistScriptData.name = "Atheist_1";
        ScriptInfo AtheistScript = new ScriptInfo();
        Il2CppSystem.Collections.Generic.List<CharacterData> AtheistList = new Il2CppSystem.Collections.Generic.List<CharacterData>();
        AtheistList.Add(Atheist);
        AtheistScript.mustInclude = AtheistList;
        AtheistScript.startingDemons = AtheistList;
        AtheistScript.startingTownsfolks = ProjectContext.Instance.gameData.advancedAscension.possibleScriptsData[0].scriptInfo.startingTownsfolks;
        AtheistScript.startingOutsiders = ProjectContext.Instance.gameData.advancedAscension.possibleScriptsData[0].scriptInfo.startingOutsiders;
        AtheistScript.startingMinions = ProjectContext.Instance.gameData.advancedAscension.possibleScriptsData[0].scriptInfo.startingMinions;
        Il2CppSystem.Collections.Generic.List<CharactersCount> AtheistCounterList = new Il2CppSystem.Collections.Generic.List<CharactersCount>();

        AtheistCounterList.Add(clocktower_8);
        AtheistCounterList.Add(setCharacterCount(4, 1, 2, 1));
        AtheistCounterList.Add(clocktower_9);
        AtheistCounterList.Add(setCharacterCount(5, 1, 2, 1));
        AtheistCounterList.Add(clocktower_10);
        AtheistCounterList.Add(setCharacterCount(6, 1, 2, 1));

        AtheistScript.characterCounts = AtheistCounterList;
        AtheistScriptData.scriptInfo = AtheistScript;

        CustomScriptData FractureScriptData = new CustomScriptData();
        FractureScriptData.name = "Fracture_1";
        ScriptInfo FractureScript = new ScriptInfo();
        Il2CppSystem.Collections.Generic.List<CharacterData> FractureList = new Il2CppSystem.Collections.Generic.List<CharacterData>();
        FractureList.Add(Fracture);
        FractureScript.mustInclude = FractureList;
        FractureScript.startingDemons = FractureList;
        FractureScript.startingTownsfolks = ProjectContext.Instance.gameData.advancedAscension.possibleScriptsData[0].scriptInfo.startingTownsfolks;
        FractureScript.startingOutsiders = ProjectContext.Instance.gameData.advancedAscension.possibleScriptsData[0].scriptInfo.startingOutsiders;
        FractureScript.startingMinions = ProjectContext.Instance.gameData.advancedAscension.possibleScriptsData[0].scriptInfo.startingMinions;
        Il2CppSystem.Collections.Generic.List<CharactersCount> FractureCounterList = new Il2CppSystem.Collections.Generic.List<CharactersCount>();

        FractureCounterList.Add(clocktower_8);
        FractureCounterList.Add(clocktower_9);
        FractureCounterList.Add(clocktower_10);
        FractureCounterList.Add(clocktower_11);
        FractureCounterList.Add(clocktower_12);
        FractureCounterList.Add(clocktower_13);
        FractureCounterList.Add(clocktower_14);
        FractureCounterList.Add(clocktower_15);

        FractureScript.characterCounts = FractureCounterList;
        FractureScriptData.scriptInfo = FractureScript;

        int testingMode = 0; // 0=off, 1=villager, 2=outcast, 3=minion, 4-mixed, 5=lying villagers

        switch (testingMode)
        {
            case 0:
                summonerCounterList.Add(summoner_7);
                summonerCounterList.Add(summoner_8);
                for (int i = 0; i < 3; i++)
                {
                    summonerCounterList.Add(summoner_9);
                    summonerCounterList.Add(summoner_10);
                    summonerCounterList.Add(summoner_11);
                    summonerCounterList.Add(summoner_12);
                    summonerCounterList.Add(summoner_13);
                }
                summonerCounterList.Add(summoner_14);
                summonerCounterList.Add(summoner_15);
                break;
            case 1:
                summonerCounterList.Add(setCharacterCount(20, 0, 0, 1)); // villager test
                break;
            case 2:
                summonerCounterList.Add(setCharacterCount(8, 12, 0, 1)); // outcast test
                break;
            case 3:
                summonerCounterList.Add(setCharacterCount(8, 0, 12, 1)); // minion test
                break;
            case 4:
                summonerCounterList.Add(setCharacterCount(6, 7, 7, 1)); // mixed test
                break;
            case 5:
                summonerCounterList.Add(setCharacterCount(5, 0, 15, 1)); // Test lying villagers
                break;
        }
        summonerScript.characterCounts = summonerCounterList;
        summonerScriptData.scriptInfo = summonerScript;

        // ------------ NIGHT PHASE ------------
        nightPhase.nightCharactersOrder.Add(Baffler);
        nightPhase.nightCharactersOrder.Add(Mystifier);
        nightPhase.nightCharactersOrder.Add(Infestation);
        nightPhase.nightCharactersOrder.Add(Follower);
        nightPhase.nightCharactersOrder.Add(Channeler);
        nightPhase.nightCharactersOrder.Add(Hitman);
        nightPhase.nightCharactersOrder.Add(Sleeper);
        nightPhase.nightCharactersOrder.Add(Fracture);
        // and now for the villagers that will act at night
        nightPhase.nightCharactersOrder.Add(Astronaut);
        nightPhase.nightCharactersOrder.Add(Sharpshooter);
        nightPhase.nightCharactersOrder.Add(Motivator);
        nightPhase.nightCharactersOrder.Add(Guide);

        // ------------ GAME START ------------
        Characters.Instance.startGameActOrder = InsertAtStartOfActOrder(Summoner);
        Characters.Instance.startGameActOrder = InsertAtStartOfActOrder(RainbowJoker);
        Characters.Instance.startGameActOrder = InsertAtStartOfActOrder(Atheist);
        Characters.Instance.startGameActOrder = InsertAtStartOfActOrder(Fracture);
        Characters.Instance.startGameActOrder = InsertAfterAct("Summoner", Kingmaker);
        Characters.Instance.startGameActOrder = InsertAfterAct("Kingmaker", Wizard);
        Characters.Instance.startGameActOrder = InsertAfterAct("Wizard", Guardian);
        Characters.Instance.startGameActOrder = InsertAfterAct("Guardian", Escapist);
        Characters.Instance.startGameActOrder = InsertAfterAct("Chancellor", Recruiter);
        Characters.Instance.startGameActOrder = InsertAfterAct("Shaman", MadScientist);
        Characters.Instance.startGameActOrder = InsertAfterAct("Mad Scientist", Confectioner);
        Characters.Instance.startGameActOrder = InsertAfterAct("Confectioner", Channeler);
        Characters.Instance.startGameActOrder = InsertAfterAct("Channeler", Trickster);
        Characters.Instance.startGameActOrder = InsertAfterAct("Witch", Veil);
        Characters.Instance.startGameActOrder = InsertAfterAct("Poisoner", Accuser);
        Characters.Instance.startGameActOrder = InsertAfterAct("Accuser", Baffler);
        Characters.Instance.startGameActOrder = InsertAfterAct("Baffler", Slanderer);
        Characters.Instance.startGameActOrder = InsertAfterAct("Slanderer", Mystifier);
        Characters.Instance.startGameActOrder = InsertAfterAct("Mystifier", Reflector);
        Characters.Instance.startGameActOrder = InsertAfterAct("Reflector", Infestation);
        Characters.Instance.startGameActOrder = InsertAfterAct("Infestation", Gambler);
        Characters.Instance.startGameActOrder = InsertAtEndOfActOrder(Sleeper);
        Characters.Instance.startGameActOrder = InsertAtEndOfActOrder(Follower);
        Characters.Instance.startGameActOrder = InsertAtEndOfActOrder(Lawyer);
        Characters.Instance.startGameActOrder = InsertAtEndOfActOrder(Muddler);
        Characters.Instance.startGameActOrder = InsertAtEndOfActOrder(Enigma);
        Characters.Instance.startGameActOrder = InsertAtEndOfActOrder(Mastermind); // This must act after any minions.
        Characters.Instance.startGameActOrder = InsertAtEndOfActOrder(Anchor);
        Characters.Instance.startGameActOrder = InsertAtEndOfActOrder(Astronaut);
        Characters.Instance.startGameActOrder = InsertAtEndOfActOrder(Prankster);
        Characters.Instance.startGameActOrder = InsertAtEndOfActOrder(Sharpshooter);
        Characters.Instance.startGameActOrder = InsertAtEndOfActOrder(Guide);

        AscensionsData advancedAscension = ProjectContext.Instance.gameData.advancedAscension;
        if (testingMode != 0)
        {
            addDemonRole(advancedAscension, Summoner, "Baa_Difficult", "Summoner_1", summonerScriptData, 500);
        }
        else
        {
            if (MelonPreferences.GetCategory("RiddlesConfig").GetEntry("Follower").GetValueAsString().ToLower() == "true")
                addDemonRole(advancedAscension, Follower, "Baa_Difficult", "Follower_1", followerScriptData, 2);
            if (MelonPreferences.GetCategory("RiddlesConfig").GetEntry("Veil").GetValueAsString().ToLower() == "true")
                addDemonRole(advancedAscension, Veil, "Baa_Difficult", "Veil_1", veilScriptData, 2);
            if (MelonPreferences.GetCategory("RiddlesConfig").GetEntry("Summoner").GetValueAsString().ToLower() == "true")
                addDemonRole(advancedAscension, Summoner, "Baa_Difficult", "Summoner_1", summonerScriptData, 2);
            if (MelonPreferences.GetCategory("RiddlesConfig").GetEntry("Infestation").GetValueAsString().ToLower() == "true")
                addDemonRole(advancedAscension, Infestation, "Baa_Difficult", "Infestation_1", infestationScriptData, 2);
            if (MelonPreferences.GetCategory("RiddlesConfig").GetEntry("Escapist").GetValueAsString().ToLower() == "true")
                addDemonRole(advancedAscension, Escapist, "Baa_Difficult", "Escapist_1", escapistScriptData, 2);
            if (MelonPreferences.GetCategory("RiddlesConfig").GetEntry("Kingmaker").GetValueAsString().ToLower() == "true")
                addDemonRole(advancedAscension, Kingmaker, "Baa_Difficult", "Kingmaker_1", kingmakerScriptData, 2);
            if (MelonPreferences.GetCategory("RiddlesConfig").GetEntry("Mystifier").GetValueAsString().ToLower() == "true")
                addDemonRole(advancedAscension, Mystifier, "Baa_Difficult", "Mystifier_1", MystifierScriptData, 2);
            if (MelonPreferences.GetCategory("RiddlesConfig").GetEntry("Rainbow Joker").GetValueAsString().ToLower() == "true")
                addDemonRole(advancedAscension, RainbowJoker, "Baa_Difficult", "RainbowJoker_1", RainbowJokerScriptData, 2);
            if (MelonPreferences.GetCategory("RiddlesConfig").GetEntry("Atheist").GetValueAsString().ToLower() == "true")
                addDemonRole(advancedAscension, Atheist, "Baa_Difficult", "Atheist_1", AtheistScriptData, 2);
            if (MelonPreferences.GetCategory("RiddlesConfig").GetEntry("Fracture").GetValueAsString().ToLower() == "true")
                addDemonRole(advancedAscension, Fracture, "Baa_Difficult", "Fracture_1", FractureScriptData, 2);
        }

        foreach (CustomScriptData scriptData in advancedAscension.possibleScriptsData)
        {
            ScriptInfo script = scriptData.scriptInfo;
            AddRole(script.startingTownsfolks, Riddler);
            AddRole(script.startingTownsfolks, Swapper);
            AddRole(script.startingTownsfolks, Mathematician);
            AddRole(script.startingTownsfolks, Commander);
            AddRole(script.startingTownsfolks, Director);
            AddRole(script.startingTownsfolks, Scanner);
            AddRole(script.startingTownsfolks, Trickster);
            AddRole(script.startingTownsfolks, Obsessor);
            AddRole(script.startingTownsfolks, Lawyer);
            AddRole(script.startingTownsfolks, Psychic);
            AddRole(script.startingTownsfolks, Weaver);
            AddRole(script.startingTownsfolks, Nurse);
            AddRole(script.startingTownsfolks, Stylist);
            AddRole(script.startingTownsfolks, Coach);
            AddRole(script.startingTownsfolks, Comedian);
            AddRole(script.startingTownsfolks, Innkeeper);
            AddRole(script.startingTownsfolks, Recruiter);
            AddRole(script.startingTownsfolks, Engineer);
            AddRole(script.startingTownsfolks, Governor);
            AddRole(script.startingTownsfolks, Officer);
            AddRole(script.startingTownsfolks, Cowboy);
            AddRole(script.startingTownsfolks, Surveyor);
            AddRole(script.startingTownsfolks, Tracker);
            AddRole(script.startingTownsfolks, Pioneer);
            AddRole(script.startingTownsfolks, Necromancer);
            AddRole(script.startingTownsfolks, Astronaut);
            AddRole(script.startingTownsfolks, Motivator);
            AddRole(script.startingTownsfolks, Therapist);
            AddRole(script.startingTownsfolks, Crewmate);
            AddRole(script.startingTownsfolks, Sharpshooter);
            AddRole(script.startingTownsfolks, Guide);
            AddRole(script.startingTownsfolks, Preacher);

            AddRole(script.startingOutsiders, MadScientist);
            AddRole(script.startingOutsiders, Hitman);
            AddRole(script.startingOutsiders, Ghost);
            AddRole(script.startingOutsiders, Muddler);
            AddRole(script.startingOutsiders, Confectioner);
            AddRole(script.startingOutsiders, Captivator);
            AddRole(script.startingOutsiders, Reflector);
            AddRole(script.startingOutsiders, Gambler);
            AddRole(script.startingOutsiders, Anchor);
            AddRole(script.startingOutsiders, Prankster);

            AddRole(script.startingMinions, Wizard);
            AddRole(script.startingMinions, Accuser);
            AddRole(script.startingMinions, Hypnotist);
            AddRole(script.startingMinions, Channeler);
            AddRole(script.startingMinions, Sleeper);
            AddRole(script.startingMinions, Guardian);
            AddRole(script.startingMinions, Mastermind);
            AddRole(script.startingMinions, Baffler);
            AddRole(script.startingMinions, Slanderer);
            AddRole(script.startingMinions, Enigma);
            AddRole(script.startingMinions, Squire);
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
                if (actList[i] != null)
                {
                    newActList[i] = actList[i];
                    if (actList[i].name == previous)
                    {
                        newActList[i + 1] = data;
                        inserted = true;
                    }
                }
            }
        }
        if (!inserted)
        {
            LoggerInstance.Msg("");
        }
        return newActList;
    }
    public CharacterData[] InsertAtStartOfActOrder(CharacterData data)
    {
        CharacterData[] actList = Characters.Instance.startGameActOrder;
        int actSize = actList.Length;
        CharacterData[] newActList = new CharacterData[actSize + 1];
        for (int i = 0; i < actSize; i++)
        {
            newActList[i + 1] = actList[i];
        }
        newActList[0] = data;
        return newActList;
    }
    public CharacterData[] InsertAtEndOfActOrder(CharacterData data)
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
    
    public int shortenNight = 0;
    public static MainMod Instance;
    public static NightModeRule CachedRule;

    [HarmonyPatch(typeof(Gameplay), "OnCharacterReveal")]
    public static class CharacterRevealPatch
    {

        [HarmonyPrefix]
        public static void DoSleeperStuff(Character obj)
        {
            if (obj == null) return;
            var mod = MainMod.Instance;
            if (mod == null) return;
            if (mod.shortenNight > 0 && CachedRule != null)
            {
                CachedRule.currentStep += mod.shortenNight;
                mod.shortenNight = 0;
            }
        }
    }
    [HarmonyPatch(typeof(NightModeRule), "Init")]
    static class Patch
    {
        static void Postfix(NightModeRule __instance)
        {
            MainMod.CachedRule = __instance;
        }
    }
    // Kingmaker hides evil counter
    // Must modify after all other mods, since Atheist does things
    [HarmonyPatch(typeof(ObjectivesUI), nameof(ObjectivesUI.UpdateObjectives))]
    [HarmonyPriority(HarmonyLib.Priority.Last)]
    public static class ChangeCounter
    {
        public static void Postfix(ObjectivesUI __instance)
        {
            bool Kingmaker = false;
            bool Atheist = false;
            foreach (Character c in Gameplay.CurrentCharacters)
            {
                if (c.dataRef.characterId == "Kingmaker_scm")
                {
                    Kingmaker = true;
                }
                if (c.dataRef.characterId == "Atheist_scm")
                {
                    Atheist = true;
                }
            }
            if (!Kingmaker && !Atheist) return;
            int minions = Gameplay.CurrentScript.minion;
            int demons = Gameplay.CurrentScript.demon;
            var deadCharacters = Gameplay.DeadCharacters;
            int EvilsKilled = 0;

            foreach (var deadCharacter in deadCharacters)
            {
                if (deadCharacter.alignment == EAlignment.Evil)
                {
                    EvilsKilled++;
                }
            }
            if (Atheist)
            {
                __instance.evilsKilled.text = string.Format("<color=grey>Evils killed:</color> <color=red>?");
            } else
            {
                __instance.evilsKilled.text = string.Format("<color=grey>Evils killed:</color> <color=red>{0}", EvilsKilled);
            }


                string minionCountText = "Minions";
            if (minions == 1)
            {
                minionCountText = "Minion";
            }
            string demonCountText = "Demons";
            if (demons == 1)
            {
                demonCountText = "Demon";
            }
            __instance.objective.text = string.Format("Find and Execute all Evil Characters<br><color=grey><size=18>(<color=orange>{0}+ {2}</color> and <color=red>{1}+ {3} </color>)", minions, demons, minionCountText, demonCountText);
            if (Atheist)
            {
                __instance.objective.text = "Find and Execute all Evil Characters.";
            }
        }
    }
    public static Il2CppSystem.Collections.Generic.List<Character> GetGameplayCurrentCharacters()
    {
        Il2CppSystem.Collections.Generic.List<Character> characters = new();
        foreach (Character c in Gameplay.CurrentCharacters)
        {
            characters.Add(c);
        }
        return characters;
    }
    
    public static GameObject CreateCircle(int size)
    {
        GameObject circle = new GameObject();
        circle.name = "Circle_" + size;
        circle.transform.SetParent(Characters.Instance.gameObject.transform);
        RectTransform rt = circle.AddComponent<RectTransform>();
        CharactersPool cp = circle.AddComponent<CharactersPool>();
        GameObject gameObject = Characters.Instance.gameObject.transform.Find("Circle_6").gameObject;
        CharactersPool component = gameObject.GetComponent<CharactersPool>();
        cp.characterPrefab = component.characterPrefab;
        cp.characters = Array.Empty<Character>();
        cp.cardPlaceHolders = new CardPlaceholder[size];
        for (int i = 0; i < size; i++)
        {
            GameObject card = new GameObject();
            card.transform.SetParent(circle.transform);
            string text = "CardPlaceholder";
            if (i > 0)
            {
                text = text + " (" + i + ")";
            }
            card.name = text;
            RectTransform card_rt = card.AddComponent<RectTransform>();
            card_rt.anchoredPosition3D = new Vector3(0f, 0f, 0f);
            CardPlaceholder cardPlaceholder = card.AddComponent<CardPlaceholder>();
            int num = i * 360 / size;
            if (num <= 30)
            {
                cardPlaceholder.actedSide = EActedSide.Down;
            }
            else if (num <= 149)
            {
                cardPlaceholder.actedSide = EActedSide.Left;
            }
            else if (num <= 210)
            {
                cardPlaceholder.actedSide = EActedSide.Up;
            }
            else if (num <= 329)
            {
                cardPlaceholder.actedSide = EActedSide.Right;
            }
            else
            {
                cardPlaceholder.actedSide = EActedSide.Down;
            }
            cp.cardPlaceHolders[i] = cardPlaceholder;
        }
        circle.transform.position = new Vector3(0f, 1f, 85.9444f);
        circle.transform.localScale = new Vector3(1f, 1f, 1f);
        circle.SetActive(false);
        addToCharsPool(cp);
        return circle;
    }
    public static void addToCharsPool(CharactersPool pool)
    {
        CharactersPool[] oldpool = Characters.Instance.characterPool;
        CharactersPool[] newPool = new CharactersPool[oldpool.Length + 1];
        for (int i = 0; i < oldpool.Length; i++)
        {
            newPool[i] = oldpool[i];
        }
        newPool[oldpool.Length] = pool;
        Characters.Instance.characterPool = newPool;
    }
}