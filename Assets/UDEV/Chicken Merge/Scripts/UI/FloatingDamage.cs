using UnityEngine;
using TMPro;

namespace UDEV.ChickenMerge
{
    public class FloatingDamage : MonoBehaviour
    {
        [SerializeField]
        private TextMeshProUGUI m_damageTxt;

        public void UpdateDamageTxt(float damage)
        {
            if (m_damageTxt != null)
            {
                m_damageTxt.text = $"-{damage.ToString("f1")}";
            }
        }
    }
}
