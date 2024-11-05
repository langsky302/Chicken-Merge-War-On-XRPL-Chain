using System.Collections.Generic;
using UnityEditor;
using UnityEngine.UIElements;

namespace UDEV.WaveManagerToolkit.Editor
{
    public class WaveTK_EnemySetList_Layout : WaveTK_LayoutBuilder
    {
        private SerializedProperty m_enemySetProp;
        private SerializedProperty m_enemiesProp;
        private WaveTK_EnemySet m_enemySetSO;

        private Foldout m_foldOut;
        private ListView m_enemySetListView;
        private VisualElement m_infoLabels;
        private Button m_addNewEnemyBtn;

        private EventCallback<ClickEvent> m_deleteEnemyCallback;
        private EventCallback<ClickEvent> m_addEnemyCallback;
        private EventCallback<ChangeEvent<int>> m_weightChangeCallback;

        private List<WaveTK_EnemySetItem_Layout> m_enemySetItemLayouts = new List<WaveTK_EnemySetItem_Layout>();

        public WaveTK_EnemySetList_Layout(string templateName, WaveTK_EditorController editorController = null) : base(templateName, editorController)
        {
        }

        public void UpdateEnemySet(WaveTK_EnemySet enemySet)
        {
            m_enemySetSO = enemySet;
        }

        public override void Initialize()
        {
            m_enemySetItemLayouts.Clear();
            base.Initialize();
        }

        public override void GetUIProperties()
        {
            m_foldOut = m_container.Q<Foldout>("EnemySetListFoldout");
            m_enemySetListView = m_container.Q<ListView>("EnemySetListView");
            m_addNewEnemyBtn = m_container.Q<Button>("AddNewEnemyBtn");
            m_infoLabels = m_container.Q<VisualElement>("InfoLabels");
        }

        public override void SetProperty(SerializedProperty property)
        {
            m_enemySetProp = property;
            if (m_enemySetSO == null || m_enemySetProp == null) return;
            var enemySetSerializedObj = new SerializedObject(m_enemySetSO);
            m_enemiesProp = enemySetSerializedObj.FindProperty("enemies");
        }

        protected override void BuildLayout()
        {
            m_foldOut.text = "List Of Enemies";
            m_foldOut.value = true;

            m_addEnemyCallback = (clickEvt) => AddNewSpawnerEnemy();

            m_addNewEnemyBtn.RegisterCallback(m_addEnemyCallback);

            SetupEnemySetListView();
        }

        private void SetupEnemySetListView()
        {
            if (m_enemySetSO == null || m_enemySetProp == null) return;

            m_enemySetListView.makeItem = () => new VisualElement();
            m_enemySetListView.itemsSource = m_enemySetSO.enemies;
            m_enemySetListView.bindItem = (e, i) =>
            {
                SetupEnemySetListItem(e, i);
            };

            WaveTK_UI_Utils.ShowElement(m_infoLabels, m_enemySetSO.enemies.Count > 0);
        }

        private void SetupEnemySetListItem(VisualElement itemUI, int itemIndex)
        {
            m_deleteEnemyCallback = (clickEvt) => DeleteSpawnerEnemy(itemIndex);
            m_weightChangeCallback = (prop) => UpdateAllSpawnRateUI();

            if (m_enemiesProp == null || m_enemiesProp.arraySize <= itemIndex) return;
            var enemyProp = m_enemiesProp.GetArrayElementAtIndex(itemIndex);
            var enemyItemLayout = new WaveTK_EnemySetItem_Layout("WaveTK_EnemySetListItem", m_editorController);
            itemUI.Clear();
            itemUI.Add(enemyItemLayout.Container);
            enemyItemLayout.UpdateEnemySet(m_enemySetSO);
            enemyItemLayout.UpdateEnemyWeighted(m_enemySetSO.enemies[itemIndex]);
            enemyItemLayout.SetProperty(enemyProp);
            enemyItemLayout.Initialize();
            m_enemySetItemLayouts.Add(enemyItemLayout);

            enemyItemLayout.deleteEnemyBtn.RegisterCallback(m_deleteEnemyCallback);
            enemyItemLayout.enemyWeightPropUI.RegisterValueChangedCallback(m_weightChangeCallback);
        }

        private void UpdateAllSpawnRateUI()
        {
            m_enemiesProp.serializedObject?.ApplyModifiedProperties();
            m_enemiesProp.serializedObject?.Update();

            if (m_enemySetItemLayouts == null || m_enemySetItemLayouts.Count <= 0) return;

            for(int i = 0; i < m_enemySetItemLayouts.Count; i++)
            {
                var enemySetItemLayout = m_enemySetItemLayouts[i];
                if(enemySetItemLayout == null) continue;
                enemySetItemLayout.UpdateSpawnRateProg();
            }
        }

        private void AddNewSpawnerEnemy()
        {
            m_editorController.AddNewSpawnerEnemy(m_enemySetSO, () =>
            {
                m_enemiesProp.serializedObject?.ApplyModifiedProperties();
                m_enemiesProp.serializedObject?.Update();
                m_enemySetItemLayouts.Clear();
                m_enemySetListView.Rebuild();
            });
        }

        private void DeleteSpawnerEnemy(int enemyIndex)
        {
            m_editorController.DeleteSpawnerEnemy(m_enemySetSO, enemyIndex, () =>
            {
                m_enemiesProp.serializedObject?.ApplyModifiedProperties();
                m_enemiesProp.serializedObject?.Update();
                m_enemySetItemLayouts.Clear();
                m_enemySetListView.Rebuild();
            });
        }
    }
}
