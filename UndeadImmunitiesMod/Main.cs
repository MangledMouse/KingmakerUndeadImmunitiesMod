using Kingmaker.Blueprints;
using System;
using UnityModManagerNet;

namespace UndeadImmunitiesMod
{
    public class Main
    {
        public static LibraryScriptableObject library;
        internal static UnityModManagerNet.UnityModManager.ModEntry.ModLogger logger;

        [System.Diagnostics.Conditional("DEBUG")]
        internal static void DebugLog(string msg)
        {
            if (logger != null) logger.Log(msg);
        }

        static bool Load(UnityModManager.ModEntry modEntry)
        {
            try
            {
                logger = modEntry.Logger;
                harmony = Harmony12.HarmonyInstance.Create(modEntry.Info.Id);
                harmony.PatchAll(Assembly.GetExecutingAssembly());

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
                if (Main.library != null)
                    return;
                Main.library = self;

                //The codes!!!
                ImmunitiesChanger.undeadImmunitiesChange();
            }
        }
    }
}
