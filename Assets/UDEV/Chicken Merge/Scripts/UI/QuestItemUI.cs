using UnityEngine;
using UnityEngine.UI;

namespace UDEV.ChickenMerge
{
    public class QuestItemUI : MonoBehaviour
    {
        [SerializeField] private Text m_descriptionTxt;
        [SerializeField] private Text m_bonusTxt;
        [SerializeField] private Image m_checkMarkIcon;
        [SerializeField] private Sprite m_notClaimBtnBgSp;
        [SerializeField] private Sprite m_canClaimBtnBgSp;
        [SerializeField] private Sprite m_claimedBtnBgSp;
        [SerializeField] private Sprite m_coinIconSp;
        [SerializeField] private Sprite m_checkMarkSp;
        public Button claimBtn;

        public void UpdateUI(Quest quest) 
        {
            QuestUserData questUserData = UserDataHandler.Ins.GetQuestData(quest.Id);

            if (questUserData == null || quest == null) return;

            if (m_descriptionTxt)
                m_descriptionTxt.text = quest.GetQuestDesctiption(quest.type);

            if (m_bonusTxt)
                m_bonusTxt.text = quest.bonus.ToString("n0");

            UpdateClainBtnState(quest, questUserData);
        }

        private void UpdateClainBtnState(Quest quest, QuestUserData questUserData)
        {
            if (claimBtn == null) return;
            var btnImg = claimBtn.GetComponent<Image>();
            if(btnImg == null) return;
            if(!questUserData.isClaimed && questUserData.amount >= quest.amountRequired)
            {
                btnImg.sprite = m_canClaimBtnBgSp;
            }else if(questUserData.isClaimed)
            {
                btnImg.sprite = m_claimedBtnBgSp;
            }
            else
            {
                btnImg.sprite = m_notClaimBtnBgSp;
            }

            if (m_checkMarkIcon)
                m_checkMarkIcon.sprite = questUserData.isClaimed ? m_checkMarkSp : m_coinIconSp;
        }
    }
}
