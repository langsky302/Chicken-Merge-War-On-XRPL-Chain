using UnityEditor;
using UnityEngine;

namespace UDEV.ChickenMerge
{
    public class ReloadAllStatMenu
    {

        [MenuItem("Chicken Merge/ Reload All Stats")]
        public static void ReloadAllStats()
        {
            var allStats = Resources.LoadAll<Stat>(StatEditorUlti.STATDATA_SO_FOLDER);
            if (allStats == null || allStats.Length <= 0) return; 
            for (int i = 0; i < allStats.Length; i++)
            {
                var stat = allStats[i];
                if (stat == null) continue;
                string fileName = $"stat_data_{stat.id}";
                string filePath = $"{StatEditorUlti.STATDATA_EDITOR_FOLDER}/{fileName}";
                StatEditorUlti.Load(filePath, stat);
            }
        }
    }
}
