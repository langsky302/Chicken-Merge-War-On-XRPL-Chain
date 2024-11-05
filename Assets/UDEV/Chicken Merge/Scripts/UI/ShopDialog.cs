using System;
using UDEV.ActionEventDispatcher;
using UDEV.DMTool;
using UnityEngine;
using UnityEngine.UI;

namespace UDEV.ChickenMerge
{
    public class ShopDialog : Dialog, IActionEventDispatcher
    {
        [SerializeField] private Text m_coinCountingTxt;

        #region ACTION
        private Action<object> m_OnUpdateUI;
        #endregion

        #region EVENTS
        public void RegisterEvents()
        {
            m_OnUpdateUI = param => UpdateUI();

            this.RegisterActionEvent(GameplayAction.BUY_GUN, m_OnUpdateUI);
            this.RegisterActionEvent(GameplayAction.UPGRADE_GUN, m_OnUpdateUI);
            this.RegisterActionEvent(GameplayAction.SKILL_BOOSTER_UPDATE, m_OnUpdateUI);
        }

        public void UnregisterEvents()
        {
            this.RemoveActionEvent(GameplayAction.BUY_GUN, m_OnUpdateUI);
            this.RemoveActionEvent(GameplayAction.UPGRADE_GUN, m_OnUpdateUI);
            this.RemoveActionEvent(GameplayAction.SKILL_BOOSTER_UPDATE, m_OnUpdateUI);
        }
        #endregion

        private void OnEnable()
        {
            RegisterEvents();
        }

        public override void Show()
        {
            base.Show();
            GameController.ChangeState(GameState.Pausing);   
            UpdateUI();
        }

        private void UpdateUI()
        {
            if (m_coinCountingTxt)
                m_coinCountingTxt.text = Helper.BigCurrencyFormat(UserDataHandler.Ins.coin);
        }

        private void OnDisable()
        {
            UnregisterEvents();
            GameController.RevertState();
        }
    }
}
