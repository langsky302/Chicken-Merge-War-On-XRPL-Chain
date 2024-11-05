using UnityEditor;
using UnityEngine;

namespace UDEV
{
    [CustomPropertyDrawer(typeof(TagListAttribute), true)]
    public class TagListDrawer : PropertyDrawer
    {

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            property.stringValue = EditorGUI.TagField(position, label, property.stringValue);
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return base.GetPropertyHeight(property, label);
        }
    }
}
