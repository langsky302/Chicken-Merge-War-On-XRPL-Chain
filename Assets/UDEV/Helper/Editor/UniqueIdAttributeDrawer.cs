using UnityEditor;
using UnityEngine;
using System;

namespace UDEV
{
    [CustomPropertyDrawer(typeof(UniqueIdAttribute), true)]
    public class UniqueIdAttributeAttributeDrawer : PropertyDrawer
    {

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {

            if (String.IsNullOrEmpty(property.stringValue))
            {
                property.stringValue = Utils.GenerateUID();
            }
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return base.GetPropertyHeight(property, label);
        }

        
    }
}
