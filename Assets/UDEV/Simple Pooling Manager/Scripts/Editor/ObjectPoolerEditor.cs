using UnityEditor;
using UnityEngine;
using System.Linq;
using System.Collections.Generic;

namespace UDEV.SPM {
    [CustomEditor(typeof(ObjectPooler))]
    public class ObjectPooler2Editor : Editor
    {
        SerializedProperty idProp;
        ObjectPooler pooler;
        SerializedProperty poolProp;
        SerializedProperty catProp;
        GUIStyle catFOstyle;
        GUIStyle style;
        GUIStyle mainCatStyle;
        string poolerDataPath;
        string poolerPath;
        string poolerSavedFileName;
        bool isUpdated;
        Dictionary<string, KeyValuePair<int, string>> poolerIds;

        public GUIStyle CatFOactiveStyle { get => CreateCatFOactiveStyle(); }
        public GUIStyle CatFOnormalStyle { get => CreateCatFOnormalStyle(); }
        public GUIStyle MainCatFOstyle { get => CreateMainCatFOstyle(); }

        public override void OnInspectorGUI()
        {
            pooler = (ObjectPooler)target;

            poolerSavedFileName = "Pooler_Data.dat";

            poolerDataPath = Constants.EDITOR_DATA_PATH + poolerSavedFileName;

            poolerPath = AssetDatabase.GetAssetPath(pooler);

            poolerIds = new Dictionary<string, KeyValuePair<int, string>>();

            idProp = serializedObject.FindProperty("id");

            catProp = serializedObject.FindProperty("categories");

            poolProp = serializedObject.FindProperty("pools");


            SerializedProperty poolerTargetProp = serializedObject.FindProperty("target");

            if(string.Compare(DataHolder.poolerId, pooler.id) != 0)
            {
                DataHolder.poolerId = pooler.id;
                DataHolder.mainCatFOstate = false;
                DataHolder.CatFOStatesInit();
                DataHolder.PoolerFOstatesInit();
            }

            if (isUpdated)
            {
                EditorGUILayout.HelpBox("Pooler is updated.", MessageType.Info);
            }

            EditorGUILayout.PropertyField(idProp, GUIContent.none);

            EditorGUILayout.BeginVertical(EditorStyles.helpBox);

            EditorGUILayout.BeginHorizontal();

            EditorGUILayout.LabelField("TARGET", EditorStyles.boldLabel);

            EditorGUILayout.PropertyField(poolerTargetProp, GUIContent.none);

            if (GUILayout.Button("Update")){
                SavePoolerData();
                isUpdated = true;
            }

            EditorGUILayout.EndHorizontal();

            EditorGUILayout.EndVertical();

            EditorGUILayout.Space();

            EditorGUILayout.BeginVertical(EditorStyles.helpBox);

            DataHolder.mainCatFOstate = EditorGUILayout.BeginFoldoutHeaderGroup(DataHolder.mainCatFOstate, "CATEGORIES", MainCatFOstyle);

            if (catProp != null && catProp.arraySize > 0)
            {
                if (!DataHolder.catFOstatesInited)
                {
                    DataHolder.catFOstates = new bool[catProp.arraySize];

                    DataHolder.CatFOStatesInit();

                    DataHolder.catFOstatesInited = true;
                }

                System.Array.Resize(ref DataHolder.catFOstates, catProp.arraySize);

                if (DataHolder.mainCatFOstate)
                {
                    for (int i = 0; i < catProp.arraySize; i++)
                    {
                        var cat = catProp.GetArrayElementAtIndex(i);

                        if (cat == null) continue;

                        style = new GUIStyle();
                        style.normal.background = EditorGUIUtility.whiteTexture;
                        style.padding = new RectOffset(0, 0, 10, 0);

                        var name = cat.FindPropertyRelative("name");
                        var id = cat.FindPropertyRelative("id");

                        EditorGUILayout.BeginHorizontal(style);
                        EditorGUILayout.BeginVertical();
                        EditorGUILayout.PropertyField(name, new GUIContent("Name"));
                        EditorGUILayout.PropertyField(id, GUIContent.none, true);
                        EditorGUILayout.EndVertical();

                        EditorGUILayout.BeginHorizontal(GUILayout.Width(25));
                        if (GUILayout.Button(" X "))
                        {
                            DataHolder.catDeletedIdx = i;

                            DataHolder.catFOstates[i] = false;

                            var curCat = catProp.GetArrayElementAtIndex(DataHolder.catDeletedIdx);

                            var curCatId = curCat.FindPropertyRelative("id");

                            var curCatName = curCat.FindPropertyRelative("name");

                            var catName = string.IsNullOrEmpty(curCatName.stringValue) ? i.ToString() : curCatName.stringValue;

                            if (EditorUtility.DisplayDialog("Delete Category", "All poolers inside " + catName + " category will be lost if you delete it.Are you sure want to do this?", "Delete", "Cancel"))
                            {
                                var poolFindeds = pooler.pools.
                                Where(p => string.Compare(curCatId.stringValue, p.category) == 0).ToArray();

                                int poolDeleted = pooler.pools.
                                RemoveAll(p => string.Compare(curCatId.stringValue, p.category) == 0);

                                if (poolDeleted == poolFindeds.Length)
                                {
                                    pooler.categories.RemoveAt(i);
                                }

                                EditorUtility.SetDirty(target);
                            }

                            GUIUtility.ExitGUI();
                        }
                        EditorGUILayout.EndHorizontal();
                        EditorGUILayout.EndHorizontal();
                    }
                }
            }

            if (GUILayout.Button("Add New Category"))
            {
                DataHolder.mainCatFOstate = true;
                pooler.categories.Add(new PoolerCategory("new_" + catProp.arraySize, ""));
                System.Array.Resize(ref DataHolder.catFOstates, catProp.arraySize + 1);
            }

            EditorGUILayout.EndFoldoutHeaderGroup();

            EditorGUILayout.EndVertical();

            EditorGUILayout.BeginVertical(EditorStyles.helpBox);

            EditorGUILayout.LabelField("POOLS: ", EditorStyles.boldLabel);

            if (catProp != null && catProp.arraySize > 0)
            {
                for (int x = 0; x < catProp.arraySize; x++)
                {
                    var catId = catProp.GetArrayElementAtIndex(x).FindPropertyRelative("id");
                    var name = catProp.GetArrayElementAtIndex(x).FindPropertyRelative("name");

                    string catFoldOutStatus = name.stringValue + "(0)";

                    if (poolProp != null && pooler.pools != null && pooler.pools.Count > 0)
                    {
                        var poolsPerCat = pooler.pools.
                            Where(p => string.Compare(catId.stringValue, p.category) == 0).ToArray();

                        catFoldOutStatus = name.stringValue + " (" + poolsPerCat.Length + ")";
                    }

                    style = DataHolder.catFOstates[x] ? CatFOactiveStyle : CatFOnormalStyle;

                    DataHolder.catFOstates[x] = EditorGUILayout.BeginFoldoutHeaderGroup(DataHolder.catFOstates[x], catFoldOutStatus, style);

                    if (DataHolder.catFOstates[x])
                    {
                        for (int k = 0; k < DataHolder.catFOstates.Length; k++)
                        {
                            if (k != x)
                            {
                                DataHolder.catFOstates[k] = false;
                            }
                        }

                        style = new GUIStyle();
                        style.normal.background = EditorGUIUtility.whiteTexture;

                        EditorGUILayout.BeginVertical(style);

                        if (poolProp != null && poolProp.arraySize > 0)
                        {
                            if (!DataHolder.poolerFOstatesInited)
                            {
                                DataHolder.poolerFOstates = new bool[poolProp.arraySize];

                                DataHolder.PoolerFOstatesInit();

                                DataHolder.poolerFOstatesInited = true;
                            }

                            System.Array.Resize(ref DataHolder.poolerFOstates, poolProp.arraySize);

                            System.Array.Resize(ref DataHolder.catFOstates, catProp.arraySize);

                            for (int i = 0; i < poolProp.arraySize; i++)
                            {
                                var pool = poolProp.GetArrayElementAtIndex(i);

                                if (pool == null) continue;

                                var poolName = pool.FindPropertyRelative("poolName");
                                var id = pool.FindPropertyRelative("id");
                                var pooledObjects = pool.FindPropertyRelative("pooledObjects");
                                var prefab = pool.FindPropertyRelative("prefab");
                                var startingParent = pool.FindPropertyRelative("startingParent");
                                var startingQuantity = pool.FindPropertyRelative("startingQuantity");
                                var category = pool.FindPropertyRelative("category");

                                if (string.Compare(catId.stringValue, category.stringValue) != 0 
                                    && !string.IsNullOrEmpty(category.stringValue)) continue;

                                style = new GUIStyle();
                                style.margin = new RectOffset(10, 0, 0, 0);

                                EditorGUILayout.BeginHorizontal(style);
                                string poolFoldOutStatus = string.IsNullOrEmpty(poolName.stringValue) ? "Element " + i : poolName.stringValue;
                                DataHolder.poolerFOstates[i] = EditorGUILayout.Foldout(DataHolder.poolerFOstates[i], poolFoldOutStatus, true);
                                EditorGUILayout.BeginHorizontal(GUILayout.Width(25));
                                if (GUILayout.Button(" X ", GUILayout.ExpandHeight(false)))
                                {
                                    DataHolder.poolerFOstates[i] = false;
                                    pooler.pools.RemoveAt(i);
                                }
                                EditorGUILayout.EndHorizontal();
                                EditorGUILayout.EndHorizontal();
                                if (DataHolder.poolerFOstates[i])
                                {
                                    EditorGUILayout.BeginHorizontal(EditorStyles.helpBox);
                                    EditorGUILayout.BeginVertical();
                                    EditorGUILayout.PropertyField(poolName, new GUIContent("Name"));
                                    EditorGUILayout.PropertyField(prefab, new GUIContent("Prefab"));
                                    EditorGUILayout.PropertyField(startingParent, new GUIContent("Starting Parent"));
                                    EditorGUILayout.PropertyField(startingQuantity, new GUIContent("Starting Quantity"));
                                    EditorGUILayout.PropertyField(category, new GUIContent("Category"));
                                    EditorGUILayout.PropertyField(id, GUIContent.none);
                                    EditorGUILayout.EndVertical();
                                    EditorGUILayout.EndHorizontal();
                                }

                            }
                        }

                        EditorGUILayout.EndVertical();

                        if (GUILayout.Button("Add New Pool"))
                        {
                            if (pooler.categories != null && pooler.categories.Count > 0)
                            {
                                pooler.pools.Add(new Pool("", null, null, 0, pooler.categories[x].id));
                                System.Array.Resize(ref DataHolder.poolerFOstates, poolProp.arraySize + 1);
                                DataHolder.poolerFOstates[DataHolder.poolerFOstates.Length - 1] = true;
                            }
                            else
                            {
                                EditorUtility.DisplayDialog("Alert Message", "You must add a Category first!.", "Ok");
                            }
                        }

                    }

                    EditorGUILayout.EndFoldoutHeaderGroup();

                    EditorGUILayout.Space();
                }
            }

            EditorGUILayout.EndVertical();

            serializedObject.ApplyModifiedProperties();

            serializedObject.Update();

            if (GUI.changed)
            {
                SavePoolerData();

                EditorUtility.SetDirty(target);
            }
        }

        GUIStyle CreateCatFOactiveStyle()
        {
            catFOstyle = new GUIStyle(EditorStyles.foldout);
            Color myStyleColor = Color.blue;
            catFOstyle.fontStyle = FontStyle.Bold;
            catFOstyle.normal.textColor = myStyleColor;
            catFOstyle.onNormal.textColor = myStyleColor;
            catFOstyle.hover.textColor = myStyleColor;
            catFOstyle.onHover.textColor = myStyleColor;
            catFOstyle.focused.textColor = myStyleColor;
            catFOstyle.onFocused.textColor = myStyleColor;
            catFOstyle.active.textColor = myStyleColor;
            catFOstyle.onActive.textColor = myStyleColor;
            catFOstyle.margin = new RectOffset(15, 5, 0, 5);
            return catFOstyle;
        }

        GUIStyle CreateCatFOnormalStyle()
        {
            catFOstyle = new GUIStyle(EditorStyles.foldout);
            Color myStyleColor = Color.black;
            catFOstyle.fontStyle = FontStyle.Bold;
            catFOstyle.normal.textColor = myStyleColor;
            catFOstyle.onNormal.textColor = myStyleColor;
            catFOstyle.hover.textColor = myStyleColor;
            catFOstyle.onHover.textColor = myStyleColor;
            catFOstyle.focused.textColor = myStyleColor;
            catFOstyle.onFocused.textColor = myStyleColor;
            catFOstyle.active.textColor = myStyleColor;
            catFOstyle.onActive.textColor = myStyleColor;
            catFOstyle.margin = new RectOffset(15, 5, 0, 5);
            return catFOstyle;
        }

        GUIStyle CreateMainCatFOstyle()
        {
            catFOstyle = new GUIStyle(EditorStyles.foldout);
            GUILayout.Width(40);
            Color myStyleColor = Color.black;
            catFOstyle.fontStyle = FontStyle.Bold;
            catFOstyle.normal.textColor = myStyleColor;
            catFOstyle.onNormal.textColor = myStyleColor;
            catFOstyle.hover.textColor = myStyleColor;
            catFOstyle.onHover.textColor = myStyleColor;
            catFOstyle.focused.textColor = myStyleColor;
            catFOstyle.onFocused.textColor = myStyleColor;
            catFOstyle.active.textColor = myStyleColor;
            catFOstyle.onActive.textColor = myStyleColor;
            catFOstyle.margin = new RectOffset(15, 5, 0, 5);
            return catFOstyle;
        }

        void SavePoolerData()
        {
            poolerIds = Utils.LoadDataFromFile(poolerDataPath, poolerIds);

            var poolerId = new KeyValuePair<int, string>((int)pooler.target, poolerPath);

            if(poolerIds == null)
            {
                poolerIds = new Dictionary<string, KeyValuePair<int, string>>();
            }

            poolerIds[pooler.id] = poolerId;

            var poolerIdsValues = poolerIds.Values.ToArray();
            var poolerIdsKeys = poolerIds.Keys.ToArray();

            for (int i = 0; i < poolerIdsValues.Length; i++)
            {
                if (!Utils.IsFileExist(poolerIdsValues[i].Value))
                    poolerIds.Remove(poolerIdsKeys[i]);
            }

            Utils.SaveDataToFile(Constants.EDITOR_DATA_PATH, poolerSavedFileName, poolerIds);
        }
    }


    internal static class DataHolder
    {
        public static string poolerId;
        public static bool poolerFOstatesInited;
        public static bool[] poolerFOstates;
        public static bool mainCatFOstate;
        public static bool catFOstatesInited;
        public static bool[] catFOstates;

        public static int catDeletedIdx;

        public static void CatFOStatesInit()
        {
            if(catFOstates != null && catFOstates.Length > 0)
            {
                for (int i = 0; i < catFOstates.Length; i++)
                {
                    catFOstates[i] = false;
                }
            }
        }

        public static void PoolerFOstatesInit()
        {
            if(poolerFOstates != null && poolerFOstates.Length > 0)
            {
                for (int h = 0; h < poolerFOstates.Length; h++)
                {
                    poolerFOstates[h] = false;
                }
            }
        }
    }
}