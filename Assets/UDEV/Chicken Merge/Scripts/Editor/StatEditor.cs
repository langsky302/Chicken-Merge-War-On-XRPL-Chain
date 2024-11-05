using UnityEditor;
using UnityEngine;
using System;
using UnityEngine.Events;

namespace UDEV.ChickenMerge
{
    [CanEditMultipleObjects]
    [CustomEditor(typeof(Stat), editorForChildClasses: true)]
    public class StatEditor : Editor
    {    
        protected Stat m_target;
        protected CSV_Ulti m_csvUlti;

        public override void OnInspectorGUI()
        {
            m_target = (Stat)target;
            m_target.thumb = (Sprite)EditorGUILayout.ObjectField(
                m_target.thumb, typeof(Sprite), false, GUILayout.Width(80), GUILayout.Height(80));
            base.OnInspectorGUI();

            StatEditorUlti.CreateFilePath(m_target.id);

            if (GUILayout.Button("Save"))
            {
                Save();
            }

            if (GUILayout.Button("Load"))
            {
                Load(StatEditorUlti.FileName);
            }

            if (GUILayout.Button("Upgrade"))
            {
                Upgrade();
            }

            if (GUILayout.Button("Upgrade To Max"))
            {
                UpgradeToMax();
            }

            if (GUILayout.Button("Save To CSV"))
            {
                SaveToCSV();
            }
        }

        protected void DrawButton(string name, Action OnClickMethod)
        {
            if (GUILayout.Button(name))
            {
                if (OnClickMethod != null)
                    OnClickMethod.Invoke();
            }
        }

        protected virtual void UpgradeToMax(UnityAction OnUpgrade = null)
        {
            m_target.UpgradeToMax(OnUpgrade);
        }

        protected virtual void Upgrade()
        {
            m_target.Upgrade();
        }

        protected virtual void Load(string fileName)
        {
            string filePath = $"{StatEditorUlti.STATDATA_EDITOR_FOLDER}/{fileName}";
            StatEditorUlti.Load(filePath, m_target);
        }

        protected virtual void Save()
        {
            StatEditorUlti.Save(m_target);
            
        }

        protected virtual void SaveToCSV()
        {

        }
    }
}
