using System;
using System.ComponentModel.Design;
using HarmonyLib;
using Il2Cpp;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.Injection;
using Il2CppInterop.Runtime.InteropTypes;
using Il2CppSystem;
using MelonLoader;
using RiddlerMod;
using UnityEngine;
using static MelonLoader.MelonLogger;

namespace RiddlerMod;
public class Djinn
{
    public static void Jinx(string demon)
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
        CharacterData babyMinion = new();
        foreach (CharacterData d in allDatas)
        {
            if (d.characterId == "BabyMinion_scm")
            {
                babyMinion = d; break;
            }
        }
        List<string> invalid = GetInvalidCharacterIDs(demon);
        foreach (Character c in Gameplay.CurrentCharacters)
        {
            if (invalid.Contains(c.dataRef.characterId))
            {
                c.Init(babyMinion);
            }
        }
    }
    public static void JinxVillagers(string demon)
    {
        List<string> invalid = GetInvalidCharacterIDs(demon);
        foreach (Character c in Gameplay.CurrentCharacters)
        {
            if (invalid.Contains(c.dataRef.characterId))
            {
                // this only works because GetRandomUniqueVillagerBluff is no longer limited to 4 random characters
                CharacterData newRole = Characters.Instance.GetRandomUniqueVillagerBluff();
                while (invalid.Contains(newRole.characterId)) { newRole = Characters.Instance.GetRandomUniqueVillagerBluff(); }
                c.Init(newRole);
            }
        }
    }
    public static List<string> GetInvalidCharacterIDs(string demon)
    {
        List<string> invalidMinions = new List<string>();
        switch (demon)
        {
            case "Kingmaker":
                invalidMinions.Add("Snowy_POW");
                invalidMinions.Add("Sunny_POW");
                invalidMinions.Add("Stormy_POW");
                invalidMinions.Add("Foggy_POW");
                invalidMinions.Add("Baron_04539999"); // can move, which is bad
                invalidMinions.Add("Swarm_Good_WING"); // it's pretty weird for a good minion to be next to this demon
                invalidMinions.Add("Cryptid_WING");
                invalidMinions.Add("Marionette_WING");
                break;
            case "Infestation":
                invalidMinions.Add("Sunny_POW");
                break;
            case "Escapist":
                invalidMinions.Add("Snowy_POW");
                invalidMinions.Add("Sunny_POW");
                invalidMinions.Add("Stormy_POW");
                invalidMinions.Add("Foggy_POW");
                invalidMinions.Add("Baron_04539999"); // Too many outcasts.
                break;
            case "Atheist":
                invalidMinions.Add("Swarm_Good_WING"); // from a bug report
                break;
            case "Veil":
                invalidMinions.Add("Sunny_POW");
                invalidMinions.Add("Stormy_POW"); // This might make villages unsolvable
                break;
            case "Summoner":
                // The following characters are villagers that depend on Outcasts or Minions
                invalidMinions.Add("Druid_89845092");
                invalidMinions.Add("Oracle_07039445");
                invalidMinions.Add("Lamb_WING");
                invalidMinions.Add("Scanner_scm");
                invalidMinions.Add("Recruiter_scm");
                invalidMinions.Add("Tracker_scm");
                invalidMinions.Add("Surveyor_scm");
                invalidMinions.Add("Marksman_POW");
                invalidMinions.Add("Executive_POW"); // some of the things this can become don't work without minions
                invalidMinions.Add("Trapper_TST");
                invalidMinions.Add("WING_Dupery_Private Eye");
                break;
        }
        return invalidMinions;
    }
    public static List<string> GetCharactersThatCannotDie()
    {
        List<string> chars = new();
        chars.Add("Squire_scm");
        chars.Add("Undying_WING");
        chars.Add("Vizier_LRZH");
        chars.Add("WING_Dupery_Scoundrel");

        return chars;
    }

    public static List<string> GetNightlyInfoActors()
    {
        List<string> chars = new();
        chars.Add("Sharpshooter_scm");
        chars.Add("Astronaut_scm");
        chars.Add("Guide_scm");

        return chars;
    }
}