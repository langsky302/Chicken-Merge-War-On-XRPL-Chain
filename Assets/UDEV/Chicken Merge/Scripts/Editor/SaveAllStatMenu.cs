using UnityEditor;
using UnityEngine;

namespace UDEV.ChickenMerge
{
    public class SaveAllStatMenu
    {
        [MenuItem("Chicken Merge/Save All Stats")]
        public static void SaveAllStats()
        {
            var allStats = Resources.LoadAll<Stat>(StatEditorUlti.STATDATA_SO_FOLDER);
            if (allStats == null || allStats.Length <= 0) return;
            for (int i = 0; i < allStats.Length; i++)
            {
                var stat = allStats[i];
                if (stat == null) continue;
                StatEditorUlti.Save(stat);
            }
        }
    }
}
