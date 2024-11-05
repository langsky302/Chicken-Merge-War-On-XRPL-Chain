using UDEV.ChickenMerge;
using UDEV.UI.Editor;
using UnityEditor.UIElements;
using UnityEditor;
using UnityEngine.UIElements;
using UnityEngine;

namespace UDEV.GameSettings.Editor
{
    public class GameSettings_Global_Layout : GameSettings_LayoutBuilder
    {
        private Inspector m_gameConfigInspector;
        private Inspector m_gameChestInspector;
        private Label m_linkDotweenLabel;
        private Button m_addDotweenDSBtn;
        private Button m_removeDotweenDSBtn;
        private Button m_removeAllGameDataBtn;
        public GameSettings_Global_Layout(string templateName, GameSettings_EditorController editorController = null) : base(templateName, editorController)
        {
        }

        public override void GetUIProperties()
        {
            m_gameConfigInspector = m_container.Q<Inspector>("global-settings-inspector");
            m_gameChestInspector = m_container.Q<Inspector>("game-chest-settings-inspector");
            m_linkDotweenLabel = m_container.Q<Label>("link-to-dotween");
            m_addDotweenDSBtn = m_container.Q<Button>("add-dotween-ds");
            m_removeDotweenDSBtn = m_container.Q<Button>("remove-dotween-ds");
            m_removeAllGameDataBtn = m_container.Q<Button>("clear-all-game-data-btn");
        }

        protected override void BuildLayout()
        {
            var gameConfig = Resources.Load<GameConfigSO>(GameSettings_Consts.GAME_CONFIG_PATH);
            var gameChest = Resources.Load<BundleRewardSO>(GameSettings_Consts.GAME_CHEST_CONFIG_PATH);

            m_gameConfigInspector.Bind(new SerializedObject(gameConfig));
            m_gameChestInspector.Bind(new SerializedObject(gameChest));

            string dotweenLink = "https://assetstore.unity.com/packages/tools/animation/dotween-hotween-v2-27676";
            m_linkDotweenLabel.text = $"Download DOTween <a href='{dotweenLink}' target='_blank'>Here</a> and Import it First !!!";
            m_linkDotweenLabel.RegisterCallback<ClickEvent>((clickEvnt) =>
            {
                Application.OpenURL(dotweenLink);
            });
            m_addDotweenDSBtn.RegisterCallback<ClickEvent>((clickEvnt) =>
            {
                ScriptingDefineSymbolUtils.AddScriptingDefineSymbol(GameSettings_Consts.DOTWEEN_DEFINE_SCRIPTING_SYMBOL);
                ShowAddDotweenDSButton(false);
            });

            m_removeDotweenDSBtn.RegisterCallback<ClickEvent>(clickEvnt =>
            {
                ScriptingDefineSymbolUtils.RemoveScriptingDefineSymbol(GameSettings_Consts.DOTWEEN_DEFINE_SCRIPTING_SYMBOL);
                ShowAddDotweenDSButton(true);
            });

            m_removeAllGameDataBtn.RegisterCallback<ClickEvent>((clickEvt) =>
            {
                PlayerPrefs.DeleteAll();
            });

            bool isDotweenDSExist = ScriptingDefineSymbolUtils.HasDefineSymbol(GameSettings_Consts.DOTWEEN_DEFINE_SCRIPTING_SYMBOL);
            ShowAddDotweenDSButton(!isDotweenDSExist);
        }

        private void ShowAddDotweenDSButton(bool isShow)
        {
            GameSettings_UI_Utils.ShowElement(m_addDotweenDSBtn, isShow);
            GameSettings_UI_Utils.ShowElement(m_removeDotweenDSBtn, !isShow);
        }
    }
}
