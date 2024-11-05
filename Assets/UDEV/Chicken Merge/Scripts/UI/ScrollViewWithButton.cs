using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace UDEV.ChickenMerge
{
    public class ScrollViewWithButton : MonoBehaviour
    {
        [SerializeField] private ScrollButton m_leftBtn;
        [SerializeField] private ScrollButton m_rightBtn;
        [SerializeField] private ScrollButton m_upBtn;
        [SerializeField] private ScrollButton m_downBtn;
        [SerializeField] private float m_scrollSpeed;

        private ScrollRect m_scrollRect;

        private void Awake()
        {
            m_scrollRect = GetComponent<ScrollRect>();
        }

        private void Update()
        {
            if (m_leftBtn != null)
            {
                if (m_leftBtn.IsPointerDown) {
                    ScrollLeft();
                }
            }

            if (m_rightBtn != null)
            {
                if (m_rightBtn.IsPointerDown) {
                    ScrollRight();
                }
            }

            if (m_upBtn != null)
            {
                if (m_upBtn.IsPointerDown)
                {
                    ScrollUp();
                }
            }

            if (m_downBtn != null)
            {
                if (m_upBtn.IsPointerDown)
                {
                    ScrollDown();
                }
            }
        }

        private void ScrollLeft()
        {
            if (m_scrollRect == null) return;
            if (m_scrollRect.horizontalNormalizedPosition < 0) return;
            m_scrollRect.horizontalNormalizedPosition -= m_scrollSpeed;
        }

        private void ScrollRight()
        {
            if (m_scrollRect == null) return;

            if (m_scrollRect.horizontalNormalizedPosition > 1) return;
            m_scrollRect.horizontalNormalizedPosition += m_scrollSpeed;
        }

        private void ScrollUp() {
            if (m_scrollRect == null) return;

            if (m_scrollRect.horizontalNormalizedPosition > 1) return;
            m_scrollRect.verticalNormalizedPosition += m_scrollSpeed;
        }

        private void ScrollDown()
        {
            if (m_scrollRect == null) return;

            if (m_scrollRect.horizontalNormalizedPosition < 0) return;
            m_scrollRect.verticalNormalizedPosition -= m_scrollSpeed;
        }
    }
}
