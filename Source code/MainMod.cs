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
using System.Reflection;
using static Il2Cpp.Interop;
using static Il2CppSystem.Array;
using static MelonLoader.MelonLaunchOptions;
using static UnityEngine.TouchScreenKeyboard;

[assembly: MelonInfo(typeof(MainMod), "Skill Cycler's Riddles", "0.9.6", "Skill Cycler")]
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
        ClassInjector.RegisterTypeInIl2Cpp<Trickster_o>();
        ClassInjector.RegisterTypeInIl2Cpp<Trickster_v>();
        ClassInjector.RegisterTypeInIl2Cpp<Trickster_m>();
        ClassInjector.RegisterTypeInIl2Cpp<Obsessor>();
        ClassInjector.RegisterTypeInIl2Cpp<Lawyer>();
        ClassInjector.RegisterTypeInIl2Cpp<Psychic>();
        ClassInjector.RegisterTypeInIl2Cpp<Weaver>();
        ClassInjector.RegisterTypeInIl2Cpp<Nurse>();
        ClassInjector.RegisterTypeInIl2Cpp<Stylist>();

        // Outcasts

        ClassInjector.RegisterTypeInIl2Cpp<MadScientist>();
        ClassInjector.RegisterTypeInIl2Cpp<Necromancer>();
        ClassInjector.RegisterTypeInIl2Cpp<Hitman>();
        ClassInjector.RegisterTypeInIl2Cpp<Ghost>();

        // Minions
        ClassInjector.RegisterTypeInIl2Cpp<Accuser>();
        ClassInjector.RegisterTypeInIl2Cpp<Hypnotist>();
        ClassInjector.RegisterTypeInIl2Cpp<Channeler>();
        ClassInjector.RegisterTypeInIl2Cpp<Sleeper>();
        ClassInjector.RegisterTypeInIl2Cpp<Guardian>();
        ClassInjector.RegisterTypeInIl2Cpp<Mastermind>();

        // Demons
        ClassInjector.RegisterTypeInIl2Cpp<Follower>();
        ClassInjector.RegisterTypeInIl2Cpp<Veil>();
        ClassInjector.RegisterTypeInIl2Cpp<Summoner>();
        ClassInjector.RegisterTypeInIl2Cpp<Infestation>();
        Instance = this;
    }
    public override void OnLateInitializeMelon()
    {
        GameObject content = GameObject.Find("Game/Gameplay/Content");
        NightPhase nightPhase = content.GetComponent<NightPhase>();
        GameplayEvents.OnDeckShuffled += new Action(OnRoundStart);

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
        Riddler.characterId = "Riddler_scm";
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
        Swapper.characterId = "Swapper_scm";
        Swapper.artBgColor = new Color(0.111f, 0.0833f, 0.1415f);
        Swapper.cardBgColor = new Color(0.26f, 0.1519f, 0.3396f);
        Swapper.cardBorderColor = new Color(0.7133f, 0.339f, 0.8679f);
        Swapper.color = new Color(1f, 0.935f, 0.7302f);
        

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
        Mathematician.characterId = "Mathematician_scm";
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
        Commander.characterId = "Commander_scm";
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
        Director.characterId = "Director_scm";
        Director.cardBgColor = new Color(0.26f, 0.1519f, 0.3396f);
        Director.cardBorderColor = new Color(0.7133f, 0.339f, 0.8679f);
        Director.color = new Color(1f, 0.935f, 0.7302f);

        CharacterData Scanner = new CharacterData();
        Scanner.role = new Scanner();
        Scanner.name = "Scanner";
        Scanner.description = "Learn how many Outcasts are Disguised or being used as a Disguise.";
        Scanner.flavorText = "\"I spy with my two little eyes, two Outcasts in disguise!\"";
        Scanner.hints = "The Outcast Trickster counts towards this.";
        Scanner.ifLies = "";
        Scanner.picking = false;
        Scanner.startingAlignment = EAlignment.Good;
        Scanner.type = ECharacterType.Villager;
        Scanner.bluffable = true;
        Scanner.characterId = "Scanner_scm";
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
        Obsessor.characterId = "Obsessor_scm";
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
        Lawyer.characterId = "Lawyer_scm";
        Lawyer.cardBgColor = new Color(0.26f, 0.1519f, 0.3396f);
        Lawyer.cardBorderColor = new Color(0.7133f, 0.339f, 0.8679f);
        Lawyer.color = new Color(1f, 0.935f, 0.7302f);

        CharacterData Psychic = new CharacterData();
        Psychic.role = new Psychic();
        Psychic.name = "Psychic";
        Psychic.description = "Learn 2 characters. Exactly 1 is in play.";
        Psychic.flavorText = "\"I may be able to read your mind.\"";
        Psychic.hints = "I can see through misregistration.";
        Psychic.ifLies = "Neither or both are in play.";
        Psychic.picking = false;
        Psychic.startingAlignment = EAlignment.Good;
        Psychic.type = ECharacterType.Villager;
        Psychic.bluffable = true;
        Psychic.characterId = "Psychic_scm";
        Psychic.cardBgColor = new Color(0.26f, 0.1519f, 0.3396f);
        Psychic.cardBorderColor = new Color(0.7133f, 0.339f, 0.8679f);
        Psychic.color = new Color(1f, 0.935f, 0.7302f);

        CharacterData Weaver = new CharacterData();
        Weaver.role = new Weaver();
        Weaver.name = "Weaver";
        Weaver.description = "Learn how many pairs of Villagers there are.";
        Weaver.flavorText = "\"The Knitter's younger sister. Still recovering from that incident with the Evil Villagers.\"";
        Weaver.hints = "";
        Weaver.ifLies = "";
        Weaver.picking = false;
        Weaver.startingAlignment = EAlignment.Good;
        Weaver.type = ECharacterType.Villager;
        Weaver.bluffable = true;
        Weaver.characterId = "Weaver_scm";
        Weaver.cardBgColor = new Color(0.26f, 0.1519f, 0.3396f);
        Weaver.cardBorderColor = new Color(0.7133f, 0.339f, 0.8679f);
        Weaver.color = new Color(1f, 0.935f, 0.7302f);

        CharacterData Nurse = new CharacterData();
        Nurse.role = new Nurse();
        Nurse.name = "Nurse";
        Nurse.description = "Pick 1 alive card: If Corrupted, cure and refresh their ability.";
        Nurse.flavorText = "\"I can cure the Drunk, I promise!\"";
        Nurse.hints = "My ability refreshes every night.";
        Nurse.ifLies = "\"I couldn't cure #x\"";
        Nurse.picking = true;
        Nurse.abilityUsage = EAbilityUsage.ResetAfterNight;
        Nurse.startingAlignment = EAlignment.Good;
        Nurse.type = ECharacterType.Villager;
        Nurse.bluffable = true;
        Nurse.characterId = "Nurse_scm";
        Nurse.cardBgColor = new Color(0.26f, 0.1519f, 0.3396f);
        Nurse.cardBorderColor = new Color(0.7133f, 0.339f, 0.8679f);
        Nurse.color = new Color(1f, 0.935f, 0.7302f);

        CharacterData Stylist = new CharacterData();
        Stylist.role = new Stylist();
        Stylist.name = "Stylist";
        Stylist.description = "Pick an alive Disguised character. Change their Disguise.";
        Stylist.flavorText = "\"Taking clients from the Swapper since 2025\"";
        Stylist.hints = "";
        Stylist.ifLies = "\"I couldn't change #x's Disguise\"";
        Stylist.picking = true;
        Stylist.startingAlignment = EAlignment.Good;
        Stylist.type = ECharacterType.Villager;
        Stylist.bluffable = true;
        Stylist.characterId = "Stylist_scm";
        Stylist.cardBgColor = new Color(0.26f, 0.1519f, 0.3396f);
        Stylist.cardBorderColor = new Color(0.7133f, 0.339f, 0.8679f);
        Stylist.color = new Color(1f, 0.935f, 0.7302f);

        CharacterData Trickster_v = new CharacterData();
        Trickster_v.role = new Trickster_v();
        Trickster_v.name = "Trickster";
        Trickster_v.description = "Game Start: There are three of us. One is a Villager, one is an Outcast, and one is a Good Minion.\nYou don't know which is which.\nLearn a card that is the same character type as me.";
        Trickster_v.flavorText = "\"If you thought the Minion twins were bad, get ready for the three of us!\"";
        Trickster_v.hints = "If I am Corrupted or abnormally Disguised as: \"I feel sick\"";
        Trickster_v.ifLies = "";
        Trickster_v.picking = false;
        Trickster_v.startingAlignment = EAlignment.Good;
        Trickster_v.type = ECharacterType.Villager;
        Trickster_v.bluffable = false;
        Trickster_v.characterId = "Trickster_v_scm";
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
        Trickster_o.characterId = "Trickster_o_scm";
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
        Trickster_m.characterId = "Trickster_m_scm";
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
        MadScientist.characterId = "MadScientist_scm";
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
        Necromancer.characterId = "Necromancer_scm";
        Necromancer.cardBgColor = new Color(0.102f, 0.0667f, 0.0392f);
        Necromancer.cardBorderColor = new Color(0.7843f, 0.6471f, 0f);
        Necromancer.color = new Color(0.9659f, 1f, 0.4472f);

        CharacterData Hitman = new CharacterData();
        Hitman.role = new Hitman();
        Hitman.name = "Hitman";
        Hitman.description = "I Lie and Disguise.\n\nAt night: Kill a random card and lose 3 HP.";
        Hitman.flavorText = "\"No one is safe from me, not even myself\"";
        Hitman.hints = "I can kill any card, including Knights, Demons, and myself.\nIf there is no night cycle, I'm just a regular Evil Outcast.";
        Hitman.ifLies = "";
        Hitman.picking = false;
        Hitman.startingAlignment = EAlignment.Evil;
        Hitman.type = ECharacterType.Outcast;
        Hitman.bluffable = false;
        Hitman.characterId = "Hitman_scm";
        Hitman.cardBgColor = new Color(0.102f, 0.0667f, 0.0392f);
        Hitman.cardBorderColor = new Color(0.7843f, 0.6471f, 0f);
        Hitman.color = new Color(0.9659f, 1f, 0.4472f);

        CharacterData Ghost = new CharacterData();
        Ghost.role = new Ghost();
        Ghost.name = "Ghost";
        Ghost.description = "On Reveal: Die, dealing 1 damage to you. One unrevealed Good character is Corrupted.";
        Ghost.flavorText = "\"I would say 'Boo!' but that's not scary anymore.\"";
        Ghost.hints = "I cannot be revived.";
        Ghost.ifLies = "";
        Ghost.picking = false;
        Ghost.startingAlignment = EAlignment.Good;
        Ghost.type = ECharacterType.Outcast;
        Ghost.bluffable = false;
        Ghost.characterId = "Ghost_scm";
        Ghost.cardBgColor = new Color(0.102f, 0.0667f, 0.0392f);
        Ghost.cardBorderColor = new Color(0.7843f, 0.6471f, 0f);
        Ghost.color = new Color(0.9659f, 1f, 0.4472f);

        CharacterData Accuser = new CharacterData();
        Accuser.role = new Accuser();
        Accuser.name = "Accuser";
        Accuser.description = "Game Start: One adjacent Good Villager registers a random Evil Minion.\n\nI Lie and Disguise.";
        Accuser.flavorText = "\"Uno reverse card!\"";
        Accuser.hints = "";
        Accuser.ifLies = "";
        Accuser.picking = false;
        Accuser.startingAlignment = EAlignment.Evil;
        Accuser.type = ECharacterType.Minion;
        Accuser.abilityUsage = EAbilityUsage.Once;
        Accuser.bluffable = false;
        Accuser.characterId = "Accuser_scm";
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
        Hypnotist.characterId = "Hypnotist_scm";
        Hypnotist.cardBgColor = new Color(0.0941f, 0.0431f, 0.0431f);
        Hypnotist.cardBorderColor = new Color(0.8208f, 0f, 0.0241f);
        Hypnotist.color = new Color(0.8491f, 0.4555f, 0f);

        CharacterData Channeler = new CharacterData();
        Channeler.role = new Channeler();
        Channeler.name = "Channeler";
        Channeler.description = "I copy the ability of another Evil.";
        Channeler.flavorText = "\"I will follow in your footsteps.\"";
        Channeler.hints = "";
        Channeler.ifLies = "";
        Channeler.picking = false;
        Channeler.startingAlignment = EAlignment.Evil;
        Channeler.type = ECharacterType.Minion;
        Channeler.abilityUsage = EAbilityUsage.Once;
        Channeler.bluffable = false;
        Channeler.characterId = "Channeler_scm";
        Channeler.cardBgColor = new Color(0.0941f, 0.0431f, 0.0431f);
        Channeler.cardBorderColor = new Color(0.8208f, 0f, 0.0241f);
        Channeler.color = new Color(0.8491f, 0.4555f, 0f);

        CharacterData Sleeper = new CharacterData();
        Sleeper.role = new Sleeper();
        Sleeper.name = "Sleeper";
        Sleeper.description = "The night cycle is 1 tick shorter if there is one.";
        Sleeper.flavorText = "\"Ever feel like you get enough sleep? Well too bad. You're not getting it anymore.\"";
        Sleeper.hints = "";
        Sleeper.ifLies = "";
        Sleeper.picking = false;
        Sleeper.startingAlignment = EAlignment.Evil;
        Sleeper.type = ECharacterType.Minion;
        Sleeper.bluffable = false;
        Sleeper.characterId = "Sleeper_scm";
        Sleeper.cardBgColor = new Color(0.0941f, 0.0431f, 0.0431f);
        Sleeper.cardBorderColor = new Color(0.8208f, 0f, 0.0241f);
        Sleeper.color = new Color(0.8491f, 0.4555f, 0f);

        CharacterData Guardian = new CharacterData();
        Guardian.role = new Guardian();
        Guardian.name = "Guardian";
        Guardian.description = "The Demon registers as a Good Villager.\n\nI sit next to the Demon.";
        Guardian.flavorText = "\"You're gonna have to get through me first.\"";
        Guardian.hints = "If there are multiple Demons, all of them register as Good.";
        Guardian.ifLies = "";
        Guardian.picking = false;
        Guardian.startingAlignment = EAlignment.Evil;
        Guardian.type = ECharacterType.Minion;
        Guardian.bluffable = false;
        Guardian.characterId = "Guardian_scm";
        Guardian.cardBgColor = new Color(0.0941f, 0.0431f, 0.0431f);
        Guardian.cardBorderColor = new Color(0.8208f, 0f, 0.0241f);
        Guardian.color = new Color(0.8491f, 0.4555f, 0f);

        CharacterData Mastermind = new CharacterData();
        Mastermind.role = new Mastermind();
        Mastermind.name = "Mastermind";
        Mastermind.description = "Game Start: Every Evil Minion becomes a Mastermind after all other Game Start effects.";
        Mastermind.flavorText = "\"It all comes back to me.\"";
        Mastermind.hints = "";
        Mastermind.ifLies = "";
        Mastermind.picking = false;
        Mastermind.startingAlignment = EAlignment.Evil;
        Mastermind.type = ECharacterType.Minion;
        Mastermind.bluffable = false;
        Mastermind.characterId = "Mastermind_scm";
        Mastermind.cardBgColor = new Color(0.0941f, 0.0431f, 0.0431f);
        Mastermind.cardBorderColor = new Color(0.8208f, 0f, 0.0241f);
        Mastermind.color = new Color(0.8491f, 0.4555f, 0f);

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
        Follower.characterId = "Follower_scm";
        Follower.artBgColor = new Color(0.111f, 0.0833f, 0.1415f);
        Follower.cardBgColor = new Color(0.0941f, 0.0431f, 0.0431f);
        Follower.cardBorderColor = new Color(0.8196f, 0.0f, 0.0275f);
        Follower.color = new Color(1f, 0.3804f, 0.3804f);


        CharacterData Veil = new CharacterData();
        Veil.role = new Veil();
        Veil.name = "Veil";
        Veil.description = "2-3 cards cannot be revealed. Villages are much bigger to compensate.\n\nI Lie and Disguise.";
        Veil.flavorText = "\"I cannot see anyone's role through this dense fog!\"";
        Veil.hints = "If someone else copies my effect, only 1 card is hidden.";
        Veil.ifLies = "";
        Veil.picking = false;
        Veil.startingAlignment = EAlignment.Evil;
        Veil.type = ECharacterType.Demon;
        Veil.bluffable = false;
        Veil.characterId = "Veil_scm";
        Veil.artBgColor = new Color(0.111f, 0.0833f, 0.1415f);
        Veil.cardBgColor = new Color(0.0941f, 0.0431f, 0.0431f);
        Veil.cardBorderColor = new Color(0.8196f, 0.0f, 0.0275f);
        Veil.color = new Color(1f, 0.3804f, 0.3804f);

        CharacterData Summoner = new CharacterData();
        Summoner.role = new Summoner();
        Summoner.name = "Summoner";
        Summoner.description = "Game Start: There are no Minions in play. 1-3 other cards become Demons. The demons I summon are not added to the Deck.\n\nI Lie and Disguise.\n\nYou might start with 5 extra HP.";
        Summoner.flavorText = "\"Let's see... What does this spell do? Summon a demon? That sounds useful.\"";
        Summoner.hints = "The night cycle is always active if I am in play.";
        Summoner.ifLies = "";
        Summoner.picking = false;
        Summoner.startingAlignment = EAlignment.Evil;
        Summoner.type = ECharacterType.Demon;
        Summoner.bluffable = false;
        Summoner.characterId = "Summoner_scm";
        Summoner.artBgColor = new Color(0.111f, 0.0833f, 0.1415f);
        Summoner.cardBgColor = new Color(0.0941f, 0.0431f, 0.0431f);
        Summoner.cardBorderColor = new Color(0.8196f, 0.0f, 0.0275f);
        Summoner.color = new Color(1f, 0.3804f, 0.3804f);

        CharacterData Infestation = new CharacterData();
        Infestation.role = new Infestation();
        Infestation.name = "Infestation";
        Infestation.description = "Game Start: 1 random character is Corrupted.\n\nAt Night: Kill all Good Corrupted characters, dealing 1 damage each. Good Characters adjacent to alive Corrupted characters are Corrupted.\n\nI Lie and Disguise.";
        Infestation.flavorText = "\"The one zombie apocalypse you'll stand no chance in\"";
        Infestation.hints = "Certain characters that remove Corruptions will stop my ability from working.";
        Infestation.ifLies = "";
        Infestation.picking = false;
        Infestation.startingAlignment = EAlignment.Evil;
        Infestation.type = ECharacterType.Demon;
        Infestation.bluffable = false;
        Infestation.characterId = "Infestation_scm";
        Infestation.artBgColor = new Color(0.111f, 0.0833f, 0.1415f);
        Infestation.cardBgColor = new Color(0.0941f, 0.0431f, 0.0431f);
        Infestation.cardBorderColor = new Color(0.8196f, 0.0f, 0.0275f);
        Infestation.color = new Color(1f, 0.3804f, 0.3804f);

        nightPhase.nightCharactersOrder.Add(Infestation);
        nightPhase.nightCharactersOrder.Add(Follower);
        nightPhase.nightCharactersOrder.Add(Channeler);
        nightPhase.nightCharactersOrder.Add(Hitman);
        nightPhase.nightCharactersOrder.Add(MadScientist); // for if it copies an outcast that acts at night
        nightPhase.nightCharactersOrder.Add(Sleeper);


        // Characters.Instance.startGameActOrder = InsertAfterAct("Baa", Sleeper);
        Characters.Instance.startGameActOrder = InsertAtStartOfActOrder(Summoner);
        Characters.Instance.startGameActOrder = InsertAfterAct("Pooka", MadScientist);
        Characters.Instance.startGameActOrder = InsertAfterAct("Witch", Veil);
        Characters.Instance.startGameActOrder = InsertAfterAct("Puppeteer", Infestation);
        Characters.Instance.startGameActOrder = InsertAfterAct("Puppeteer", Channeler);
        Characters.Instance.startGameActOrder = InsertAfterAct("Shaman", Trickster_v);
        Characters.Instance.startGameActOrder = InsertAfterAct("Alchemist", Accuser);
        Characters.Instance.startGameActOrder = InsertAfterAct("Alchemist", Guardian);
        Characters.Instance.startGameActOrder = InsertAfterAct("Accuser", Hypnotist);
        Characters.Instance.startGameActOrder = InsertAfterAct("Hypnotist", Follower);
        Characters.Instance.startGameActOrder = InsertAtEndOfActOrder(Sleeper);
        Characters.Instance.startGameActOrder = InsertAtEndOfActOrder(Lawyer);
        Characters.Instance.startGameActOrder = InsertAtEndOfActOrder(Mastermind);


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
        /*followerCounterList.Add(follower_12a);
        followerCounterList.Add(follower_12b);
        followerCounterList.Add(follower_12c);
        followerCounterList.Add(follower_12d);
        followerCounterList.Add(follower_13a);
        followerCounterList.Add(follower_13b);*/
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


        /*veilCounterList.Add(veil_13a);
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
        veilCounterList.Add(veil_15b);*/
        veilCounterList.Add(setCharacterCount(8, 0, 2, 1)); // temp fix until 12+ cards work again
        veilCounterList.Add(setCharacterCount(7, 1, 2, 1)); // temp fix until 12+ cards work again

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
        CharactersCount summoner_7a = setCharacterCount(6, 0, 0, 1);
        CharactersCount summoner_7b = setCharacterCount(5, 1, 0, 1);
        CharactersCount summoner_8a = setCharacterCount(7, 0, 0, 1);
        CharactersCount summoner_8b = setCharacterCount(6, 1, 0, 1);
        // 9-10 cards: 1-2 summons
        CharactersCount summoner_9a = setCharacterCount(8, 0, 0, 1);
        CharactersCount summoner_9b = setCharacterCount(7, 1, 0, 1);
        CharactersCount summoner_10a = setCharacterCount(8, 1, 0, 1);
        CharactersCount summoner_10b = setCharacterCount(7, 2, 0, 1);
        // 11-12 cards: 2 summons
        CharactersCount summoner_11a = setCharacterCount(9, 1, 0, 1);
        CharactersCount summoner_11b = setCharacterCount(8, 2, 0, 1);
        CharactersCount summoner_12a = setCharacterCount(10, 1, 0, 1);
        CharactersCount summoner_12b = setCharacterCount(9, 2, 0, 1);
        // 13+ cards: 2-3 summons
        CharactersCount summoner_13a = setCharacterCount(11, 1, 0, 1);
        CharactersCount summoner_13b = setCharacterCount(10, 2, 0, 1);
        CharactersCount summoner_14a = setCharacterCount(12, 1, 0, 1);
        CharactersCount summoner_14b = setCharacterCount(11, 2, 0, 1);
        CharactersCount summoner_15a = setCharacterCount(13, 1, 0, 1);
        CharactersCount summoner_15b = setCharacterCount(12, 2, 0, 1);
        Il2CppSystem.Collections.Generic.List<CharactersCount> summonerCounterList = new Il2CppSystem.Collections.Generic.List<CharactersCount>();


        summonerCounterList.Add(summoner_7a);
        summonerCounterList.Add(summoner_7b);
        summonerCounterList.Add(summoner_8a);
        summonerCounterList.Add(summoner_8b);
        summonerCounterList.Add(summoner_9a);
        summonerCounterList.Add(summoner_9b);
        summonerCounterList.Add(summoner_10a);
        summonerCounterList.Add(summoner_10b);
        summonerCounterList.Add(summoner_11a);
        summonerCounterList.Add(summoner_11b);
        /*summonerCounterList.Add(summoner_12a);
        summonerCounterList.Add(summoner_12b);
        summonerCounterList.Add(summoner_13a);
        summonerCounterList.Add(summoner_13b);
        summonerCounterList.Add(summoner_14a);
        summonerCounterList.Add(summoner_14b);
        summonerCounterList.Add(summoner_15a);
        summonerCounterList.Add(summoner_15b);*/
        summonerScript.characterCounts = summonerCounterList;
        summonerScriptData.scriptInfo = summonerScript;

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
        CharactersCount infestation_8 = setCharacterCount(5, 1, 1, 1);
        CharactersCount infestation_9 = setCharacterCount(5, 2, 1, 1);
        CharactersCount infestation_10 = setCharacterCount(7, 0, 2, 1);
        CharactersCount infestation_11 = setCharacterCount(7, 1, 2, 1);
        CharactersCount infestation_12 = setCharacterCount(7, 2, 2, 1);
        CharactersCount infestation_13 = setCharacterCount(9, 0, 3, 1);
        CharactersCount infestation_14 = setCharacterCount(9, 1, 3, 1);
        CharactersCount infestation_15 = setCharacterCount(9, 2, 3, 1);
        Il2CppSystem.Collections.Generic.List<CharactersCount> infestationCounterList = new Il2CppSystem.Collections.Generic.List<CharactersCount>();

        
        infestationCounterList.Add(infestation_8);
        infestationCounterList.Add(infestation_9);
        infestationCounterList.Add(infestation_10);
        infestationCounterList.Add(infestation_11);
        /*infestationCounterList.Add(infestation_12);
        infestationCounterList.Add(infestation_13);
        infestationCounterList.Add(infestation_14);
        infestationCounterList.Add(infestation_15);*/
        //infestationCounterList.Add(setCharacterCount(2, 7, 0, 1)); // use this to test outcasts
        //infestationCounterList.Add(setCharacterCount(2, 0, 7, 1)); // use this to test minions

        infestationScript.characterCounts = infestationCounterList;
        infestationScriptData.scriptInfo = infestationScript;

        AscensionsData advancedAscension = ProjectContext.Instance.gameData.advancedAscension;
        addDemonRole(advancedAscension, Follower, "Baa_Difficult", "Follower_1", followerScriptData, 2);
        addDemonRole(advancedAscension, Veil, "Baa_Difficult", "Veil_1", veilScriptData, 2);
        addDemonRole(advancedAscension, Summoner, "Baa_Difficult", "Summoner_1", summonerScriptData, 2);
        addDemonRole(advancedAscension, Infestation, "Baa_Difficult", "Infestation_1", infestationScriptData, 2);

        foreach (CustomScriptData scriptData in advancedAscension.possibleScriptsData)
        {
            ScriptInfo script = scriptData.scriptInfo;
            AddRole(script.startingTownsfolks, Riddler);
            AddRole(script.startingTownsfolks, Swapper);
            AddRole(script.startingTownsfolks, Mathematician);
            AddRole(script.startingTownsfolks, Commander);
            AddRole(script.startingTownsfolks, Director);
            AddRole(script.startingTownsfolks, Scanner);
            //AddRole(script.startingTownsfolks, Trickster_v); They're taking a break from their trickery.
            AddRole(script.startingTownsfolks, Obsessor);
            AddRole(script.startingTownsfolks, Lawyer);
            AddRole(script.startingTownsfolks, Psychic);
            AddRole(script.startingTownsfolks, Weaver);
            AddRole(script.startingTownsfolks, Nurse);
            AddRole(script.startingTownsfolks, Stylist);


            AddRole(script.startingOutsiders, MadScientist);
            AddRole(script.startingOutsiders, Necromancer);
            AddRole(script.startingOutsiders, Hitman);
            AddRole(script.startingOutsiders, Ghost);


            AddRole(script.startingMinions, Accuser);
            AddRole(script.startingMinions, Hypnotist);
            AddRole(script.startingMinions, Channeler);
            AddRole(script.startingMinions, Sleeper);
            AddRole(script.startingMinions, Guardian);
            AddRole(script.startingMinions, Mastermind);
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
    /*
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
    }
    public static class Statics
    {
        public static Dictionary<string, CharacterData> roles = new Dictionary<string, CharacterData>();
        public static CharacterData[] charactersArray = Il2CppSystem.Array.Empty<CharacterData>();
        static GameObject circChar = GameObject.Find("Game/Gameplay/Content/Canvas/Characters/Circle_6/CardPlaceholder");
        static GameObject circCharLeft = GameObject.Find("Game/Gameplay/Content/Canvas/Characters/Circle_6/CardPlaceholder (1)");
        static GameObject circCharRight = GameObject.Find("Game/Gameplay/Content/Canvas/Characters/Circle_6/CardPlaceholder (4)");
        static GameObject circCharDown = GameObject.Find("Game/Gameplay/Content/Canvas/Characters/Circle_6/CardPlaceholder (3)");
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


                circlePool.characters[i] = new Character();
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
    */
    public bool shortenNight = false;
    public static MainMod Instance;
    private void OnRoundStart()
    {
        shortenNight = false;
        foreach (Character c in Gameplay.CurrentCharacters)
        {
            if (c.dataRef.characterId == "Sleeper_scm")
                shortenNight = true;
        }

    }
    public static NightModeRule CachedRule;

    [HarmonyPatch(typeof(Gameplay), "OnCharacterReveal")]
    public static class CharacterRevealPatch
    {

        [HarmonyPostfix]
        public static void DoSleeperStuff(Character obj)
        {
            
            if (obj == null) return;
            var mod = MainMod.Instance;
            if (mod == null) return;
            if (mod.shortenNight)
            {
                CachedRule.currentStep++;
                mod.shortenNight = false;
            }
            if (obj.dataRef.characterId == "Ghost_scm")
            {
                CachedRule.currentStep++;
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

}