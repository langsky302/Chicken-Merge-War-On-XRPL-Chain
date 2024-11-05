using UnityEngine;
using UnityEngine.UI;

namespace UDEV.ChickenMerge
{
    public class ScrollView_Fix : MonoBehaviour
    {
        private ScrollRect m_scrollRect;
        private float m_waitingTime = 0.25f;

        private void Awake()
        {
            m_scrollRect = GetComponent<ScrollRect>();
        }

        private void OnEnable()
        {
            m_waitingTime = 0.25f;
        }

        private void Update()
        {
            if (m_waitingTime <= 0) return;
            m_waitingTime -= Time.deltaTime;
            m_scrollRect?.StopMovement();
            ResetContentAchorPos();
        }

        private void ResetContentAchorPos()
        {
            if (m_scrollRect == null) return;
            m_scrollRect.content.anchoredPosition = new Vector2(0, 1f);
            LayoutRebuilder.ForceRebuildLayoutImmediate(m_scrollRect.GetComponent<RectTransform>());
        }
    }
}
