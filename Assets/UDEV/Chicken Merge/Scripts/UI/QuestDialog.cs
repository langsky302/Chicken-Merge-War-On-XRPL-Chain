using System.Collections.Generic;
using UDEV.DMTool;
using UnityEngine;

namespace UDEV.ChickenMerge
{
    public class QuestDialog : Dialog
    {
        [SerializeField] private Transform m_gridRoot;
        [SerializeField] private QuestItemUI m_itemUIPrefab;

        private Quest[] m_questItems;

        public override void Show()
        {
            base.Show();
            m_questItems = DataGroup.Ins.questData.quests;
            m_questItems = SortingByClaimable();
            UpdateUI();
        }

        private Quest[] SortingByClaimable()
        {
            List<Quest> questItemsList = new List<Quest>();
            List<Quest> result = new List<Quest>();
            questItemsList.AddRange(m_questItems);

            if (questItemsList == null || questItemsList.Count <= 0) return null;

            for (int i = 0; i < m_questItems.Length; i++)
            {
                var questItem = m_questItems[i];
                if (questItem == null) continue;
                QuestUserData questUserData = UserDataHandler.Ins.GetQuestData(questItem.Id);
                if (questItem == null || questUserData == null) continue;

                if ((questItem.IsCompleted(questItem.Id) && !questItem.IsClaimed(questItem.Id)) 
                    || questItem.IsClaimed(questItem.Id))
                {
                    result.Add(questItem);
                    questItemsList.Remove(questItem);
                }
            }
            result.AddRange(questItemsList);
            return result.ToArray();
        }

        private void UpdateUI()
        {
            Helper.ClearChilds(m_gridRoot);
            if (m_questItems == null || m_questItems.Length <= 0) return;

            for (int i = 0; i < m_questItems.Length; i++)
            {
                int questIdx = i;
                var questItem = m_questItems[questIdx];
                if(questItem == null) continue;

                var itemUIClone = Instantiate(m_itemUIPrefab);
                itemUIClone.UpdateUI(questItem);
                Helper.AssignToRoot(m_gridRoot, itemUIClone.transform, Vector3.zero, Vector3.one);

                if (itemUIClone.claimBtn)
                {
                    itemUIClone.claimBtn.onClick.RemoveAllListeners();
                    itemUIClone.claimBtn.onClick.AddListener(() => ClainBtnEvent(questItem, questItem.Id));
                }
            }
        }

        private void ClainBtnEvent(Quest quest , int id)
        {
            DataGroup.Ins?.ClaimAQuest(quest, id, () =>
            {
                RewardDialog rewardDialog = (RewardDialog)DialogDB.Ins.GetDialog(DialogType.Reward);

                if (rewardDialog)
                {
                    rewardDialog.AddCoinRewardItem(quest.bonus);
                    DialogDB.Ins.Show(rewardDialog, ShowType.STACK);
                }
                UpdateUI();

                var questBtnUI = FindObjectOfType<QuestBtnUI>();
                questBtnUI?.UpdateUI();
            });
        }
    }
}
