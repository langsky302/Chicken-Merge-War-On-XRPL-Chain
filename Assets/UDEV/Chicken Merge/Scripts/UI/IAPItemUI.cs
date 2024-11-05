using UnityEngine;
using UnityEngine.UI;

namespace UDEV.ChickenMerge
{
    public class IAPItemUI : MonoBehaviour
    {
        [SerializeField] private Text m_priceText;
        [SerializeField] private Text m_amountText;
        public Button buyBtn;

#if USE_IAP
        public void UpdateUI(IAPItem item)
        {
            if(m_priceText && IAPManager.Ins)
            {
                m_priceText.text = IAPManager.Ins.GetPriceString(item ,"$");
            }

            if (m_amountText)
                m_amountText.text = "+" + item.amount.ToString("n0");
        }
#endif
    }
}
