using System;
using System.Collections;
using UnityEngine;
#if USE_DOTWEEN
using DG.Tweening;
#endif
using UnityEngine.UI;

namespace UDEV
{
    public static class TweenUltis
    {

        public static void LineMoveInOut(
            RectTransform trans,
            Vector3 inPos,
            float time,
            bool ignoreTimescale = false,
            Action InComplete = null,
            Action OutComplete = null)
        {
            if (trans == null) return;
#if USE_DOTWEEN
            trans.DOComplete(true);
            Vector2 startPos = trans.anchoredPosition;
            var outPos = new Vector2(startPos.x * -1, startPos.y);
            var sequence = DOTween.Sequence();
            var inTween = trans.DOAnchorPos(inPos, time).SetUpdate(ignoreTimescale);
            var outTween = trans.DOAnchorPos(outPos, time).SetDelay(1f).SetUpdate(ignoreTimescale);

            inTween.OnComplete(() =>
            {
                InComplete?.Invoke();
            });

            outTween.OnComplete(() =>
            {
                trans.anchoredPosition = new Vector2(outPos.x * -1, outPos.y);
                OutComplete?.Invoke();
            });

            sequence.Append(inTween);
            sequence.Append(outTween);
#endif
        }

        public static IEnumerator ScaleBlinkLoop(Transform trans, float inScale, float outScale, float time, float rate, bool ignoreTimescale = false)
        {
#if USE_DOTWEEN
            trans.DOComplete(true);
#endif
            while (true)
            {
#if USE_DOTWEEN
                var sequence = DOTween.Sequence();
                var inScaleTween = trans.DOScale(inScale, time).SetUpdate(ignoreTimescale);
                var outScaleTween = trans.DOScale(outScale, time).SetUpdate(ignoreTimescale);

                sequence.Append(inScaleTween);
                sequence.Append(outScaleTween);
#endif
                if(ignoreTimescale)
                    yield return new WaitForSecondsRealtime(time * 2 + rate);
                else
                    yield return new WaitForSeconds(time * 2 + rate);
            }
        }

        public static void ScaleBlink(Transform trans, float startScale, float outScale, float time, bool ignoreTimescale = false)
        {
            if (trans == null) return;
#if USE_DOTWEEN
            trans.DOComplete(true);
            var sequence = DOTween.Sequence();
            trans.localScale = startScale * Vector3.one;
            var blinkTween = trans.DOScale(outScale, time).SetUpdate(ignoreTimescale);
            var normalTween = trans.DOScale(startScale, time).SetUpdate(ignoreTimescale);

            sequence.Append(blinkTween);
            sequence.Append(normalTween);
#endif
        }

        public static void ScaleAnim(Transform trans, float startScale, float outScale, float time, Action OnCompleted = null, bool ignoreTimescale = false)
        {
            if (trans == null) return;
#if USE_DOTWEEN           
            trans.DOComplete(true);
            trans.localScale = Vector3.one * startScale; 
            var scaleOutTween = trans.DOScale(outScale, time);
            scaleOutTween.OnComplete(() =>
            {
                OnCompleted?.Invoke();
            });
            scaleOutTween.SetUpdate(ignoreTimescale);
#endif
        }

        public static void FillAmountAnim(Image img, float amount, float time, Action OnCompleted = null)
        {
            if(img == null) return;
#if USE_DOTWEEN
            var fillTween = img.DOFillAmount(amount, time);
            fillTween.OnComplete(() =>
            {
                OnCompleted?.Invoke();
            });
#endif
        }

        public static void CanvasGroup_Fade(CanvasGroup canvasGroup, float startVal, float endVal, float time, bool ignoreTimescale = false)
        {
#if USE_DOTWEEN
            canvasGroup.DOComplete(true);
            var sequence = DOTween.Sequence();
            var startingTween = canvasGroup.DOFade(startVal, 0f).SetUpdate(ignoreTimescale);
            var fadeTween = canvasGroup.DOFade(endVal, time).SetUpdate(ignoreTimescale);

            sequence.Append(startingTween);
            sequence.Append(fadeTween);
#endif
        }

        public static void Image_Fade(Image image, float startVal, float endVal, float time, bool ignoreTimescale = false)
        {
#if USE_DOTWEEN
            image.DOComplete(true);
            var sequence = DOTween.Sequence();
            var startingTween = image.DOFade(startVal, 0f).SetUpdate(ignoreTimescale);
            var fadeTween = image.DOFade(endVal, time).SetUpdate(ignoreTimescale);

            sequence.Append(startingTween);
            sequence.Append(fadeTween);
#endif
        }

        public static void Text_Fade(Text text, float startVal, float endVal, float time, bool ignoreTimescale = false, Action OnComplete = null)
        {
#if USE_DOTWEEN
            text.DOComplete(true);
            var sequence = DOTween.Sequence();
            var startingTween = text.DOFade(startVal, 0f).SetUpdate(ignoreTimescale);
            var fadeTween = text.DOFade(endVal, time).SetUpdate(ignoreTimescale);

            sequence.Append(startingTween);
            sequence.Append(fadeTween);
            sequence.OnComplete(() =>
            {
                OnComplete?.Invoke();
            });
#endif
        }

        public static void RotScale(Transform trans, float scale, float rotZ, float time, bool ignoreTimescale = false, Action OnComlete = null)
        {
#if USE_DOTWEEN
            trans.DOComplete(true);
            Vector3 startingRot = trans.rotation.eulerAngles;
            Vector3 startingScale = trans.localScale;

            var sequence = DOTween.Sequence();
            var rotateTween = trans.DORotate(new Vector3(trans.rotation.x, trans.rotation.y, rotZ), time).SetUpdate(ignoreTimescale);
            var scaleTween = trans.DOScale(scale, time).SetUpdate(ignoreTimescale);

            sequence.Append(rotateTween);
            sequence.Append(scaleTween);
            sequence.OnComplete(() =>
            {
                OnComlete?.Invoke();
            });
#endif
        }

        public static void FxMergeTwo(Vector3 position, Transform tf1, Transform tf2, float duration, Action OnCompleted = null)
        {
#if USE_DOTWEEN
            tf1.DOComplete(true);
            tf2.DOComplete(true);

            tf1.localPosition = position;
            tf2.localPosition = position;

            var tween1 = tf1.DOLocalMoveY(position.y - 0.5f, duration);
            var tween2 = tf2.DOLocalMoveY(position.y + 0.5f, duration) ;

            tween1.OnComplete(() => { tf1.DOLocalMoveY(position.y, duration); });
            tween2.OnComplete(() =>
            {
                var tween = tf2.DOLocalMoveY(position.y, duration);

                tween.OnComplete(() =>
                {
                    OnCompleted?.Invoke();
                });
            });
#endif
        }

        public static void KillAllTweens()
        {
#if USE_DOTWEEN
            DOTween.KillAll();
#endif
        }

        public static void KillTween(Component obj)
        {
#if USE_DOTWEEN
            if (obj == null) return;
            obj.DOKill();
#endif
        }
    }
}
