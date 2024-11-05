using UnityEngine;
using UnityEngine.UI;

namespace UDEV.ChickenMerge
{
    public class StarItemUI : MonoBehaviour
    {
        private Image m_image;
        [SerializeField] private Sprite m_activeStarSp;
        [SerializeField] private Sprite m_inactiveStarSp;

        private void Awake()
        {
            m_image = GetComponent<Image>();
        }

        public void ActiveStar(bool isActive)
        {
            if (m_image == null) return;
            m_image.sprite = isActive ? m_activeStarSp : m_inactiveStarSp;
        }
    }
}
