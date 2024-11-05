using System.Collections.Generic;
using System.Linq;
using UnityEditor;

namespace UDEV
{
    public static class ScriptingDefineSymbolUtils
    {
        #region Add Symbols
        public static void AddScriptingDefineSymbol(string defineSymbol)
        {
            AddSymbolForTarget(BuildTargetGroup.Standalone, defineSymbol);
            AddSymbolForTarget(BuildTargetGroup.Android, defineSymbol);
            AddSymbolForTarget(BuildTargetGroup.iOS, defineSymbol);
        }


        private static void AddSymbolForTarget(BuildTargetGroup targetGroup, string defineSymbol)
        {
            string defines = PlayerSettings.GetScriptingDefineSymbolsForGroup(targetGroup);
            if (defines.Contains(defineSymbol))
            {
                return;
            }
            PlayerSettings.SetScriptingDefineSymbolsForGroup(targetGroup, (defines + ";" + defineSymbol));
        }
        #endregion

        #region Remove Symbols
        public static void RemoveScriptingDefineSymbol(string defineSymbol)
        {
            RemoveSymbolForTarget(BuildTargetGroup.Standalone, defineSymbol);
            RemoveSymbolForTarget(BuildTargetGroup.Android, defineSymbol);
            RemoveSymbolForTarget(BuildTargetGroup.iOS, defineSymbol);
        }

        private static void RemoveSymbolForTarget(BuildTargetGroup targetGroup, string defineSymbol)
        {
            string defines = PlayerSettings.GetScriptingDefineSymbolsForGroup(targetGroup);
            List<string> allSymbols = defines.Split(';').ToList();

            if (allSymbols.Contains(defineSymbol))
            {
                allSymbols.Remove(defineSymbol);
            }

            string newDefine = "";
            foreach (string symbol in allSymbols)
            {
                newDefine = newDefine + ";" + symbol;
            }
            PlayerSettings.SetScriptingDefineSymbolsForGroup(targetGroup, newDefine);
        }
        #endregion

        public static bool HasDefineSymbol(string defineSymbol)
        {
            string defines = "";

#if UNITY_IOS
            defines = PlayerSettings.GetScriptingDefineSymbolsForGroup((BuildTargetGroup.iOS));
#elif UNITY_ANDROID
            defines = PlayerSettings.GetScriptingDefineSymbolsForGroup((BuildTargetGroup.Android));
#endif

            if (defines.Contains(defineSymbol))
            {
                return true;
            }
            return false;
        }
    }
}
