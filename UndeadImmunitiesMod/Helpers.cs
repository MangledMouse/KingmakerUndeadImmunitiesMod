using Kingmaker.Blueprints;
using System;
using System.Collections.Generic;
using System.Text;

namespace UndeadImmunitiesMod
{
    class Helpers
    {
    }
    static class ExtensionMethods
    {
        public static T Get<T>(this LibraryScriptableObject library, String assetId) where T : BlueprintScriptableObject
        {
            return (T)library.BlueprintsByAssetId[assetId];
        }
    }
}
