using System;
using System.Collections;
using System.Collections.Generic;
using UDEV.ActionEventDispatcher;
using UDEV.DMTool;
using UnityEngine;
using UnityEngine.UI;

namespace UDEV.ChickenMerge
{
    public class UpgradeShieldButton : MonoBehaviour, IActionEventDispatcher
    {
        [SerializeField] Text m_durabilityStatTxt;
        [SerializeField] Text m_upgradePriceTxt;

        private Shield m_shield;
        private ShieldStatSO m_shieldStat;

        #region ACTION
        private Action<object> m_OnUpdateUI;
        #endregion

        #region EVENTS
        public void RegisterEvents()
        {
            m_OnUpdateUI = (param) => UpdateUI();

            this.RegisterActionEvent(GameState.Starting, m_OnUpdateUI);
            this.RegisterActionEvent(GameplayAction.UPGRADE_SKILL, m_OnUpdateUI);
        }

        public void UnregisterEvents()
        {
            this.RemoveActionEvent(GameState.Starting, m_OnUpdateUI);
            this.RemoveActionEvent(GameplayAction.UPGRADE_SKILL, m_OnUpdateUI);
        }
        #endregion

        private void OnEnable()
        {
            RegisterEvents();
        }

        private void OnDisable()
        {
            UnregisterEvents();
        }

        private void Start()
        {
            if (GameController.Ins == null || GameController.Ins.CurShield == null) return;
            if(GameController.Ins.CurShield.ShieldStat == null) return;

            m_shield = GameController.Ins.CurShield;
            m_shieldStat = m_shield.ShieldStat;

            UpdateUI();
        }

        public void Upgrade()
        {
            m_shield?.Upgrade(UpdateUI, ShowOutOfCoinDialog);
        }

        private void UpdateUI()
        {
            if(m_shieldStat == null) return;

            if(m_durabilityStatTxt != null)
            {
                m_durabilityStatTxt.text = $"+{m_shieldStat.DurabilityUpInfo}";
            }

            if (m_upgradePriceTxt)
            {
                m_upgradePriceTxt.text = Helper.BigCurrencyFormat(m_shield.UpgradePrice);
            }
        }

        private void ShowOutOfCoinDialog()
        {
            DialogDB.Ins.Show(DialogType.OutOfCoin);
        }
    }
}
