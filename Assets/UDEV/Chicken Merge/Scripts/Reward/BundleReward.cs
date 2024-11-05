using System.Collections.Generic;
using UnityEngine;
using UDEV.DMTool;
using Unity.VisualScripting;

namespace UDEV.ChickenMerge
{
    public class BundleReward : MonoBehaviour
    {
        [SerializeField] private BundleRewardSO m_rewardStat;
        private List<SkillCollectedType> m_skillTypesTracking;

        public void GetReward(int bonusMultier = 1)
        {
            int openChestPrice = DataGroup.Ins.gameConfig.openChestPrice * (UserDataHandler.Ins.curLevelId + 1);

            m_skillTypesTracking = new List<SkillCollectedType>();
            int coinBonus = Random.Range(m_rewardStat.minCoinBonus, m_rewardStat.maxCoinBonus) * bonusMultier;
            int totalSkillType = Helper.GetEnumCounting<SkillCollectedType>();
            m_rewardStat.numberSkillBonus = Mathf.Clamp(m_rewardStat.numberSkillBonus, 0, totalSkillType);

            ChestUserData chestUserData = new ChestUserData();
            chestUserData.coinsBonus = coinBonus;
            chestUserData.openPrice = openChestPrice;

            for (int i = 0; i < m_rewardStat.numberSkillBonus; i++)
            {
                var skillBonusType = GetSkillTypeNoDupplicate();
                int skillBonusAmount = Random.Range(m_rewardStat.minSkillBonus, m_rewardStat.maxSkillBonus);
                SkillBonusUserData skillBonusUserData = new SkillBonusUserData();
                skillBonusUserData.type = skillBonusType;
                skillBonusUserData.bonusAmount = skillBonusAmount;
                chestUserData.skillBonusList.Add(skillBonusUserData);
            }
            int chestId = UserDataHandler.Ins.chests.Count;
            UserDataHandler.Ins.UpdateChest(chestId, chestUserData);
        }

        private SkillCollectedType GetSkillTypeNoDupplicate()
        {
            var skillType = Helper.GetRandomEnum<SkillCollectedType>();
            int crashCounting = 0;
            while (m_skillTypesTracking.Contains(skillType))
            {
                crashCounting++;
                skillType = Helper.GetRandomEnum<SkillCollectedType>();
                if (crashCounting >= 1000) break;
            }
            m_skillTypesTracking.Add(skillType);
            return skillType;
        }
    }
}
