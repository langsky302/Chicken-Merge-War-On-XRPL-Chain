using System;
using UDEV.ActionEventDispatcher;
using UDEV.DMTool;
using UnityEngine;
using UnityEngine.UI;

namespace UDEV.ChickenMerge
{
    public class MainUI : MonoBehaviour, IActionEventDispatcher
    {
        [SerializeField] private Text m_coinCountingTxt;

        #region ACTION
        private Action<object> m_OnItemUpdateCoin;
        #endregion

        #region EVENTS
        public void RegisterEvents()
        {
            m_OnItemUpdateCoin = param => UpdateCoinCounting();

            this.RegisterActionEvent(GameplayAction.REWARD_DIALOG_CLOSED, m_OnItemUpdateCoin);
            this.RegisterActionEvent(GameplayAction.BUY_BOOSTER, m_OnItemUpdateCoin);
        }

        public void UnregisterEvents()
        {
            this.RemoveActionEvent(GameplayAction.REWARD_DIALOG_CLOSED, m_OnItemUpdateCoin);
            this.RemoveActionEvent(GameplayAction.BUY_BOOSTER, m_OnItemUpdateCoin);
        }
        #endregion

        private void Awake()
        {
            UpdateCoinCounting();
        }

        private void OnEnable()
        {
            AudioController.Ins.RegisterEvents();
            RegisterEvents();
        }

        private void Start()
        {
            AudioBase.Ins.PlayMusic(AudioBase.Ins.data.menus);
            this.PostActionEvent(GameplayAction.UPDATE_COIN);
        }

        private void OnDisable()
        {
            UnregisterEvents();
        }

        private void UpdateCoinCounting()
        {
            if (m_coinCountingTxt)
                m_coinCountingTxt.text = Helper.BigCurrencyFormat(UserDataHandler.Ins.coin);
        }

        public void PlayGame()
        {
            DialogDB.Ins.Show(DialogType.LevelSelection, ShowType.NOT_SHOW_WHEN_OTHER_ACTIVE);
        }

        public void OpenIAPShopDialog()
        {
            DialogDB.Ins.Show(DialogType.IAPShop, ShowType.NOT_SHOW_WHEN_OTHER_ACTIVE);
        }

        public void OpenBoosterDialog()
        {
            DialogDB.Ins.Show(DialogType.BoosterShop, ShowType.NOT_SHOW_WHEN_OTHER_ACTIVE);
        }

        public void OpenQuestDialog()
        {
            DialogDB.Ins.Show(DialogType.Quest, ShowType.NOT_SHOW_WHEN_OTHER_ACTIVE);
        }

        public void OpenRateUsDialog()
        {
            DialogDB.Ins.Show(DialogType.RateUs, ShowType.NOT_SHOW_WHEN_OTHER_ACTIVE);
        }

        public void OpenChestDialog()
        {
            if (ChestController.Ins.CurrentChest == null || ChestController.Ins.ChestRemaining <= 0) return;

            OpenChestDialog openChestDialog = (OpenChestDialog)DialogDB.Ins.GetDialog(DialogType.OpenChest);
            if (openChestDialog)
            {
                DialogDB.Ins.Show(openChestDialog, ShowType.NOT_SHOW_WHEN_OTHER_ACTIVE);
            }
        }
    }
}
