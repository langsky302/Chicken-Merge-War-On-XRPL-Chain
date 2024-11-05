using UDEV.DMTool;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

namespace UDEV.ChickenMerge
{
    public class BoosterShopDialog : Dialog
    {
        [SerializeField] private Transform m_gridRoot;
        [SerializeField] private BoosterShopItemUI m_itemUIPrefab;
        [SerializeField] private Text m_coinCountingTxt;

        private BoosterController[] m_boosterCtrs;

        public override void Show()
        {
            base.Show();
            GameController.ChangeState(GameState.Pausing);
            m_boosterCtrs = BoosterManager.Ins.boosterControllers;
            m_boosterCtrs = m_boosterCtrs.OrderBy(b => b.stat.price).ToArray();
            UpdateUI();
        }

        private void UpdateUI()
        {
            if (m_coinCountingTxt)
                m_coinCountingTxt.text = Helper.BigCurrencyFormat(UserDataHandler.Ins.coin);

            Helper.ClearChilds(m_gridRoot);

            if (m_boosterCtrs == null || m_boosterCtrs.Length <= 0) return;

            for (int i = 0; i < m_boosterCtrs.Length; i++)
            {
                int boosterCtrId = i;
                var boosterCtr = m_boosterCtrs[boosterCtrId];
                var itemUIClone = Instantiate(m_itemUIPrefab);
                itemUIClone.UpdateUI(boosterCtr);
                Helper.AssignToRoot(m_gridRoot, itemUIClone.transform, Vector3.zero, Vector3.one);
                if (itemUIClone.buyingBtn) {
                    itemUIClone.buyingBtn.onClick.RemoveAllListeners();
                    itemUIClone.buyingBtn.onClick.AddListener(() => BuyBoosterBtnEvent(boosterCtr));
                }
            }
        }

        private void BuyBoosterBtnEvent(BoosterController boosterCtr)
        {
            DataGroup.Ins?.BuyBooster(boosterCtr, () =>
            {
                UpdateUI();   
            }, ShowOutOfCoinDialog);
        }

        private void ShowOutOfCoinDialog()
        {
            DialogDB.Ins.Show(DialogType.OutOfCoin);
        }

        private void OnDisable()
        {
            GameController.RevertState();
        }
    }
}
