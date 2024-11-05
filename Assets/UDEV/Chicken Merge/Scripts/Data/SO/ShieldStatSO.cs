using UnityEngine;
using UnityEngine.Events;

namespace UDEV.ChickenMerge
{
    public class ShieldStatSO : Stat
    {
        [Header("Common:")]
        public float durability;

        [Header("Level Up Base:")]
        public int level;
        public int maxLevel;
        public int repairPrice;
        public int upgradePrice;

        [Header("Level Up:")]
        public float durabilityUp;
        public int repairPriceUp;
        public int upgradePriceUp;        

        [Header("Level Up Factor:")]
        public float durabilityUpFactor = 2;

        public float DurabilityUp
        {
            get => Helper.UpgradeForm(level, durabilityUpFactor) * durabilityUp;
        }

        public float DurabilityUpInfo
        {
            get => Helper.UpgradeForm(level + 1, durabilityUpFactor) * durabilityUp;
        }
        public override bool IsMaxLevel()
        {
            return level >= maxLevel;
        }

        public override string ToJson()
        {
            return JsonUtility.ToJson(this);
        }

        public override void UpgradeToMax(UnityAction OnUpgrade = null)
        {
            while (!IsMaxLevel())
            {
                UpgradeCore();
                OnUpgrade?.Invoke();
            }
        }

        protected override void UpgradeCore()
        {
            durability += DurabilityUp;
            upgradePrice += upgradePriceUp * level;
            level++;
        }

        public void Repair()
        {
            repairPrice += repairPriceUp * level;
        }

        public override void Save()
        {
            UpgradeableItemUserData newShieldData = new UpgradeableItemUserData();
            newShieldData.stat = JsonUtility.ToJson(this);

            UserDataHandler.Ins.UpdateShieldData(newShieldData);
            UserDataHandler.Ins.SaveData();
        }

        public override void Load()
        {
            UpgradeableItemUserData shieldData = UserDataHandler.Ins?.GetShieldData();
            if (shieldData == null) return;
            string stat = shieldData.stat;
            if (!string.IsNullOrEmpty(stat))
            {
                JsonUtility.FromJsonOverwrite(stat, this);
            }
        }
    }
}
