using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace UDEV.ChickenMerge
{
    public class UserDataHandler : Singleton<UserDataHandler>
    {
        public int coin;
        public int gems;
        public int curLevelId;
        public int gunSelectedIndex;
        public SettingUserData setting;
        public List<ItemStateUserData> levelStates;
        public UpgradeableItemUserData shieldStat;
        public List<UpgradeableItemUserData> gunStats;
        public List<UpgradeableItemUserData> skillStats;
        public List<QuestUserData> questStates;
        public List<ChestUserData> chests;
        public List<TimeActionUserData> timeActions;

        public UnityEvent OnInit;
        public UnityEvent OnLocalLoaded;
        public UnityEvent OnCloudLoaded;
        public UnityEvent OnDataLoaded;

        public ItemStateUserData CurLvData
        {
            get => GetLevelData(curLevelId);
        }

        protected override void Awake()
        {
            base.Awake();
            setting = new SettingUserData();
            levelStates = new List<ItemStateUserData>();
            timeActions = new List<TimeActionUserData>();
            gunStats = new List<UpgradeableItemUserData>();
            questStates = new List<QuestUserData>();
            chests = new List<ChestUserData>();
        }

        private void Init()
        {
            setting.musicVol = 0.55f;
            setting.soundVol = 1f;

            if (OnInit != null)
            {
                OnInit.Invoke();
            }

            SaveData();
        }

        public void SaveData()
        {
            Pref.GameData = JsonUtility.ToJson(this);
        }

        private void LoadData()
        {
            string data = Pref.GameData;
            if (string.IsNullOrEmpty(data)) return;

            JsonUtility.FromJsonOverwrite(data, this);

            if (OnDataLoaded != null)
            {
                OnDataLoaded.Invoke();
            }
        }

        public void LoadLocal()
        {
            if (Pref.IsFirstTime)
            {
                Init();
            }
            else
            {
                LoadData();
                SaveData();

                if (OnLocalLoaded != null)
                {
                    OnLocalLoaded.Invoke();
                }
            }
        }

        public bool IsEnoughCoin(int coinChecking)
        {
            return coin >= coinChecking;
        }

        #region BASE
        private T GetValue<T>(List<T> dataList, int idx)
        {
            if (dataList == null || dataList.Count <= 0 || idx < 0 || idx >= dataList.Count) return default;

            return dataList[idx];
        }

        private void UpdateValue<T>(ref List<T> dataList, int idx, T value)
        {
            if (dataList == null || idx < 0) return;

            if (dataList.Count <= 0 || (dataList.Count > 0 && idx >= dataList.Count))// 2 2
            {
                dataList.Add(value);
            }
            else
            {
                dataList[idx] = value;
            }
        }

        private UpgradeableItemUserData GetItemData(List<UpgradeableItemUserData> datas, int itemId)
        {
            return GetValue(datas, itemId);
        }

        private void UpdateItemData(ref List<UpgradeableItemUserData> datas, int itemId, UpgradeableItemUserData value)
        {
            UpdateValue(ref datas, itemId, value);
        }

        private void UpdateItemUnlocked(ref List<UpgradeableItemUserData> datas, int itemId, bool isUnlocked)
        {
            UpgradeableItemUserData itemData = GetValue(datas, itemId);
            if (itemData == null) return;
            itemData.isUnlocked = isUnlocked;
            UpdateItemData(ref datas, itemId, itemData);
        }

        private void UpdateItemStat(ref List<UpgradeableItemUserData> datas, int itemId, string stat)
        {
            UpgradeableItemUserData itemData = GetValue(datas, itemId);
            if (itemData == null) return;
            itemData.stat = stat;
            UpdateItemData(ref datas, itemId, itemData);
        }
        #endregion

        #region GUN_DATA
        public UpgradeableItemUserData GetGunData(int gunId)
        {
            return GetItemData(gunStats, gunId);
        }

        public void UpdateGunData(int gunId, UpgradeableItemUserData value)
        {
            UpdateItemData(ref gunStats, gunId, value);
        }

        public void UpdateGunUnlocked(int gunId, bool isUnlocked)
        {
            UpdateItemUnlocked(ref gunStats, gunId, isUnlocked);
        }

        public bool IsGunUnlocked(int gundId)
        {
            var gunData = GetGunData(gundId);
            if(gunData == null) return false;
            return gunData.isUnlocked;
        }
        public void UpdateGunStat(int skillId, string stat)
        {
            UpdateItemStat(ref gunStats, skillId, stat);
        }
        #endregion

        #region SKILL_DATA
        public UpgradeableItemUserData GetSkillData(int skillId)
        {
            return GetItemData(skillStats, skillId);
        }

        public void UpdateSkillData(int skillId, UpgradeableItemUserData value)
        {
            UpdateItemData(ref skillStats, skillId, value);
        }

        public void UpdateSkillStat(int skillId, string stat)
        {
            UpdateItemStat(ref skillStats, skillId, stat);
        }
        #endregion

        #region SHIELD_DATA
        public UpgradeableItemUserData GetShieldData()
        {
            return shieldStat;
        }

        public void UpdateShieldData(UpgradeableItemUserData value)
        {
            shieldStat = value;
        }
        #endregion

        #region LEVEL
        public ItemStateUserData GetLevelData(int levelId)
        {
            return GetValue(levelStates, levelId);
        }

        public void UpdateLevelData(int levelId, ItemStateUserData value)
        {
            UpdateValue(ref levelStates, levelId, value);
        }

        public void UpdateLevelUnlocked(int levelId, bool isUnlocked)
        {
            var levelData = GetLevelData(levelId);
            if (levelData == null) return;
            levelData.isUnlocked = isUnlocked;
            UpdateLevelData(levelId, levelData);
        }

        public bool IsLevelUnlocked(int levelId)
        {
            var levelData = GetLevelData(levelId);
            if (levelData == null) return false;
            return levelData.isUnlocked;
        }
        #endregion

        #region QUEST
        public void UpdateQuestData(int questId, QuestUserData value)
        {
            UpdateValue(ref questStates, questId, value);
        }

        public QuestUserData GetQuestData(int questId)
        {
            return GetValue(questStates, questId);
        }
        #endregion

        #region CHEST
        public void UpdateChest(int chestId, ChestUserData value)
        {
            UpdateValue(ref chests, chestId, value);
        }
        #endregion
    }
}
