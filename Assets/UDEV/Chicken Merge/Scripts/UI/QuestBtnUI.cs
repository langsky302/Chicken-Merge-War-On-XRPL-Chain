using System;
using UDEV.ActionEventDispatcher;
using UnityEngine;
using UnityEngine.UI;

namespace UDEV.ChickenMerge
{
    public class QuestBtnUI : MonoBehaviour, IActionEventDispatcher
    {
        [SerializeField] private Text m_claimableCountingTxt;

        #region ACTION
        private Action<object> m_OnUseBooster;
        private Action<object> m_OnUpdateQuest;
        #endregion

        #region EVENTS
        public void RegisterEvents()
        {
            m_OnUseBooster = param => UpdateUI();
            m_OnUpdateQuest = param => UpdateUI();

            this.RegisterActionEvent(GameplayAction.BUY_BOOSTER, m_OnUseBooster);
            this.RegisterActionEvent(GameplayAction.UPDATE_QUEST, m_OnUpdateQuest);
        }

        public void UnregisterEvents()
        {
            this.RemoveActionEvent(GameplayAction.BUY_BOOSTER, m_OnUseBooster);
            this.RemoveActionEvent(GameplayAction.UPDATE_QUEST, m_OnUpdateQuest);
        }
        #endregion

        private void Start()
        {
            UpdateUI();
            RegisterEvents();
        }

        public void UpdateUI()
        {
            StopBlinkAnim();
            int questClaimableCounting = DataGroup.Ins.QuestClaimableCounting;
            if (questClaimableCounting > 0)
            {
                if (m_claimableCountingTxt)
                {
                    m_claimableCountingTxt.gameObject.SetActive(true);
                    m_claimableCountingTxt.text = "x" + questClaimableCounting.ToString();
                }
                ShowBlinkAnim();
            }
            else if(m_claimableCountingTxt)
            {
                m_claimableCountingTxt.gameObject.SetActive(false);
            }
        }

        private void ShowBlinkAnim()
        {
            StartCoroutine(TweenUltis.ScaleBlinkLoop(m_claimableCountingTxt.transform, 1.3f, 1.2f, 0.35f, 0.1f));
        }

        private void StopBlinkAnim()
        {
            StopAllCoroutines();
            m_claimableCountingTxt.transform.localScale = Vector3.one;
        }

        private void OnDisable()
        {
            StopBlinkAnim();
            TweenUltis.KillAllTweens();
            UnregisterEvents();
        }
    }
}
