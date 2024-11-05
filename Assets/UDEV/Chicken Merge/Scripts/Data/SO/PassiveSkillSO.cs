using UnityEngine;
using UnityEngine.Events;

namespace UDEV.ChickenMerge
{
    public class PassiveSkillSO : Stat
    {
        [Header("Common:")]
        public PassiveSkillType type;
        public float bonusRate;
        [TextArea(5, 10)]
        public string description;

        [Header("Level Up Base:")]
        public int maxLevel;
        public int level;
        public int upgradePrice;

        [Header("Level Up:")]
        [Range(0f, 1f)]
        public float bonusRateUp;
        public int upgradePriceUp;
        public override bool IsMaxLevel()
        {
            return level >= maxLevel;
        }

        public override void Load(int id)
        {
            UpgradeableItemUserData skillUserData = UserDataHandler.Ins.GetSkillData(id);
            if (skillUserData == null) return;
            string stat = skillUserData.stat;
            if (!string.IsNullOrEmpty(stat))
            {
                JsonUtility.FromJsonOverwrite(stat, this);
            }
        }

        public override void Save(int id)
        {
            string stat = JsonUtility.ToJson(this);
            UserDataHandler.Ins?.UpdateSkillStat(id, stat);
            UserDataHandler.Ins?.SaveData();
        }

        protected override void UpgradeCore()
        {
            level++;
            level = Mathf.Clamp(level, 0, maxLevel);

            bonusRate += bonusRateUp;
            upgradePrice += upgradePriceUp * level;
        }

        public override void UpgradeToMax(UnityAction OnUpgrade = null)
        {
            while (!IsMaxLevel())
            {
                UpgradeCore();
            }
        }

        public override string ToJson()
        {
            return JsonUtility.ToJson(this);
        }

        public float GetBonusValue(float value)
        {
            return bonusRate * value;
        }
    }
}
