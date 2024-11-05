using UnityEngine;
using UnityEngine.UI;

namespace UDEV.ChickenMerge
{
    public class NoAdsBtn : MonoBehaviour
    {
        [SerializeField] private Text m_priceTxt;

        private void Start()
        {
            UpdatePriceTxt();
        }

        private void UpdatePriceTxt()
        {
#if USE_IAP
            if (m_priceTxt && IAPManager.Ins && IAPManager.Ins.data)
            {
                IAPItem iapItem = new IAPItem();
                iapItem.id = IAPManager.Ins.data.noadsId;
                iapItem.localPrice = IAPManager.Ins.data.noadsLocalPrice;

                m_priceTxt.text = IAPManager.Ins.GetPriceString(iapItem);
            }
#endif
        }

        public void Purchase()
        {
#if USE_IAP
            if (UserDataHandler.Ins.setting.isNoAds) return;

            IAPManager.Ins?.MakeItemBuying(IAPManager.Ins.data.noadsId);
#endif
        }
    }
}
