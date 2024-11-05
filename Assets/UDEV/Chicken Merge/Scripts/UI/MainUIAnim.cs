using UnityEngine;
#if USE_DOTWEEN
using DG.Tweening;
#endif

namespace UDEV.ChickenMerge
{
    public class MainUIAnim : MonoBehaviour
    {
        [SerializeField] private RectTransform m_logo;
        [SerializeField] private Transform m_playBtn;
        [SerializeField] private Transform[] m_midBtns;
        [SerializeField] private RectTransform m_coinCounting;

#if USE_DOTWEEN
        private void Awake()
        {
            UpdateUI();
        }

        private void UpdateUI()
        {
            var squence = DOTween.Sequence();

            squence.Append(AnimRect(m_logo, new Vector2(0, 1500f), 0.5f));
            squence.Append(AnimPlayBtn());
            squence.Append(AnimMidBtns());
            squence.Join(AnimRect(m_coinCounting, new Vector2(-500f, 0f), 0.3f));
            squence.OnComplete(() =>
            {
                StartCoroutine(TweenUltis.ScaleBlinkLoop(m_playBtn, 1f, 1.1f, 0.3f, 0.1f));
            });
        }

        private Sequence AnimRect(RectTransform rect, Vector2 animPos, float time)
        {
            var squence = DOTween.Sequence();
            if (rect == null) return squence;

            var orignalPos = rect.anchoredPosition;
            var animTargetPos = orignalPos + animPos;
            rect.anchoredPosition = animTargetPos;
            var logoTween = rect.DOAnchorPos(orignalPos, time);
            squence.Append(logoTween);
            return squence;
        }

        private Sequence AnimPlayBtn()
        {
            var squence = DOTween.Sequence();
            if (m_playBtn == null) return squence;

            m_playBtn.localScale = Vector3.zero;
            var playBtnTween = m_playBtn.DOScale(1f, 0.2f);
            squence.Append(playBtnTween);
            return squence;
        }

        private Sequence AnimMidBtns()
        {
            var squence = DOTween.Sequence();
            if (m_midBtns == null || m_midBtns.Length <= 0) return squence;

            for (int i = 0; i < m_midBtns.Length; i++)
            {
                var btnTrans = m_midBtns[i];
                if (btnTrans == null) continue;
                btnTrans.localScale = Vector3.zero;
                var midBtnTween = btnTrans.DOScale(1f, 0.15f);
                squence.Append(midBtnTween);
            }
            return squence;
        }

        private void OnDisable()
        {
            DOTween.KillAll();
        }
#endif
    }
}
