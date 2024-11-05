using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace UDEV.WaveManagerToolkit.Editor
{
    public class WaveTK_EnemySetItem_Layout : WaveTK_LayoutBuilder
    {
        private WaveTK_EnemySet m_enemySetSO;
        private WaveTK_EnemyWeighted m_enemyWeighted;
        private SerializedProperty m_enemyProp;
        private SerializedProperty m_poolKeyProp;
        private SerializedProperty m_weightedProp;
        private float m_spawnRate;

        private PropertyField m_enemyPoolKeyPropUI;
        public SliderInt enemyWeightPropUI;
        private ProgressBar m_spawnRateProgBar;
        public Button deleteEnemyBtn;

        public WaveTK_EnemySetItem_Layout(string templateName, WaveTK_EditorController editorController = null) : base(templateName, editorController)
        {
        }

        public void UpdateEnemySet(WaveTK_EnemySet enemySet)
        {
            m_enemySetSO = enemySet;
        }

        public void UpdateEnemyWeighted(WaveTK_EnemyWeighted enemyWeighted)
        {
            m_enemyWeighted = enemyWeighted;
        }

        public override void GetUIProperties()
        {
            m_enemyPoolKeyPropUI = m_container.Q<PropertyField>("EnemyPoolKey");
            enemyWeightPropUI = m_container.Q<SliderInt>("EnemyWeight");
            m_spawnRateProgBar = m_container.Q<ProgressBar>("SpawnRateProg");
            deleteEnemyBtn = m_container.Q<Button>("DeleteEnemyBtn");
        }

        public override void SetProperty(SerializedProperty property)
        {
            m_enemyProp = property;

            m_poolKeyProp = m_enemyProp.FindPropertyRelative("enemyPoolKey");
            m_weightedProp = m_enemyProp.FindPropertyRelative("weight");

            
        }

        protected override void BuildLayout()
        {
            m_enemyPoolKeyPropUI.label = "";
            enemyWeightPropUI.label = "";

            m_enemyPoolKeyPropUI.BindProperty(m_poolKeyProp);
            enemyWeightPropUI.BindProperty(m_weightedProp);

            UpdateSpawnRateProg();
        }

        public void UpdateSpawnRateProg()
        {
            if (m_enemySetSO == null || m_enemyWeighted == null) return;

            m_enemySetSO.UpdateTotalWeight();
            if (m_enemySetSO.totalWeight > 0)
            {
                m_spawnRate = (m_enemyWeighted.weight / (float)m_enemySetSO.totalWeight) * 100;
            }

            m_spawnRateProgBar.title = m_spawnRate.ToString("f1") + "%";
            m_spawnRateProgBar.value = m_spawnRate;
        }
    }
}
