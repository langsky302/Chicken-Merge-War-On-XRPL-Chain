using UDEV.ChickenMerge;
using UDEV.UI.Editor;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace UDEV.GameSettings.Editor
{
    public class GameSettings_InApp_Layout : GameSettings_LayoutBuilder
    {
        private Inspector m_inspector;
        private Button m_addIAPDSBtn;
        private Button m_removeIAPDSBtn;

        public GameSettings_InApp_Layout(string templateName, GameSettings_EditorController editorController = null) : base(templateName, editorController)
        {
        }

        public override void GetUIProperties()
        {
            m_inspector = m_container.Q<Inspector>("in-app-settings-inspector");
            m_addIAPDSBtn = m_container.Q<Button>("add-iap-ds-btn");
            m_removeIAPDSBtn = m_container.Q<Button>("remove-iap-ds-btn");
        }

        protected override void BuildLayout()
        {
            var iapData = Resources.Load<IAPSO>(GameSettings_Consts.IAP_CONFIG_PATH);

            m_inspector.Bind(new SerializedObject(iapData));

            bool isIAPDSExist = ScriptingDefineSymbolUtils.HasDefineSymbol(GameSettings_Consts.IAP_DEFINE_SCRIPTING_SYMBOL);
            ShowAddIAPDSButton(!isIAPDSExist);

            m_addIAPDSBtn.RegisterCallback<ClickEvent>((clickEvt) =>
            {
                ScriptingDefineSymbolUtils.AddScriptingDefineSymbol(GameSettings_Consts.IAP_DEFINE_SCRIPTING_SYMBOL);
                ShowAddIAPDSButton(false);
            });

            m_removeIAPDSBtn.RegisterCallback<ClickEvent>(clickEvt =>
            {
                ScriptingDefineSymbolUtils.RemoveScriptingDefineSymbol(GameSettings_Consts.IAP_DEFINE_SCRIPTING_SYMBOL);
                ShowAddIAPDSButton(true);
            });
        }

        private void ShowAddIAPDSButton(bool isShow)
        {
            GameSettings_UI_Utils.ShowElement(m_addIAPDSBtn, isShow);
            GameSettings_UI_Utils.ShowElement(m_removeIAPDSBtn, !isShow);
        }
    }
}
