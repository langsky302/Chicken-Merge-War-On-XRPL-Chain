using UnityEditor;
using UnityEngine;

namespace UDEV.ChickenMerge
{
    public static class ExportCSV_AllEnenyStat_Menu
    {
        private static CSV_Ulti m_csvUlti;

        [MenuItem("Chicken Merge/Export Enemy Stats To CSV")]
        public static void ExportAllStats()
        {
            m_csvUlti = new CSV_Ulti("CSV/EnemyStats", new string[]
            {
                "Game Level", "Health", "Move Speed"
            });
            var allStats = Resources.LoadAll<EnemyStatSO>(StatEditorUlti.STATDATA_SO_FOLDER);
            if (allStats == null || allStats.Length <= 0) return;
            for (int i = 0; i < allStats.Length; i++)
            {
                var stat = allStats[i];
                if (stat == null) continue;
                string fileName = $"stat_data_{stat.id}";
                string filePath = $"{StatEditorUlti.STATDATA_EDITOR_FOLDER}/{fileName}";
                StatEditorUlti.Load(filePath, stat);
                StatEditorUlti.Save(stat);
                string[] statInfos = new string[]
                {
                    stat.editorLevel.ToString(),
                    stat.hp.ToString(),
                    stat.moveSpeed.ToString()
                };
                m_csvUlti?.SetFilename(stat.name + ".csv");
                m_csvUlti?.VerifyDirectory();
                m_csvUlti?.CreateCSV();
                m_csvUlti?.AppendToCSV(statInfos);

                stat.UpgradeToMax(() =>
                {
                    string[] statInfos = new string[]
                    {
                        stat.editorLevel.ToString(),
                        stat.hp.ToString(),
                        stat.moveSpeed.ToString()
                    };
                    m_csvUlti?.AppendToCSV(statInfos);
                });
                StatEditorUlti.Load(filePath, stat);
            }
        }
    }

}