using UnityEditor;
using UnityEngine.UIElements;

namespace UDEV.GameSettings.Editor
{
    public abstract class GameSettings_LayoutBuilder
    {
        protected GameSettings_EditorController m_editorController;
        protected TemplateContainer m_container;
        public TemplateContainer Container { get => m_container; }

        protected GameSettings_LayoutBuilder(string templateName, GameSettings_EditorController editorController = null)
        {
            m_editorController = editorController;
            VisualTreeAsset visualTree = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>
                    (GameSettings_Consts.UXML_LAYOUT_PATH + templateName + ".uxml");

            m_container = visualTree.Instantiate();
        }

        public virtual void Initialize()
        {
            GetUIProperties();
            BuildLayout();
        }

        public abstract void GetUIProperties();

        protected abstract void BuildLayout();

        public virtual void SetProperty(SerializedProperty property)
        {

        }
    }
}
