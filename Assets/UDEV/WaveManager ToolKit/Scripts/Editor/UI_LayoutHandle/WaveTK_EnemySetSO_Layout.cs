using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace UDEV.WaveManagerToolkit.Editor
{
    public class WaveTK_EnemySetSO_Layout : WaveTK_LayoutBuilder
    {
        private int m_assetId;
        private string m_assetIdFilePath = Application.dataPath + WaveTK_Const.EDITOR_DATA_PATH;
        private WaveTK_EnemySet m_enemySetSO;

        private SerializedProperty m_enemySetProp;

        private VisualElement m_enemySetWrapUI;
        private WaveTK_EnemySetList_Layout m_enemySetListLayout;
        private PropertyField m_enemySetPropUI;    
        public Button createEnemySetBtn;
        public Button deleteEnemySetBtn;

        private EventCallback<ClickEvent> m_deleteEnemySetCallback;
        private EventCallback<ClickEvent> m_addEnemySetCallback;
        private EventCallback<UnityEditor.UIElements.SerializedPropertyChangeEvent> m_enemySetChangeCallback;

        public WaveTK_EnemySetSO_Layout(string templateName, WaveTK_EditorController editorController = null) : base(templateName, editorController)
        {
        }

        public override void GetUIProperties()
        {
            m_enemySetWrapUI = m_container.Q<VisualElement>("EnemySetWrap");
            createEnemySetBtn = m_container.Q<Button>("AddEnemyBtn");
            deleteEnemySetBtn = m_container.Q<Button>("DeleteEnemyBtn");
            m_enemySetPropUI = m_container.Q<PropertyField>("EnemySetSO");
        }

        public override void SetProperty(SerializedProperty property)
        {
            m_enemySetProp = property;
            m_enemySetSO = (WaveTK_EnemySet)m_enemySetProp.objectReferenceValue;
        }

        protected override void BuildLayout()
        {
            m_enemySetListLayout = new WaveTK_EnemySetList_Layout("WaveTK_EnemySetList", m_editorController);
            m_deleteEnemySetCallback = (clickEvt) => DeleteEnemySet();
            m_addEnemySetCallback = (clickEvt) => CreateEnemySet();
            m_enemySetChangeCallback = (prop) =>
            {
                DisplayButtons();
                DisplayEnemyList();
            };

            EnemySetListLayoutInitialize(m_enemySetSO);

            m_enemySetWrapUI.Add(m_enemySetListLayout.Container);
            m_enemySetPropUI.BindProperty(m_enemySetProp);

            createEnemySetBtn.RegisterCallback(m_addEnemySetCallback);
            deleteEnemySetBtn.RegisterCallback(m_deleteEnemySetCallback);
            m_enemySetPropUI.RegisterValueChangeCallback(m_enemySetChangeCallback);
        }

        private void CreateEnemySet()
        {
            m_assetId = Utils.LoadDataFromFile(m_assetIdFilePath + "EnemySetIds.dat", m_assetId);
            m_assetId++;
            m_enemySetSO = WMN_ToolKit_SO_Utils<WaveTK_EnemySet>.CreateSO($"EnemySet {m_assetId}", WaveTK_Utils.holdingAssetPath);
            m_enemySetProp.objectReferenceValue = m_enemySetSO;
            m_enemySetProp.serializedObject?.ApplyModifiedProperties();
            m_enemySetProp.serializedObject?.Update();

            //WaveTK_UI_Utils.ShowElement(createEnemySetBtn, false);
            //WaveTK_UI_Utils.ShowElement(deleteEnemySetBtn, true);
            WaveTK_UI_Utils.ShowElement(m_enemySetListLayout.Container, true);
            EnemySetListLayoutInitialize(m_enemySetSO);

            Utils.SaveDataToFile(m_assetIdFilePath, "EnemySetIds.dat", m_assetId);
        }

        private void DeleteEnemySet()
        {
            WMN_ToolKit_SO_Utils<WaveTK_EnemySet>.DeleteSO(m_enemySetSO);
            m_enemySetSO = null;
            m_enemySetProp.objectReferenceValue = m_enemySetSO;

            WaveTK_UI_Utils.ShowElement(createEnemySetBtn, true);
            WaveTK_UI_Utils.ShowElement(deleteEnemySetBtn, false);
            WaveTK_UI_Utils.ShowElement(m_enemySetListLayout.Container, false);
            EnemySetListLayoutInitialize(m_enemySetSO);
        }

        private void EnemySetListLayoutInitialize(WaveTK_EnemySet enemySet)
        {
            if (m_enemySetListLayout == null) return;

            m_enemySetListLayout.UpdateEnemySet(enemySet);
            m_enemySetListLayout.SetProperty(m_enemySetProp);
            m_enemySetListLayout.Initialize();
        }

        private void DisplayButtons()
        {
            WaveTK_UI_Utils.ShowElement(createEnemySetBtn, true);
            WaveTK_UI_Utils.ShowElement(deleteEnemySetBtn, false);
        }

        private void DisplayEnemyList()
        {
            var enemySetRefObj = m_enemySetProp.objectReferenceValue;            
            WaveTK_UI_Utils.ShowElement(m_enemySetListLayout.Container, enemySetRefObj != null);
            EnemySetListLayoutInitialize((WaveTK_EnemySet)enemySetRefObj);
        }
    }
}
