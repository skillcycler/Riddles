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

namespace RiddlerMod;

[RegisterTypeInIl2Cpp]
public class Riddler : Role
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
        List<string> infos = new List<string>();
        infos.Add("It is impossible for an Alchemist to claim to have cured 4 Corruptions.");
        //infos.Add("The Wretch must register as the same minion to different characters.");  This statement has conflicting info whether it is true or not
        infos.Add("A Gemcrafter or Medium referencing herself must be Evil.");
        infos.Add("A bard claiming to be 5 cards away from a Corrupted character is always truthful.");
        infos.Add("A Druid that claims to have seen a Wretch is lying.");
        infos.Add("Poisoner acts before Puppeteer in the Game Start order.");
        infos.Add("Lilis cannot kill herself in the night.");
        infos.Add("The Bishop will only name 2 cards if Wretch is the only Outcast.");
        infos.Add("A card cannot be both Evil and Corrupted.");
        infos.Add("It is possible for there to be 3 Original Bakers.");
        infos.Add("A Witness claiming that no character was affected by an Evil is always truthful.");
        infos.Add("An Alchemist cannot lie about having cured 3 Corruptions.");
        infos.Add("A game with 2 base Minions can only have 1 base Outcast.");
        infos.Add("Only a lying Slayer will use up his ability when attacking a dead character.");
        infos.Add("A Scout claiming that the Puppet is 2 cards away from an Evil is always lying.");
        infos.Add("A Scout claiming that the Puppet is 3 cards away from an Evil is always lying.");
        infos.Add("A Scout claiming an Evil is 4 cards away from another Evil is always truthful.");
        infos.Add("A Knitter claiming that there are 3 pairs of Evils is always truthful.");
        infos.Add("A Hunter claiming to be 5 cards away from his closest Evil is always lying.");
        infos.Add("A truthful Hunter can claim to be 4 cards away from his closest Evil.");
        infos.Add("A Puppet disguised as a Confessor will say they are dizzy.");
        infos.Add("A Puppet disguised as Alchemist or Baker will not have their ability work.");    
        infos.Add("The Judge, Jester, Fortune Teller, and Dreamer can be used every night.");
        infos.Add("The Witness cannot detect the Witch's ability.");
        infos.Add("A Judge targeting a dizzy Confessor will say she is telling the truth.");
        infos.Add("A lying Druid targeting an Outcast will say there are no Outcasts.");
        infos.Add("Lilis cannot kill an uncorrupted Knight.");
        infos.Add("A village with Pooka and 2 minions must have Minion or Twin Minion in it.");
        infos.Add("Only truthful Mediums can call out a Drunk or Doppelganger.");
        infos.Add("A self-referencing Empress must be a Puppet.");
        infos.Add("Outcasts are immune to external sources of Corruption.");
        infos.Add("Minions and Demons are immune to Corruption.");
        infos.Add("The only time a Dreamer can call out a Puppet is if she targets the Puppet.");
        infos.Add("A Druid calling out a Bombardier or Plague Doctor is always truthful.");
        infos.Add("A Doppelganger disguised as Alchemist will act after all real Alchemists.");
        infos.Add("There can be at most 9 characters in a village with Baa.");
        infos.Add("A village with 9 cards can have anywhere from 2 to 4 evils.");
        infos.Add("A 10 card village can have at most 9 Baker claims.");
        infos.Add("Lilis will prioritize killing Good characters over Minions.");
        infos.Add("The Poet is the only character that can directly call someone Evil.");
        infos.Add("The night cycle continues after Lilis is killed.");
        infos.Add("The Puppeteer acts before the Plague Doctor in the Game Start order.");
        infos.Add("A truthful Bishop can reference herself.");
        infos.Add("A Knitter will say there are 3 pairs of evils if 4 evils are in a row.");
        infos.Add("The Bombardier's loss condition does not directly interact with your HP.");

        // easter eggs
        infos.Add("A lying Dreamer or Oracle can mention a Marionette if there are no Minions in play.");
        infos.Add("It is possible for the decklist to contain 5 outcasts in Standard mode.");
        infos.Add("13 special character skins were introduced in 2025.");
        infos.Add("The top and bottom cards count towards both sides for the Architect.");
        string info = infos[UnityEngine.Random.RandomRangeInt(0, infos.Count)];
        ActedInfo actedInfo = new ActedInfo(info);
        return actedInfo;
    }

    public override ActedInfo GetBluffInfo(Character charRef)
    {
        List<string> infos = new List<string>();
        infos.Add("It is possible for an Alchemist to claim to have cured 4 Corruptions.");
        //infos.Add("The Wretch can register as different minions to different characters."); This statement has conflicting info whether it is true or not
        infos.Add("A Gemcrafter or Medium referencing herself can be Good.");
        infos.Add("A bard claiming to be 5 cards away from a Corrupted character is always lying.");
        infos.Add("A bard claiming to be 4 cards away from a Corrupted character is always lying.");
        infos.Add("A Druid that claims to have seen a Wretch is telling the truth.");
        infos.Add("Puppeteer acts before Poisoner in the Game Start order.");
        infos.Add("Lilis can kill herself in the night.");
        infos.Add("A truthful Bishop will name 3 cards if Wretch is the only Outcast.");
        infos.Add("A card can be both Evil and Corrupted.");
        infos.Add("There can only ever be 1 original Baker.");
        infos.Add("A lying Witness can claim that no character was affected by an Evil.");
        infos.Add("An Alchemist can lie about having cured 3 Corruptions.");
        infos.Add("A game with 2 base Minions can have 2 base Outcasts.");
        infos.Add("A truthful Slayer will use up his ability when attacking a dead character.");
        infos.Add("A truthful Scout can claim that the Puppet is 2 cards away from an Evil.");
        infos.Add("A Scout can lie about an Evil being 4 cards away from another Evil.");
        infos.Add("A lying Knitter can claim that there are 3 pairs of Evils.");
        infos.Add("A lying Knitter can claim that there are 4 pairs of Evils.");
        infos.Add("A truthful Hunter can claim to be 5 cards away from his closest Evil.");
        infos.Add("A Puppet disguised as a Confessor will say they are Good.");
        infos.Add("A Puppet disguised as Alchemist or Baker will have their ability work.");
        infos.Add("The Slayer, Druid, and Plague Doctor can be used every night.");
        infos.Add("The Witness can see which card the Witch cursed.");
        infos.Add("A Judge targeting a dizzy Confessor will say she is Lying.");
        infos.Add("A lying Druid targeting an Outcast can say there is a different Outcast.");
        infos.Add("An uncorrupted Knight can be killed by Lilis.");
        infos.Add("A village can contain Pooka, Poisoner, and Witch.");
        infos.Add("A lying Medium can call out a Drunk or Doppelganger.");
        infos.Add("A Good Empress can reference herself.");
        infos.Add("Outcasts can be Corrupted by the Poisoner or Pooka.");
        infos.Add("Minions can be Corrupted by the Poisoner or Pooka.");
        infos.Add("A Dreamer might call out a Puppet when targeting a non-Puppet card.");
        infos.Add("A Druid can lie about seeing a Plague Doctor or Bombardier.");
        infos.Add("A Doppelganger disguised as Alchemist will act before all real Alchemists.");
        infos.Add("A village with Baa can have 10 cards.");
        infos.Add("A village with 9 cards must have 2 minions.");
        infos.Add("A 10 card village can have at most 8 Baker claims.");
        infos.Add("A 10 card village can have at most 7 Baker claims.");
        infos.Add("The Demon can disguise as an in play villager.");
        infos.Add("The Demon can disguise as an Outcast.");
        infos.Add("The Drunk can disguise as an in play villager.");
        infos.Add("There is a character with 3 different skins.");
        infos.Add("The night cycle stops after Lilis is killed.");
        infos.Add("The Plague Doctor acts before the Puppeteer in the Game Start order.");
        infos.Add("A truthful Bishop must reference 3 other cards.");
        infos.Add("The Gemcrafter is unaffected by the Wretch's misregistration.");
        infos.Add("A Knitter will say there are 2 pairs of evils if 4 evils are in a row.");
        infos.Add("2 truthful Lovers next to each other can both claim to be adjacent to 2 Evils.");
        infos.Add("The Minion and Twin Minion might register as each other's characters.");

        string info = infos[UnityEngine.Random.RandomRangeInt(0, infos.Count)];
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
    public Riddler() : base(ClassInjector.DerivedConstructorPointer<Riddler>())
    {
        ClassInjector.DerivedConstructorBody((Il2CppObjectBase)this);

    }

    public Riddler(System.IntPtr ptr) : base(ptr)
    {

    }
}