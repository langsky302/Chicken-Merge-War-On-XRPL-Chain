using UnityEngine;
#if USE_DOTWEEN
using DG.Tweening;
#endif
using System;

namespace UDEV.DMTool
{
    public enum InAnim
    {
        Scale,
        ScaleRoz,
        MoveLeft,
        MoveRight,
        MoveUp,
        MoveDown,
        MoveLeft_Roz,
        MoveRight_Roz,
        MoveUp_Roz,
        MoveDown_Roz
    }

    public enum OutAnim
    {
        Scale,
        ScaleRoz,
        MoveLeft,
        MoveRight,
        MoveUp,
        MoveDown,
        MoveLeft_Roz,
        MoveRight_Roz,
        MoveUp_Roz,
        MoveDown_Roz
    }
#if USE_DOTWEEN
    [System.Serializable]
    public class InOpt
    {
        public InAnim anim;
        public Ease ease;
    }

    [System.Serializable]
    public class OutOpt
    {
        public OutAnim anim;
        public Ease ease;
    }
#endif
    public class DialogAnim : MonoBehaviour
    {
#if USE_DOTWEEN
        public RectTransform main;
        public InOpt inOption;
        public OutOpt outOption;
        public float inTime = 0.3f;
        public float outTime = 0.3f;
        Vector2 m_startingRect;
        Vector2 m_rootSize;

        private void Awake()
        {
            if (main != null) return;
            main = transform.GetChild(0).GetChild(0).GetComponent<RectTransform>();
            if (main)
            {
                m_startingRect = main.anchoredPosition;
            }
            m_rootSize = GetComponent<RectTransform>().sizeDelta;
        }

        public void ShowInAnim(Action OnCompleted = null)
        {
            switch (inOption.anim)
            {
                case InAnim.Scale:
                    if(main)
                    {
                        main.localScale = Vector3.zero;
                        main.DOScale(1f, inTime).OnComplete(() =>
                        {
                            if (OnCompleted != null)
                                OnCompleted.Invoke();
                        }).SetEase(inOption.ease).SetUpdate(true);
                    }    
                    break;
                case InAnim.ScaleRoz:
                    if (main)
                    {
                        main.localScale = Vector3.zero;
                        main.localRotation = Quaternion.Euler(0f, 0f, 180f);
                        main.DORotate(Vector3.zero, inTime).SetEase(inOption.ease).SetUpdate(true); ;
                        main.DOScale(1f, inTime).OnComplete(() =>
                        {
                            if (OnCompleted != null)
                                OnCompleted.Invoke();
                        }).SetEase(inOption.ease).SetUpdate(true);
                    }
                    break;
                case InAnim.MoveLeft:
                    if (main)
                    {
                        main.anchoredPosition = new Vector2(-m_rootSize.x, m_startingRect.y);
                        main.DOAnchorPos(new Vector2(m_startingRect.x, m_startingRect.y), inTime).OnComplete(() =>
                        {
                            if (OnCompleted != null)
                                OnCompleted.Invoke();
                        }).SetEase(inOption.ease).SetUpdate(true);
                    }
                    break;
                case InAnim.MoveRight:
                    if (main)
                    {
                        main.anchoredPosition = new Vector2(m_rootSize.x, m_startingRect.y);
                        main.DOAnchorPos(new Vector2(m_startingRect.x, m_startingRect.y), inTime).OnComplete(() =>
                        {
                            if (OnCompleted != null)
                                OnCompleted.Invoke();
                        }).SetEase(inOption.ease).SetUpdate(true);
                    }
                    break;
                case InAnim.MoveUp:
                    if (main)
                    {
                        main.anchoredPosition = new Vector2(m_startingRect.x, m_rootSize.y);
                        main.DOAnchorPos(new Vector2(m_startingRect.x, m_startingRect.y), inTime).OnComplete(() =>
                        {
                            if (OnCompleted != null)
                                OnCompleted.Invoke();
                        }).SetEase(inOption.ease).SetUpdate(true);
                    }
                    break;
                case InAnim.MoveDown:
                    if (main)
                    {
                        main.anchoredPosition = new Vector2(m_startingRect.x, -m_rootSize.y);
                        main.DOAnchorPos(new Vector2(m_startingRect.x, m_startingRect.y), inTime).OnComplete(() =>
                        {
                            if (OnCompleted != null)
                                OnCompleted.Invoke();
                        }).SetEase(inOption.ease).SetUpdate(true);
                    }
                    break;
                case InAnim.MoveLeft_Roz:
                    if (main)
                    {
                        main.localRotation = Quaternion.Euler(0f, 0f, 180f);
                        main.DORotate(new Vector3(0f, 0f, 0f), inTime).SetEase(inOption.ease).SetUpdate(true);
                        main.anchoredPosition = new Vector2(-m_rootSize.x, m_startingRect.y);
                        main.DOAnchorPos(new Vector2(m_startingRect.x, m_startingRect.y), inTime).OnComplete(() =>
                        {
                            if (OnCompleted != null)
                                OnCompleted.Invoke();
                        }).SetEase(inOption.ease).SetUpdate(true);
                    }
                    break;
                case InAnim.MoveRight_Roz:
                    if (main)
                    {
                        main.localRotation = Quaternion.Euler(0f, 0f, 180f);
                        main.DORotate(new Vector3(0f, 0f, 0f), inTime).SetEase(inOption.ease).SetUpdate(true);
                        main.anchoredPosition = new Vector2(m_rootSize.x, m_startingRect.y);
                        main.DOAnchorPos(new Vector2(m_startingRect.x, m_startingRect.y), inTime).OnComplete(() =>
                        {
                            if (OnCompleted != null)
                                OnCompleted.Invoke();
                        }).SetEase(inOption.ease).SetUpdate(true);
                    }
                    break;
                case InAnim.MoveUp_Roz:
                    if (main)
                    {
                        main.localRotation = Quaternion.Euler(0f, 0f, 180f);
                        main.DORotate(new Vector3(0f, 0f, 0f), inTime).SetEase(inOption.ease).SetUpdate(true);
                        main.anchoredPosition = new Vector2(m_startingRect.x, m_rootSize.y);
                        main.DOAnchorPos(new Vector2(m_startingRect.x, m_startingRect.y), inTime).OnComplete(() =>
                        {
                            if (OnCompleted != null)
                                OnCompleted.Invoke();
                        }).SetEase(inOption.ease).SetUpdate(true);
                    }
                    break;
                case InAnim.MoveDown_Roz:
                    if (main)
                    {
                        main.localRotation = Quaternion.Euler(0f, 0f, 180f);
                        main.DORotate(new Vector3(0f, 0f, 0f), inTime).SetEase(inOption.ease).SetUpdate(true);
                        main.anchoredPosition = new Vector2(m_startingRect.x, -m_rootSize.y);
                        main.DOAnchorPos(new Vector2(m_startingRect.x, m_startingRect.y), inTime).OnComplete(() =>
                        {
                            if (OnCompleted != null)
                                OnCompleted.Invoke();
                        }).SetEase(inOption.ease).SetUpdate(true);
                    }
                    break;
            }
        }

        public void ShowOutAnim(Action OnCompleted = null)
        {
            switch (outOption.anim)
            {
                case OutAnim.Scale:
                    if (main)
                    {
                        main.DOScale(0f, outTime).OnComplete(() =>
                        {
                            if (OnCompleted != null)
                                OnCompleted.Invoke();
                        }).SetEase(outOption.ease).SetUpdate(true);
                    }
                    break;
                case OutAnim.ScaleRoz:
                    if (main)
                    {
                        main.localRotation = Quaternion.Euler(0f, 0f, 0);
                        main.DORotate(new Vector3(0f, 0f, 180f), inTime).SetEase(outOption.ease).SetUpdate(true);
                        main.DOScale(0f, inTime).OnComplete(() =>
                        {
                            if (OnCompleted != null)
                                OnCompleted.Invoke();
                        }).SetEase(outOption.ease).SetUpdate(true);
                    }
                    break;
                case OutAnim.MoveLeft:
                    if (main)
                    {
                        main.anchoredPosition = m_startingRect;
                        main.DOAnchorPos(new Vector2(-m_rootSize.x, m_startingRect.y), inTime).OnComplete(() =>
                        {
                            if (OnCompleted != null)
                                OnCompleted.Invoke();
                        }).SetEase(outOption.ease).SetUpdate(true);
                    }
                    break;
                case OutAnim.MoveRight:
                    if (main)
                    {
                        main.anchoredPosition = m_startingRect;
                        main.DOAnchorPos(new Vector2(m_rootSize.x, m_startingRect.y), inTime).OnComplete(() =>
                        {
                            if (OnCompleted != null)
                                OnCompleted.Invoke();
                        }).SetEase(outOption.ease).SetUpdate(true);
                    }
                    break;
                case OutAnim.MoveUp:
                    if (main)
                    {
                        main.anchoredPosition = m_startingRect;
                        main.DOAnchorPos(new Vector2(m_startingRect.x, m_rootSize.y), inTime).OnComplete(() =>
                        {
                            if (OnCompleted != null)
                                OnCompleted.Invoke();
                        }).SetEase(outOption.ease).SetUpdate(true);
                    }
                    break;
                case OutAnim.MoveDown:
                    if (main)
                    {
                        main.anchoredPosition = m_startingRect;
                        main.DOAnchorPos(new Vector2(m_startingRect.x, -m_rootSize.y), inTime).OnComplete(() =>
                        {
                            if (OnCompleted != null)
                                OnCompleted.Invoke();
                        }).SetEase(outOption.ease).SetUpdate(true);
                    }
                    break;
                case OutAnim.MoveLeft_Roz:
                    if (main)
                    {
                        main.localRotation = Quaternion.Euler(0f, 0f, 0f);
                        main.DORotate(new Vector3(0f, 0f, 180f), outTime).SetEase(outOption.ease).SetUpdate(true);
                        main.anchoredPosition = m_startingRect;
                        main.DOAnchorPos(new Vector2(-m_rootSize.x, m_startingRect.y), inTime).OnComplete(() =>
                        {
                            if (OnCompleted != null)
                                OnCompleted.Invoke();
                        }).SetEase(outOption.ease).SetUpdate(true);
                    }
                    break;
                case OutAnim.MoveRight_Roz:
                    if (main)
                    {
                        main.localRotation = Quaternion.Euler(0f, 0f, 0f);
                        main.DORotate(new Vector3(0f, 0f, 180f), outTime).SetEase(outOption.ease).SetUpdate(true);
                        main.anchoredPosition = m_startingRect;
                        main.DOAnchorPos(new Vector2(m_rootSize.x, m_startingRect.y), inTime).OnComplete(() =>
                        {
                            if (OnCompleted != null)
                                OnCompleted.Invoke();
                        }).SetEase(outOption.ease).SetUpdate(true);
                    }
                    break;
                case OutAnim.MoveUp_Roz:
                    if (main)
                    {
                        main.localRotation = Quaternion.Euler(0f, 0f, 0f);
                        main.DORotate(new Vector3(0f, 0f, 180f), outTime).SetEase(outOption.ease).SetUpdate(true);
                        main.anchoredPosition = m_startingRect;
                        main.DOAnchorPos(new Vector2(m_startingRect.x, m_rootSize.y), inTime).OnComplete(() =>
                        {
                            if (OnCompleted != null)
                                OnCompleted.Invoke();
                        }).SetEase(outOption.ease).SetUpdate(true);
                    }
                    break;
                case OutAnim.MoveDown_Roz:
                    if (main)
                    {
                        main.localRotation = Quaternion.Euler(0f, 0f, 0f);
                        main.DORotate(new Vector3(0f, 0f, 180f), outTime).SetEase(outOption.ease).SetUpdate(true);
                        main.anchoredPosition = m_startingRect;
                        main.DOAnchorPos(new Vector2(m_startingRect.x, -m_rootSize.y), inTime).OnComplete(() =>
                        {
                            if (OnCompleted != null)
                                OnCompleted.Invoke();
                        }).SetEase(outOption.ease).SetUpdate(true);
                    }
                    break;
            }
        }

        private void OnDestroy()
        {
            DOTween.Kill(main);
        }
#endif
    }
}
