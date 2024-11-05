using UnityEditor;

namespace UDEV.ChickenMerge
{
    [CanEditMultipleObjects]
    [CustomEditor(typeof(EnemyStatSO), editorForChildClasses: true)]
    public class EnemyStatEditor : StatEditor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            m_csvUlti = new CSV_Ulti("CSV/EnemyStats", new string[]
            {
                "Game Level", "Health", "Move Speed"
            });
        }

        protected override void SaveToCSV()
        {
            Load(StatEditorUlti.FileName);
            Save();
            var enemyStat = (EnemyStatSO)target;
            string[] statInfos = new string[]
            {
                enemyStat.editorLevel.ToString(),
                enemyStat.hp.ToString(),
                enemyStat.moveSpeed.ToString()
            };
            m_csvUlti?.SetFilename(m_target.name + ".csv");
            m_csvUlti?.VerifyDirectory();
            m_csvUlti?.CreateCSV();
            m_csvUlti?.AppendToCSV(statInfos);

            UpgradeToMax(() =>
            {
                string[] statInfos = new string[]
                {
                    enemyStat.editorLevel.ToString(),
                    enemyStat.hp.ToString(),
                    enemyStat.moveSpeed.ToString()
                };
                m_csvUlti?.AppendToCSV(statInfos);
            });
            Load(StatEditorUlti.FileName);
        }

        protected override void Upgrade()
        {
            m_target.Upgrade();
        }
    }
}
