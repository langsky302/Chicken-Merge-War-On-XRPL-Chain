using UnityEditor;

namespace UDEV.ChickenMerge
{
    [CanEditMultipleObjects]
    [CustomEditor(typeof(GunStatSO), editorForChildClasses: true)]
    public class GunStatEditor : StatEditor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            m_csvUlti = new CSV_Ulti("CSV/GunStats", new string[]
            {
                "Gun Level", "Damage", "Firerate", "Critical Rate", "Buying Price", "Upgrade Price"
            });
        }

        protected override void SaveToCSV()
        {
            Load(StatEditorUlti.FileName);
            Save();
            var gunStat = (GunStatSO)target;
            string[] statInfos = new string[]
            {
                gunStat.level.ToString(),
                gunStat.damage.ToString(),
                gunStat.fireRate.ToString(),
                gunStat.critRate.ToString(),
                gunStat.buyingPrice.ToString(),
                gunStat.upgradePrice.ToString()
            };
            m_csvUlti?.SetFilename(m_target.name + ".csv");
            m_csvUlti?.VerifyDirectory();
            m_csvUlti?.CreateCSV();
            m_csvUlti?.AppendToCSV(statInfos);

            UpgradeToMax(() =>
            {
                string[] statInfos = new string[]
                {
                    gunStat.level.ToString(),
                    gunStat.damage.ToString(),
                    gunStat.fireRate.ToString(),
                    gunStat.critRate.ToString(),
                    gunStat.buyingPrice.ToString(),
                    gunStat.upgradePrice.ToString()
                };
                m_csvUlti?.AppendToCSV(statInfos);
            });
            Load(StatEditorUlti.FileName);
        }
    }
}
