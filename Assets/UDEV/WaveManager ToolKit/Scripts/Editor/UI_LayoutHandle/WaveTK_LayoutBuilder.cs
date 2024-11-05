using UnityEditor;
using UnityEngine.UIElements;

namespace UDEV.WaveManagerToolkit.Editor
{
    public abstract class WaveTK_LayoutBuilder
    {
        protected WaveTK_EditorController m_editorController;
        protected TemplateContainer m_container;
        public TemplateContainer Container { get => m_container;}

        protected WaveTK_LayoutBuilder(string templateName, WaveTK_EditorController editorController = null)
        {
            m_editorController = editorController;
            VisualTreeAsset visualTree = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>
                    (WaveTK_Const.UXML_LAYOUT_PATH + templateName + ".uxml");

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
