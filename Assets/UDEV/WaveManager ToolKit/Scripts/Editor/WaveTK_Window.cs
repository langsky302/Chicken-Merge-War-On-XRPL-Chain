using UnityEditor;
using UnityEditor.Callbacks;
using UnityEngine;
using UnityEngine.UIElements;

namespace UDEV.WaveManagerToolkit.Editor
{
    public class WaveTK_Window : EditorWindow
    {
        private static EditorWindow m_window;

        private static WaveTK_WaveController m_waveCtr;
        private WaveTK_EditorController m_editorController;

        private WaveTK_WaveList_Layout m_waveListLayout;
        private WaveTK_WaveCtrSettings_Layout m_waveCtrSettingsLayout;

        private VisualElement m_containerUI;
        private VisualElement m_leftContentUI;
        private VisualElement m_waveCtrSettingUI;

        private int m_waveCtrId;

        [MenuItem("Assets/WaveManager ToolKit")]
        public static void ShowWMNWindow()
        {
            if (m_window)
            {
                m_window.Close();
            }

            m_window = GetWindow<WaveTK_Window>();
            m_window.titleContent = new GUIContent("WaveManager ToolKit");
        }


        private void OnSelectionChange()
        {
            var objSelected = Selection.activeObject;
            if (objSelected != null && objSelected is GameObject)
            {
                var obj = (GameObject)objSelected;
                m_waveCtr = obj.GetComponent<WaveTK_WaveController>();
            }
        }

        private void CreateGUI()
        {
            OnSelectionChange();

            if (m_waveCtr != null)
            {
                m_containerUI = rootVisualElement;
                VisualTreeAsset visualTree = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>
                    (WaveTK_Const.UXML_LAYOUT_PATH + "WaveTK_Window.uxml");
                m_containerUI.Add(visualTree.Instantiate());

                StyleSheet styleSheet = AssetDatabase.LoadAssetAtPath<StyleSheet>
                    (WaveTK_Const.USS_LAYOUT_PATH + "WaveTK_Window.uss");
                m_containerUI.styleSheets.Add(styleSheet);

                m_editorController = new WaveTK_EditorController(m_waveCtr, m_containerUI);
                m_waveListLayout = new WaveTK_WaveList_Layout("WaveTK_WaveList", m_editorController);
                m_waveListLayout.Initialize();

                m_waveCtrSettingsLayout = new WaveTK_WaveCtrSettings_Layout("Wave_TK_WaveControllerSettings", m_editorController);
                m_waveCtrSettingsLayout.SetWaveCtrSerializedObj(new SerializedObject(m_waveCtr));
                m_waveCtrSettingsLayout.Initialize();

                m_editorController.DeleteNullWave();

                WaveTK_Utils.holdingAssetPath = WaveTK_Utils.GetAssetPath(m_waveCtr);
                m_waveCtrId = m_editorController.GetIdFromFilename(m_waveCtr.name, "WaveController");

                m_containerUI.Q<Label>("WindowTitle").text = $"Wave Controller [{m_waveCtrId}] Config";
                m_leftContentUI = m_containerUI.Q<VisualElement>("LeftContent");
                m_waveCtrSettingUI = m_containerUI.Q<VisualElement>("WaveCtrSettings");
                m_leftContentUI.Add(m_waveListLayout.Container);
                m_waveCtrSettingUI.Add(m_waveCtrSettingsLayout.Container);
 
            }
            else
            {
                Debug.LogError("[WMN_ToolKit] Please select a Wave Controller !!!");
            }
        }

        [OnOpenAsset(1)]
        public static bool OnOpenAsset(int instanceID, int line)
        {
            if (EditorUtility.InstanceIDToObject(instanceID) is GameObject selectedObj)
            {
                var waveManager = selectedObj.GetComponent<WaveTK_WaveController>();
                if (waveManager == null) return false;
                ShowWMNWindow();
                return true;
            }

            return false;
        }
    }
}
