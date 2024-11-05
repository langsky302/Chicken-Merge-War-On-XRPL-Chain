using UnityEngine;
using UnityEditor;
using System.IO;

namespace UDEV.WaveManagerToolkit.Editor
{
    public class WaveTK_ToolKit_Menu
    {
        private static int id; 
        private static string m_idFilePath = Application.dataPath + WaveTK_Const.EDITOR_DATA_PATH;

        [MenuItem("Assets/Create/UDEV/WaveTK/Create Wave Controller", priority = 0)]
        public static void CreateWaveController()
        {
            var currentPath = AssetDatabase.GetAssetPath(Selection.activeObject) + "/";
            id = Utils.LoadDataFromFile(m_idFilePath + "WaveControllerIds.dat", id);
            id++;
            var waveCtr = new GameObject("Wave Controller");
            waveCtr.AddComponent(typeof(WaveTK_WaveController));
            var waveCtrSavingPath = Path.Combine(currentPath + $"WaveController {id}.prefab");
            PrefabUtility.SaveAsPrefabAsset(waveCtr, waveCtrSavingPath);
            GameObject.DestroyImmediate(waveCtr);
            Utils.SaveDataToFile(m_idFilePath, "WaveControllerIds.dat", id);
        }
    }
}
