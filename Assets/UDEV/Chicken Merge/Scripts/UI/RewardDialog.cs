using System.Collections.Generic;
using UDEV.ActionEventDispatcher;
using UDEV.DMTool;
using UnityEngine;

namespace UDEV.ChickenMerge
{
    public class RewardDialog : Dialog
    {
        [SerializeField] private Transform m_gridRoot;
        [SerializeField] private RewardItemUI m_itemUIPrefab;
        [SerializeField] private Sprite m_coinIconSp;
        private List<RewardDialogItem> m_rewardItems;

        protected override void Awake()
        {
            base.Awake();
            m_rewardItems = new List<RewardDialogItem>();
        }

        public override void Show()
        {
            base.Show();
            GameController.ChangeState(GameState.Pausing);
            UpdateUI();
            Invoke("Close", 2f);
        }

        private void UpdateUI()
        {
            Helper.ClearChilds(m_gridRoot);
            if (m_rewardItems == null || m_rewardItems.Count <= 0) return;

            for (int i = 0; i < m_rewardItems.Count; i++)
            {
                var rewardItem = m_rewardItems[i];
                if(rewardItem == null) continue;
                var itemUIClone = Instantiate(m_itemUIPrefab);
                itemUIClone.UpdateUI(rewardItem);
                Helper.AssignToRoot(m_gridRoot, itemUIClone.transform, Vector3.zero, Vector3.one);
            }

            this.PostActionEvent(GameplayAction.GET_REWARD);
        }

        public void AddRewardItem(RewardDialogItem item)
        {
            m_rewardItems.Add(item);
        }

        public void AddCoinRewardItem(int amount)
        {
            m_rewardItems.Add(new RewardDialogItem(m_coinIconSp, amount));
        }

        public override void Close()
        {
            base.Close();
            GameController.ChangeState(GameState.Playing);
            this.PostActionEvent(GameplayAction.REWARD_DIALOG_CLOSED);
        }
    }
}
