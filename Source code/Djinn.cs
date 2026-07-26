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
        foreach (Character c in Gameplay.CurrentCharacters)
        {
            if (GetInvalidCharacterIDs(demon).Contains(c.dataRef.characterId))
            {
                c.Init(babyMinion);
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
}