using System;
using System.Collections;
using System.Collections.Generic;
using UDEV.ActionEventDispatcher;
using UDEV.DMTool;
using UnityEngine;
using UnityEngine.UI;

namespace UDEV.ChickenMerge
{
    public class RepairShieldButton : MonoBehaviour
    {
        [SerializeField] private Text m_repairPriceTxt;
        [SerializeField] private Image m_durabilityFilled;

        private Shield m_shield;

        #region ACTION
        private Action<object> m_OnUpdateUI;
        #endregion

        #region EVENTS
        public void RegisterEvents()
        {
            m_OnUpdateUI = (param) => UpdateUI();

            this.RegisterActionEvent(GameState.Starting, m_OnUpdateUI);
            this.RegisterActionEvent(GameplayAction.UPGRADE_SHEILD, m_OnUpdateUI);
            this.RegisterActionEvent(GameplayAction.SHIELD_HITTED, m_OnUpdateUI);
            this.RegisterActionEvent(GameplayAction.UPGRADE_SKILL, m_OnUpdateUI);
        }

        public void UnregisterEvents()
        {
            this.RemoveActionEvent(GameState.Starting, m_OnUpdateUI);
            this.RemoveActionEvent(GameplayAction.UPGRADE_SHEILD, m_OnUpdateUI);
            this.RemoveActionEvent(GameplayAction.SHIELD_HITTED, m_OnUpdateUI);
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
            if (GameController.Ins == null) return;

            m_shield = GameController.Ins.CurShield;

            UpdateUI();
        }

        public void Repair()
        {
            m_shield?.Repair(UpdateUI, ShowOutOfCoinDialog);
        }

        private void UpdateUI()
        {
            if(m_shield == null || m_shield.ShieldStat == null) return;

            if(m_repairPriceTxt != null)
            {
                m_repairPriceTxt.text = Helper.BigCurrencyFormat(m_shield.RepairPrice);
            }

            if(m_durabilityFilled != null)
            {
                m_durabilityFilled.fillAmount = m_shield.DurabilityRemainingRate;
            }
        }

        private void ShowOutOfCoinDialog()
        {
            DialogDB.Ins.Show(DialogType.OutOfCoin);
        }
    }
}
