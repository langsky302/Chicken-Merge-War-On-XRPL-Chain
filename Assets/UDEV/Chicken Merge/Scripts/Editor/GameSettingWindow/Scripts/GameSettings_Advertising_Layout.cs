using UDEV.UI.Editor;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace UDEV.GameSettings.Editor
{
    public class GameSettings_Advertising_Layout : GameSettings_LayoutBuilder
    {
        private SerializedProperty m_adsTypeProp;

        private PropertyField m_adsTypePropUI;
        private Inspector m_inspector;
        private Label m_adsImportSettingsLabel;
        private Label m_adsImportSettingsLink;
        private Button m_addAdsDSBtn;
        private Button m_removeAdsDsBtn;

        private EventCallback<ClickEvent> m_addAdsDSCallback;
        private EventCallback<ClickEvent> m_removeAdsDSCallback;

        public GameSettings_Advertising_Layout(string templateName, GameSettings_EditorController editorController = null) : base(templateName, editorController)
        {
        }

        public override void GetUIProperties()
        {
            m_adsTypePropUI = m_container.Q<PropertyField>("ads-type-prop");
            m_inspector = m_container.Q<Inspector>("ads-settings-inspector");
            m_adsImportSettingsLabel = m_container.Q<Label>("ads-import-settings-label");
            m_adsImportSettingsLink = m_container.Q<Label>("ads-import-link");
            m_addAdsDSBtn = m_container.Q<Button>("add-ads-ds-btn");
            m_removeAdsDsBtn = m_container.Q<Button>("remove-ads-ds-btn");
        }

        protected override void BuildLayout()
        {
            var adsManager = Resources.Load<AdManager>(GameSettings_Consts.ADS_MANAGER_PATH);
            var adsManagerSerializedObj = new SerializedObject(adsManager);
            m_adsTypeProp = adsManagerSerializedObj.FindProperty("adsPlatform");
            m_adsTypePropUI.BindProperty(m_adsTypeProp);
            m_adsTypePropUI.RegisterValueChangeCallback((prop) =>
            {
                AdsPlatform adsPlatform = (AdsPlatform)prop.changedProperty.enumValueIndex;
                AdConfig adsConfig = null;
                if (adsPlatform == AdsPlatform.Admob)
                {
                    adsConfig = Resources.Load<AdmobConfig>(GameSettings_Consts.ADMOB_CONFIG_PATH);
                    BuildAdmobSettingsLayout();
                }
                else if(adsPlatform == AdsPlatform.UnityAds)
                {
                    adsConfig = Resources.Load<UnityAdConfig>(GameSettings_Consts.UINITY_ADS_CONFIG_PATH);
                    BuildUnityAdsSettingsLayout();
                }

                m_inspector.Bind(new SerializedObject(adsConfig));
            });
        }

        private void BuildAdmobSettingsLayout()
        {
            string googleAdsDownloadLink = "https://github.com/googleads/googleads-mobile-unity/releases";
            m_adsImportSettingsLabel.text = "Google Mobile Ads Import Setting :";
            m_adsImportSettingsLink.text = $"Download Google Mobile Ads <a href= '{googleAdsDownloadLink}'>Here</a> and Import it First !!!";
            m_adsImportSettingsLink.RegisterCallback<ClickEvent>((prop) =>
            {
                Application.OpenURL(googleAdsDownloadLink);
            });

            m_addAdsDSBtn.UnregisterCallback(m_addAdsDSCallback);
            m_removeAdsDsBtn.UnregisterCallback(m_removeAdsDSCallback);

            m_addAdsDSCallback = (clickEvt) => AddAdsDSCallback(GameSettings_Consts.ADMOB_DEFINE_SCRIPTING_SYMBOL);
            m_removeAdsDSCallback = (clickEvt) => RemoveAdsDSCallback(GameSettings_Consts.ADMOB_DEFINE_SCRIPTING_SYMBOL);

            m_addAdsDSBtn.RegisterCallback(m_addAdsDSCallback);
            m_removeAdsDsBtn.RegisterCallback(m_removeAdsDSCallback);

            bool isAdmobDSExist = ScriptingDefineSymbolUtils.HasDefineSymbol(GameSettings_Consts.ADMOB_DEFINE_SCRIPTING_SYMBOL);
            ShowAddAdsDSButton(!isAdmobDSExist);
        }

        private void BuildUnityAdsSettingsLayout()
        {
            m_adsImportSettingsLabel.text = "Unity Ads Import Setting :";
            m_adsImportSettingsLink.text = "Please Setup Unity Ads First !!!";

            m_addAdsDSBtn.UnregisterCallback(m_addAdsDSCallback);
            m_removeAdsDsBtn.UnregisterCallback(m_removeAdsDSCallback);

            m_addAdsDSCallback = (clickEvt) => AddAdsDSCallback(GameSettings_Consts.UNITY_ADS_DEFINE_SCRIPTING_SYMBOL);
            m_removeAdsDSCallback = (clickEvt) => RemoveAdsDSCallback(GameSettings_Consts.UNITY_ADS_DEFINE_SCRIPTING_SYMBOL);

            m_addAdsDSBtn.RegisterCallback(m_addAdsDSCallback);
            m_removeAdsDsBtn.RegisterCallback(m_removeAdsDSCallback);

            bool isUnityAdsDSExist = ScriptingDefineSymbolUtils.HasDefineSymbol(GameSettings_Consts.UNITY_ADS_DEFINE_SCRIPTING_SYMBOL);
            ShowAddAdsDSButton(!isUnityAdsDSExist);
        }

        private void AddAdsDSCallback(string symbol)
        {
            ScriptingDefineSymbolUtils.AddScriptingDefineSymbol(symbol);
            ShowAddAdsDSButton(false);
        }

        private void RemoveAdsDSCallback(string symbol)
        {
            ScriptingDefineSymbolUtils.RemoveScriptingDefineSymbol(symbol);
            ShowAddAdsDSButton(true);
        }

        private void ShowAddAdsDSButton(bool isShow)
        {
            GameSettings_UI_Utils.ShowElement(m_addAdsDSBtn, isShow);
            GameSettings_UI_Utils.ShowElement(m_removeAdsDsBtn, !isShow);
        }
    }
}
