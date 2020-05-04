using UnityModManagerNet;
using System;
using System.Reflection;
using System.Linq;
using Kingmaker.Blueprints;
using Kingmaker;
using Kingmaker.Blueprints.Classes;
using Kingmaker.UnitLogic.FactLogic;
using Kingmaker.UnitLogic.Mechanics.Components;
using Kingmaker.UnitLogic.Buffs.Blueprints;
using Kingmaker.Designers.Mechanics.Buffs;
using System.Collections.Generic;
using Kingmaker.Blueprints.Items;
using Newtonsoft.Json;
using System.IO;
using Newtonsoft.Json.Linq;

namespace UndeadImmunitiesMod
{
    internal class Main
    {

        internal class Settings
        {
            internal Settings()
            {

                using (StreamReader settings_file = File.OpenText("Mods/UndeadImmunitiesMod/settings.json"))
                using (JsonTextReader reader = new JsonTextReader(settings_file))
                {
                    JObject jo = (JObject)JToken.ReadFrom(reader);
                }
            }
        }

        internal static UnityModManagerNet.UnityModManager.ModEntry.ModLogger logger;
        internal static Harmony12.HarmonyInstance harmony;
        internal static LibraryScriptableObject library;


        static readonly Dictionary<Type, bool> typesPatched = new Dictionary<Type, bool>();
        static readonly List<String> failedPatches = new List<String>();
        static readonly List<String> failedLoading = new List<String>();

        [System.Diagnostics.Conditional("DEBUG")]
        internal static void DebugLog(string msg)
        {
            if (logger != null) logger.Log(msg);
        }
        internal static void DebugError(Exception ex)
        {
            if (logger != null) logger.Log(ex.ToString() + "\n" + ex.StackTrace);
        }
        internal static bool enabled;

        static bool Load(UnityModManager.ModEntry modEntry)
        {
            try
            {
                logger = modEntry.Logger;
                harmony = Harmony12.HarmonyInstance.Create(modEntry.Info.Id);
                harmony.PatchAll(Assembly.GetExecutingAssembly());
                logger.Log("Do I at least make it here?");
            }
            catch (Exception ex)
            {
                DebugError(ex);
                throw ex;
            }
            return true;
        }

        [Harmony12.HarmonyPatch(typeof(LibraryScriptableObject), "LoadDictionary")]
        [Harmony12.HarmonyPatch(typeof(LibraryScriptableObject), "LoadDictionary", new Type[0])]
        static class LibraryScriptableObject_LoadDictionary_Patch
        {
            static void Postfix(LibraryScriptableObject __instance)
            {
                var self = __instance;
                logger.Log("Did make it to undead ImmunitiesMod");
                logger.Log("Library loaded? " + (Main.library != null).ToString());
                if (Main.library != null) return;
                Main.library = self;
                try
                {
                    //Here is wehre everything gets accomplished
                    Core.load();
                }
                catch (Exception ex)
                {
                    Main.DebugError(ex);
                }
            }
        }
        
        internal static Exception Error(String message)
        {
            logger?.Log(message);
            return new InvalidOperationException(message);
        }
    }
}

