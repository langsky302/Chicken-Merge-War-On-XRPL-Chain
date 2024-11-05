using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace UDEV.WaveManagerToolkit.Editor
{
    public class WaveTK_SpawnerSingleType_Layout : WaveTK_LayoutBuilder
    {
        private SerializedProperty m_spawnerProp;
        private SerializedProperty m_randomEnemyProp;
        private SerializedProperty m_enemyPoolKeyProp;
        private SerializedProperty m_isBossProp;
        private SerializedProperty m_enemySetProp;

        private PropertyField m_randomEnemyUIProp;
        private PropertyField m_enemyPoolKeyUIProp;
        private PropertyField m_bossOptUIProp;

        private VisualElement m_enemySetContentUI;

        

        public WaveTK_SpawnerSingleType_Layout(string templateName, WaveTK_EditorController editorController = null) : base(templateName, editorController)
        {
        }

        protected override void BuildLayout()
        {
            m_randomEnemyUIProp.BindProperty(m_randomEnemyProp);
            m_enemyPoolKeyUIProp.BindProperty(m_enemyPoolKeyProp);
            m_bossOptUIProp.BindProperty(m_isBossProp);

            var enemySetSO_Layout = new WaveTK_EnemySetSO_Layout("WaveTK_EnemySetSO", m_editorController);
            enemySetSO_Layout.SetProperty(m_enemySetProp);
            enemySetSO_Layout.Initialize();

            m_randomEnemyUIProp.RegisterValueChangeCallback((prop) =>
            {
                if (prop.changedProperty.boolValue)
                {
                    m_enemySetContentUI.Add(enemySetSO_Layout.Container);
                    WaveTK_UI_Utils.ShowElement(m_enemyPoolKeyUIProp, false);
                }else
                {
                    WaveTK_UI_Utils.ShowElement(m_enemyPoolKeyUIProp, true);
                    m_enemySetContentUI.Clear();
                }
            });
        }

        public override void GetUIProperties()
        {
            m_randomEnemyUIProp = m_container.Q<PropertyField>("RandomSpawnOpt");
            m_enemyPoolKeyUIProp = m_container.Q<PropertyField>("EnemyPoolKey");
            m_bossOptUIProp = m_container.Q<PropertyField>("BossOpt");
            m_enemySetContentUI = m_container.Q<VisualElement>("EnemySetContent");
        }

        public override void SetProperty(SerializedProperty property)
        {
            m_spawnerProp = property;
            m_randomEnemyProp = m_spawnerProp.FindPropertyRelative("randomEnemy");
            m_enemyPoolKeyProp = m_spawnerProp.FindPropertyRelative("enemyPoolKey");
            m_isBossProp = m_spawnerProp.FindPropertyRelative("isBoss");
            m_enemySetProp = m_spawnerProp.FindPropertyRelative("enemySet");
        }

        public void ShowIsBossOptProp(bool isShow)
        {
            WaveTK_UI_Utils.ShowElement(m_bossOptUIProp, isShow);
        }
    }
}
