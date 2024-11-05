using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine.UIElements;

namespace UDEV.WaveManagerToolkit.Editor
{
    public class WaveTK_WaveList_Layout : WaveTK_LayoutBuilder
    {
        private ListView m_waveListView;
        private Button m_addWaveBtn;
        private VisualElement m_spawnerList;
        private List<WaveTK_Wave> m_waves;
        private int m_curWaveIndex = -1;

        private EventCallback<ClickEvent> m_deleteWaveCallback;
        private EventCallback<ClickEvent> m_addWaveCallback;
        private Action<IEnumerable<object>> m_OnWaveItemSelectionChanged;

        public WaveTK_WaveList_Layout(string templateName, WaveTK_EditorController editorController = null) : base(templateName, editorController)
        {
            m_waves = editorController.WaveController.waves;
        }

        protected override void BuildLayout()
        {
            SetupWaveList();
        }

        private void SetupWaveList()
        {
            m_OnWaveItemSelectionChanged = (items) => OnItemSelectionChanged(items);
            m_addWaveCallback = (clickEvt) => AddNewWave();

            m_addWaveBtn.UnregisterCallback(m_addWaveCallback);
            m_addWaveBtn.RegisterCallback(m_addWaveCallback);
            
            m_waveListView.makeItem = () => new VisualElement();
            m_waveListView.itemsSource = m_waves;
            m_waveListView.bindItem = (e, i) =>
            {
                WaveItemSetup(e, i);   
            };
        }

        private void WaveItemSetup(VisualElement itemUI, int itemIndex)
        {
            m_deleteWaveCallback = (clickEvt) => DeleteWave(itemIndex);

            itemUI.Clear();
            var waveItemLayout = new WaveTK_WaveItem_Layout("WaveTK_WaveListItem", m_editorController);
            waveItemLayout.Initialize();
            itemUI.Add(waveItemLayout.Container);
            waveItemLayout.waveLabel.text = $"Wave {itemIndex + 1}";

            waveItemLayout.deleteWaveBtn.RegisterCallback(m_deleteWaveCallback);

            m_waveListView.selectionChanged -= OnItemSelectionChanged;
            m_waveListView.selectionChanged += OnItemSelectionChanged;
        }

        private void OnItemSelectionChanged(IEnumerable<object> items)
        {
            if (m_waveListView.selectedIndex == m_curWaveIndex) return;
            m_curWaveIndex = m_waveListView.selectedIndex;
            foreach (var item in items)
            {
                var wave = (WaveTK_Wave)item;
                if (wave != null)
                {
                    SetupSpawnerList(wave);
                    continue;
                };
                DeleteWave(m_curWaveIndex);
                return;
            }
        }

        private void SetupSpawnerList(WaveTK_Wave wave)
        {
            var spawnerListView = new WaveTK_SpawnerList_Layout("WaveTK_SpawnerList", m_editorController);
            spawnerListView.Initialize(wave);
            WaveTK_UI_Utils.ShowElement(spawnerListView.addSpawnerBtn, true);
            m_spawnerList = m_editorController.UiRootElement.Q<VisualElement>("SpawnerList");
            m_spawnerList.Clear();
            m_spawnerList.Add(spawnerListView.Container);
        }

        public override void GetUIProperties()
        {
            m_waveListView = m_container.Q<ListView>("WaveListView");
            m_addWaveBtn = m_container.Q<Button>("AddNewWaveBtn");
        }

        private void AddNewWave()
        {
            m_editorController?.AddNewWave(() =>
            {
                m_waveListView.Rebuild();
                m_waveListView.SetSelection(m_waves.Count - 1);
            });
        }

        public void DeleteWave(int waveIndex)
        {
            m_editorController?.DeleteWave(waveIndex, () =>
            {
                if(m_waves == null || m_waves.Count <= 0 || m_waveListView.selectedIndex == m_curWaveIndex)
                {
                    m_spawnerList?.Clear();
                }
                m_waveListView.Rebuild();
            });
        }
    }
}
