using System.Collections.Generic;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace UDEV.WaveManagerToolkit.Editor
{
    public class WaveTK_SpawnerList_Layout : WaveTK_LayoutBuilder
    {
        private ListView m_spawnerListView;
        public Button addSpawnerBtn;
        private WaveTK_Wave m_wave;
        private SerializedObject m_waveSerialized;
        private SerializedProperty m_spawnersProp;
        private List<Foldout> m_foldouts = new List<Foldout>();
        private int m_prevFoldoutIndex = -1;

        private EventCallback<ClickEvent> m_deleteSpawnerCallback;
        private EventCallback<ClickEvent> m_addSpawnerCallback;

        public WaveTK_SpawnerList_Layout(string templateName, WaveTK_EditorController editorController = null) : base(templateName, editorController)
        {
            
        }

        public void Initialize(WaveTK_Wave wave)
        {
            m_wave = wave;
            m_waveSerialized = new SerializedObject(m_wave);
            Initialize();
            m_foldouts.Clear();
        }

        protected override void BuildLayout()
        {
            m_addSpawnerCallback = (clickEvt) => AddNewSpawner();

            addSpawnerBtn.UnregisterCallback(m_addSpawnerCallback);
            addSpawnerBtn.RegisterCallback(m_addSpawnerCallback);

            SpawnerListViewSetup();   
        }

        private void SpawnerListViewSetup()
        {
            m_spawnerListView.makeItem = () => new VisualElement();
            m_spawnerListView.itemsSource = m_wave.spawners;
            m_spawnerListView.bindItem = (e, i) =>
            {
                SpawnerItemSetup(e, i);
            };
        }

        private void SpawnerItemSetup(VisualElement itemUI, int itemIndex)
        {
            m_deleteSpawnerCallback = (clickEvt) => DeleteSpawner(itemIndex);

            itemUI.Clear();
            var spawnerItemLayout = new WaveTK_SpawnerItem_Layout("WaveTK_SpawnerListItem", m_editorController);
            itemUI.Add(spawnerItemLayout.Container);

            spawnerItemLayout.GetUIProperties();
            spawnerItemLayout.foldOut.text = $"Spawner {itemIndex + 1}";
            spawnerItemLayout.foldOut.value = false;
            m_foldouts.Add(spawnerItemLayout.foldOut);
            GetSpawnerProp();
            if (m_spawnersProp == null || m_spawnersProp.arraySize <= itemIndex) return;
            var spawnerProp = m_spawnersProp.GetArrayElementAtIndex(itemIndex);
            var spawnerTypeProp = spawnerProp.FindPropertyRelative("type");

            spawnerItemLayout.SetProperty(spawnerProp);
            spawnerItemLayout.Initialize();
            spawnerItemLayout.spawnerTypePropUI.BindProperty(spawnerTypeProp);
            spawnerItemLayout.deleteSpawnerBtn.RegisterCallback(m_deleteSpawnerCallback);

            spawnerItemLayout.foldOut.RegisterCallback<ClickEvent>((clickEvt) =>
            {
                if(m_prevFoldoutIndex != itemIndex && spawnerItemLayout.foldOut.value)
                {
                    spawnerItemLayout.Initialize();
                    m_foldouts[itemIndex] = spawnerItemLayout.foldOut;
                }

                UpdadeAllFolouts(itemIndex);
                m_prevFoldoutIndex = itemIndex;
            });
        }

        private void UpdadeAllFolouts(int clickedIndex)
        {
            if (m_foldouts == null || m_foldouts.Count <= 0 || clickedIndex == m_prevFoldoutIndex) return;
            for (int i = 0; i < m_spawnersProp.arraySize; i++)
            {
                var foldout = m_foldouts[i];
                if (foldout == null) continue;
                foldout.value = false;
                if (i != clickedIndex) continue;
                foldout.value = true;
            }
        }

        private void GetSpawnerProp()
        {
            m_spawnersProp = m_waveSerialized.FindProperty("spawners");
        }

        public override void GetUIProperties()
        {
            m_spawnerListView = m_container.Q<ListView>("SpawnerListView");
            addSpawnerBtn = m_container.Q<Button>("AddNewSpawnerBtn");
        }

        private void DeleteSpawner(int spawnerIndex)
        {
            m_editorController.DeleteSpawner(m_wave, spawnerIndex, () =>
            {
                m_spawnersProp?.serializedObject?.ApplyModifiedProperties();
                m_spawnersProp?.serializedObject?.Update();
                m_foldouts.Clear();
                m_spawnerListView.Rebuild();
            });
        }

        private void AddNewSpawner()
        {
            m_editorController.AddNewSpawner(m_wave, () =>
            {
                GetSpawnerProp();
                m_spawnersProp?.serializedObject?.ApplyModifiedProperties();
                m_spawnersProp?.serializedObject?.Update();
                m_foldouts.Clear();
                m_prevFoldoutIndex = -1;
                m_spawnerListView.Rebuild();
                m_spawnerListView.SetSelection(m_wave.spawners.Count - 1);
            });
        }
    }
}
