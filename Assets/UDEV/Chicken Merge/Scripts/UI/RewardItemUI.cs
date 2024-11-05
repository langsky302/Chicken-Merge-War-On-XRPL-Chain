using UnityEngine;
using UnityEngine.UI;

namespace UDEV.ChickenMerge
{
    public class RewardItemUI : MonoBehaviour
    {
        [SerializeField] private Image m_icon;
        [SerializeField] private Text m_amountTxt;

        public void UpdateUI(RewardDialogItem rewardItem)
        {
            if (m_icon)
                m_icon.sprite = rewardItem.icon;

            if(m_amountTxt)
                m_amountTxt.text = "+" + Helper.BigCurrencyFormat(rewardItem.amount);
        }
    }
}
