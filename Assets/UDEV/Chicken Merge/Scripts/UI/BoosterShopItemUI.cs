using UDEV.ActionEventDispatcher;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace UDEV.ChickenMerge
{
    public class BoosterShopItemUI : MonoBehaviour
    {
        [SerializeField] private Image m_boosterIcon;
        [SerializeField] private Text m_descriptionTxt;
        [SerializeField] private Text m_priceTxt;
        [SerializeField] private TimerText m_timerTxt;
        [SerializeField] private GameObject m_timeArea;
        public Button buyingBtn;
        private BoosterController m_boosterCtr;

        #region ACTION
        private UnityAction<double> m_OnTriggerTimeCounting;
        private UnityAction m_OnEndTime;
        #endregion

        public void UpdateUI(BoosterController boosterCtr)
        {
            if (boosterCtr == null) return;

            m_boosterCtr = boosterCtr;

            var dataSO = m_boosterCtr.stat;

            if (dataSO == null) return;

            ShowTimeArea(m_boosterCtr.IsActive);

            if(m_boosterIcon)
                m_boosterIcon.sprite = dataSO.icon;

            if(m_priceTxt)
                m_priceTxt.text = Helper.BigCurrencyFormat(dataSO.price);

            if(m_descriptionTxt)
                m_descriptionTxt.text = dataSO.description;

            m_OnTriggerTimeCounting = time => TriggerTimeCounting(time, m_boosterCtr);
            m_OnEndTime = () => EndTime(m_boosterCtr);

            m_boosterCtr.onActionReached.AddListener(m_OnEndTime);
            m_boosterCtr.OnBoosterRunning.AddListener(m_OnTriggerTimeCounting);

            m_boosterCtr.UpdateAction();
        }

        private void ShowTimeArea(bool isShow)
        {
            if(buyingBtn)
                buyingBtn.gameObject.SetActive(!isShow);

            if (m_timeArea)
                m_timeArea.gameObject.SetActive(isShow);
        }

        public void TriggerTimeCounting(double time, BoosterController boosterCtr)
        {
            ShowTimeArea(true);

            if (m_timerTxt == null) return;
            m_timerTxt.OnCountDownComplete -= () => EndTime(boosterCtr);
            m_timerTxt.OnCountDownComplete += () => EndTime(boosterCtr);
            m_timerTxt.SetTime((int)time);
            m_timerTxt.Run(this);
        }

        public void EndTime(BoosterController boosterCtr)
        {
            if(boosterCtr) boosterCtr.DeactiveBooster();

            ShowTimeArea(false);

            this.PostActionEvent(GameplayAction.SKILL_BOOSTER_UPDATE);
        }

        private void OnDisable()
        {
            if (m_boosterCtr == null) return;

            m_boosterCtr.OnBoosterRunning.RemoveListener(m_OnTriggerTimeCounting);
            m_boosterCtr.onActionReached.RemoveListener(m_OnEndTime);
        }
    }
}
