using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace UDEV.ChickenMerge
{
    public static class StatEditorUlti
    {
        public const string RESOURCE_PATH = "/UDEV/Chicken Merge/Editor/Resources/";
        public const string STATDATA_EDITOR_FOLDER = "StatData";
        public const string STATDATA_SO_FOLDER = "Data";
        private static string m_fileName;

        private static string SaveFilePath
        {
            get => Application.dataPath + RESOURCE_PATH + STATDATA_EDITOR_FOLDER;
        }
        public static string FileName { get => m_fileName; }

        public static string CreateFilePath(string id)
        {
            m_fileName = $"stat_data_{id}";
            return $"{SaveFilePath}/{m_fileName}.txt";
        }

        public static string GenerateFilepath(Stat stat)
        {
            stat.id = Helper.GenerateUID();
            return CreateFilePath(stat.id);
        }

        public static void Save(Stat stat)
        {
            string filePath = CreateFilePath(stat.id);
            if (IsDupplicateId(stat.id) || string.IsNullOrEmpty(stat.id))
            {
                filePath = GenerateFilepath(stat);
            }
            string thumbPath = "thumb:" + AssetDatabase.GetAssetPath(stat.thumb);
            File.WriteAllText(filePath, stat.ToJson());
            EditorUtility.SetDirty(stat);
            AssetDatabase.Refresh();

        }

        public static void Load(string filePath, Stat stat)
        {
            var data = Resources.Load<TextAsset>(filePath);
            if (data == null) return;
            if (!string.IsNullOrEmpty(data.ToString()))
            {
                var thumbAssetPath = AssetDatabase.GetAssetPath(stat.thumb);
                JsonUtility.FromJsonOverwrite(data.ToString(), stat);
                stat.thumb = (Sprite)AssetDatabase.LoadAssetAtPath(thumbAssetPath, typeof(Sprite));
                EditorUtility.SetDirty(stat);
            }
        }

        public static bool IsDupplicateId(string id)
        {
            var data = Resources.LoadAll<Stat>(STATDATA_SO_FOLDER);
            var finder = data.Where(d => string.Compare(d.id, id) == 0);
            if (finder == null) return false;

            var rs = finder.ToArray();
            if (rs == null || rs.Length == 0) return false;

            return rs.Length > 1;
        }
    }
}
