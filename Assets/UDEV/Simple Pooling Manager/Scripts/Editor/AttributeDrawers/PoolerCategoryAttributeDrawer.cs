using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace UDEV.SPM
{
    [CustomPropertyDrawer(typeof(PoolerCategoryAttribute))]
    public class PoolerCategoryAttributeDrawer : PropertyDrawer
    {
        int _choiceIndex;
        List<PoolerCategory> categories;

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            categories = new List<PoolerCategory>();

            ObjectPooler beh = property.serializedObject.targetObject as ObjectPooler;

            if (beh)
            {
                categories = beh.categories;

                if (categories != null && categories.Count > 0)
                {
                    string[] catIds = new string[categories.Count];
                    string[] catNames = new string[categories.Count];

                    for (int i = 0; i < catIds.Length; i++)
                    {
                        if (categories[i] == null) continue;
                        catIds[i] = categories[i].id;
                    }

                    for (int i = 0; i < catIds.Length; i++)
                    {
                        if (categories[i] == null) continue;
                        catNames[i] = categories[i].name;
                    }

                    _choiceIndex = Array.IndexOf(catIds, property.stringValue);

                    if (_choiceIndex < 0)
                    {
                        _choiceIndex = 0;
                        property.stringValue = catIds[_choiceIndex];
                    }

                    _choiceIndex = EditorGUI.Popup(position, label.text, _choiceIndex, catNames);

                    property.stringValue = catIds[_choiceIndex];
                }
            }

            if (beh == null || categories == null || categories.Count <= 0)
            {
                EditorGUI.BeginProperty(position, label, property);
                property.stringValue = EditorGUI.TextField(position, label.text, property.stringValue);
                EditorGUI.EndProperty();
            }
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return base.GetPropertyHeight(property, label);
        }
    }
}
