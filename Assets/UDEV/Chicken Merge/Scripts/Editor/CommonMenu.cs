using UnityEditor;
using UnityEngine;

namespace UDEV.ChickenMerge
{
    public static class CommonMenu
    {
        public const string DOCUMENT_PATH = "/UDEV/Chicken Merge/Document/Document.pdf";

        [MenuItem("Chicken Merge/Game Document", priority = 0)]
        public static void GotoGameDocument()
        {
            Application.OpenURL(Application.dataPath + DOCUMENT_PATH);
        }

        [MenuItem("Chicken Merge/Clear Game Data", priority = 1)]
        public static void ClearGameData()
        {
            PlayerPrefs.DeleteAll();
        }
    }
}
