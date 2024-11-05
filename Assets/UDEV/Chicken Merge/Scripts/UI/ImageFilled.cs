using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

namespace UDEV.ChickenMerge
{
    public class ImageFilled : MonoBehaviour
    {

        [SerializeField] protected Image m_filledImg;

        public UnityEvent OnComplete;

        protected float m_fillAmount;
        private Transform m_root;

        public Transform Root { get => m_root; set => m_root = value; }

        public virtual void UpdateValue(float progress, bool isReverse = false)
        {
            if (m_filledImg == null) return;

            m_fillAmount = 0;

            if (isReverse)
            {
                m_fillAmount = 1f - progress;
            }
            else
            {
                m_fillAmount = progress;
            }

#if USE_DOTWEEN
            TweenUltis.FillAmountAnim(m_filledImg, m_fillAmount, 0.1f);
#else
            m_filledImg.fillAmount = m_fillAmount;  
#endif
        }

        public void Show(bool isShow)
        {
            gameObject.SetActive(isShow);
        }

        protected virtual void Update()
        {
            if (m_root)
            {
                transform.localRotation = m_root.rotation;
            }
        }
    }
}
