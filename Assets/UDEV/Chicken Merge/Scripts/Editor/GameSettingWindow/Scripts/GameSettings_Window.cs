using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace UDEV.GameSettings.Editor
{
    public class GameSettings_Window : EditorWindow
    {
        private static EditorWindow m_window;

        private GameSettings_EditorController m_editorController;

        private VisualElement m_containerUI;
        private ListView m_leftMenuList;
        private VisualElement m_rightColumn;
        private Label m_settingsLabel;

        private Action<IEnumerable<object>> m_OnWaveItemSelectionChanged;

        [MenuItem("Chicken Merge/Game Settings", priority = 0)]
        public static void ShowWMNWindow()
        {
            if (m_window)
            {
                m_window.Close();
            }

            m_window = GetWindow<GameSettings_Window>();
            m_window.titleContent = new GUIContent("Game Settings");
        }

        private void CreateGUI()
        {
            m_containerUI = rootVisualElement;
            VisualTreeAsset visualTree = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>
                (GameSettings_Consts.UXML_LAYOUT_PATH + "GameSettings_Window.uxml");

            m_containerUI.Add(visualTree.Instantiate());

            StyleSheet styleSheet = AssetDatabase.LoadAssetAtPath<StyleSheet>
                (GameSettings_Consts.USS_LAYOUT_PATH + "GameSettings_Window.uss");
            
            m_containerUI.styleSheets.Add(styleSheet);

            CreateSettingsMenu();
        }

        private void CreateSettingsMenu()
        {
            m_OnWaveItemSelectionChanged = OnMenuItemSelectionChanged;

            m_leftMenuList = m_containerUI.Q<ListView>("left-menu-list");
            m_rightColumn = m_containerUI.Q<VisualElement>("right-column");
            m_settingsLabel = m_containerUI.Q<Label>("settings-label");

            if (m_leftMenuList == null) return;

            List<string> menuNames = new List<string>() { "Global", "Advertising", "In-App", "Credits"};
            m_leftMenuList.makeItem = () => new Label();
            m_leftMenuList.itemsSource = menuNames;
            m_leftMenuList.bindItem = (e, i) =>
            {
                var labelItem = (Label)e;
                var menuName = menuNames[i];
                labelItem.text = menuName;
            };

            m_leftMenuList.selectionChanged -= m_OnWaveItemSelectionChanged;
            m_leftMenuList.selectionChanged += m_OnWaveItemSelectionChanged;

            m_leftMenuList.SetSelection(0);
        }

        private void OnMenuItemSelectionChanged(IEnumerable<object> items)
        {
            ShowMenu((GameSettings_Type)m_leftMenuList.selectedIndex);
        }

        private void ShowMenu(GameSettings_Type type)
        {
            m_rightColumn.Clear();
            switch (type) {
                case GameSettings_Type.GLOBAL:
                    BuildGlobalSettingsLayout();
                    break;
                case GameSettings_Type.ADS:
                    BuildAdsSettingsLayout();
                    break;
                case GameSettings_Type.IAP:
                    BuildIAPSettingsLayout();
                    break;
                case GameSettings_Type.CREDITS:
                    BuildCreditsLayout();
                    break;
            }
            
        }

        private void BuildGlobalSettingsLayout()
        {
            m_settingsLabel.text = "Global Settings";
            var globalSettingsLayout = new GameSettings_Global_Layout("GameSettings_Global", m_editorController);
            globalSettingsLayout.Initialize();
            m_rightColumn.Add(globalSettingsLayout.Container);
        }

        private void BuildAdsSettingsLayout()
        {
            m_settingsLabel.text = "Advertising Settings";
            var adsSettingsLayout = new GameSettings_Advertising_Layout("GameSettings_Advertising", m_editorController);
            adsSettingsLayout.Initialize();
            m_rightColumn.Add(adsSettingsLayout.Container);
        }

        private void BuildIAPSettingsLayout()
        {
            m_settingsLabel.text = "In-App Purchases Settings";
            var inappSettingsLayout = new GameSettings_InApp_Layout("GameSettings_InApp_Purchases", m_editorController);
            inappSettingsLayout.Initialize();
            m_rightColumn.Add(inappSettingsLayout.Container);
        }

        private void BuildCreditsLayout()
        {
            m_settingsLabel.text = "Credits";
            var creditsLayout = new GameSettings_Credits_Layout("GameSettings_Credits", m_editorController);
            creditsLayout.Initialize();
            m_rightColumn.Add(creditsLayout.Container);
        }
    }
}
