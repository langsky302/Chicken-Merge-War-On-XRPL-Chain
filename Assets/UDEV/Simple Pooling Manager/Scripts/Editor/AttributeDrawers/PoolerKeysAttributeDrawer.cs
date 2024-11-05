using UnityEditor;
using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

namespace UDEV.SPM
{
    [CustomPropertyDrawer(typeof(PoolerKeysAttribute))]
    public class PoolerKeysAttributeDrawer : PropertyDrawer
    {
        int _choiceIndex = 0;
        Dictionary<string, KeyValuePair<int, string>> poolerData;
        Dictionary<string, string> mainPoolIds;
        PoolerKeysAttribute poolerKeysAttribute;
        bool isAssetMissing;

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            poolerKeysAttribute = (PoolerKeysAttribute)attribute;

            LoadPoolerData();

            mainPoolIds = new Dictionary<string, string>();

            if (poolerData != null && poolerData.Count > 0)
            {
                var poolerIdsPaths = poolerData.Values.ToList().
                    Where( t=> t.Key == (int) poolerKeysAttribute.target).ToArray();
                
                if (poolerIdsPaths != null && poolerIdsPaths.Length > 0)
                {
                    for (int i = 0; i < poolerIdsPaths.Length; i++)
                    {
                        if (Utils.IsFileExist(poolerIdsPaths[i].Value))
                        {
                            isAssetMissing = false;

                            ObjectPooler poolerObj = (ObjectPooler)AssetDatabase.
                                LoadAssetAtPath(poolerIdsPaths[i].Value, typeof(ObjectPooler));

                            var poolIds = poolerObj.GetPoolIds();

                            if (poolIds == null || poolIds.Count <= 0) continue;
                            var poolIdKeys = poolIds.Keys.ToArray();
                            var poolIdValues = poolIds.Values.ToArray();

                            for (int j = 0; j < poolIds.Count; j++)
                            {
                                mainPoolIds[poolIdKeys[j]] = poolIdValues[j];
                            }
                        }
                        else
                        {
                            isAssetMissing = true;
                        }
                    }
                    if (mainPoolIds != null && mainPoolIds.Count > 0)
                    {
                        var keyList = mainPoolIds.Keys.ToList();
                        keyList.Insert(0, "None");
                        var keyArray = keyList.ToArray();

                        var values = mainPoolIds.Values.ToList();
                        values.Insert(0, "None");
                        var valueArray = values.ToArray();

                        _choiceIndex = Array.IndexOf(keyArray, property.stringValue);

                        if (_choiceIndex < 0)
                        {
                            _choiceIndex = 0;
                            property.stringValue = keyArray[_choiceIndex];
                        }

                        _choiceIndex = EditorGUI.Popup(position, label.text, _choiceIndex, valueArray);

                        property.stringValue = keyArray[_choiceIndex];
                    }
                    else
                    {
                        if (isAssetMissing)
                        {
                            EditorGUI.HelpBox(position, "Pooler for " + poolerKeysAttribute.target + " does not exist!.", MessageType.Error);
                        }
                        else
                        {
                            EditorGUI.HelpBox(position, "You don't have any pool for " + poolerKeysAttribute.target + "!.", MessageType.Warning);
                        }
                    }
                }
                else
                {
                    EditorGUI.HelpBox(position, "Pooler for " + poolerKeysAttribute.target + " does not exist!.", MessageType.Error);
                }
            }
            else
            {
                EditorGUI.HelpBox(position, "You don't have any pooler for " + poolerKeysAttribute.target + "!.Let's create one pooler.", MessageType.Warning);
            }

        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return base.GetPropertyHeight(property, label);
        }

        void LoadPoolerData()
        {
            poolerData = new Dictionary<string, KeyValuePair<int, string>>();

            string filePath = Constants.EDITOR_DATA_PATH + "Pooler_Data.dat";

            poolerData = Utils.LoadDataFromFile(filePath, poolerData);
        }
    }
}
