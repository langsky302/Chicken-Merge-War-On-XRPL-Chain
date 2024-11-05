using System;
using UDEV.ActionEventDispatcher;
using UDEV.DMTool;
using UnityEngine;
using UnityEngine.UI;

namespace UDEV.ChickenMerge
{
    public class GunSelectionUI : MonoBehaviour, IActionEventDispatcher
    {
        [Header("Buying Button:")]
        [SerializeField] private Button m_buyingBtn;
        [SerializeField] private Image m_gunIcon;
        [SerializeField] private Text m_gunNumberTxt;
        [SerializeField] private Text m_buyingPriceTxt;

        [Header("Upgrade Button:")]
        [SerializeField] private Button m_upgradeBtn;
        [SerializeField] private Text m_upgradePriceTxt;
        [SerializeField] private Transform m_starGrid;
        [SerializeField] private StarItemUI m_starPrefab;

        [Header("Navigator:")]
        [SerializeField] private Button m_nextBtn;
        [SerializeField] private Button m_prevBtn;

        private GunStatSO[] m_gunStats;
        private int m_gunSlectedIdx;

        #region ACTION
        private Action<object> m_OnUpgradeGunSkill;
        private Action<object> m_OnAutoSpawnDone;
        #endregion

        #region EVENTS
        public void RegisterEvents()
        {
            m_OnUpgradeGunSkill = param => UpdateUI(m_gunSlectedIdx);
            m_OnAutoSpawnDone = param => UpdateUI((int)param);

            this.RegisterActionEvent(GameplayAction.UPGRADE_GUN, m_OnUpgradeGunSkill);
            this.RegisterActionEvent(GameplayAction.UPGRADE_SKILL, m_OnUpgradeGunSkill);
            this.RegisterActionEvent(GameplayAction.AUTO_SPAWN_GUN_DONE, m_OnAutoSpawnDone);
        }
        public void UnregisterEvents()
        {
            this.RemoveActionEvent(GameplayAction.UPGRADE_GUN, m_OnUpgradeGunSkill);
            this.RemoveActionEvent(GameplayAction.UPGRADE_SKILL, m_OnUpgradeGunSkill);
            this.RemoveActionEvent(GameplayAction.AUTO_SPAWN_GUN_DONE, m_OnAutoSpawnDone);
        }
        #endregion

        private void Awake()
        {
            m_gunSlectedIdx = UserDataHandler.Ins.gunSelectedIndex;
            m_gunStats = DataGroup.Ins.gunShopData.gunStats;

            if (m_nextBtn)
            {
                m_nextBtn.onClick.RemoveListener(NextBtnEvent);
                m_nextBtn.onClick.AddListener(NextBtnEvent);
            }

            if (m_prevBtn)
            {
                m_prevBtn.onClick.RemoveListener(PrevBtnEvent);
                m_prevBtn.onClick.AddListener(PrevBtnEvent);
            }

            UpdateUI(m_gunSlectedIdx);
        }

        private void OnEnable()
        {
            RegisterEvents();
        }

        private void OnDisable()
        {
            UnregisterEvents();
        }

        public void UpdateUI(int gunSlectedIdx)
        {
            m_gunSlectedIdx = gunSlectedIdx;
            UserDataHandler.Ins.gunSelectedIndex = m_gunSlectedIdx;

            if (m_gunStats == null || m_gunStats.Length <= 0) return;

            var gunStat = m_gunStats[gunSlectedIdx];

            if (gunStat == null) return;

            if (m_gunIcon)
                m_gunIcon.sprite = gunStat.thumb;

            if(m_gunNumberTxt)
                m_gunNumberTxt.text = (gunSlectedIdx + 1).ToString("00");

            UpdateBuyingPriceTxt(gunSlectedIdx);
            UpdateUpgradePriceTxt(gunStat);
            
            UpdateStars(gunStat);

            if (m_buyingBtn)
            {
                m_buyingBtn.onClick.RemoveAllListeners();
                m_buyingBtn.onClick.AddListener(() => BuyGunBtnEvent(gunSlectedIdx, gunStat));
            }

            if (m_upgradeBtn)
            {
                m_upgradeBtn.onClick.RemoveAllListeners();
                m_upgradeBtn.onClick.AddListener(() => UpgradeGunBtnEvent(gunSlectedIdx, gunStat));
            }
        }

        private void UpdateBuyingPriceTxt(int gunId)
        {
            var gunStat = m_gunStats[m_gunSlectedIdx];
            int buyingPrice = DataGroup.Ins.GetRealGunPrice(gunStat.buyingPrice, PassiveSkillType.REDUCE_BUYING_PRICE);
            m_buyingPriceTxt.text = Helper.BigCurrencyFormat(buyingPrice);
        }

        private void UpdateUpgradePriceTxt(GunStatSO gunStat)
        {
            if (m_upgradePriceTxt == null) return;

            if (gunStat.IsMaxLevel())
            {
                m_upgradePriceTxt.text = "Max";
            }
            else
            {
                int upgradePrice = DataGroup.Ins.GetRealGunPrice(gunStat.upgradePrice, PassiveSkillType.REDUCE_UPGRADE_PRICE);
                m_upgradePriceTxt.text = Helper.BigCurrencyFormat(upgradePrice);
            }
        }

        private void BuyGunBtnEvent(int gundId, GunStatSO gunStat)
        {
            DataGroup.Ins?.BuyGun(gundId, gunStat, () => UpdateBuyingPriceTxt(gundId), ShowOutOfCoinDialog);
        }

        private void UpgradeGunBtnEvent(int gundId, GunStatSO gunStat)
        {
            DataGroup.Ins?.UpgradeGun(gundId, gunStat,() => UpdateUI(gundId), ShowOutOfCoinDialog);
        }

        private void ShowOutOfCoinDialog()
        {
            DialogDB.Ins.Show(DialogType.OutOfCoin);
        }

        private void UpdateStars(GunStatSO gunStat)
        {
            Helper.ClearChilds(m_starGrid);
            if (gunStat == null || !m_starPrefab) return;

            for (int i = 0; i < gunStat.maxLevel; i++)
            {
                var starClone = Instantiate(m_starPrefab);
                Helper.AssignToRoot(m_starGrid, starClone.transform, Vector3.zero, Vector3.one);
                starClone.ActiveStar(gunStat.level >= (i + 1));
            }
        }

        private void NextBtnEvent()
        {
            var isNextGunUnlocked = UserDataHandler.Ins.IsGunUnlocked(m_gunSlectedIdx + 1);
            if (!isNextGunUnlocked) return;

            m_gunSlectedIdx++;
            m_gunSlectedIdx = Mathf.Clamp(m_gunSlectedIdx, 0, m_gunStats.Length - 1);
            UserDataHandler.Ins.gunSelectedIndex = m_gunSlectedIdx;
            UpdateUI(m_gunSlectedIdx);
        }

        private void PrevBtnEvent()
        {
            m_gunSlectedIdx--;
            m_gunSlectedIdx = Mathf.Clamp(m_gunSlectedIdx, 0, m_gunStats.Length - 1);
            UserDataHandler.Ins.gunSelectedIndex = m_gunSlectedIdx;
            UpdateUI(m_gunSlectedIdx);
        }
    }
}
