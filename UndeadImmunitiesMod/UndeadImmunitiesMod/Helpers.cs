using Kingmaker.Blueprints;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UndeadImmunitiesMod
{
    class Helpers
    {
    }

    public static class ExtensionMethods
    {
        public static T Get<T>(this LibraryScriptableObject library, String assetId) where T : BlueprintScriptableObject
        {
            return (T)library.BlueprintsByAssetId[assetId];
        }
    }
}
