using UnityEditor;
using UnityEngine.UIElements;

namespace UDEV.WaveManagerToolkit.Editor
{
    public class WaveTK_WaveItem_Layout : WaveTK_LayoutBuilder
    {
        public Label waveLabel;
        public Button deleteWaveBtn;

        public WaveTK_WaveItem_Layout(string templateName, WaveTK_EditorController editorController = null) : base(templateName, editorController)
        {
        }

        protected override void BuildLayout()
        {
            
        }

        public override void GetUIProperties()
        {
            waveLabel = m_container.Q<Label>("WaveItemLabel");
            deleteWaveBtn = m_container.Q<Button>("DeleteWaveBtn");
        }
    }
}
