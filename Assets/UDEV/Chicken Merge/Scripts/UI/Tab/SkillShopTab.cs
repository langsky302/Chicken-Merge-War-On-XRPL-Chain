using UDEV.DMTool;
using UnityEngine;

namespace UDEV.ChickenMerge
{
    public class SkillShopTab : TabContent
    {
        [SerializeField] private Transform m_gridRoot;
        [SerializeField] private SkillShopItemUI m_itemUIPrefab;

        private PassiveSkillSO[] m_skillStats;

        public override void LoadContent()
        {
            if (DataGroup.Ins == null || DataGroup.Ins.skillShopData == null) return;

            m_skillStats = DataGroup.Ins.skillShopData.skillStats;

            UpdateUI();
        }

        private void UpdateUI()
        {
            Helper.ClearChilds(m_gridRoot);

            if (m_skillStats == null || m_skillStats.Length <= 0) return;

            for (int i = 0; i < m_skillStats.Length; i++)
            {
                int itemId = i;
                var skillStat = m_skillStats[i];
                if (skillStat == null) continue;
                skillStat.Load(itemId);
                var itemUIClone = Instantiate(m_itemUIPrefab);
                itemUIClone.UpdateUI(skillStat);
                Helper.AssignToRoot(m_gridRoot, itemUIClone.transform, Vector3.zero, Vector3.one);

                if(itemUIClone.upgradeBtn)
                {
                    itemUIClone.upgradeBtn.onClick.RemoveAllListeners();
                    itemUIClone.upgradeBtn.onClick.AddListener(() => UpgradeBtnEvent(itemId, skillStat));
                }
            }
        }
        
        private void UpgradeBtnEvent(int id, PassiveSkillSO skillStat)
        {
            DataGroup.Ins?.UpgradeSkill(id, skillStat, UpdateUI, ShowOutOfCoinDialog);
        }

        private void ShowOutOfCoinDialog()
        {
            DialogDB.Ins.Show(DialogType.OutOfCoin);
        }
    }
}
