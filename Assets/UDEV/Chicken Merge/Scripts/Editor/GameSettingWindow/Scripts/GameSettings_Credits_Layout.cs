using UnityEngine;
using UnityEngine.UIElements;

namespace UDEV.GameSettings.Editor
{
    public class GameSettings_Credits_Layout : GameSettings_LayoutBuilder
    {
        private Label m_websiteLink;
        private Label m_emailLink;
        private Label m_moreTemplatesLink;

        public GameSettings_Credits_Layout(string templateName, GameSettings_EditorController editorController = null) : base(templateName, editorController)
        {
        }

        public override void GetUIProperties()
        {
            m_websiteLink = m_container.Q<Label>("credits-website-link");
            m_emailLink = m_container.Q<Label>("credits-email-link");
            m_moreTemplatesLink = m_container.Q<Label>("credits-more-template-link");
        }

        protected override void BuildLayout()
        {
            m_websiteLink.RegisterCallback<ClickEvent>((clickEvt) => {
                Application.OpenURL("https://www.udevcorp.com");
            });

            m_moreTemplatesLink.RegisterCallback<ClickEvent>((clickEvt) =>
            {
                Application.OpenURL("https://assetstore.unity.com/publishers/42790");
            });
        }
    }
}
