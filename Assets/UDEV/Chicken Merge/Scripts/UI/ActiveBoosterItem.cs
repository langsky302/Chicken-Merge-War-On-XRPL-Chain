using UDEV.ActionEventDispatcher;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace UDEV.ChickenMerge
{
    public class ActiveBoosterItem : MonoBehaviour
    {
        [SerializeField] private Image m_boosterIcon;
        [SerializeField] private TimerText m_timerTxt;
        private BoosterController m_boosterCtr;

        #region ACTION
        private UnityAction<double> m_OnTriggerTimeCounting;
        private UnityAction m_OnEndTime;
        #endregion

        public void UpdateUI(BoosterController boosterCtr)
        {
            if (boosterCtr == null || boosterCtr.stat == null) return;
            m_boosterCtr = boosterCtr;

            if (m_boosterIcon)
                m_boosterIcon.sprite = m_boosterCtr.stat.icon;

            m_OnTriggerTimeCounting = time => TriggerTimeCounting(time, m_boosterCtr);
            m_OnEndTime = () => EndTime(m_boosterCtr);

            m_boosterCtr.OnBoosterRunning.AddListener(m_OnTriggerTimeCounting);
            m_boosterCtr.onActionReached.AddListener(m_OnEndTime);

            m_boosterCtr.UpdateAction();
        }

        public void TriggerTimeCounting(double time, BoosterController boosterCtr)
        {
            if (!m_timerTxt) return;
            m_timerTxt.OnCountDownComplete -= () => EndTime(boosterCtr);
            m_timerTxt.OnCountDownComplete += () => EndTime(boosterCtr);
            m_timerTxt.SetTime((int)time);
            m_timerTxt.Run(this);
        }

        public void EndTime(BoosterController boosterCtr)
        {
            if (boosterCtr) boosterCtr.DeactiveBooster();

            if(m_timerTxt)
                m_timerTxt.gameObject.SetActive(false);

            this.PostActionEvent(GameplayAction.SKILL_BOOSTER_UPDATE);

            Destroy(gameObject);
        }

        private void OnDisable()
        {
            if (!m_boosterCtr) return;

            m_boosterCtr.OnBoosterRunning.RemoveListener(m_OnTriggerTimeCounting);
            m_boosterCtr.onActionReached.RemoveListener(m_OnEndTime);
        }
    }
}
