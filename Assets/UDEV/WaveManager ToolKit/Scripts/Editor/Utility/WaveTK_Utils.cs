using System;
using UnityEditor;
using UnityEngine;

namespace UDEV.WaveManagerToolkit.Editor
{
    public static class WaveTK_Utils
    {
        public static string holdingAssetPath;

        public static String GetAssetPath(UnityEngine.Object obj)
        {
            string result = "";
            string path = AssetDatabase.GetAssetPath(obj);
            var splits = path.Split('/');
            for (int i = 0; i < splits.Length; i++)
            {
                var str = splits[i];
                if (i == splits.Length - 1) break;
                result += str + "\\";
            }
            return result;
        }
    }

    public static class WMN_ToolKit_SO_Utils<T> where T : ScriptableObject
    {
        public static T CreateSO(string filename, string path)
        {
            T newSO = ScriptableObject.CreateInstance<T>();
            AssetDatabase.CreateAsset(newSO, path + $"{filename}.asset");
            return newSO;
        }

        public static void DeleteSO(T SO)
        {
            if (SO == null) return;

            string assetPath = AssetDatabase.GetAssetPath(SO);
            AssetDatabase.DeleteAsset(assetPath);
        }
    }
}
