using UDEV.SPM;
using UnityEngine;
using UnityEngine.Events;

namespace UDEV.ChickenMerge
{
    //[CreateAssetMenu(fileName = "New Gun Data", menuName = "UDEV/Create Gun Data")]
    public class GunStatSO : Stat
    {
        [Header("Common:")]
        public int numberOfBullets;
        [PoolerKeys(target = PoolerTarget.NONE)]
        public string poolKey;
        public float damage;
        [Range(0f, 1f)]
        public float damageMissRange;
        public float fireRate;
        public float minFireRate = 0.05f;
        [Range(0f, 1f)]
        public float critRate;

        [Header("Level Up Base:")]
        public int maxLevel;
        public int level;
        public int buyingPrice;
        public int upgradePrice;

        [Header("Level Up:")]
        public float fireRateUp;
        public float dmgUp;
        [Range (0f, 1f)]
        public float critRateUp;
        public int buyingPriceUp;
        public int upgradePriceUp;

        [Header("Level Up Factor:")]
        public float fireRateUpFactor = 2;
        public float damageUpFactor = 6;
        public float critRateUpFactor = 10; 

        public override bool IsMaxLevel()
        {
            return level >= maxLevel; 
        }

        public float DmgUp
        {
            get => Helper.UpgradeForm(level, damageUpFactor) * dmgUp;
        }

        public float FireRateUp
        {
            get => Helper.UpgradeForm(level, fireRateUpFactor) * fireRateUp;
        }
       
        public float CritUp
        {
            get => Helper.UpgradeForm(level, critRateUpFactor) * critRateUp;
        }

        public float DmgUpInfo
        {
            get => Helper.UpgradeForm(level + 1, damageUpFactor) * dmgUp;
        }

        public float FireRateUpInfo
        {
            get => Helper.UpgradeForm(level + 1, fireRateUpFactor) * fireRateUp;
        }

        public float CritUpInfo
        {
            get => Helper.UpgradeForm(level + 1, critRateUpFactor) * critRateUp;
        }

        public override void Load(int id)
        {
            UpgradeableItemUserData gunData = UserDataHandler.Ins?.GetGunData(id);
            if (gunData == null) return;
            string stat = gunData.stat;
            if (!string.IsNullOrEmpty(stat))
            {
                JsonUtility.FromJsonOverwrite(stat, this);
            }
        }

        public override void Save(int id)
        {
            string stat = JsonUtility.ToJson(this);
            UserDataHandler.Ins?.UpdateGunStat(id, stat);
            UserDataHandler.Ins?.SaveData();
        }

        protected override void UpgradeCore()
        {
            level++;
            level = Mathf.Clamp(level, 0, maxLevel);

            critRate += CritUp;
            fireRate -= FireRateUp;
            damage += DmgUp;
            upgradePrice += upgradePriceUp * level;
            buyingPrice += buyingPriceUp * level;
            fireRate = Mathf.Clamp(fireRate, minFireRate, fireRate);
        }

        public void InscreaseBuyingPrice()
        {
            if(level <= 0)
            {
                buyingPrice += buyingPriceUp * (level + 1);
                return;
            }
            buyingPrice += buyingPriceUp * level;
        }

        public override void UpgradeToMax(UnityAction OnUpgrade = null)
        {
            while (level < maxLevel)
            {
                UpgradeCore();
                OnUpgrade?.Invoke();
            }
        }

        public void LevelUp()
        {
            
        }

        public override void Save()
        {
            
        }

        public override void Load()
        {
            
        }

        public override string ToJson()
        {
            return JsonUtility.ToJson(this);
        }
    }
}
