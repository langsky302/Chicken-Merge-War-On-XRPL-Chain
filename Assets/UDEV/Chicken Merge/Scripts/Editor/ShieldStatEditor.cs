using UnityEngine;
using UnityEditor;

namespace UDEV.ChickenMerge
{
    [CustomEditor(typeof(ShieldStatSO))]
    public class ShieldStatEditor : StatEditor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            if (GUILayout.Button("Repair"))
            {
                var shieldStat = (ShieldStatSO)m_target;

                shieldStat?.Repair();
            }
        }
    }
}
