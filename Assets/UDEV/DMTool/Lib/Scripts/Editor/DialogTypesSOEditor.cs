using UnityEditor;

namespace UDEV.DMTool
{
    [CustomEditor(typeof(DialogTypesSO))]
    [CanEditMultipleObjects]
    public class DialogTypesSOEditor : BaseEditor
    {
        public SerializedProperty baseDialogs;
        private void OnEnable()
        {
            baseDialogs = serializedObject.FindProperty("dialogs");
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            serializedObject.Update();
            ShowArrayProperty(baseDialogs, typeof(DialogType), "Dialogs");
            serializedObject.ApplyModifiedProperties();
        }
    }
}