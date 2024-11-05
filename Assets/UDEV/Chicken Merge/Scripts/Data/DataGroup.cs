using System.Collections.Generic;
using System.Linq;
using UDEV.ActionEventDispatcher;
using UDEV.WaveManagerToolkit;
using UnityEngine;
using UnityEngine.Events;

namespace UDEV.ChickenMerge
{
    public class DataGroup : Singleton<DataGroup>
    {
        public GameConfigSO gameConfig;
        public GunShopSO gunShopData;
        public SkillShopSO skillShopData;
        public LevelManagerSO levelManagerData;
        public QuestGroupSO questData;

        public int LevelBonus
        {
            get => gameConfig.LevelBonus;
        }

        public int QuestClaimableCounting
        {
            get
            {
                if (questData == null) return 0;
                var quests = questData.quests;
                int questCompleted = 0;
                if (quests == null || quests.Length <= 0) return 0;
                for (int i = 0; i < quests.Length; i++)
                {
                    var quest = quests[i];
                    if (quest == null) continue;
                    if (quest.IsCompleted(i) && !quest.IsClaimed(i))
                    {
                        ++questCompleted;
                    }
                }
                return questCompleted;
            }
        }

        public WaveTK_WaveController CurWavePrefab
        {
            get {
                UserDataHandler.Ins.curLevelId = 
                    Mathf.Clamp(UserDataHandler.Ins.curLevelId, 0, levelManagerData.waveControllers.Length-1);
                return levelManagerData.waveControllers[UserDataHandler.Ins.curLevelId];
            }
        }

        public bool IsMaxGameLevel
        {
            get => UserDataHandler.Ins.curLevelId >= levelManagerData.waveControllers.Length;
        }

        private void Start()
        {
            Initialize();
        }

        private void Initialize()
        {
            UserDataHandler.Ins?.LoadLocal();

            LevelDataInitialize();
            GunDataInitialize();
            SkillDataInitialize();
            QuestDataInitialize();

            AdsController.Ins.SetIsNoAds(true);
            AudioBase.Ins.SetMusicVolume(UserDataHandler.Ins.setting.musicVol);
            AudioBase.Ins.SetSoundVolume(UserDataHandler.Ins.setting.soundVol);

            if (Pref.IsFirstTime && UserDataHandler.Ins.coin <= 0)
            {
                UserDataHandler.Ins.coin += gameConfig.testingModeEnable ? 1000000 : gameConfig.startingCoin;
                AddFirstChest();
                Pref.IsFirstTime = false;
            }

            if (ResourceBoost.Instance != null)
            {
                UserDataHandler.Ins.coin += ResourceBoost.Instance.golds;
                UserDataHandler.Ins.gems += ResourceBoost.Instance.gems;
                ResourceBoost.Instance.golds = 0;
                ResourceBoost.Instance.gems = 0;
            }

            PlayerPrefs.SetInt("gems", UserDataHandler.Ins.gems);
            PlayerPrefs.Save(); // Đảm bảo lưu lại ngay lập tức

            UserDataHandler.Ins.SaveData();

            Debug.Log("Initialize");
        }

        #region GUN
        private void GunDataInitialize()
        {
            if (gunShopData == null) return;
            var gunShopItems = gunShopData.gunStats;
            if (gunShopItems == null || gunShopItems.Length <= 0) return;
            for (int i = 0; i < gunShopItems.Length; i++)
            {
                var gunShopItem = gunShopItems[i];
                if (gunShopItem == null) continue;
                var gunUserData = UserDataHandler.Ins?.GetGunData(i);
                if (gunUserData != null) continue;
                gunUserData = new UpgradeableItemUserData();
                gunUserData.isUnlocked = gameConfig.testingModeEnable ? true : i == 0;
                UserDataHandler.Ins?.UpdateGunData(i, gunUserData);
            }
        }

        public void BuyGun(int gunId, GunStatSO gunDataSO, UnityAction BuyingSuccess = null, UnityAction BuyingFailed = null)
        {
            bool isNodeFull = BoardController.Ins.IsAllNodeFull();
            var isGunUnlocked = UserDataHandler.Ins.IsGunUnlocked(gunId);
            if (!isGunUnlocked || isNodeFull) return;

            int buyingPrice = GetRealGunPrice(gunDataSO.buyingPrice, PassiveSkillType.REDUCE_BUYING_PRICE);

            if (UserDataHandler.Ins.IsEnoughCoin(buyingPrice)) {
                UserDataHandler.Ins.coin -= buyingPrice;
                gunDataSO.InscreaseBuyingPrice();
                UserDataHandler.Ins?.SaveData();

                BuyingSuccess?.Invoke();
                this.PostActionEvent(GameplayAction.BUY_GUN, gunId);
                return;
            }

            BuyingFailed?.Invoke();
        }

        public void UpgradeGun(int gunId, GunStatSO gunDataSO, UnityAction UpgradeSuccess = null, UnityAction UpgradeFailed = null)
        {
            var isGunUnlocked = UserDataHandler.Ins.IsGunUnlocked(gunId);
            if (!isGunUnlocked) return;

            int upgradePrice = GetRealGunPrice(gunDataSO.upgradePrice, PassiveSkillType.REDUCE_UPGRADE_PRICE);

            if (UserDataHandler.Ins.IsEnoughCoin(upgradePrice))
            {
                UserDataHandler.Ins.coin -= upgradePrice;
                UserDataHandler.Ins?.SaveData();
                gunDataSO.Upgrade(gunId, () =>
                {
                    UpgradeSuccess?.Invoke();
                    this.PostActionEvent(GameplayAction.UPGRADE_GUN, gunId);
                });
                return;
            }

            UpgradeFailed?.Invoke();
        }

        public int GetRealGunPrice(int originalPrice, PassiveSkillType affectedType)
        {
            float bonusFromSkill = GetSkillBonus(affectedType, originalPrice);
            int reducedPrice = Mathf.RoundToInt(bonusFromSkill);
            return originalPrice - reducedPrice;
        }
        #endregion
        #region SKILL
        public float GetSkillBonus(PassiveSkillType type, float value)
        {
            return skillShopData.GetSkillBonus(type, value);
        }

        public void SkillDataInitialize()
        {
            if (skillShopData == null) return;
            var skillShopItems = skillShopData.skillStats;
            if (skillShopItems == null || skillShopItems.Length <= 0) return;
            for (int i = 0; i < skillShopItems.Length; i++)
            {
                var skillShopItem = skillShopItems[i];
                if (skillShopItem == null) continue;
                var skillUserData = UserDataHandler.Ins?.GetSkillData(i);
                if (skillUserData != null) continue;
                skillUserData = new UpgradeableItemUserData();
                skillUserData.isUnlocked = gameConfig.testingModeEnable ? true : i == 0;
                UserDataHandler.Ins?.UpdateSkillData(i, skillUserData);
            }
        }

        public void UpgradeSkill(int id,PassiveSkillSO skillDataSO, UnityAction UpgradeSuccess = null, UnityAction UpgradeFailed = null)
        {
            if (UserDataHandler.Ins.IsEnoughCoin(skillDataSO.upgradePrice))
            {
                UserDataHandler.Ins.coin -= skillDataSO.upgradePrice;
                UserDataHandler.Ins?.SaveData();
                skillDataSO.Upgrade(id, () =>
                {
                    UpgradeSuccess?.Invoke();

                    this.PostActionEvent(GameplayAction.SKILL_BOOSTER_UPDATE);
                    this.PostActionEvent(GameplayAction.UPGRADE_SKILL);
                });
                return;
            }

            UpgradeFailed?.Invoke();
        }
        #endregion
        #region LEVEL
        private void LevelDataInitialize()
        {
            if (levelManagerData == null) return;
            var levels = levelManagerData.waveControllers;
            if(levels == null || levels.Length <= 0) return;
            for (int i = 0; i < levels.Length; i++)
            {
                var levelUserData = UserDataHandler.Ins.GetLevelData(i);
                if (levelUserData != null) continue;
                levelUserData = new ItemStateUserData();
                levelUserData.isUnlocked = gameConfig.testingModeEnable ? true : i == 0;
                UserDataHandler.Ins.UpdateLevelData(i, levelUserData);
            }
        }

        public void SelectLevel(ItemStateUserData levelItem, int itemId, UnityAction OnSuccess = null)
        {
            ItemStateUserData levelUserData = UserDataHandler.Ins.GetLevelData(itemId);
            if (levelUserData == null) return;
            if (levelUserData.isUnlocked)
            {
                UserDataHandler.Ins.curLevelId = itemId;
                UserDataHandler.Ins.SaveData();
                OnSuccess?.Invoke();
            } 
        }
        #endregion
        #region BOOSTER
        public void BuyBooster(BoosterController boosterCtr, UnityAction BuyingSuccess = null, UnityAction BuyingFailed = null)
        {
            if (boosterCtr == null || boosterCtr.stat == null) return;

            if (UserDataHandler.Ins.IsEnoughCoin(boosterCtr.stat.price))
            {
                UserDataHandler.Ins.coin -= boosterCtr.stat.price;

                boosterCtr.SetTimeAction();

                UserDataHandler.Ins?.SaveData();

                BuyingSuccess?.Invoke();

                this.PostActionEvent(GameplayAction.BUY_BOOSTER);
                return;
            }

            BuyingFailed?.Invoke();
        }
        #endregion
        #region QUEST
        private void QuestDataInitialize()
        {
            if (questData == null) return;

            var quests = questData.quests;

            if (quests == null || quests.Length <= 0) return;

            for (int i = 0; i < quests.Length; i++)
            {
                var quest = quests[i];               
                if (quest == null) continue;
                quest.Id = i;
                var questUserData = UserDataHandler.Ins.GetQuestData(i);
                if (questUserData != null) continue;
                questUserData = new QuestUserData();
                questUserData.amount = 0;
                questUserData.isClaimed = false;
                UserDataHandler.Ins.UpdateQuestData(i, questUserData);
            }
        }

        public void UpdateQuests(QuestType type, int amount, bool incremental = true)
        {
            var questIds = GetQuestIds(type);

            if (questIds == null || questIds.Count <= 0) return;

            for (int i = 0; i < questIds.Count; i++)
            {
                var questId = questIds[i];
                QuestUserData questUserData = UserDataHandler.Ins.GetQuestData(questId);
                if (incremental)
                {
                    questUserData.amount += amount;
                }
                else if (amount > questUserData.amount)
                {
                    questUserData.amount = amount;
                }
                UserDataHandler.Ins.UpdateQuestData(questId, questUserData);
            }

            this.PostActionEvent(GameplayAction.UPDATE_QUEST);
        }

        private List<int> GetQuestIds(QuestType type)
        {
            List<int> results = new List<int>();

            var findeds = questData.quests.Select((Value, Index) => new { Value, Index }).
                Where(q => q.Value.type == type);

            if (findeds == null) return null;

            foreach (var finded in findeds)
            {
                results.Add(finded.Index);
            }

            return results;
        }

        public void ClaimAQuest(Quest quest, int id, UnityAction ClaimSuccess = null)
        {
            QuestUserData questUserData = UserDataHandler.Ins.GetQuestData(id);
            if (quest == null || questUserData == null) return;

            if (quest.IsCompleted(id) && !quest.IsClaimed(id))
            {
                questUserData.isClaimed = true;
                UserDataHandler.Ins.UpdateQuestData(id, questUserData);
                UserDataHandler.Ins.coin += quest.bonus;
                UserDataHandler.Ins.SaveData();

                ClaimSuccess?.Invoke();

                this.PostActionEvent(GameplayAction.UPDATE_QUEST);
            }
        }
        #endregion

        private void AddFirstChest()
        {
            ChestUserData chestUserData = new ChestUserData();
            chestUserData.openPrice = 0;
            chestUserData.coinsBonus = 500;

            SkillBonusUserData skillBonusUserData_1 = new SkillBonusUserData();
            skillBonusUserData_1.type = SkillCollectedType.RainOfMissile;
            skillBonusUserData_1.bonusAmount = 1;

            SkillBonusUserData skillBonusUserData_2 = new SkillBonusUserData();
            skillBonusUserData_2.type = SkillCollectedType.FreezeAllEnemy;
            skillBonusUserData_2.bonusAmount = 1;

            chestUserData.skillBonusList.Add(skillBonusUserData_1);
            chestUserData.skillBonusList.Add(skillBonusUserData_2);

            int chestId = UserDataHandler.Ins.chests.Count;
            UserDataHandler.Ins.UpdateChest(chestId, chestUserData);
        }
    }
}
