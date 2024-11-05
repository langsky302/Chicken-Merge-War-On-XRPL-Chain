using UnityEditor;
using UnityEngine.UIElements;

namespace UDEV.WaveManagerToolkit.Editor
{
    public class WaveTK_SpawnerItem_Layout : WaveTK_LayoutBuilder
    {
        private WaveTK_SpawnerSingleType_Layout m_spawnerSingleType_Layout;
        private WaveTK_SpawnerDelayType_Layout m_spawnerDelayType_Layout;

        public Foldout foldOut;
        public Button deleteSpawnerBtn;
        public EnumField spawnerTypePropUI;
        public VisualElement spawnerItemContent;

        private SerializedProperty m_spawnerProp;
        public WaveTK_SpawnerItem_Layout(string templateName, WaveTK_EditorController editorController = null) : base(templateName, editorController)
        {
        }

        public override void SetProperty(SerializedProperty property)
        {
            m_spawnerProp = property;
        }

        protected override void BuildLayout()
        {
            BuildLayoutCore();

            spawnerTypePropUI.RegisterValueChangedCallback((prop) =>
            {
                BuildLayoutCore();
            });
        }

        private void BuildLayoutCore()
        {
            var spawnerTypeProp = m_spawnerProp.FindPropertyRelative("type");
            var spawnerType = (WaveTK_SpawnerType)spawnerTypeProp.enumValueIndex;
            switch (spawnerType)
            {
                case WaveTK_SpawnerType.SINGLE:
                    m_spawnerSingleType_Layout = new WaveTK_SpawnerSingleType_Layout("WaveTK_SpawnerSingleType", m_editorController);
                    BuildLayoutHandle(m_spawnerSingleType_Layout);
                    m_spawnerSingleType_Layout.ShowIsBossOptProp(true);
                    break;
                case WaveTK_SpawnerType.DELAY:
                    m_spawnerDelayType_Layout = new WaveTK_SpawnerDelayType_Layout("WaveTK_SpawnerDelayType", m_editorController);
                    BuildLayoutHandle(m_spawnerDelayType_Layout);
                    break;
                case WaveTK_SpawnerType.BUNDLE:
                    m_spawnerSingleType_Layout = new WaveTK_SpawnerSingleType_Layout("WaveTK_SpawnerSingleType", m_editorController);
                    m_spawnerDelayType_Layout = new WaveTK_SpawnerDelayType_Layout("WaveTK_SpawnerDelayType", m_editorController);
                    BuildLayoutHandle(m_spawnerSingleType_Layout);
                    BuildLayoutHandle(m_spawnerDelayType_Layout, false);
                    m_spawnerSingleType_Layout.ShowIsBossOptProp(false);
                    break;
            }
        }

        private void BuildLayoutHandle(WaveTK_LayoutBuilder layout, bool isForeClear = true)
        {
            if (isForeClear) spawnerItemContent.Clear();
            layout.SetProperty(m_spawnerProp);
            layout.Initialize();
            spawnerItemContent.Add(layout.Container);
        }

        public override void GetUIProperties()
        {
            foldOut = m_container.Q<Foldout>("SpawnerItemFoldout");
            deleteSpawnerBtn = m_container.Q<Button>("DeleteSpawnerBtn");
            spawnerTypePropUI = m_container.Q<EnumField>("SpawnerType");
            spawnerItemContent = m_container.Q<VisualElement>("SpawnerItemContent");
        }
    }
}
