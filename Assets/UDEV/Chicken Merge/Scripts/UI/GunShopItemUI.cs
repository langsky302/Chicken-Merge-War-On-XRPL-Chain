using UnityEngine;
using UnityEngine.UI;

namespace UDEV.ChickenMerge
{
    public class GunShopItemUI : MonoBehaviour
    {
        [SerializeField] private GameObject m_lockedArea;
        [SerializeField] private GameObject m_unlockedArea;
        [SerializeField] private Image m_gunIcon;
        [SerializeField] private Transform m_starGrid;
        [SerializeField] private StarItemUI m_starPrefab;
        [SerializeField] private Text m_levelCountingTxt;
        [SerializeField] private Text m_gunNumberTxt;
        [SerializeField] private Text m_upgradePriceTxt;
        [SerializeField] private Text m_buyingPriceTxt;
        public Button upgradeBtn;
        public Button buyingBtn;

        public void UpdateUI(int gunId, GunStatSO gunStat)
        {
            if (gunStat == null) return;

            UnlockUI(gunId);

            UpdateStars(gunStat);

            UpdatePriceTxts(gunStat);

            if (m_levelCountingTxt)
                m_levelCountingTxt.text = $"LEVEL {gunStat.level}";

            if (m_gunNumberTxt)
                m_gunNumberTxt.text = (gunId + 1).ToString("00");

            if (m_gunIcon)
                m_gunIcon.sprite = gunStat.thumb;
        }

        private void UnlockUI(int gundId)
        {
            var isGunUnlocked = UserDataHandler.Ins.IsGunUnlocked(gundId);

            if (m_lockedArea)
                m_lockedArea.SetActive(!isGunUnlocked);

            if (m_unlockedArea)
                m_unlockedArea.SetActive(isGunUnlocked);
        }

        private void UpdateStars(GunStatSO gunStat)
        {
            Helper.ClearChilds(m_starGrid);
            if (gunStat == null || m_starPrefab == null) return;

            for (int i = 0; i < gunStat.maxLevel; i++)
            {
                var starClone = Instantiate(m_starPrefab);
                Helper.AssignToRoot(m_starGrid, starClone.transform, Vector3.zero, Vector3.one);
                starClone.ActiveStar(gunStat.level >= (i + 1));
            }
        }

        private void UpdatePriceTxts(GunStatSO gunStat)
        {
            if (gunStat == null) return;

            if (m_buyingPriceTxt)
            {
                int buyingPrice = DataGroup.Ins.GetRealGunPrice(gunStat.buyingPrice, PassiveSkillType.REDUCE_BUYING_PRICE);
                m_buyingPriceTxt.text = Helper.BigCurrencyFormat(buyingPrice);
            }    

            if (m_upgradePriceTxt)
            {
                if(gunStat.IsMaxLevel())
                {
                    m_upgradePriceTxt.text = "Max Level";
                }
                else
                {
                    int upgradePrice = DataGroup.Ins.GetRealGunPrice(gunStat.upgradePrice, PassiveSkillType.REDUCE_UPGRADE_PRICE);
                    m_upgradePriceTxt.text = Helper.BigCurrencyFormat(upgradePrice);
                }
            }
        }
    }
}
