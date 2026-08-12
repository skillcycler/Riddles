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

namespace RiddlerMod;
public class PatchNights
{
    public static bool Patch()
    {
        // This code is my attempt at patching Wingidon's demons to have a night cycle. I've never tried something like this before so hopefully it isn't broken.
        Assembly wingsAssembly = AppDomain.CurrentDomain.GetAssemblies().FirstOrDefault(a => a.GetName().Name == "WingsExpansion");

        if (wingsAssembly != null)
        {
            foreach (Type t in wingsAssembly.GetTypes())
                MelonLogger.Msg(t.FullName);
            List<string> demons = new();
            demons.Add("w_Iris");
            HashSet<MethodInfo> patched = new();
            foreach (string demon in demons)
            {
                Type targetType = wingsAssembly.GetType($"WingidonExpansionPack.{demon}");

                if (targetType == null)
                {
                    MelonLogger.Msg($"Could not find {demon}");
                    continue;
                }
                MethodInfo targetMethod = targetType.GetMethod("GetRules", BindingFlags.Instance | BindingFlags.Public);

                if (targetMethod != null)
                {
                    targetMethod = targetMethod.GetBaseDefinition();
                }
                if (targetMethod == null)
                {
                    MelonLogger.Msg($"Could not find {demon}.GetRules");
                    continue;
                }
                if (!patched.Add(targetMethod))
                {
                    MelonLogger.Msg($"Already patched {targetMethod.DeclaringType.Name}.GetRules");
                    continue;
                }
                MelonLogger.Msg($"Target type: {targetType.FullName}");
                MelonLogger.Msg($"Method: {targetMethod}");
                MelonLogger.Msg($"Declared in: {targetMethod.DeclaringType.FullName}");
                MainMod.Instance.HarmonyInstance.Patch(
                    targetMethod,
                    postfix: new HarmonyMethod(
                        typeof(MyCompatPatch),
                        nameof(MyCompatPatch.Postfix))
                );
                MelonLogger.Msg($"Patched night cycle for demons in Wingidon's Expansion Pack");
            }
        }


        Assembly powerplay = AppDomain.CurrentDomain.GetAssemblies().FirstOrDefault(a => a.GetName().Name == "Demon Bluff Mods" || a.GetName().Name == "Demon_Bluff_Mods");

        if (powerplay != null)
        {
            foreach (Type t in powerplay.GetTypes())
                MelonLogger.Msg(t.FullName);
            List<string> demons2 = new();
            demons2.Add("Vortox");
            HashSet<MethodInfo> patched2 = new();
            foreach (string demon in demons2)
            {
                Type targetType = powerplay.GetType($"Demon_Bluff_Mods.{demon}");

                if (targetType == null)
                {
                    MelonLogger.Msg($"Could not find {demon}");
                    continue;
                }
                MethodInfo targetMethod = targetType.GetMethod("GetRules", BindingFlags.Instance | BindingFlags.Public);

                if (targetMethod != null)
                {
                    targetMethod = targetMethod.GetBaseDefinition();
                }
                if (targetMethod == null)
                {
                    MelonLogger.Msg($"Could not find {demon}.GetRules");
                    continue;
                }
                if (!patched2.Add(targetMethod))
                {
                    MelonLogger.Msg($"Already patched {targetMethod.DeclaringType.Name}.GetRules");
                    continue;
                }
                MelonLogger.Msg($"Target type: {targetType.FullName}");
                MelonLogger.Msg($"Method: {targetMethod}");
                MelonLogger.Msg($"Declared in: {targetMethod.DeclaringType.FullName}");
                MainMod.Instance.HarmonyInstance.Patch(
                    targetMethod,
                    postfix: new HarmonyMethod(
                        typeof(MyCompatPatch),
                        nameof(MyCompatPatch.Postfix))
                );
                MelonLogger.Msg($"Patched night cycle for demons in Powerplay");
            }
        }

        Assembly dupery = AppDomain.CurrentDomain.GetAssemblies().FirstOrDefault(a => a.GetName().Name == "DuperyBluff");

        if (dupery != null)
        {
            foreach (Type t in dupery.GetTypes())
                MelonLogger.Msg(t.FullName);
            List<string> demons3 = new();
            demons3.Add("w_Dupe_Critic");
            HashSet<MethodInfo> patched3 = new();
            foreach (string demon in demons3)
            {
                Type targetType = dupery.GetType($"DuperyBluff.{demon}");

                if (targetType == null)
                {
                    MelonLogger.Msg($"Could not find {demon}");
                    continue;
                }
                MethodInfo targetMethod = targetType.GetMethod("GetRules", BindingFlags.Instance | BindingFlags.Public);

                if (targetMethod != null)
                {
                    targetMethod = targetMethod.GetBaseDefinition();
                }
                if (targetMethod == null)
                {
                    MelonLogger.Msg($"Could not find {demon}.GetRules");
                    continue;
                }
                if (!patched3.Add(targetMethod))
                {
                    MelonLogger.Msg($"Already patched {targetMethod.DeclaringType.Name}.GetRules");
                    continue;
                }
                MelonLogger.Msg($"Target type: {targetType.FullName}");
                MelonLogger.Msg($"Method: {targetMethod}");
                MelonLogger.Msg($"Declared in: {targetMethod.DeclaringType.FullName}");
                MainMod.Instance.HarmonyInstance.Patch(
                    targetMethod,
                    postfix: new HarmonyMethod(
                        typeof(MyCompatPatch),
                        nameof(MyCompatPatch.Postfix))
                );
                MelonLogger.Msg($"Patched night cycle for demons in Dupery Bluff");
            }
        }

        return true;
    }
    public static class MyCompatPatch
    {
        public static void Postfix(object __instance, ref Il2CppSystem.Collections.Generic.List<SpecialRule> __result)
        {
            // The following are the demons without night cycles
            List<string> demons = new();
            demons.Add("w_Iris");
            demons.Add("w_Leviathan");
            demons.Add("w_InvertDemon");
            demons.Add("w_Praesect");
            demons.Add("w_Mezepheles");
            demons.Add("w_TwinDemon");
            demons.Add("w_TwinDemonThree");
            demons.Add("w_TwinDemonTwin");
            demons.Add("Vortox");
            demons.Add("Famine");
            demons.Add("Crazed");
            demons.Add("Starspawn");
            demons.Add("Auditor");
            demons.Add("Godfather");
            demons.Add("w_Dupe_Critic");
            demons.Add("w_Dupe_Idol");
            demons.Add("w_Dupe_Recruiter");
            demons.Add("w_Dupe_Kingpin");
            demons.Add("w_Dupe_Hitman");
            if (demons.Contains(__instance.GetType().Name))
            {
                Il2CppSystem.Collections.Generic.List<SpecialRule> sr = new Il2CppSystem.Collections.Generic.List<SpecialRule>();
                sr.Add(new NightModeRule(4));
                __result = sr;
            }
        }
    }
    // Force night to always be active
    [HarmonyPatch(typeof(Imp), nameof(Imp.GetRules))]
    private static class ForceNightBaa
    {
        private static void Postfix(Imp __instance, ref Il2CppSystem.Collections.Generic.List<SpecialRule> __result)
        {
            Il2CppSystem.Collections.Generic.List<SpecialRule> sr = new Il2CppSystem.Collections.Generic.List<SpecialRule>();
            sr.Add(new NightModeRule(4));
            __result = sr;
        }
    }
    [HarmonyPatch(typeof(Pooka), nameof(Pooka.GetRules))]
    private static class ForceNightPooka
    {
        private static void Postfix(Pooka __instance, ref Il2CppSystem.Collections.Generic.List<SpecialRule> __result)
        {
            Il2CppSystem.Collections.Generic.List<SpecialRule> sr = new Il2CppSystem.Collections.Generic.List<SpecialRule>();
            sr.Add(new NightModeRule(4));
            __result = sr;
        }
    }

    [HarmonyPatch(typeof(Lycanthrope), nameof(Lycanthrope.GetRules))]
    private static class RemoveDuplicateNight
    {
        private static void Postfix(Lycanthrope __instance, ref Il2CppSystem.Collections.Generic.List<SpecialRule> __result)
        {
            Il2CppSystem.Collections.Generic.List<SpecialRule> sr = new Il2CppSystem.Collections.Generic.List<SpecialRule>();
            __result = sr;
        }
    }
}