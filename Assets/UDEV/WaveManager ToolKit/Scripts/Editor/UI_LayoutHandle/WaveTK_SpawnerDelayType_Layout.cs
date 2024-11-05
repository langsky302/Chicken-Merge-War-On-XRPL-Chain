using UnityEditor.UIElements;
using UnityEditor;
using UnityEngine.UIElements;

namespace UDEV.WaveManagerToolkit.Editor
{
    public class WaveTK_SpawnerDelayType_Layout : WaveTK_LayoutBuilder
    {
        private SerializedProperty m_spawnerProp;
        private SerializedProperty m_totalEnemyProp;
        private SerializedProperty m_randomTimeSpawnProp;
        private SerializedProperty m_timeSpawnProp;
        private SerializedProperty m_minTimeProp;
        private SerializedProperty m_maxTimeProp;

        private PropertyField m_totalEnemyPropUI;
        private PropertyField m_randomTimeSpawnUIProp;
        private PropertyField m_timeSpawnUIProp;
        private PropertyField m_minTimeUIProp;
        private PropertyField m_maxTimeUIProp;
        private VisualElement m_randomTimeWrap;

        public WaveTK_SpawnerDelayType_Layout(string templateName, WaveTK_EditorController editorController = null) : base(templateName, editorController)
        {
        }

        protected override void BuildLayout()
        {
            m_randomTimeSpawnUIProp.BindProperty(m_randomTimeSpawnProp);
            m_timeSpawnUIProp.BindProperty(m_timeSpawnProp);
            m_minTimeUIProp.BindProperty(m_minTimeProp);
            m_maxTimeUIProp.BindProperty(m_maxTimeProp);

            m_randomTimeSpawnUIProp.RegisterValueChangeCallback((prop) =>
            {
                WaveTK_UI_Utils.ShowElement(m_timeSpawnUIProp, !prop.changedProperty.boolValue);
                WaveTK_UI_Utils.ShowElement(m_randomTimeWrap, prop.changedProperty.boolValue);
                WaveTK_UI_Utils.ShowElement(m_randomTimeWrap, prop.changedProperty.boolValue);
            });

            var spawnerTypeProp = m_spawnerProp.FindPropertyRelative("type");
            var spawnerType = (WaveTK_SpawnerType)spawnerTypeProp.enumValueIndex;
            if (spawnerType == WaveTK_SpawnerType.BUNDLE)
            {
                m_totalEnemyPropUI.BindProperty(m_totalEnemyProp);
            }
        }

        public override void GetUIProperties()
        {
            m_totalEnemyPropUI = m_container.Q<PropertyField>("TotalEnemy");
            m_randomTimeSpawnUIProp = m_container.Q<PropertyField>("RandomTimeSpawn");
            m_timeSpawnUIProp = m_container.Q<PropertyField>("TimeSpawn");
            m_minTimeUIProp = m_container.Q<PropertyField>("MinTime");
            m_maxTimeUIProp = m_container.Q<PropertyField>("MaxTime");
            m_randomTimeWrap = m_container.Q<VisualElement>("RandomTimeWrap");

            m_minTimeUIProp.label = "";
            m_maxTimeUIProp.label = "";
        }

        public override void SetProperty(SerializedProperty property)
        {
            m_spawnerProp = property;
            m_totalEnemyProp = m_spawnerProp.FindPropertyRelative("totalEnemy");
            m_randomTimeSpawnProp = m_spawnerProp.FindPropertyRelative("randomTime");
            m_timeSpawnProp = m_spawnerProp.FindPropertyRelative("timeSpawn");
            var timeRange = m_spawnerProp.FindPropertyRelative("timeRange");
            m_minTimeProp = timeRange.FindPropertyRelative("x");
            m_maxTimeProp = timeRange.FindPropertyRelative("y");
        }
    }
}
