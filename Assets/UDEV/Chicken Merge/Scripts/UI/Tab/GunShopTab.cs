using UDEV.DMTool;
using UnityEngine;

namespace UDEV.ChickenMerge
{
    public class GunShopTab : TabContent
    {
        [SerializeField] private Transform m_gridRoot;
        [SerializeField] private GunShopItemUI m_itemUIPrefab;
        private GunStatSO[] m_gunStats;

        public override void LoadContent()
        {
            if (DataGroup.Ins == null || DataGroup.Ins.gunShopData == null) return;

            m_gunStats = DataGroup.Ins.gunShopData.gunStats;

            UpdateUI();
        }

        private void UpdateUI()
        {
            Helper.ClearChilds(m_gridRoot);
            if (m_gunStats == null || m_gunStats.Length <= 0) return;

            for (int i = 0; i < m_gunStats.Length; i++)
            {
                int gunId = i;
                var gunStat = m_gunStats[gunId];
                if (gunStat == null) continue;
                gunStat.Load(gunId);
                var shopItemUIClone = Instantiate(m_itemUIPrefab);
                Helper.AssignToRoot(m_gridRoot, shopItemUIClone.transform, Vector3.zero, Vector3.one);
                shopItemUIClone.UpdateUI(gunId, gunStat);

                if (shopItemUIClone.buyingBtn)
                {
                    shopItemUIClone.buyingBtn.onClick.RemoveAllListeners();
                    shopItemUIClone.buyingBtn.onClick.AddListener(() => BuyGunBtnEvent(gunId, gunStat));
                }

                if (shopItemUIClone.upgradeBtn)
                {
                    shopItemUIClone.upgradeBtn.onClick.RemoveAllListeners();
                    shopItemUIClone.upgradeBtn.onClick.AddListener(() => UpgradeGunBtnEvent(gunId, gunStat));
                }
            }
        }

        private void BuyGunBtnEvent(int gundId, GunStatSO gunStat)
        {
            DataGroup.Ins?.BuyGun(gundId, gunStat, UpdateUI, ShowOutOfCoinDialog);
        }

        private void UpgradeGunBtnEvent(int gundId, GunStatSO gunStat)
        {
            DataGroup.Ins?.UpgradeGun(gundId, gunStat, UpdateUI, ShowOutOfCoinDialog);
        }

        private void ShowOutOfCoinDialog()
        {
            DialogDB.Ins.Show(DialogType.OutOfCoin);
        }
    }
}
